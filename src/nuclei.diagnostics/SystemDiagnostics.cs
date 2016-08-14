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
        /// The action that logs the given string to the underlying loggers.
        /// </summary>
        private readonly Action<LevelToLog, string> _logger;

        /// <summary>
        /// The object that provides metrics collection methods.
        /// </summary>
        private readonly IMetricsCollector _metrics;

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemDiagnostics"/> class.
        /// </summary>
        /// <param name="logger">The action that logs the given string to the underlying loggers.</param>
        /// <param name="metrics">The object that provides metrics collection methods. May be <see langword="null" />.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="logger"/> is <see langword="null" />.
        /// </exception>
        public SystemDiagnostics(Action<LevelToLog, string> logger, IMetricsCollector metrics)
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
            _logger(severity, message);
        }

        /// <summary>
        /// Passes the given message to the system loggers.
        /// </summary>
        /// <param name="severity">The severity for the message.</param>
        /// <param name="prefix">The prefix text that should be placed at the start of the logged text.</param>
        /// <param name="message">The message.</param>
        public void Log(LevelToLog severity, string prefix, string message)
        {
            _logger(
                severity,
                string.Format(
                    CultureInfo.InvariantCulture,
                    "{0} - {1}",
                    prefix,
                    message));
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
