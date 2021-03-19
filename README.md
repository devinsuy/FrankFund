# FrankFund REST API

## Overview
REST data endpoints using standard HTTP Requests for **GET/POST/DELETE/PUT** methods.


### Jump To API Documentation
[Savings Goals](#savingsgoals)

[User Accounts](#useraccounts)

[Transactions](#transactions)

[Receipts](#receipts)

---

# Authentication
HTTP requests must be in the form:

```https://frankfund.appspot.com/api/...&apikey={apikey}```

- The trailing part of the endpoint should be the literal "&apikey="
followed immediately by your issued developer API key

- You can find your developer **apikey** in the Github repo:
    /Credentials/Auth{YourName}.json

- Your corresponding apikey is labeled **"private_key_id"** in your Auth.json file.

---

# Error Codes
- Improperly authenticated requests will return a **HTTP 401 Unauthorized**.

- Improperly formated requests or improper data will return a **HTTP 400 Bad Request**.

- Retrieving data that returns 0 rows in BigQuery will return a **HTTP 204 No Content**.

- POST requests to create an object that already exists with the given id will return a **HTTP 409 Conflict**.

- PATCH requests to update an object that does not exist will return a **HTTP 404 Not Found**.

---

# Active REST Endpoints:

# SavingsGoals

```[GET] https://frankfund.appspot.com/api/SGID={SGID}&apikey={apikey})```

Returns JSON data of the SavingsGoal with the given SGID

**Example Request:** ```HTTP GET https://frankfund.appspot.com/api/SGID=1&apikey=c55f8d138f6ccfd43612b15c98706943e1f4bea3```

---

# UserAccounts

```[GET] https://frankfund.appspot.com/api/accID={accID}&apikey={apikey})```

Returns JSON data of the UserAccount with the given AccountID

**Example Request:** ```HTTP GET https://frankfund.appspot.com/api/accID=1&apikey=f2f1178729cb2e1c9188ed847066743c4e843a21```

---

# Transactions
```[GET] https://frankfund.appspot.com/api/TID={TID}&apikey={apikey})```

Returns JSON data of the Transaction with the given TransactionID

**Example Request:**
```HTTP GET https://frankfund.appspot.com/api/TID=1&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0"```

---

```[POST] https://frankfund.appspot.com/api/TID={TID}&apikey={apikey})```

Create a new Transaction with the specified TID and data payload. Request payload must specify **all attributes except DateTransactionEntered**

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

Delete a Transaction with the specified TID, has no effect if no Transaction exists with the given TID.

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

Otherwise if a Transaction with the given TID **already exists** request will update it. **Must specify ALL attributes** even if only changing some.
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

```[PATCH] https://frankfund.appspot.com/api/TID={TID}&apikey={apikey})```

Update an existing Transaction with the specified TID, data payload **does not need to specify all attributes**. Any number of attributes can be specified to update the Receipt.

**Example Requests:**
```HTTP PUT https://frankfund.appspot.com/api/TID=1&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0"```

```json
{
	"TransactionName": "Hulu",
}
```
PATCH request with this request body will update **just the TransactionName section** of the existing Transaction.

```json
{
	"Amount": "15.99",
}
```
PATCH request with this request body will update **just the Amount section** of the existing Transaction.


---

# Receipts

```[GET] https://frankfund.appspot.com/api/RID={RID}&apikey={apikey})```

Returns JSON data of the Receipt with the given ReceiptID

**Example Request:** 
```HTTP GET https://frankfund.appspot.com/api/RID=1&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0"```

---

```[POST] https://frankfund.appspot.com/api/RID={RID}&apikey={apikey})```

Create a new Receipt with the specified RID and data payload. Request payload must specify **ALL attributes** to create a new Receipt.

**Example Request:**
```HTTP POST https://frankfund.appspot.com/api/RID=1&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0"```

```json
{
	"TID": 1,
	"ImgURL": "hellothisisatest.png",
	"PurchaseDate": "2021-03-16",
	"Notes": "short note"
}
```
---

```[DELETE] https://frankfund.appspot.com/api/TID={TID}&apikey={apikey})```

Delete a Receipt with the specified RID, has no effect if no Receipt exists with the given RID.

**Example Request:**
```HTTP DELETE https://frankfund.appspot.com/api/RID=1&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0"```

---

```[POST] https://frankfund.appspot.com/api/RID={RID}&apikey={apikey})```

Update or create a new Receipt with the specified RID and data payload. Request payload must specify **ALL attributes** to create or update the Receipt.

**Example Request:**
```HTTP PUT https://frankfund.appspot.com/api/RID=1&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0"```

```json
{
	"TID": 1,
	"ImgURL": "hellothisisatest.png",
	"PurchaseDate": "2021-03-16",
	"Notes": "short note"
}
```

If a Receipt with the given RID **does not exist**, request will create one. Otherwise if a Receipt with the given RID **already exists** request will update it.

---

```[PATCH] https://frankfund.appspot.com/api/RID={RID}&apikey={apikey})```

Update an existing Receipt with the specified RID, data payload **does not need to specify all attributes**. Any number of attributes can be specified to update the Receipt.

**Example Requests:**
```HTTP PUT https://frankfund.appspot.com/api/RID=1&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0"```

```json
{
	"ImgURL": "newimage.png",
}
```
PATCH request with this request body will update **just the ImgURL** of the existing receipt.

```json
{
	"Notes": "New updated note",
}
```
PATCH request with this request body will update **just the Notes section** of the existing receipt.
