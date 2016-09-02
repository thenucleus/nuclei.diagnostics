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
    /// Defines the interface for objects that remote logging calls.
    /// </summary>
    public interface ILogMessagesFromRemoteAppDomains
    {
        /// <summary>
        /// Passes the given message to the system loggers.
        /// </summary>
        /// <param name="severity">The severity for the message.</param>
        /// <param name="message">The message.</param>
        void Log(LevelToLog severity, string message);

        /// <summary>
        /// Logs the specified format message with the given arguments. Messages as formatted
        /// with the <see cref="CultureInfo.InvariantCulture"/> as format provider.
        /// </summary>
        /// <param name="severity">The severity for the message.</param>
        /// <param name="format">The string to format.</param>
        /// <param name="parameters">The parameters for the format string.</param>
        void Log(LevelToLog severity, string format, params object[] parameters);

        /// <summary>
        /// Logs the specified format message with the given arguments and format provider.
        /// </summary>
        /// <param name="severity">The severity for the message.</param>
        /// <param name="provider">An object that provides the culture-specific formatting information.</param>
        /// <param name="format">The string to format.</param>
        /// <param name="parameters">The parameters for the format string.</param>
        void Log(LevelToLog severity, IFormatProvider provider, string format, params object[] parameters);
    }
}
