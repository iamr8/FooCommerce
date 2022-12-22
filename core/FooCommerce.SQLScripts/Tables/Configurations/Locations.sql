CREATE TABLE [dbo].[Locations]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
    [IsDeleted] BIT DEFAULT 0 NOT NULL, 
    [PublicId] INT NOT NULL, 
    [IsHidden] BIT DEFAULT 0 NOT NULL,
	[Division] TINYINT NOT NULL, 
	[Name] NVARCHAR(100) NOT NULL,
	[Coordinate] [sys].[geometry] NULL,
    [ParentId] uniqueidentifier NULL, 
    CONSTRAINT [FK_Locations_Locations] FOREIGN KEY ([ParentId]) REFERENCES [Locations]([Id]), 
    CONSTRAINT [CK_Locations_ValidName] CHECK (LEN([Name]) >= 2),
)

GO

CREATE UNIQUE INDEX [IX_Locations_Id] ON [dbo].[Locations] ([Id])

GO

CREATE FULLTEXT INDEX ON [dbo].[Locations] ([Name]) KEY INDEX [IX_Locations_Id] ON [FullTextCatalog] WITH CHANGE_TRACKING AUTO


