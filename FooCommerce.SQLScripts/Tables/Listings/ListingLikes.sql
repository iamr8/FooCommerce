CREATE TABLE [dbo].[ListingLikes]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
    [ListingId] uniqueidentifier NOT NULL,
	[UserId] uniqueidentifier NOT NULL, 
    CONSTRAINT [FK_ListingLikes_Listings] FOREIGN KEY ([ListingId]) REFERENCES [Listings]([Id]), 
    CONSTRAINT [FK_ListingLikes_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
)

GO

CREATE UNIQUE INDEX [IX_ListingLikes_Id] ON [dbo].[ListingLikes] ([Id])

GO