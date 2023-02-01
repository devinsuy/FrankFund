


# FrankFund REST API

## Overview
REST API data endpoints using standard HTTP Requests for **GET/POST/DELETE/PUT/PATCH** methods. 


### Jump To API Documentation
[Savings Goals](#savingsgoals)

[User Accounts](#useraccounts)

[Transactions](#transactions)

[Receipts](#receipts)

[User Transactions](#usertransactions)

[User SavingsGoals](#usersavingsgoals)

[User Receipts](#userreceipts)

[User Subscriptions](#usersubscriptions)


---

# Authentication
HTTP requests must be in the form:

```https://frankfund.appspot.com/api/...&apikey={apikey}```

- The trailing part of the endpoint should be the literal "&apikey="
followed immediately by your issued developer API key

- Your apikey is labeled **"private_key_id"** in your Auth.json file.

---

# Error Codes
- Improperly authenticated requests will return a **HTTP 401 Unauthorized**.

- Improperly formated requests or improper data will return a **HTTP 400 Bad Request**.

- Retrieving data that returns 0 rows in BigQuery will return a **HTTP 204 No Content**.

- POST requests to create an object that already exists with the given id will return a **HTTP 409 Conflict**.

- PATCH requests to update an object that does not exist will return a **HTTP 404 Not Found**.

- Non GET/DELETE requests that do not specify Content-Type header will return a **HTTP 415 Unsupported Media Type**.

---

# Active REST Endpoints:

# SavingsGoals

```[GET] /api/SavingsGoal/SGID={SGID}&apikey={apikey}```

Serves JSON response of the SavingsGoal data with the given SavingsGoalID.  
Returns **HTTP 204 No Content** if no SavingsGoal exists with the given SGID.

**Example Request:** 
```HTTP GET https://frankfund.appspot.com/api/SavingsGoal/SGID=1&apikey={apikey}```

---

```[POST] /api/SavingsGoal&apikey={apikey}```

Creates a new SavingsGoal with the next available SGID and JSON request payload. 
Request returns **HTTP 200 OK** with a JSON containing all attributes of the newly created SavingsGoal.


**Example Requests:** 
```HTTP POST https://frankfund.appspot.com/api/SavingsGoal&apikey={apikey}```
```json
{
	"Name": "Tuition",
	"AccountID": 2,
	"GoalAmt": 3425,
	"Period": "Weekly",
	"EndDate": "2021-06-04"
}
```

Create a **SavingsGoal by date**, system will dynamically calculate { ContrAmt, NumPeriods, StartDate, EndDate } parameters.


---

```[POST] /api/SavingsGoal/SGID={SGID}&apikey={apikey}```

Creates a new SavingsGoal with the given SavingsGoalID and JSON request payload. 

Returns **HTTP 409 Conflict** if a SavingsGoal already exists with the given SGID. 

Request returns **HTTP 200 OK** with a JSON containing all attributes of the newly created SavingsGoal.

**Example Requests:** 
```HTTP POST https://frankfund.appspot.com/api/SavingsGoal/SGID=1&apikey={apikey}```
```json
{
	"Name": "Tuition",
	"AccountID": 2,
	"GoalAmt": 3425,
	"Period": "Weekly",
	"EndDate": "2021-06-04"
}
```

Create a **SavingsGoal by date**, system will dynamically calculate { ContrAmt, NumPeriods, StartDate, EndDate } parameters.

```json
{
	"Name": "Tuition",
	"AccountID": 2,
	"GoalAmt": 3425,
	"Period": "Weekly",
	"ContrAmt": 325
}
```

Create a **SavingsGoal by allotted contribution**, system will dynamically calculate {ContrAmt, NumPeriods, StartDate, EndDate } parameters.  

---

```[DELETE] /api/SavingsGoal/SGID={SGID}&apikey={apikey}```

Delete the SavingsGoal with the given SGID, has no effect it no SavingsGoal exists with the given SGID.

**Example Request:** 
```HTTP DELETE https://frankfund.appspot.com/api/SavingsGoal/SGID=1&apikey={apikey}```

---

```[PATCH] /api/SavingsGoal/SGID={SGID}&apikey={apikey}```

For updating parameters **Name, Period, EndDate**:
- Updates a **single attribute** of the SavingsGoal with the given SGID and dynamically recalculate the remaining parameters to reflect the change applies. 
- Returns **HTTP 400 Bad Request** if JSON request payload contains more than a single parameter to update 

For updating parameter **GoalAmt**:
- JSON request body must also specify a boolean parameter **ExtendDate** with the value:
   - **True** if the system should shorten/lengthen the **EndDate** of the SavingsGoal to reflect the change in the GoalAmt
   - **False** if the system should instead increase/decrease the **NumPeriods** toreflect the change in the GoalAmt

For updating parameter **ContrAmt**:
- JSON request body MAY **optionally** specify the **Period** parameter to update ContrAmt and Period simultaneously.
- List of supported periods: { Daily, Weekly, BiWeekly, Monthly, BiMonthly }

Returns **HTTP 404 Not Found** if no SavingsGoal exists with the given SGID.

Request returns **HTTP 200 OK** with a JSON containing all attributes of the newly updated SavingsGoal.

**Example Requests:** 
```HTTP PATCH https://frankfund.appspot.com/api/SavingsGoal/SGID=1&apikey={apikey}```
```json
{
	"Name" : "Tuition"
}
```
PATCH request with the above JSON request body will update the associated Name attribute of the SavingsGoal.
```json
{
	"EndDate" : "2021-07-04"
}
```
PATCH request with the above JSON request body will update the EndDate attribute of the SavingsGoal, causing the **NumPeriods and contrAmt** per period to dynamically recalculate.
```json
{
	"GoalAmt": 4425,
	"ExtendDate": true
}
```
PATCH request with the above JSON request body will update the GoalAmt without affecting payment amount by modifying the SavingsGoal's EndDate and NumPeriods.
```json
{
    	"ContrAmt": 425,
		"Period": "BiWeekly",
}
```
PATCH request with the above JSON request body will update the ContrAmt per BiWeekly period to $425.00, causing **NumPeriods and EndDate** to dynamically recalculate.  

---

# UserAccounts
```[POST] /api/account/create&apikey={apikey}```

Create a new UserAccount with the specified JSON request payload. Request payload must specify the attributes: **{ AccountUsername, EmailAddress, Password }**, otherwise returns **HTTP 400 Bad Request**. AccountID is automatically assigned as the next available ID.

Returns **HTTP 409 Conflict** if:
- Account already exists with the given Username
- Email address is invalid/malformed, or an account already exists with the given Email
- Password fails to meet security requirements

**Example Request:**
```HTTP POST https://frankfund.appspot.com/api/account/create&apikey={apikey}```
```json
{
	"AccountUsername" : "testUname",
	"EmailAddress" : "testEmail2@gmail.com",
	"Password" : "tesp@ssw0rd2"
}
```
---

```[GET] /api/account/accID={accID}&apikey={apikey}```

```[GET] /api/account/user={user}&apikey={apikey}```

```[GET] /api/account/email={email}&apikey={apikey}```


Serves JSON response of the associated UserAccount data.
Returns **HTTP 204 No Content** if no UserAccount exists with the given accID.

**Example Request:** 
```HTTP GET https://frankfund.appspot.com/api/account/accID=1&apikey={apikey}```

```HTTP GET https://frankfund.appspot.com/api/account/user=Devin&apikey={apikey}```

```HTTP GET https://frankfund.appspot.com/api/account/email=DevinSuy@gmail.com&apikey={apikey}```

---

```[DELETE] /api/account/accID={accID}&apikey={apikey}```

```[DELETE] /api/account/user={user}&apikey={apikey}```

```[DELETE] /api/account/email={email}&apikey={apikey}```

Delete the UserAccount with the given identifier, has no effect if no account exists with the given accID.

**Example Request:** 
```HTTP DELETE https://frankfund.appspot.com/api/account/accID=1&apikey={apikey}```

```HTTP DELETE https://frankfund.appspot.com/api/account/user=DevinSuy&apikey={apikey}```

```HTTP DELETE https://frankfund.appspot.com/api/account/email=DevinSuy@gmail.com&apikey={apikey}```

---

```[PATCH] /api/account/accID={accID}&apikey={apikey}```

Update an existing User Account with the specified AccountID, data payload **does not need to specify all attributes**. Any number of attributes can be specified simultaneously to update the User Account.

Returns **HTTP 404 Not Found** if no User Account exists with the given accID.

Request returns **HTTP 200 OK** with a JSON containing all attributes of the newly updated UserAccount.

**Example Requests:**
```https://frankfund.appspot.com/api/account/accID=5&apikey={apikey}```

```json
{
    "AccountUsername": "testing",
    "EmailAddress": "testing@gmail.com",
    "Password": "Password2"
}
```
PATCH request with this request body will update all attributes of the existing User Account.

```json
{
    "AccountUsername": "newUsername"
}
```
PATCH request with this request body will update just the AccountUsername of the existing User Account.

---

```[PATCH] /api/account/user={user}&apikey={apikey}```

Update an existing User Account with the specified AccountID, data payload **does not need to specify all attributes**. Any number of attributes can be specified simultaneously to update the User Account.

Returns **HTTP 404 Not Found** if no User Account exists with the given accID.

Request returns **HTTP 200 OK** with a JSON containing all attributes of the newly updated UserAccount.

**Example Requests:**
```https://frankfund.appspot.com/api/account/user=DevinSuy&apikey={apikey}```


```json
{
    "EmailAddress": "newEmail@gmail.com"
}
```
PATCH request with this request body will update just the EmailAddress of the existing User Account.

```json
{
    "Password": "newPassword"
}
```
PATCH request with this request body will update just the Password of the existing User Account.


# Transactions
```[GET] /api/Transaction/TID={TID}&apikey={apikey}```

Serves JSON response of the Transaction data with the given TransactionID.
Returns **HTTP 204 No Content** if no Transaction exists with the given TID.

**Example Request:**
```HTTP GET https://frankfund.appspot.com/api/Transaction/TID=1&apikey={apikey}```

---

```[POST] /api/Transaction&apikey={apikey}```

Create a new Transaction with the next available TID and the JSON request payload. Request payload must specify **all attributes except DateTransactionEntered**, otherwise returns **HTTP 400 Bad Request**.

Request returns **HTTP 200 OK** with a JSON containing all attributes of the newly updated Transaction.

**Example Request:**
```HTTP POST https://frankfund.appspot.com/api/Transaction&apikey={apikey}```
```json
{
	"SGID": 2,
	"AccountID": 1,
	"TransactionName": "Netflix",
	"Amount": 9.99,
	"DateTransactionMade": "2020-12-14",
	"IsExpense": true,
	"TransactionCategory": "Entertainment"
}
```

---


```[POST] /api/Transaction/TID={TID}&apikey={apikey}```

Create a new Transaction with the specified TransactionID and JSON request payload. Request payload must specify **all attributes except DateTransactionEntered**, otherwise returns **HTTP 400 Bad Request**.

Returns **HTTP 409 Conflict** if a Transaction already exists with the given TID

Request returns **HTTP 200 OK** with a JSON containing all attributes of the newly updated Transaction.


**Example Request:**
```HTTP POST https://frankfund.appspot.com/api/Transaction/TID=1&apikey={apikey}```
```json
{
	"SGID": 2,
	"AccountID": 1,
	"TransactionName": "Netflix",
	"Amount": 9.99,
	"DateTransactionMade": "2020-12-14",
	"IsExpense": true,
	"TransactionCategory": "Entertainment"
}
```

---

```[DELETE] /api/Transaction/TID={TID}&apikey={apikey}```

Delete a Transaction with the specified TramsactopmID, has no effect if no Transaction exists with the given TID.

**Example Request:**
```HTTP DELETE https://frankfund.appspot.com/api/Transaction/TID=1&apikey={apikey}```

---

```[PUT] /api/Transaction/TID={TID}&apikey={apikey}```

Update or create a new Transaction with the specified TID and data payload.

Request returns **HTTP 200 OK** with a JSON containing all attributes of the newly updated or created Transaction.

**Example Request:**
```HTTP PUT https://frankfund.appspot.com/api/Transaction/TID=1&apikey={apikey}```
```json
{
	"SGID": 2,
	"AccountID": 1,
	"TransactionName": "Netflix",
	"Amount": 9.99,
	"DateTransactionMade": "2020-12-14",
	"IsExpense": true,
	"TransactionCategory": "Entertainment"
}
```
If a Transaction with the given TID **does not exist**, request will create one. Must specify **all attributes except DateTransactionEntered** to create the Transaction, otherwise returns **HTTP 400 Bad Request**.

```json
{
	"SGID": 2,
	"AccountID": 1,
	"TransactionName": "Netflix",
	"Amount": 9.99,
	"DateTransactionMade": "2020-12-14",
	"DateTransactionEntered": "2020-12-17",
	"IsExpense": true,
	"TransactionCategory": "Entertainment"
}
```
Otherwise if a Transaction with the given TID **already exists** request will update it. HTTP PUT protocol specifies that you **must specify ALL attributes** even if only changing some, otherwise returns **HTTP 400 Bad Request**.

---

```[PATCH] /api/Transaction/TID={TID}&apikey={apikey}```

Update an existing Transaction with the specified TransactionID, data payload **does not need to specify all attributes**. Any number of attributes can be specified simultaneously to update the Transaction.

Returns **HTTP 404 Not Found** if no Transaction exists with the given TID.

Request returns **HTTP 200 OK** with a JSON containing all attributes of the newly updated Transaction.

**Example Requests:**
```HTTP PATCH https://frankfund.appspot.com/api/Transaction/TID=1&apikey={apikey}```

```json
{
	"TransactionName": "Hulu"
}
```
PATCH request with this request body will update **just the TransactionName attribute** of the existing Transaction.

```json
{
	"TransactionName": "Hulu",
	"Amount": "5.99"
}
```
PATCH request with this request body will update **both the TransactionName and Amount attributes** of the existing Transaction.

---

# Receipts

```[GET] /api/Receipt/RID={RID}&apikey={apikey}```

Serves JSON response of the Receipt data with the given ReceiptID
Returns **HTTP 204 No Content** if no Receipt exists with the given RID.

**Example Request:** 
```HTTP GET https://frankfund.appspot.com/api/Receipt/RID=1&apikey={apikey}```

---

```[POST] /api/Receipt&apikey={apikey}```

Create a new Receipt with the next available RID and JSON request payload. Request payload must specify **ALL attributes** to create a new Receipt, returns **HTTP 400 Bad Request** otherwise. 

Request returns **HTTP 200 OK** with a JSON containing all attributes of the newly updated Receipt.

**Example Request:**
```HTTP POST https://frankfund.appspot.com/api/Receipt&apikey={apikey}```

```json
{
	"TID": 1,
	"ImgURL": "hellothisisatest.png",
	"PurchaseDate": "2021-03-16",
	"Notes": "short note"
}
```

---

```[POST] /api/Receipt/RID={RID}&apikey={apikey}```

Create a new Receipt with the specified ReceiptID and JSON request payload. Request payload must specify **ALL attributes** to create a new Receipt, returns **HTTP 400 Bad Request** otherwise.

Returns **HTTP 409 Conflict** if a Receipt already exists with the given RID.

Request returns **HTTP 200 OK** with a JSON containing all attributes of the newly created Receipt.

**Example Request:**
```HTTP POST https://frankfund.appspot.com/api/Receipt/RID=1&apikey={apikey}```

```json
{
	"TID": 1,
	"ImgURL": "hellothisisatest.png",
	"PurchaseDate": "2021-03-16",
	"Notes": "short note"
}
```
---

```[DELETE] /api/Receipt/RID={TID}&apikey={apikey}```

Delete a Receipt with the specified RID, has no effect if no Receipt exists with the given RID.

**Example Request:**
```HTTP DELETE https://frankfund.appspot.com/api/Receipt/RID=1&apikey={apikey}```

---

```[PUT] /api/Receipt/RID={RID}&apikey={apikey}```

Update or create a new Receipt with the specified RID and data payload. Request payload must specify **ALL attributes** to create or update the Receipt.

**Example Request:**
```HTTP PUT https://frankfund.appspot.com/api/Receipt/RID=1&apikey={apikey}```

```json
{
	"TID": 1,
	"ImgURL": "hellothisisatest.png",
	"PurchaseDate": "2021-03-16",
	"Notes": "short note"
}
```

If a Receipt with the given RID **does not exist**, request will create one. Otherwise if a Receipt with the given RID **already exists** request will update it.

Request returns **HTTP 200 OK** with a JSON containing all attributes of the newly created or updated Receipt.

---

```[PATCH] /api/Receipt/RID={RID}&apikey={apikey}```

Update an existing Receipt with the specified ReceiptID, request data payload **does not need to specify all attributes**.  Any number of attributes can be specified and simultaneously updated for the Receipt.

Request returns **HTTP 200 OK** with a JSON containing all attributes of the  newly updated Receipt.

**Example Requests:**
```HTTP PATCH https://frankfund.appspot.com/api/Receipt/RID=1&apikey={apikey}```

```json
{
	"ImgURL": "newimage.png"
}
```
PATCH request with this request body will update **just the ImgURL parameter** of the existing receipt.

```json
{
	"ImgURL": "newimage.png",
	"Notes": "New updated note"
}
```
PATCH request with this request body will update **both the ImgURL and Notes parameters** of the existing receipt.

---


# UserTransactions

```[GET] /api/account/user={user}/Transactions&apikey={apikey}```

```[GET] /api/account/accID={accID}/Transactions&apikey={apikey}```

Serves JSON response of all the Transactions associated with the given UserAccount.
Returns **HTTP 204 No Content** if no Transactions exists with the given UserAccount or no UserAccount exists with the given Username.


**Example Requests:** 
```HTTP GET https://frankfund.appspot.com/api/account/user=DevinSuy/Transactions&apikey={apikey}```

```HTTP GET https://frankfund.appspot.com/api/account/accID=4/Transactions&apikey={apikey}```

---

```[GET] /api/account/user={user}/Transactions/Filter/ByCategory={category}&apikey={apikey}```

```[GET] /api/account/accID={accID}/Transactions/Filter/ByCategory={category}&apikey={apikey}```

Serves JSON response of all the Transactions associated with the given UserAccount filtered down to only those that are labeled with the given category. 

**Transaction Categories**: 
-  Entertainment
-  Restaurants
-  Transportation
-  HomeAndUtilities
-  Education 
-  Insurance 
-  Health 
-  Groceries
-  Deposits
-  Shopping 
- Uncategorized

Returns **HTTP 204 No Content** if no Transactions exists with the given UserAccount or no query results.

**Example Requests:** 
```HTTP GET https://frankfund.appspot.com/api/account/user=DevinSuy/Transactions/Filter/ByCategory=Entertainment&apikey={apikey}```

```HTTP GET https://frankfund.appspot.com/api/account/accID=4/Transactions/Filter/ByCategory=Education&apikey={apikey}```

---

```[GET] /api/account/user={user}/Transactions/Filter/ByTime/num={num}&periodCode={periodCode}&apikey={apikey}```

```[GET] /api/account/accID={accID}/Transactions/Filter/ByTime/num={num}&periodCode={periodCode}&apikey={apikey}```

Serves JSON response of all the Transactions associated with the given UserAccount filtered down to only those within the last **{ num } { periodCode }**. 

**Period Codes**:
- 0 : days
- 1 : weeks
- 2 : months
- 3 : years

**Example Requests:** 

```HTTP GET https://frankfund.appspot.com/api/account/user=KennethTran/Transactions/Filter/ByTime/num=75&periodCode=0&apikey={apikey}```

Filter user's transactions to those made in the **last 75 days**.

```HTTP GET https://frankfund.appspot.com/api/account/accID=4/Transactions/Filter/ByTime/num=4&periodCode=2&apikey={apikey}```

Filter user's transactions to those made in the **last 4 months**.

---

```[GET] /api/account/user={user}/Transactions/Sorted/ByCategory&apikey={apikey}```

```[GET] /api/account/accID={accID}/Transactions/Sorted/ByCategory&apikey={apikey}```

Retrieve **all** transactions associated with the UserAccount, **sorted by the TransactionCategory** attribute.

**Example Requests:** 

```HTTP GET https://frankfund.appspot.com/api/account/user=KennethTran/Transactions/Sorted/ByCategory&apikey={apikey}```


```HTTP GET https://frankfund.appspot.com/api/account/accID=4/Transactions/Sorted/ByCategory&apikey={apikey}```


---

# UserSavingsGoals

```[GET] /api/account/accID={accID}/SavingsGoals&apikey={apikey}```

Serves JSON response of all the SavingsGoals associated with the UserAccount with the given accID
Returns **HTTP 204 No Content** if no SavingsGoals exists with the given UserAccount or no UserAccount exists with the given accID.

**Example Request:** 
```HTTP GET https://frankfund.appspot.com/api/account/accID=2/SavingsGoals&apikey={apikey}```

---

```[GET] /api/account/user={user}/SavingsGoals&apikey={apikey}```

Serves JSON response of all the SavingsGoals associated with the UserAccount with the given Username
Returns **HTTP 204 No Content** if no SavingsGoals exists with the given UserAccount or no UserAccount exists with the given Username.

**Example Request:** 
```HTTP GET https://frankfund.appspot.com/api/account/user=DevinSuy/SavingsGoals&apikey={apikey}```

---

# UserReceipts

```[GET] /api/account/user={user}/Receipts&apikey={apikey}```

```[GET] /api/account/accID={accID}/Receipts&apikey={apikey}```

Serves JSON response of all the Receipts associated with the given UserAccount.
Returns **HTTP 204 No Content** if no Receipts exists with the given UserAccount or no UserAccount exists with the given Username.


**Example Requests:** 
```HTTP GET https://frankfund.appspot.com/api/account/user=AutumnNguyen/Receipts&apikey={apikey}```

```HTTP GET https://frankfund.appspot.com/api/account/accID=1/Receipts&apikey={apikey}```

---

# UserSubscriptions

```[GET] /api/account/user={user}/Subscriptions&apikey={apikey}```

```[GET] /api/account/accID={accID}/Subscriptions&apikey={apikey}```

Serves JSON response of all the Subscriptions associated with the given UserAccount.
Returns **HTTP 204 No Content** if no Subscriptions exists with the given UserAccount or no UserAccount exists with the given Username.


**Example Requests:** 
```HTTP GET https://frankfund.appspot.com/api/account/user=DevinSuy/Subscriptions&apikey={apikey}```

```HTTP GET https://frankfund.appspot.com/api/account/accID=1/Subscriptions&apikey={apikey}```

---

```[GET] /api/account/accID={accID}/Subscriptions/count&apikey={apikey}```

Serves JSON response of the number of Subscriptions associate with the given UserAccount. 
Returns **HTTP 204 No Content** if no Subscriptions exists with the given UserAccount or no UserAccount exists with the given Username.

**Example Requests:** 
```HTTP GET https://frankfund.appspot.com/api/account/user=RachelPai/Subscriptions/Count&apikey={apikey}```

**Example Response:**
```json
{
	"SubCount" : 1
}
```

---
