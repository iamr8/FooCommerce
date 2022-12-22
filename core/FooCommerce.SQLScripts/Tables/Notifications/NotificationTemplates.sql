CREATE TABLE [dbo].[NotificationTemplates]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
    [Type] TINYINT NOT NULL,
    [JsonTemplate] NVARCHAR(MAX) NOT NULL,
    [IncludeRequest] BIT NOT NULL,
    [NotificationId] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [FK_NotificationTemplates_Notifications] FOREIGN KEY ([NotificationId]) REFERENCES [Notifications]([Id])
)

GO

CREATE UNIQUE INDEX [IX_NotificationTemplates_Id] ON [dbo].[NotificationTemplates] ([Id])

GO

CREATE FULLTEXT INDEX ON [dbo].[NotificationTemplates] ([JsonTemplate]) KEY INDEX [IX_NotificationTemplates_Id] ON [FullTextCatalog] WITH CHANGE_TRACKING AUTO
