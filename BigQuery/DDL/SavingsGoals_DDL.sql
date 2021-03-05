CREATE TABLE FrankFund.SavingsGoals (
    SGID            INT64         NOT NULL,
    Name            STRING        NOT NULL,
    GoalAmt         DECIMAL       NOT NULL,
    ContrAmt        DECIMAL       NOT NULL,
    Period          String        NOT NULL,
    NumPeriods      INT64         NOT NULL,
    StartDate       DATE          NOT NULL,
    EndDate         DATE          NOT NULL
);