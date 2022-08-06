CREATE TABLE [dbo].[UserPasswords]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
	[Hash] NVARCHAR(MAX) NOT NULL,
    [Salt] NVARCHAR(MAX) NOT NULL,
	[UserId] uniqueidentifier NOT NULL, 
    CONSTRAINT [FK_UserPasswords_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
    CONSTRAINT [CK_UserPasswords_PasswordMinCheck] CHECK (LEN([Hash]) >= 8)
)

GO

CREATE UNIQUE INDEX [IX_UserPasswords_Id] ON [dbo].[UserPasswords] ([Id])

