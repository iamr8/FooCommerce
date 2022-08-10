CREATE TABLE [dbo].[PurchasePrices]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
	[Amount] MONEY NOT NULL,
	[ListingId] uniqueidentifier NULL,
    CONSTRAINT [FK_PurchasePrices_Listings] FOREIGN KEY ([ListingId]) REFERENCES [Listings]([Id])
)

GO

CREATE UNIQUE INDEX [IX_PurchasePrices_Id] ON [dbo].[PurchasePrices] ([Id])

