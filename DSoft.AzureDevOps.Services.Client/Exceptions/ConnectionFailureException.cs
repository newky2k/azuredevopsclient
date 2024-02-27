using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.AzureDevOps.Services.Client.Exceptions
{
    public class ConnectionFailureException : Exception
    {
        public ConnectionFailureException() : base("Unable to connect")
        {

        }
    }
}
