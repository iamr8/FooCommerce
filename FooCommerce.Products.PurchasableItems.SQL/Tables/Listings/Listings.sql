CREATE TABLE [dbo].[Listings]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
    [PublicId] INT NOT NULL, 
    [IsSuspended] BIT DEFAULT 0 NOT NULL, 
	[IsCancelled] BIT DEFAULT 0 NOT NULL,
	[Name] NVARCHAR(300) NOT NULL,
    [ProductId] UNIQUEIDENTIFIER NOT NULL,
    [UserSubscriptionId] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [FK_Listings_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id]), 
    CONSTRAINT [FK_Listings_UserSubscriptions] FOREIGN KEY ([UserSubscriptionId]) REFERENCES [UserSubscriptions]([Id]), 
)

GO

CREATE UNIQUE INDEX [IX_Listings_Id] ON [dbo].[Listings] ([Id])

GO

CREATE FULLTEXT INDEX ON [dbo].[Listings] ([Name]) KEY INDEX [IX_Listings_Id] ON [FullTextCatalog] WITH CHANGE_TRACKING AUTO

