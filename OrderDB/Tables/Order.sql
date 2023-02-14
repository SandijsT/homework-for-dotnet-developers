CREATE TABLE [dbo].[Order]
(
	[OrderId]                  INT             PRIMARY KEY IDENTITY (1, 1) NOT NULL,
	[CustomerId]               INT             NOT NULL,
	[OrderAmount]              INT             NOT NULL,
	[ExpectedDeliveryDate]     DATETIME2 (7)   NOT NULL,
	[Price]                    DECIMAL (19, 2) NOT NULL,
)
