using Autofac;

using FooCommerce.Application.DbProvider;
using FooCommerce.Products.Interfaces;
using FooCommerce.Products.Products.Entities;
using FooCommerce.Products.PurchasableItems.Commands;
using FooCommerce.Products.PurchasableItems.Models;

using MassTransit.Testing;

using Microsoft.EntityFrameworkCore;

namespace FooCommerce.Products.PurchasableItems.Tests.Services;

public class PostingServiceTests : Fixture
{
    private readonly IDbContextFactory<AppDbContext> dbContextFactory;

    public PostingServiceTests()
    {
        dbContextFactory = this.Container.Resolve<IDbContextFactory<AppDbContext>>();
    }

    private void Initialize()
    {
        using var dbContext = dbContextFactory.CreateDbContext();
        var category = new AdCategory
        {
            Name = "Books",
        };

        dbContext.Add(category);
        dbContext.SaveChanges();
    }

    [Fact]
    public async Task NewAsync()
    {
        // Arrange
        Initialize();

        await using var dbContext = await dbContextFactory.CreateDbContextAsync();
        var category = dbContext.Set<AdCategory>().First();

        var request = new NewPurchasableItemRequest
        {
            CategoryId = category.PublicId,
            FullQualifiedName = "Asus ROG Strix G513IE-HN060 15.6 inch Laptop",
            Name = "لپ تاپ 15.6 اینچی ایسوس مدل ROG Strix G513IE-HN060",
            Specifications = new Dictionary<long, object[]>
            {
                {1, new []{"Ryzen 7"}},
                {2, new []{"8 Gigabyte"}},
                {3, new[] {"DDR4"}},
                {4, new [] {"Nvidia"}},
                {5, new [] {"15.6\""}},
                {6, new [] {"1920×1080px (FullHD)"}},
                {7, new [] {"Games", "Productive"}}
            },
            Images = new[]
            {
                "https://dkstatics-public.digikala.com/digikala-products/7cea7aca2b40f8a5152f86aa28449efa34634a7d_1645858428.jpg?x-oss-process=image/resize,m_lfit,h_800,w_800/quality,q_90",
                "https://dkstatics-public.digikala.com/digikala-products/7cea7aca2b40f8a5152f86aa28449efa34634a7d_1645858428.jpg?x-oss-process=image/resize,m_lfit,h_800,w_800/quality,q_90",
                "https://dkstatics-public.digikala.com/digikala-products/7cea7aca2b40f8a5152f86aa28449efa34634a7d_1645858428.jpg?x-oss-process=image/resize,m_lfit,h_800,w_800/quality,q_90",
                "https://dkstatics-public.digikala.com/digikala-products/7cea7aca2b40f8a5152f86aa28449efa34634a7d_1645858428.jpg?x-oss-process=image/resize,m_lfit,h_800,w_800/quality,q_90",
                "https://dkstatics-public.digikala.com/digikala-products/7cea7aca2b40f8a5152f86aa28449efa34634a7d_1645858428.jpg?x-oss-process=image/resize,m_lfit,h_800,w_800/quality,q_90",
                "https://dkstatics-public.digikala.com/digikala-products/7cea7aca2b40f8a5152f86aa28449efa34634a7d_1645858428.jpg?x-oss-process=image/resize,m_lfit,h_800,w_800/quality,q_90",
                "https://dkstatics-public.digikala.com/digikala-products/7cea7aca2b40f8a5152f86aa28449efa34634a7d_1645858428.jpg?x-oss-process=image/resize,m_lfit,h_800,w_800/quality,q_90",
            },
            Price = "36000000",

        };

        // Act
        await TestHarness.Start();
        var client = TestHarness.GetRequestClient<NewPurchasableItemRequest>();

        var response1 = await client.GetResponse<IAdRequestResult>(request);
        var response2 = TestHarness.Sent.Select<IAdRequestResult>().First().MessageObject as IAdRequestResult;

        // Assert
        Assert.True(await TestHarness.Sent.Any<IAdRequestResult>());
        Assert.True(await TestHarness.Consumed.Any<NewPurchasableItemRequest>());
        Assert.True(await TestHarness.GetConsumerHarness<CreatePurchasableItemConsumer>().Consumed.Any<NewPurchasableItemRequest>());
        Assert.NotNull(response1.Message);
        Assert.NotNull(response2);
        Assert.True(response2.IsSuccess);

        await TestHarness.Stop();
    }
}