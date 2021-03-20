SELECT a.accountID, t.TransactionCategory, SUM(t.amount) AS CategoryTotal, 
total.TotalExpenses, ROUND(100 * (SUM(t.amount) / total.TotalExpenses), 2) AS CategoryPercentage
FROM FrankFund.Transactions t
INNER JOIN FrankFund.Accounts a
ON t.AccountID = a.AccountID

-- Subquery to determine each accounts total expenses
INNER JOIN
  (SELECT a.accountID, SUM(t.amount) AS TotalExpenses
  FROM FrankFund.Transactions t
  INNER JOIN FrankFund.Accounts a
  ON t.AccountID = a.AccountID
  GROUP BY a.accountID) AS total
ON total.accountID = a.accountID

-- Display breakdown of spending per category for each account, from highest spending category to lowest 
GROUP BY a.AccountID, TransactionCategory, total.TotalExpenses
ORDER BY accountID ASC, ROUND(100 * (SUM(t.amount) / total.TotalExpenses), 2) DESC