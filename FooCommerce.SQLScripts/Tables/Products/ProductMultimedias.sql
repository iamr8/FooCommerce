CREATE TABLE [dbo].[ProductMultimedias]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
    [IsDeleted] BIT DEFAULT 0 NOT NULL, 
    [PublicId] INT NOT NULL, 
    [IsVideo] BIT NOT NULL,
	[Coordinate] [sys].[geometry] NULL,
    [Metadata] NVARCHAR(MAX) NULL,
    [Path] NVARCHAR(MAX) NOT NULL,
    [IsOriginal] BIT NOT NULL,
    [Order] TINYINT NOT NULL,
    [ProductId] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [FK_ProductPictures_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id]),
)

GO

CREATE UNIQUE INDEX [IX_ProductMultimedias_Id] ON [dbo].[ProductMultimedias] ([Id])

GO

CREATE FULLTEXT INDEX ON [dbo].[ProductMultimedias] ([Metadata]) KEY INDEX [IX_ProductMultimedias_Id] ON [FullTextCatalog] WITH CHANGE_TRACKING AUTO
