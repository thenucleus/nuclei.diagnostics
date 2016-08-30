//-----------------------------------------------------------------------
// <copyright company="TheNucleus">
// Copyright (c) TheNucleus. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENCE.md file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Nuclei.Diagnostics.Properties;

namespace Nuclei.Diagnostics.Logging
{
    /// <summary>
    /// Defines a logging object that sends <see cref="LogMessage"/> objects to many differen loggers.
    /// </summary>
    public sealed class DistributedLogger : ILogger
    {
        private static bool IsMessageLoggable(LogMessage message)
        {
            if (message == null)
            {
                return false;
            }

            if (message.Level == LevelToLog.None)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// The collection of loggers which are called for each valid log message.
        /// </summary>
        private readonly IEnumerable<ILogger> _loggers;

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributedLogger"/> class.
        /// </summary>
        /// <param name="loggers">The collection of loggers which should be called for each valid log message.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown if <paramref name="loggers"/> is <see langword="null"/>.
        /// </exception>
        public DistributedLogger(IEnumerable<ILogger> loggers)
        {
            if (loggers == null)
            {
                throw new ArgumentNullException("loggers");
            }

            _loggers = loggers;
        }

        /// <summary>
        /// Stops the logger and ensures that all log messages have been
        /// saved to the log.
        /// </summary>
        public void Close()
        {
            foreach (var logger in _loggers)
            {
                logger.Close();
            }
        }

        /// <summary>
        ///  Performs application-defined tasks associated with freeing, releasing, or
        ///  resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        /// <summary>
        /// Gets or sets the current <see cref="LevelToLog"/>.
        /// </summary>
        public LevelToLog Level
        {
            get
            {
                var result = LevelToLog.None;
                foreach (var logger in _loggers)
                {
                    if (logger.Level < result)
                    {
                        result = logger.Level;
                    }
                }

                return result;
            }

            set
            {
                foreach (var logger in _loggers)
                {
                    logger.Level = value;
                }
            }
        }

        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "0",
            Justification = "The 'ShouldLog' method validates the message.")]
        public void Log(LogMessage message)
        {
            if (!IsMessageLoggable(message))
            {
                return;
            }

            List<LoggingException> exceptions = new List<LoggingException>();
            foreach (var logger in _loggers)
            {
                if (logger.ShouldLog(message))
                {
                    try
                    {
                        logger.Log(message);
                    }
                    catch (LoggingException e)
                    {
                        exceptions.Add(e);
                    }
                }
            }

            if (exceptions.Count > 0)
            {
                throw new LoggingException(
                    Resources.Exceptions_Messages_LoggingFailure,
                    exceptions);
            }
        }

        /// <summary>
        /// Indicates if a message will be written to the log file based on the
        /// current log level and the level of the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        /// <see langword="true" /> if the message will be logged; otherwise, <see langword="false" />.
        /// </returns>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1062:Validate arguments of public methods",
            MessageId = "0",
            Justification = "The 'message' parameter is validated in the 'IsMessageLoggable' method.")]
        [SuppressMessage(
            "Microsoft.StyleCop.CSharp.DocumentationRules",
            "SA1628:DocumentationTextMustBeginWithACapitalLetter",
            Justification = "Documentation can start with a language keyword")]
        public bool ShouldLog(LogMessage message)
        {
            if (!IsMessageLoggable(message))
            {
                return false;
            }

            foreach (var logger in _loggers)
            {
                if ((logger.Level != LevelToLog.None) && (message.Level >= logger.Level))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
