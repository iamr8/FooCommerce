﻿using FooCommerce.Domain.DbProvider;
using FooCommerce.Domain.DbProvider.Interfaces;

namespace FooCommerce.Products.Entities.Products
{
    public class ProductSpecification : Entity, IEntityProductSpecification
    {
        public int Key { get; set; }
        public string Value { get; set; }
        public Guid? ProductId { get; set; }
        public Guid? AdId { get; set; }
    }
}