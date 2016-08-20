//-----------------------------------------------------------------------
// <copyright company="TheNucleus">
// Copyright (c) TheNucleus. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENCE.md file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Moq;
using Nuclei.Diagnostics.Logging;
using Nuclei.Diagnostics.Metrics;
using NUnit.Framework;

namespace Nuclei.Diagnostics
{
    [TestFixture]
    public sealed class SystemDiagnosticsTest
    {
        [Test]
        [SuppressMessage(
            "Microsoft.Usage",
            "CA1806:DoNotIgnoreMethodResults",
            MessageId = "Nuclei.Diagnostics.SystemDiagnostics",
            Justification = "Testing that the constructor throws.")]
        public void CreateWithNullLogger()
        {
            Assert.Throws<ArgumentNullException>(() => new SystemDiagnostics(null, new Mock<IMetricsCollector>().Object));
        }

        [Test]
        public void LogWithSeverityAndMessage()
        {
            var storedLevel = LevelToLog.None;
            var storedMessage = string.Empty;
            Action<LevelToLog, string> logger =
                (l, m) =>
                {
                    storedLevel = l;
                    storedMessage = m;
                };
            var diagnostics = new SystemDiagnostics(logger, null);

            var providedLevel = LevelToLog.Error;
            var providedMessage = "This is a message";
            diagnostics.Log(providedLevel, providedMessage);

            Assert.AreEqual(providedLevel, storedLevel);
            Assert.AreEqual(providedMessage, storedMessage);
        }

        [Test]
        public void LogWithSeverityPrefixAndMessage()
        {
            var storedLevel = LevelToLog.None;
            var storedMessage = string.Empty;
            Action<LevelToLog, string> logger =
                (l, m) =>
                {
                    storedLevel = l;
                    storedMessage = m;
                };
            var diagnostics = new SystemDiagnostics(logger, null);

            var providedLevel = LevelToLog.Error;
            var providedPrefix = "Prefix";
            var providedMessage = "This is a message";
            diagnostics.Log(providedLevel, providedPrefix, providedMessage);

            Assert.AreEqual(providedLevel, storedLevel);
            Assert.AreEqual(
                string.Format(
                    CultureInfo.InvariantCulture,
                    "{0} - {1}",
                    providedPrefix,
                    providedMessage),
                storedMessage);
        }
    }
}
