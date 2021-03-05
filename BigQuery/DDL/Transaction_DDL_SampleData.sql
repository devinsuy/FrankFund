---TRANSACTION DDL---
CREATE TABLE Transactions (
	TransactionName 	VARCHAR		NOT NULL
	Amount			DECIMAL		NOT NULL
	DateTransactionMade	DATETIME	NOT NULL
	TransactionType		ENUM		NOT NULL
	Transaction Category	ENUM		NOT NULL
	DateTransactionEntered	DATETIME	NOT NULL
);
---END DDL---

---POPULATE TABLE---
INSERT INTO Transactions *
VALUES 
();