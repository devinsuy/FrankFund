from datetime import datetime, timedelta
import requests
import random
import string
import time


periods = ["Daily", "Weekly", "BiWeekly", "Monthly", "BiMonthly"]

def post(data: dict):
    url = 'https://frankfund.appspot.com/api/SavingsGoal/custom&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0'
    res = requests.post(url, json=data, headers={'Content-type':'application/json'})
    if(res.status_code != requests.codes.ok):
        print("FAILED:", res.content)
    else:
        print("SUCCESS, posted:", data)

def getRandName() -> str:
    return ''.join(random.choice(string.ascii_lowercase) for i in range(random.randrange(6, 10)))

def getRandAmt() -> float:
    return round(random.uniform(100, 1500), 2)

def getRandStartDate() -> datetime:
    end = datetime(2021, 4, 27, 00, 00, 00)
    start = end - timedelta(days=200)
    return str((end - (end - start) * random.random()).date())

def getRandEndDate() -> datetime:
    today = datetime(2021, 5, 27, 00, 00, 00)
    end = today + timedelta(days=180)
    return str((today + (end - today) * random.random()).date())

def getRandPeriod() -> str:
    return random.choice(periods)

def getRandGoal(ACC_ID: int) -> dict:
    return {
        "Name" : getRandName(),             # Transactions that are expenses are not linked to any savings goal
        "AccountID" : ACC_ID,
        "GoalAmt" : getRandAmt(),
        "Period" : getRandPeriod(),
        "StartDate" : getRandStartDate(),
        "EndDate": getRandEndDate(),
    }

def createGoals(ACC_ID: int, num_goals: int):
    for i in range(num_goals):
        print("[" + str(i) + "]")
        post(getRandGoal(ACC_ID))
        time.sleep(1.5)

if __name__ == "__main__":
    ACC_ID = 12                                                 # User account to post transactions on to
    num_goals = 10
    createGoals(ACC_ID, num_goals)