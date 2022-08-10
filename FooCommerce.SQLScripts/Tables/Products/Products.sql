CREATE TABLE [dbo].[Products]
(
    [Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
    [IsDeleted] BIT DEFAULT 0 NOT NULL, 
	[Name] NVARCHAR(300) NULL,
    [CategoryId] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [FK_Products_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [Categories]([Id]), 
)

GO

CREATE UNIQUE INDEX [IX_Products_Id] ON [dbo].[Products] ([Id])

GO

CREATE FULLTEXT INDEX ON [dbo].[Products] ([Name]) KEY INDEX [IX_Products_Id] ON [FullTextCatalog] WITH CHANGE_TRACKING AUTO
