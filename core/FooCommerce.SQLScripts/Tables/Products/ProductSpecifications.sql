CREATE TABLE [dbo].[ProductSpecifications]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
    [Value] NVARCHAR(MAX) NULL,
    [Order] TINYINT NOT NULL,
    [SpecificationId] UNIQUEIDENTIFIER NOT NULL,
    [ProductId] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [FK_ProductSpecifications_Specifications] FOREIGN KEY ([SpecificationId]) REFERENCES [Specifications]([Id]), 
    CONSTRAINT [FK_ProductSpecifications_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id]),
)

GO

CREATE UNIQUE INDEX [IX_ProductSpecifications_Id] ON [dbo].[ProductSpecifications] ([Id])

GO

CREATE FULLTEXT INDEX ON [dbo].[ProductSpecifications] ([Value]) KEY INDEX [IX_ProductSpecifications_Id] ON [FullTextCatalog] WITH CHANGE_TRACKING AUTO
