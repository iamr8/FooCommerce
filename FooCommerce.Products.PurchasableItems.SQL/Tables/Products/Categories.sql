CREATE TABLE [dbo].[Categories]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
    [IsDeleted] BIT DEFAULT 0 NOT NULL, 
    [PublicId] INT NOT NULL, 
    [IsHidden] BIT DEFAULT 0 NOT NULL,
    [Type] SMALLINT DEFAULT 0 NOT NULL,
	[Name] NVARCHAR(300) NOT NULL,
    [Description] NVARCHAR(MAX) NOT NULL,
	[Icon] NVARCHAR(400) NOT NULL,
    [ParentId] UNIQUEIDENTIFIER NULL, 
    [TopCategoryId] UNIQUEIDENTIFIER NOT NULL,
    CONSTRAINT [FK_Categories_Categories] FOREIGN KEY ([ParentId]) REFERENCES [Categories]([Id]), 
    CONSTRAINT [FK_Categories_TopCategories] FOREIGN KEY ([TopCategoryId]) REFERENCES [TopCategories]([Id]) 
)

GO

CREATE UNIQUE INDEX [IX_Categories_Id] ON [dbo].[Categories] ([Id])

GO

CREATE FULLTEXT INDEX ON [dbo].[Categories] ([Name]) KEY INDEX [IX_Categories_Id] ON [FullTextCatalog] WITH CHANGE_TRACKING AUTO

