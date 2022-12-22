CREATE TABLE [dbo].[ListingRatings]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
	[Rate] TINYINT DEFAULT 1 NOT NULL,
    [ListingId] uniqueidentifier NOT NULL,
	[UserId] uniqueidentifier NOT NULL, 
    CONSTRAINT [FK_ListingRatings_Listings] FOREIGN KEY ([ListingId]) REFERENCES [Listings]([Id]), 
    CONSTRAINT [FK_ListingRatings_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
)

GO

CREATE UNIQUE INDEX [IX_ListingRatings_Id] ON [dbo].[ListingRatings] ([Id])

GO