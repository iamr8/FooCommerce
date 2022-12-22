CREATE TABLE [dbo].[Translations]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
    [Key] NVARCHAR(300) DEFAULT 0 NOT NULL,
	[Value] NVARCHAR(MAX) NOT NULL,
)

GO

CREATE UNIQUE INDEX [IX_Translations_Id] ON [dbo].[Translations] ([Id])

GO

CREATE FULLTEXT INDEX ON [dbo].[Translations] ([Key]) KEY INDEX [IX_Translations_Id] ON [FullTextCatalog] WITH CHANGE_TRACKING AUTO

