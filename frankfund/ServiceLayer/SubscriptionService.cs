using System;
using DataAccessLayer;
using Google.Cloud.BigQuery.V2;
using DataAccessLayer.Models;
using System.Collections.Generic;

namespace ServiceLayer
{
	public class SubscriptionService: Service<Subscription>
    {
        private readonly SubscriptionDataAccess SubscriptionDataAccess;

        public SubscriptionService()
        {
            this.SubscriptionDataAccess = new SubscriptionDataAccess();
        }

        public Subscription reinstantiate(BigQueryRow row)
        {
            long RID = -1;
            string notes = "";   // Nullable attribute (receipt ID)

            if (row["RID"] != null)
            {
                RID = (long)row["RID"];
            }

           
            if (row["Notes"] != null)
            {
                notes = (string)row["Notes"];
            }

            return new Subscription(
                (long)row["SID"], (long)row["AccountID"], RID,
                (DateTime)row["PurchaseDate"], notes,
                this.SubscriptionDataAccess.castBQNumeric(row["Amount"]),
                this.SubscriptionDataAccess.ParseEnum<SubscriptionFrequency>((string)row["RenewFrequency"]));
        }


        public SubscriptionFrequency castFrequency(string p)
        {
            if (p.Equals("Weekly"))
                return SubscriptionFrequency.Weekly;
            else if (p.Equals("Monthly"))
                return SubscriptionFrequency.Monthly;
            else if (p.Equals("everyThreeMonths"))
                return SubscriptionFrequency.everyThreeMonths;
            else if (p.Equals("everySixMonths"))
                return SubscriptionFrequency.everySixMonths;
            else
                return SubscriptionFrequency.Yearly;
        }


        /* Retrieve a Subscription from db with a given SID
        Params: The SID of the Subscription to retrieve
        Returns: A reinstantiated Subscription matching the SID or null if non existant
        */
        public Subscription getUsingID(long SID)
        {
            long RID = -1;                     // Nullable attribute
            string notes = "";
            Subscription subscription = null;
            foreach (BigQueryRow row in this.SubscriptionDataAccess.getUsingID(SID))
            {
                if (row["RID"] != null)
                {
                    RID = (long)row["RID"];
                }

                if (row["Notes"] != null)
                {
                    notes = (string)row["Notes"];
                }
                subscription = reinstantiate(row);
            }
            return subscription;
        }

        /*
        Serialize a Subscription object into a String array
        Returns: A string array with each element in order of its column attribute (see Subscription DB schema)
        */
        public string[] serialize(Subscription s)
        {
            long checkRID = s.getRID();
            string checkNotes = s.getNotes();

            if (checkRID == null)
            {
                checkRID = 0;
            }

           if (checkNotes == null)
            {
                checkNotes = "This subscription has no notes to show.";
            }
            
            
            return new string[] {
                s.getSID().ToString(),
                s.getAccID().ToString(),
                checkRID.ToString(),
                s.getPurchaseDate().ToString("yyyy-MM-dd"),
                checkNotes.ToString(),
                s.getPurchaseAmount().ToString(),
                s.getFrequency().ToString()
            };

       
        }

        /*
        Convert a Subscription object into JSON format
        Params: A Subscription object to convert
        Returns: The JSON string representation of the object
        */
        public string getJSON(Subscription s)
        {
            if (s == null)
            {
                return "{}";
            }
            string[] serialized = serialize(s);
            string jsonStr = "{"
                + $"\"SID\":{serialized[0]},"
                + $"\"AccountID\":{serialized[1]},"
                + $"\"RID\":{serialized[2]},"
                + $"\"PurchaseDate\":\"" + serialized[3] + "\","
                + $"\"Notes\":\"{serialized[4]}\","
                + $"\"Amount\":{serialized[5]},"
                + $"\"RenewFrequency\":\"" + serialized[6] + "\""
            + "}";
            return jsonStr;
        }


        public string getJSON(List<Subscription> subscriptions)
        {
            if (subscriptions == null || subscriptions.Count == 0)
            {
                return "{}";
            }
            string jsonStr = "{\"Subscriptions\":[";
            for (int i = 0; i < subscriptions.Count; i++)
            {
                if (i == subscriptions.Count - 1)
                {
                    jsonStr += getJSON(subscriptions[i]);
                }
                else
                {
                    jsonStr += (getJSON(subscriptions[i]) + ", ");
                }
            }

            return jsonStr + "]}";
        }

        // Delete a Subscription with the given PK Identifier
        public void delete(long SID)
        {
            this.SubscriptionDataAccess.delete(SID);
        }

        // Serialize a NEWLY created Subscription runtime object and write it to BigQuery for the first time
        public void write(Subscription s)
        {
            string[] serializedSubscription = serialize(s);
            this.SubscriptionDataAccess.write(serializedSubscription);
        }


        // Serialize and update an EXISTING Subscription in BigQuery only if it CHANGED during runtime
        public void update(Subscription s)
        {
            if (s.changed)
            {
                string[] serializedSubscription = serialize(s);
                this.SubscriptionDataAccess.update(serializedSubscription);
            }
        }

        /* Wrapper method, query DB for next available SID
        Returns: Next available SID (1 + the maximum SID currently in the DB)
        */
        public long getNextAvailID()
        {
            return SubscriptionDataAccess.getNextAvailID();
        }

        /*
        Returns all transactions that is associated with the given account ID ordered by date entered
        Params: The User Account ID 
        Returns: A list of Transactions associated with the given ID
        */
        public List<Subscription> getSubscriptionsFromAccount(long accID)
        {
            List<Subscription> subscriptionList = new List<Subscription>();

            foreach (BigQueryRow row in this.SubscriptionDataAccess.getSubscriptionsFromAccount(accID))
            {
                Subscription subscription = reinstantiate(row);
                subscriptionList.Add(subscription);
            }
            return subscriptionList;
        }

        public List<Subscription> getSubscriptionsFromAccount(string username)
        {
            List<Subscription> subscriptionList = new List<Subscription>();

            foreach (BigQueryRow row in this.SubscriptionDataAccess.getSubscriptionsFromAccount(username))
            {
                Subscription subscription = reinstantiate(row);
                subscriptionList.Add(subscription);
            }
            return subscriptionList;
        }

        /*
        Returns all subscriptions associated with the given SID and sorted by the given category.
        Params: The SID
                The category
        Returns: A list of Transactions associated with user account sorted by category
        */
        public List<Subscription> getSubscriptionByRenewalFrequency(long SID, string renewFrequency)
        {
            List<Subscription> subscriptionList = new List<Subscription>();
            foreach (BigQueryRow row in this.SubscriptionDataAccess.getSubscriptionViaFrequency(SID, renewFrequency))
            {
                Subscription subscription = reinstantiate(row);
                subscriptionList.Add(subscription);
            }
            return subscriptionList;
        }

        /* Cast a frequency string to subscriptionfreqeuency enum
         Params: The category string to cast
         Returns: The corresponding enum subscriptionfrequency
        */
        public SubscriptionFrequency castSubscriptionFrequency(string casting)
        {
            if (casting.Equals("Weekly"))
                return SubscriptionFrequency.Weekly;
            else if (casting.Equals("Monthly"))
                return SubscriptionFrequency.Monthly;
            else if (casting.Equals("everyThreeMonths"))
                return SubscriptionFrequency.everyThreeMonths;
            else if (casting.Equals("everySixMonths"))
                return SubscriptionFrequency.everySixMonths;
            else if (casting.Equals("Yearly"))
                return SubscriptionFrequency.Yearly;
            else
                return SubscriptionFrequency.NotSpecified;

        }
    }
}