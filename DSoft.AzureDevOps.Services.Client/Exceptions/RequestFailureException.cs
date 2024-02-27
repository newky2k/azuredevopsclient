using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.AzureDevOps.Services.Client.Exceptions
{
    public class RequestFailureException : Exception
    {
        public RequestFailureException(string message) : base(message)
        {

        }
    }
}
