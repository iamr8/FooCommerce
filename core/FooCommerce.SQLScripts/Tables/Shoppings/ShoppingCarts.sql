CREATE TABLE [dbo].[ShoppingCarts]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
    [IsDeleted] BIT DEFAULT 0 NOT NULL, 
    [PublicId] INT NOT NULL, 
    [Quantity] SMALLINT NOT NULL DEFAULT 1,
	[Amount] MONEY DEFAULT 0 NOT NULL,
	[PurchasePriceId] uniqueidentifier NULL,
    CONSTRAINT [FK_ShoppingCarts_PurchasePrices] FOREIGN KEY ([PurchasePriceId]) REFERENCES [PurchasePrices]([Id]), 
    CONSTRAINT [CK_ShoppingCarts_QuantityEqualGreaterThanOne] CHECK ([Quantity] >= 1),
)

GO

CREATE UNIQUE INDEX [IX_ShoppingCarts_Id] ON [dbo].[ShoppingCarts] ([Id])

GO