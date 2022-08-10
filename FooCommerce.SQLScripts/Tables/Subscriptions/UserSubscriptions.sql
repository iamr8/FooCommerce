CREATE TABLE [dbo].[UserSubscriptions]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
	[IsSuspended] BIT DEFAULT 0 NOT NULL,
	[IsTrial] BIT DEFAULT 0 NOT NULL,
	[IsCancelled] BIT DEFAULT 0 NOT NULL,
	[ValidUntil] DATETIME2 NULL,
	[CancellationReason] TINYINT NULL,
	[CancellationMore] NVARCHAR(MAX) NULL,
	[DeactivationReason] TINYINT NULL,
	[DeactivationMore] NVARCHAR(MAX) NULL,
    [PlanId] uniqueidentifier NOT NULL,
	[UserId] uniqueidentifier NOT NULL, 
    CONSTRAINT [FK_UserSubscriptions_PricingPlans] FOREIGN KEY ([PlanId]) REFERENCES [PricingPlans]([Id]), 
    CONSTRAINT [FK_UserSubscriptions_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
)

GO

CREATE UNIQUE INDEX [IX_UserSubscriptions_Id] ON [dbo].[UserSubscriptions] ([Id])

GO

CREATE FULLTEXT INDEX ON [dbo].[UserSubscriptions] ([CancellationMore]) KEY INDEX [IX_UserSubscriptions_Id] ON [FullTextCatalog] WITH CHANGE_TRACKING AUTO

GO