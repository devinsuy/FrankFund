---ACCOUNTS DDL---
CREATE TABLE FrankFund.Accounts (
	AccountID	 	INT64	NOT NULL,
	AccountUsername	STRING	NOT NULL,
	EmailAddress	STRING	NOT NULL,
	Password		STRING	NOT NULL,
	FacebookID		DECIMAL,		
	GoogleID		DECIMAL		
);
---END DDL---