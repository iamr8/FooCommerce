CREATE TABLE [dbo].[UserCommunications]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
	[Type] BIT NOT NULL DEFAULT 0,
	[Value] NVARCHAR(150) NOT NULL,
	[IsVerified] BIT NOT NULL DEFAULT 0,
	[IsOpenId] BIT NOT NULL DEFAULT 0,
	[OpenIdProvider] BIT NULL,
	[OpenIdScope] NVARCHAR(MAX) NULL,
	[UserId] uniqueidentifier NOT NULL, 
    CONSTRAINT [FK_UserCommunications_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]), 
    CONSTRAINT [CK_UserCommunications_ValueMinLength] CHECK (LEN([Value]) >= 6),
)

GO

CREATE UNIQUE INDEX [IX_UserCommunications_Id] ON [dbo].[UserCommunications] ([Id])

GO


CREATE FULLTEXT INDEX ON [dbo].[UserCommunications] ([Value]) KEY INDEX [IX_UserCommunications_Id] ON [FullTextCatalog] WITH CHANGE_TRACKING AUTO
