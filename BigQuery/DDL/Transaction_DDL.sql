---TRANSACTION DDL---
CREATE TABLE FrankFund.Transactions (
	TID 					INT64 		NOT NULL,
	AccountID				STRING 		NOT NULL,
	SGID					INT64,
	TransactionName 		STRING		NOT NULL,
	Amount					DECIMAL		NOT NULL,
	DateTransactionMade		DATE		NOT NULL,
	DateTransactionEntered	DATE		NOT NULL,
	IsExpense				BOOL		NOT NULL,
	TransactionCategory		STRING		NOT NULL,
);
---END DDL---