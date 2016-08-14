//-----------------------------------------------------------------------
// <copyright company="Nuclei">
//     Copyright 2013 Nuclei. Licensed under the Apache License, Version 2.0.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Nuclei.Configuration;
using Nuclei.Diagnostics.Logging;

namespace Nuclei.Diagnostics
{
    /// <summary>
    /// Defines the <see cref="ConfigurationKeyBase"/> objects for the diagnostics layers.
    /// </summary>
    public static class DiagnosticsConfigurationKeys
    {
        /// <summary>
        /// The <see cref="ConfigurationKeyBase"/> that is used to retrieve the TCP port (int).
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes",
            Justification = "ConfigurationKey is immutable")]
        public static readonly ConfigurationKey<LevelToLog> DefaultLogLevel
            = new ConfigurationKey<LevelToLog>("DefaultLogLevel");

        /// <summary>
        /// Returns a collection containing all the configuration keys for the diagnostics section.
        /// </summary>
        /// <returns>A collection containing all the configuration keys for the diagnostics section.</returns>
        public static IEnumerable<ConfigurationKeyBase> ToCollection()
        {
            return new List<ConfigurationKeyBase>
                {
                    DefaultLogLevel,
                };
        }
    }
}
