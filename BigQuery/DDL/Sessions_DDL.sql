---SESSIONS DDL---
CREATE TABLE FrankFund.Sessions (
	SessionID	 INT64	NOT NULL,
	JWTToken	STRING	NOT NULL,
	AccountUsername	STRING	NOT NULL,
	DateIssued	STRING NOT NULL
);
---END DDL---