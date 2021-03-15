# FrankFund REST API

## Overview
Endpoints return data from GCP BigQuery in JSON format

## Authentication
HTTP requests must be in the form:

"http://frankfund.appspot.com/api/...&apikey={apikey}"

The trailing part of the endpoint should be the literal "&apikey="
followed immediately by your issued developer API key.

You can find your developer **apikey** in the Github repo:
    /Credentials/Auth{YourName}.json

Your corrseponding apikey is labeled **"private_key_id"** in your Auth.json file.

## Error Codes
Improperly authenticated requests will return a HTTP 401 Unauthorized.

Improperly formated requests will return a HTTP 400 Bad Request.

Retrieving data that returns 0 rows in BigQuery will return a HTTP 204 No Content.


## Active Endpoints

```[GET] http://frankfund.appspot.com/api/SGID={SGID}&apikey={apikey})```

Returns JSON data of the SavingsGoal with the given SGID
