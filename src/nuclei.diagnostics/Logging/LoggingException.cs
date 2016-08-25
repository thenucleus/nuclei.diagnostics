//-----------------------------------------------------------------------
// <copyright company="TheNucleus">
// Copyright (c) TheNucleus. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENCE.md file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Nuclei.Diagnostics.Properties;

namespace Nuclei.Diagnostics.Logging
{
    /// <summary>
    /// An exception thrown when the loading of a plugin type fails.
    /// </summary>
    [Serializable]
    public sealed class LoggingException : AggregateException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingException"/> class.
        /// </summary>
        public LoggingException()
            : this(Resources.Exceptions_Messages_LoggingFailure)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public LoggingException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingException"/> class.
        /// </summary>
        /// <param name="innerExceptions">The exceptions that are the cause of the current exception.</param>
        public LoggingException(params Exception[] innerExceptions)
            : base(innerExceptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingException"/> class.
        /// </summary>
        /// <param name="innerExceptions">The exceptions that are the cause of the current exception.</param>
        public LoggingException(IEnumerable<Exception> innerExceptions)
            : base(innerExceptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public LoggingException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerExceptions">The exceptions that are the cause of the current exception.</param>
        public LoggingException(string message, IEnumerable<Exception> innerExceptions)
            : base(message, innerExceptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerExceptions">The exceptions that are the cause of the current exception.</param>
        public LoggingException(string message, params Exception[] innerExceptions)
            : base(message, innerExceptions)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingException"/> class.
        /// </summary>
        /// <param name="info">
        ///     The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object
        ///     data about the exception being thrown.
        /// </param>
        /// <param name="context">
        ///     The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information
        ///     about the source or destination.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        private LoggingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
