# FrankFund REST API

## Overview
Endpoints return data from GCP BigQuery in JSON format

## Authentication
HTTP requests must be in the form:

```http://frankfund.appspot.com/api/...&apikey={apikey}```

The trailing part of the endpoint should be the literal "&apikey="
followed immediately by your issued developer API key

You can find your developer **apikey** in the Github repo:
    /Credentials/Auth{YourName}.json

Your corrseponding apikey is labeled **"private_key_id"** in your Auth.json file.

## Error Codes
Improperly authenticated requests will return a HTTP 401 Unauthorized.

Improperly formated requests will return a HTTP 400 Bad Request.

Retrieving data that returns 0 rows in BigQuery will return a HTTP 204 No Content.


## Active Endpoints

### SavingsGoals

```[GET] http://frankfund.appspot.com/api/SGID={SGID}&apikey={apikey})```

Returns JSON data of the SavingsGoal with the given SGID

**Example Request:** WebRequest.Create("http://frankfund.appspot.com/api/SGID=1&apikey=c55f8d138f6ccfd43612b15c98706943e1f4bea3");
\


### UserAccounts

```[GET] http://frankfund.appspot.com/api/accID={accID}&apikey={apikey})```

Returns JSON data of the UserAccount with the given AccountID

**Example Request:** WebRequest.Create("http://frankfund.appspot.com/api/accID=1&apikey=f2f1178729cb2e1c9188ed847066743c4e843a21");
\


### Transactions

```[GET] http://frankfund.appspot.com/api/TID={TID}&apikey={apikey})```

Returns JSON data of the Transaction with the given TransactionID

**Example Request:** WebRequest.Create("http://frankfund.appspot.com/api/TID=1&apikey=bd0eecf7cf275751a421a6101272f559b0391fa0");

