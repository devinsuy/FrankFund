import requests
import time
from datetime import datetime, timedelta
from google.cloud import bigquery, storage


def load_table(tableID: str, csvPath: str):
    # Append data.csv to BQ table
    client = bigquery.Client()
    qualifier = 'frankfund.FrankFund'
    qualified_table = qualifier + tableID

    dataset_ref = client.dataset(qualifier)
    table_ref = dataset_ref.table(tableID)
    job_config = bigquery.LoadJobConfig()
    job_config.source_format = bigquery.SourceFormat.CSV

    with open(csvPath, "rb") as source_file:
        job = client.load_table_from_file(source_file, table_ref, job_config=job_config)

    job.result()  # Waits for table load to complete.

    print("Loaded {} rows into {}.".format(job.output_rows, qualified_table))

