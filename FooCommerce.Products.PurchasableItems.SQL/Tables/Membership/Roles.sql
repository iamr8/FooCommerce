﻿CREATE TABLE [dbo].[Roles]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
    [IsDeleted] BIT DEFAULT 0 NOT NULL,
    [IsHidden] BIT DEFAULT 0 NOT NULL,
	[Type] TINYINT DEFAULT 0 NOT NULL,
)

GO

CREATE UNIQUE INDEX [IX_Roles_Id] ON [dbo].[Roles] ([Id])
