# FrankFund REST API

## Overview
REST data endpoints using standard HTTP Requests for **GET/POST/DELETE/PUT** methods.


### Jump To API Documentation
[Savings Goals](###savingsgoals)
[User Accounts](###useraccounts)
[Transactions](###transactions)
[Receipts](###receipts)

---

## Authentication
HTTP requests must be in the form:

```https://frankfund.appspot.com/api/...&apikey={apikey}```

The trailing part of the endpoint should be the literal "&apikey="
followed immediately by your issued developer API key

You can find your developer **apikey** in the Github repo:
    /Credentials/Auth{YourName}.json

Your corresponding apikey is labeled **"private_key_id"** in your Auth.json file.

---

## Error Codes
Improperly authenticated requests will return a **HTTP 401 Unauthorized**.

Improperly formated requests or improper data will return a **HTTP 400 Bad Request**.

Retrieving data that returns 0 rows in BigQuery will return a **HTTP 204 No Content**.

POST requests to create an object that already exists with the given id will return a **HTTP 409 Conflict**.

PATCH requests to update an object that does not exist will return a **HTTP 404 Not Found**.

---

## Active Endpoints

### SavingsGoals

```[GET] https://frankfund.appspot.com/api/SGID={SGID}&apikey={apikey})```

Returns JSON data of the SavingsGoal with the given SGID

**Example Request:** ```HTTP GET https://frankfund.appspot.com/api/SGID=1&apikey=c55f8d138f6ccfd43612b15c98706943e1f4bea3```

---

### UserAccounts


```[GET] https://frankfund.appspot.com/api/accID={accID}&apikey={apikey})```

Returns JSON data of the UserAccount with the given AccountID

**Example Request:** ```HTTP GET https://frankfund.appspot.com/api/accID=1&apikey=f2f1178729cb2e1c9188ed847066743c4e843a21```

---

### Transactions
```[GET] https://frankfund.appspot.com/api/TID={TID}&apikey={apikey})```

Returns JSON data of the Transaction with the given TransactionID

**Example Request:**
```HTTP GET https://frankfund.appspot.com/api/TID=1&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0"```

---

```[POST] https://frankfund.appspot.com/api/TID={TID}&apikey={apikey})```

Create a new Transaction with the specified TID and data payload. Response payload must specify **all attributes except DateTransactionEntered** or will return a **HTTP 400 Bad Request**.

**Example Request:**
```HTTP POST https://frankfund.appspot.com/api/TID=1&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0"```
```json
{
	"TID": 40,
	"AccountID": 1,
	"SGID": 2,
	"TransactionName": "Netflix",
	"Amount": 9.99,
	"DateTransactionMade": "2020-12-14",
	"IsExpense": true,
	"TransactionCategory": "Entertainment"
}
```

---

```[DELETE] https://frankfund.appspot.com/api/TID={TID}&apikey={apikey})```

Delete a Transaction with the specified TID, has no effect no Transaction exists with the given TID.

**Example Request:**
```HTTP DELETE https://frankfund.appspot.com/api/TID=1&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0"```

---

```[PUT] https://frankfund.appspot.com/api/TID={TID}&apikey={apikey})```

Update or create a new Transaction with the specified TID and data payload.

**Example Request:**
```HTTP PUT https://frankfund.appspot.com/api/TID=1&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0"```

If a Transaction with the given TID **does not exist**, request will create one. Must specify **all attributes except DateTransactionEntered**.
```json
{
	"TID": 40,
	"AccountID": 1,
	"SGID": 2,
	"TransactionName": "Netflix",
	"Amount": 9.99,
	"DateTransactionMade": "2020-12-14",
	"IsExpense": true,
	"TransactionCategory": "Entertainment"
}
```

Otherwise if a Transaction with the given TID **already exists** request will update it. **Must specify ALL attributes**.
```json
{
	"TID": 40,
	"AccountID": 1,
	"SGID": 2,
	"TransactionName": "Netflix",
	"Amount": 9.99,
	"DateTransactionMade": "2020-12-14",
	"DateTransactionEntered": "2020-12-17",
	"IsExpense": true,
	"TransactionCategory": "Entertainment"
}
```

---

### Receipts

```[GET] https://frankfund.appspot.com/api/TID={TID}&apikey={apikey})```

Returns JSON data of the Receipt with the given ReceiptID

**Example Request:** ```HTTP GET https://frankfund.appspot.com/api/RID=1&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0"```

---


