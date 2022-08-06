CREATE TABLE [dbo].[ListingReports]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
	[Reason] TINYINT NOT NULL,
    [Description] NVARCHAR(MAX) NULL,
    [ListingId] uniqueidentifier NOT NULL,
	[UserId] uniqueidentifier NOT NULL, 
    CONSTRAINT [FK_ListingReports_Listings] FOREIGN KEY ([ListingId]) REFERENCES [Listings]([Id]), 
    CONSTRAINT [FK_ListingReports_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
)

GO

CREATE UNIQUE INDEX [IX_ListingReports_Id] ON [dbo].[ListingReports] ([Id])

GO

CREATE FULLTEXT INDEX ON [dbo].[ListingReports] ([Description]) KEY INDEX [IX_ListingReports_Id] ON [FullTextCatalog] WITH CHANGE_TRACKING AUTO