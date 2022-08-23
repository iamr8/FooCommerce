CREATE TABLE [dbo].[Notifications]
(
    [Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
    [Action] SMALLINT NOT NULL,
)

GO

CREATE UNIQUE INDEX [IX_Notifications_Id] ON [dbo].[Notifications] ([Id])
