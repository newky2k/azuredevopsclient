using System;
using System.Collections.Generic;
using System.Text;

namespace DSoft.AzureDevOps.Services.Client.Exceptions
{
    /// <summary>
    /// Class RequestFailureException.
    /// Implements the <see cref="Exception" />
    /// </summary>
    /// <seealso cref="Exception" />
    public class RequestFailureException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RequestFailureException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public RequestFailureException(string message) : base(message)
        {

        }
    }
}
