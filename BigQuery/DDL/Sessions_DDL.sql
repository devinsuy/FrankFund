---SESSIONS DDL---
CREATE TABLE FrankFund.Sessions (
	SessionID	 INT64	NOT NULL,
	JWTToken	STRING	NOT NULL,
	AccountUsername	STRING	NOT NULL,
	DateIssued	DATE	NOT NULL
);
---END DDL---