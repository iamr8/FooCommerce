CREATE TABLE [dbo].[UserNotifications]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
    [RenderedContent] NVARCHAR(MAX) NOT NULL,
    [IPAddress] NCHAR(39) NOT NULL, 
    [UserAgent] NVARCHAR(MAX) NOT NULL, 
    [Sent] DATETIME2 NULL, 
    [Delivered] DATETIME2 NULL, 
    [Seen] DATETIME2 NULL, 
    [AuthTokenId] UNIQUEIDENTIFIER NOT NULL, 
    [NotificationId] UNIQUEIDENTIFIER NOT NULL, 
    [UserId] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [FK_UserNotifications_Notifications] FOREIGN KEY ([NotificationId]) REFERENCES [Notifications]([Id]),
    CONSTRAINT [FK_UserNotifications_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_UserNotifications_AuthTokens] FOREIGN KEY ([AuthTokenId]) REFERENCES [AuthTokens]([Id])
)

GO

CREATE UNIQUE INDEX [IX_UserNotifications_Id] ON [dbo].[UserNotifications] ([Id])

GO

CREATE FULLTEXT INDEX ON [dbo].[UserNotifications] ([RenderedContent]) KEY INDEX [IX_UserNotifications_Id] ON [FullTextCatalog] WITH CHANGE_TRACKING AUTO
