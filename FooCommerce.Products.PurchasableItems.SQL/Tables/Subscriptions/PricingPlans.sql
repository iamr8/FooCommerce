CREATE TABLE [dbo].[PricingPlans]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
    [IsActive] BIT DEFAULT 0 NOT NULL,
	[Type] TINYINT NOT NULL,
    [Price] MONEY NOT NULL,
    [AcceptedRoles] NVARCHAR(300) NOT NULL,
    [Order] TINYINT NOT NULL,
)

GO

CREATE UNIQUE INDEX [IX_PricingPlans_Id] ON [dbo].[PricingPlans] ([Id])

