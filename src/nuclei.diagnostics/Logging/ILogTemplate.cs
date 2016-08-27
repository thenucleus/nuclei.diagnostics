//-----------------------------------------------------------------------
// <copyright company="TheNucleus">
// Copyright (c) TheNucleus. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENCE.md file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace Nuclei.Diagnostics.Logging
{
    /// <summary>
    /// Defines a template for changing <see cref="LogMessage"/> objects into the
    /// appropriate string representation.
    /// </summary>
    public interface ILogTemplate : IEquatable<ILogTemplate>
    {
        /// <summary>
        /// Gets the name of the template.
        /// </summary>
        /// <value>The name of the template.</value>
        string Name
        {
            get;
        }

        /// <summary>
        /// Returns the default log level which is used if no changes in
        /// log level are requested after the system starts.
        /// </summary>
        /// <returns>
        /// The default log level.
        /// </returns>
        LevelToLog DefaultLogLevel();

        /// <summary>
        /// Translates the specified message.
        /// </summary>
        /// <param name="message">The message that must be translated.</param>
        /// <returns>
        /// The desired string representation of the log message.
        /// </returns>
        string Translate(LogMessage message);
    }
}
