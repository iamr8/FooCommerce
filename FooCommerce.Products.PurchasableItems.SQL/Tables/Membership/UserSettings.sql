CREATE TABLE [dbo].[UserSettings]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
	[Key] NVARCHAR(255) NOT NULL,
	[Value] NVARCHAR(255) NOT NULL,
	[UserId] uniqueidentifier NOT NULL, 
    CONSTRAINT [FK_UserSettings_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
)

GO

CREATE UNIQUE INDEX [IX_UserSettings_Id] ON [dbo].[UserSettings] ([Id])

GO

CREATE FULLTEXT INDEX ON [dbo].[UserSettings] ([Key]) KEY INDEX [IX_UserSettings_Id] ON [FullTextCatalog] WITH CHANGE_TRACKING AUTO
