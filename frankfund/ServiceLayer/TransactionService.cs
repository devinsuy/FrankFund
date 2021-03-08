using System;
using System.Collections.Generic;
using System.Text;
using DataAccessLayer;
using Google.Cloud.BigQuery.V2;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace ServiceLayer
{
    class TransactionService
    {
        private readonly TransactionDataAccess TransactionDataAccess;

        public TransactionService()
        {
            this.TransactionDataAccess = new TransactionDataAccess();
        }

        public void AddTransaction(Transaction transaction)
        {
             
        }
    }
}
