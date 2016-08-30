//-----------------------------------------------------------------------
// <copyright company="TheNucleus">
// Copyright (c) TheNucleus. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENCE.md file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Globalization;
using Nuclei.Diagnostics.Logging;
using Nuclei.Diagnostics.Metrics;

namespace Nuclei.Diagnostics
{
    /// <summary>
    /// Provides methods that help with diagnosing possible issues with the framework.
    /// </summary>
    public sealed class SystemDiagnostics
    {
        /// <summary>
        /// The object that logs the given messages.
        /// </summary>
        private readonly ILogger _logger;

        /// <summary>
        /// The object that provides metrics collection methods.
        /// </summary>
        private readonly IMetricsCollector _metrics;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemDiagnostics"/> class.
        /// </summary>
        /// <param name="logger">The object that logs the given messages.</param>
        /// <param name="metrics">The object that provides metrics collection methods. May be <see langword="null" />.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="logger"/> is <see langword="null" />.
        /// </exception>
        public SystemDiagnostics(ILogger logger, IMetricsCollector metrics)
        {
            if (logger == null)
            {
                throw new ArgumentNullException("logger");
            }

            _logger = logger;
            _metrics = metrics;
        }

        /// <summary>
        /// Passes the given message to the system loggers.
        /// </summary>
        /// <param name="severity">The severity for the message.</param>
        /// <param name="message">The message.</param>
        public void Log(LevelToLog severity, string message)
        {
            _logger.Log(new LogMessage(severity, message));
        }

        /// <summary>
        /// Logs the specified format message with the given arguments. Messages as formatted
        /// with the <see cref="CultureInfo.InvariantCulture"/> as format provider.
        /// </summary>
        /// <param name="severity">The severity for the message.</param>
        /// <param name="format">The string to format.</param>
        /// <param name="parameters">The parameters for the format string.</param>
        public void Log(LevelToLog severity, string format, params object[] parameters)
        {
            _logger.Log(new LogMessage(severity, format, parameters));
        }

        /// <summary>
        /// Logs the specified format message with the given arguments and format provider.
        /// </summary>
        /// <param name="severity">The severity for the message.</param>
        /// <param name="provider">An object that provides the culture-specific formatting information.</param>
        /// <param name="format">The string to format.</param>
        /// <param name="parameters">The parameters for the format string.</param>
        public void Log(LevelToLog severity, IFormatProvider provider, string format, params object[] parameters)
        {
            _logger.Log(new LogMessage(severity, provider, format, parameters));
        }

        /// <summary>
        /// Gets the profiler that can be used to gather timing intervals for any specific action
        /// that is executed in the framework.
        /// </summary>
        public IMetricsCollector Metrics
        {
            [DebuggerStepThrough]
            get
            {
                return _metrics;
            }
        }
    }
}
