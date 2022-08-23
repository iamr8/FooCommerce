CREATE TABLE [dbo].[AuthTokens]
(
    [Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
    [Action] TINYINT NOT NULL, 
    [Token] NVARCHAR(20) NOT NULL, 
    [Authorized] DATETIME2 NULL, 
    [UserNotificationId] UNIQUEIDENTIFIER NOT NULL, 
    [UserCommunicationId] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [FK_AuthTokens_UserCommunications] FOREIGN KEY ([UserCommunicationId]) REFERENCES [UserCommunications]([Id]), 
    CONSTRAINT [FK_AuthTokens_Notifications] FOREIGN KEY ([UserNotificationId]) REFERENCES [UserNotifications]([Id]), 
)

GO

CREATE UNIQUE INDEX [IX_AuthTokens_Id] ON [dbo].[AuthTokens] ([Id])
GO

CREATE TRIGGER [dbo].[Trigger_AuthTokensUpdateUserCommunications]
    ON [dbo].[AuthTokens]
    AFTER UPDATE
    AS
    BEGIN
        SET NoCount ON

        IF (UPDATE(Authorized))
		BEGIN
	        UPDATE [UserCommunications]
            SET [IsVerified] = (CASE WHEN [INSERTED].Authorized IS NOT NULL THEN 1 ELSE 0 END)
			FROM [UserCommunications]
            JOIN INSERTED
                ON [UserCommunications].[Id] = [INSERTED].[UserCommunicationId]
		END
    END