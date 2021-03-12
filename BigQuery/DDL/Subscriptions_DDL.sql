CREATE TABLE FrankFund.Subscriptions(
    SID 			INT64 		NOT NULL,
    RID 			INT64,
    PurchaseDate 	DATE 		NOT NULL,
    Notes 			STRING,
    Amount 			DECIMAL 	NOT NULL,
    RenewFrequency  STRING      NOT NULL
)