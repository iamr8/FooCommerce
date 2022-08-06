CREATE TABLE [dbo].[ShoppingBaskets]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
    [IsDeleted] BIT DEFAULT 0 NOT NULL,
    [PublicId] INT NOT NULL, 
    [TopCategoryId] UNIQUEIDENTIFIER NOT NULL,
	[UserSubscriptionId] uniqueidentifier NOT NULL, 
    CONSTRAINT [FK_ShoppingBaskets_UserSubscriptions] FOREIGN KEY ([UserSubscriptionId]) REFERENCES [UserSubscriptions]([Id]), 
    CONSTRAINT [FK_ShoppingBaskets_TopCategories] FOREIGN KEY ([TopCategoryId]) REFERENCES [TopCategories]([Id])
)

GO

CREATE UNIQUE INDEX [IX_ShoppingBaskets_Id] ON [dbo].[ShoppingBaskets] ([Id])