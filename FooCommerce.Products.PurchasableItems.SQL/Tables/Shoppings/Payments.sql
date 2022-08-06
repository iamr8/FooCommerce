CREATE TABLE [dbo].[Payments]
(
    [Id] uniqueidentifier ROWGUIDCOL PRIMARY KEY DEFAULT NEWSEQUENTIALID(),
    [Created] DATETIME2 DEFAULT GETUTCDATE() NOT NULL, 
    [RowVersion] ROWVERSION, 
	[Amount] MONEY DEFAULT 0 NOT NULL,
    [Currency] TINYINT NOT NULL,
	[Gateway] TINYINT NOT NULL,
	[Method] TINYINT NOT NULL,
	[Status] TINYINT DEFAULT 0 NOT NULL,
	[Details] NVARCHAR(MAX) NULL,
    [Reference] NVARCHAR(MAX) NULL,
    [TransactionId] NVARCHAR(MAX) NULL,
	[CheckoutId] UNIQUEIDENTIFIER NULL, 
    CONSTRAINT [FK_Payments_Checkouts] FOREIGN KEY ([CheckoutId]) REFERENCES [Checkouts]([Id]), 
    CONSTRAINT [CK_Payments_AmountGreaterThanZero] CHECK ([Amount] >= 0),
)

GO

CREATE UNIQUE INDEX [IX_Payments_Id] ON [dbo].[Payments] ([Id])

GO

CREATE TRIGGER [dbo].[Trigger_PaymentsUpdateCheckouts]
    ON [dbo].[Payments]
    AFTER UPDATE
    AS
    BEGIN
        SET NoCount ON

        IF (UPDATE(Status))
		BEGIN
	        UPDATE [Checkouts]
            SET [IsSuccessful] = (CASE WHEN [INSERTED].Status = 1 THEN 1 ELSE 0 END)
			FROM [Checkouts]
            JOIN INSERTED
                ON [Checkouts].[Id] = [INSERTED].[CheckoutId]
		END
    END