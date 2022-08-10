CREATE TABLE [dbo].[ListingComments]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
    [PublicId] INT NOT NULL, 
	[Comment] NVARCHAR(MAX) NOT NULL,
	[CommentId] uniqueidentifier NULL,
	[ListingId] uniqueidentifier NOT NULL,
	[UserId] uniqueidentifier NOT NULL, 
    CONSTRAINT [FK_ListingComments_ListingComments] FOREIGN KEY ([CommentId]) REFERENCES [ListingComments]([Id]), 
    CONSTRAINT [FK_ListingComments_Listings] FOREIGN KEY ([ListingId]) REFERENCES [Listings]([Id]), 
    CONSTRAINT [FK_ListingComments_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
)

GO

CREATE UNIQUE INDEX [IX_ListingComments_Id] ON [dbo].[ListingComments] ([Id])

GO

CREATE FULLTEXT INDEX ON [dbo].[ListingComments] ([Comment]) KEY INDEX [IX_ListingComments_Id] ON [FullTextCatalog] WITH CHANGE_TRACKING AUTO

