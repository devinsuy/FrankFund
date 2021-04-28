from datetime import datetime, timedelta
import requests
import random
import string
import time


categories = [
    'Entertainment', 'Restaurants', 'Transportation', 'HomeAndUtilities', 'Education',
    'Insurance', 'Health', 'Groceries', 'Shopping', 'Uncategorized'
]

def post(data: dict):
    url = 'https://frankfund.appspot.com/api/Transaction&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0'
    res = requests.post(url, json=data, headers={'Content-type':'application/json'})
    if(res.status_code != requests.codes.ok):
        print("FAILED:", res.content)
    else:
        print("SUCCESS, posted:", data)

def getRandName() -> str:
    return ''.join(random.choice(string.ascii_lowercase) for i in range(random.randrange(6, 10)))

def getRandAmt() -> float:
    return round(random.uniform(5, 150), 2)

def getRandDate() -> datetime:
    min_year = 2020
    max_year = 2021
    start = datetime(min_year, 4, 1, 00, 00, 00)
    years = max_year - min_year
    end = start + timedelta(days=365+20 * years)
    return str((start + (end - start) * random.random()).date())

def getRandCategory() -> str:
    return random.choice(categories)

def getRandTransaction(ACC_ID) -> dict:
    return {
        "SGID" : -1,             # Transactions that are expenses are not linked to any savings goal
        "AccountID" : ACC_ID,
        "TransactionName" : getRandName(),
        "Amount" : getRandAmt(),
        "DateTransactionMade" : getRandDate(),
        "IsExpense": True,
        "TransactionCategory" : getRandCategory()
    }


def createTransactions(ACC_ID: int, num_transactions: int):
    for i in range(num_transactions):
        print("[" + str(i) + "]")
        post(getRandTransaction())
        time.sleep(1.5)

if __name__ == "__main__":
    ACC_ID = 12                                                 # User account to post transactions on to
    # num_transactions = 100
    # createTransactions(ACC_ID, num_transactions)
    post(getRandTransaction(ACC_ID))