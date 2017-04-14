//-----------------------------------------------------------------------
// <copyright company="TheNucleus">
// Copyright (c) TheNucleus. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENCE.md file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Globalization;

namespace Nuclei.Diagnostics.Logging
{
    /// <summary>
    /// Provides methods to forward log messages across an <c>AppDomain</c> boundary.
    /// </summary>
    public sealed class LogForwardingPipe : MarshalByRefObject, ILogMessagesFromRemoteAppDomains
    {
        /// <summary>
        /// The objects that provides the diagnostics methods for the application.
        /// </summary>
        private readonly SystemDiagnostics _diagnostics;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogForwardingPipe"/> class.
        /// </summary>
        /// <param name="diagnostics">The object that provides the diagnostics methods for the application.</param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="diagnostics"/> is <see langword="null" />.
        /// </exception>
        public LogForwardingPipe(SystemDiagnostics diagnostics)
        {
            if (diagnostics == null)
            {
                throw new ArgumentNullException("diagnostics");
            }

            _diagnostics = diagnostics;
        }

        /// <summary>
        /// Passes the given message to the system loggers.
        /// </summary>
        /// <param name="severity">The severity for the message.</param>
        /// <param name="message">The message.</param>
        public void Log(LevelToLog severity, string message)
        {
            _diagnostics.Log(severity, message);
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
            _diagnostics.Log(severity, format, parameters);
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
            _diagnostics.Log(severity, provider, format, parameters);
        }
    }
}
