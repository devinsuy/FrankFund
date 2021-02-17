-- SavingsGoal DDL
CREATE TABLE SavingsGoals (
    Name            VARCHAR(255)    NOT NULL,
    GoalAmt         DECIMAL(8,2),
    ContrAmt        DECIMAL(8,2),
    PeriodCode      INT(255),
    EndDate         DATE            NOT NULL
);

