CREATE TABLE [dbo].[TopCategories]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
    [IsDeleted] BIT DEFAULT 0 NOT NULL, 
    [IsHidden] BIT DEFAULT 0 NOT NULL,
	[Name] NVARCHAR(100) NOT NULL, 
	[Slug] NVARCHAR(50) NOT NULL,
	[Description] NVARCHAR(MAX) NOT NULL,
	[Icon] NVARCHAR(400) NOT NULL,
    [Order] TINYINT NOT NULL,
    CONSTRAINT [CK_TopCategories_NameNotEmpty] CHECK (LEN([Name]) >= 1),
    CONSTRAINT [CK_TopCategories_SlugNotEmpty] CHECK (LEN([Slug]) >= 3)
)

GO

CREATE UNIQUE INDEX [IX_TopCategories_Id] ON [dbo].[TopCategories] ([Id])

GO

CREATE FULLTEXT INDEX ON [dbo].[TopCategories] ([Name]) KEY INDEX [IX_TopCategories_Id] ON [FullTextCatalog] WITH CHANGE_TRACKING AUTO

