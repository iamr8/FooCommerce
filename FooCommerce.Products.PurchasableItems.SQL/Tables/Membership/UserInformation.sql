CREATE TABLE [dbo].[UserInformation]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
	[Type] TINYINT NOT NULL,
	[Value] NVARCHAR(MAX) NOT NULL,
	[UserId] uniqueidentifier NOT NULL, 
    CONSTRAINT [FK_UserInformation_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
)

GO

CREATE UNIQUE INDEX [IX_UserInformation_Id] ON [dbo].[UserInformation] ([Id])

GO

CREATE FULLTEXT INDEX ON [dbo].[UserInformation] ([Value]) KEY INDEX [IX_UserInformation_Id] ON [FullTextCatalog] WITH CHANGE_TRACKING AUTO

GO