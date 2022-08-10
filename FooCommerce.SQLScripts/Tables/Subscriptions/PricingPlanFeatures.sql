CREATE TABLE [dbo].[PricingPlanFeatures]
(
	[Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
    [IsDeleted] BIT DEFAULT 0 NOT NULL,
	[IsActive] BIT DEFAULT 0 NOT NULL,
	[Type] SMALLINT DEFAULT 0 NOT NULL,
	[PlanId] uniqueidentifier NOT NULL, 
    CONSTRAINT [FK_PricingPlanFeatures_PricingPlans] FOREIGN KEY ([PlanId]) REFERENCES [PricingPlans]([Id]),
)

GO

CREATE UNIQUE INDEX [IX_PricingPlanFeatures_Id] ON [dbo].[PricingPlanFeatures] ([Id])
