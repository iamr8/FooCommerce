CREATE TABLE [dbo].[UserLockouts]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
	[ReasonType] TINYINT NOT NULL,
	[ReasonMore] NVARCHAR(MAX) NULL,
	[LockedUntil] DATETIME2 NULL,
	[UserId] uniqueidentifier NOT NULL, 
    CONSTRAINT [FK_UserLockouts_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
)

GO

CREATE UNIQUE INDEX [IX_UserLockouts_Id] ON [dbo].[UserLockouts] ([Id])

GO

CREATE FULLTEXT INDEX ON [dbo].[UserLockouts] ([ReasonMore]) KEY INDEX [IX_UserLockouts_Id] ON [FullTextCatalog] WITH CHANGE_TRACKING AUTO

GO