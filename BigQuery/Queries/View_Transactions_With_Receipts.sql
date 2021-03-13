SELECT * FROM FrankFund.Transactions f
INNER JOIN FrankFund.Receipts r
ON f.TID = r.TID
ORDER BY DateTransactionMade ASC