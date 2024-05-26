using System;
using System.Collections.Generic;
using System.Text;

namespace LoDaTek.AzureDevOps.Services.Client.Exceptions
{
    /// <summary>
    /// Class ConnectionFailureException.
    /// Implements the <see cref="Exception" />
    /// </summary>
    /// <seealso cref="Exception" />
    public class ConnectionFailureException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectionFailureException"/> class.
        /// </summary>
        public ConnectionFailureException() : base("Unable to connect")
        {

        }
    }
}
