SELECT * FROM FrankFund.Transactions f
INNER JOIN FrankFund.Accounts a
ON f.AccountID = a.AccountID
ORDER BY a.AccountID ASC