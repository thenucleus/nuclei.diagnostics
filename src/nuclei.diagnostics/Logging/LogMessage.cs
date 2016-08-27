//-----------------------------------------------------------------------
// <copyright company="TheNucleus">
// Copyright (c) TheNucleus. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENCE.md file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Nuclei.Diagnostics.Properties;

namespace Nuclei.Diagnostics.Logging
{
    /// <summary>
    /// Defines a message that should be logged by an <see cref="ILogger"/> object.
    /// </summary>
    [Serializable]
    public sealed class LogMessage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        /// <param name="level">The level of the log message.</param>
        /// <param name="text">The text for the log message.</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if <paramref name="level"/> is <see cref="LevelToLog.None"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="text"/> is <see langword="null" />.
        /// </exception>
        public LogMessage(LevelToLog level, string text)
            : this(level, null, null, text, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        /// <param name="level">The level of the log message.</param>
        /// <param name="format">The text for the log message.</param>
        /// <param name="formatParameters">The collection of format providers.</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if <paramref name="level"/> is <see cref="LevelToLog.None"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="format"/> is <see langword="null" />.
        /// </exception>
        public LogMessage(LevelToLog level, string format, params object[] formatParameters)
            : this(level, null, null, format, formatParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        /// <param name="level">The level of the log message.</param>
        /// <param name="provider">The object that provides the culture-specific format information.</param>
        /// <param name="format">The text for the log message.</param>
        /// <param name="formatParameters">The collection of format providers.</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if <paramref name="level"/> is <see cref="LevelToLog.None"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="format"/> is <see langword="null" />.
        /// </exception>
        public LogMessage(LevelToLog level, IFormatProvider provider, string format, params object[] formatParameters)
            : this(level, null, provider, format, formatParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        /// <param name="level">The level of the log message.</param>
        /// <param name="text">The text for the log message.</param>
        /// <param name="properties">The dictionary that contains the additional properties for the current message.</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if <paramref name="level"/> is <see cref="LevelToLog.None"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="text"/> is <see langword="null" />.
        /// </exception>
        public LogMessage(LevelToLog level, IDictionary<string, object> properties, string text)
            : this(level, properties, null, text, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        /// <param name="level">The level of the log message.</param>
        /// <param name="properties">The dictionary that contains the additional properties for the current message.</param>
        /// <param name="format">The text for the log message.</param>
        /// <param name="formatParameters">The collection of format providers.</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if <paramref name="level"/> is <see cref="LevelToLog.None"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="format"/> is <see langword="null" />.
        /// </exception>
        public LogMessage(LevelToLog level, IDictionary<string, object> properties, string format, params object[] formatParameters)
            : this(level, properties, null, format, formatParameters)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogMessage"/> class.
        /// </summary>
        /// <param name="level">The level of the log message.</param>
        /// <param name="properties">The dictionary that contains the additional properties for the current message.</param>
        /// <param name="provider">The object that provides the culture-specific format information.</param>
        /// <param name="format">The text for the log message.</param>
        /// <param name="formatParameters">The collection of format providers.</param>
        /// <exception cref="ArgumentException">
        ///     Thrown if <paramref name="level"/> is <see cref="LevelToLog.None"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if <paramref name="format"/> is <see langword="null" />.
        /// </exception>
        public LogMessage(
            LevelToLog level,
            IDictionary<string, object> properties,
            IFormatProvider provider,
            string format,
            params object[] formatParameters)
        {
            if (level == LevelToLog.None)
            {
                throw new ArgumentException(Resources.Exceptions_Messages_CannotLogMessageWithLogLevelSetToNone);
            }

            if (format == null)
            {
                throw new ArgumentNullException("format");
            }

            FormatProvider = provider ?? CultureInfo.InvariantCulture;
            FormatParameters = formatParameters ?? new object[0];
            Level = level;
            Properties = (properties != null)
                ? new ReadOnlyDictionary<string, object>(properties)
                : new ReadOnlyDictionary<string, object>(new Dictionary<string, object>());
            Text = format;
        }

        /// <summary>
        /// Gets an array of objects containing the format parameters.
        /// </summary>
        [SuppressMessage(
            "Microsoft.Performance",
            "CA1819:PropertiesShouldNotReturnArrays",
            Justification = "Just returns the object provided in the constructor.")]
        public object[] FormatParameters
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the object that provides the culture-specific formatting information.
        /// </summary>
        public IFormatProvider FormatProvider
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether the message contains additional parameters that
        /// should be processed when the message is written to the log.
        /// </summary>
        public bool HasAdditionalInformation
        {
            [DebuggerStepThrough]
            get
            {
                return Properties.Count > 0;
            }
        }

        /// <summary>
        /// Gets the desired log level for this message.
        /// </summary>
        /// <value>The desired level.</value>
        public LevelToLog Level
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a collection that contains additional parameters which should be
        /// processed when the message is written to the log.
        /// </summary>
        public ReadOnlyDictionary<string, object> Properties
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the message text for this message.
        /// </summary>
        public string Text
        {
            get;
            private set;
        }
    }
}
