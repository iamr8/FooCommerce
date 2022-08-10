CREATE TABLE [dbo].[Specifications]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
    [IsDeleted] BIT DEFAULT 0 NOT NULL, 
    [IsHidden] BIT DEFAULT 0 NOT NULL,
	[Name] NVARCHAR(300) DEFAULT 0 NOT NULL,
)

GO

CREATE UNIQUE INDEX [IX_Specifications_Id] ON [dbo].[Specifications] ([Id])

GO

CREATE FULLTEXT INDEX ON [dbo].[Specifications] ([Name]) KEY INDEX [IX_Specifications_Id] ON [FullTextCatalog] WITH CHANGE_TRACKING AUTO
