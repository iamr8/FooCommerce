CREATE TABLE [dbo].[AuthTokens]
(
    [Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
    [Action] BIT NOT NULL, 
    [Token] NVARCHAR(20) NOT NULL, 
    [IPAddress] NCHAR(39) NOT NULL, 
    [UserAgent] NVARCHAR(MAX) NOT NULL, 
    [Sent] DATETIME2 NULL, 
    [Delivered] DATETIME2 NULL, 
    [Authorized] DATETIME2 NULL, 
    [UserCommunicationId] UNIQUEIDENTIFIER NOT NULL, 
    CONSTRAINT [FK_Tokens_ToTable] FOREIGN KEY ([UserCommunicationId]) REFERENCES [UserCommunications]([Id]), 
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
                ON [UserCommunications].[Id] = [INSERTED].[UserContactId]
		END
    END