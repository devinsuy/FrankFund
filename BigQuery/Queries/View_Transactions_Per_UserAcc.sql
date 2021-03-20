SELECT a.accountID, t.TID, t.TransactionName, t.amount, t.DateTransactionMade, t.TransactionCategory
FROM FrankFund.Transactions t
INNER JOIN FrankFund.Accounts a
ON t.AccountID = a.AccountID
ORDER BY a.AccountID, TransactionCategory
