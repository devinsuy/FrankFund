SELECT a.accountID, t.TransactionCategory, SUM(t.amount) AS CategoryTotal
FROM FrankFund.Transactions t
INNER JOIN FrankFund.Accounts a
ON t.AccountID = a.AccountID
GROUP BY a.AccountID, TransactionCategory
ORDER BY accountID ASC