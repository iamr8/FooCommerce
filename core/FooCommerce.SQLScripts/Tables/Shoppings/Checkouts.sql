CREATE TABLE [dbo].[Checkouts]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
	[OrderId] INT NOT NULL,
    [Status] TINYINT DEFAULT 0 NOT NULL,
	[Amount] MONEY DEFAULT 0 NOT NULL,
	[IsSuccessful] BIT DEFAULT 0 NOT NULL,
	[BasketId] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [FK_Checkouts_ShoppingBaskets] FOREIGN KEY ([BasketId]) REFERENCES [ShoppingBaskets]([Id]),
)

GO

CREATE UNIQUE INDEX [IX_Checkouts_Id] ON [dbo].[Checkouts] ([Id])
