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
            LogMessage storedMessage = null;
            var logger = new Mock<ILogger>();
            {
                logger.Setup(l => l.Log(It.IsAny<LogMessage>()))
                    .Callback<LogMessage>(m => storedMessage = m);
            }

            var diagnostics = new SystemDiagnostics(logger.Object, null);

            var providedLevel = LevelToLog.Error;
            var providedMessage = "This is a message";
            diagnostics.Log(providedLevel, providedMessage);

            Assert.AreEqual(providedLevel, storedMessage.Level);
            Assert.AreEqual(providedMessage, storedMessage.Text);
        }

        [Test]
        public void LogWithSeverityMessageAndFormatParameters()
        {
            LogMessage storedMessage = null;
            var logger = new Mock<ILogger>();
            {
                logger.Setup(l => l.Log(It.IsAny<LogMessage>()))
                    .Callback<LogMessage>(m => storedMessage = m);
            }

            var diagnostics = new SystemDiagnostics(logger.Object, null);

            var providedLevel = LevelToLog.Error;
            var providedMessage = "This is a message";
            var parameter = 10;
            diagnostics.Log(providedLevel, providedMessage, parameter);

            Assert.AreEqual(providedLevel, storedMessage.Level);
            Assert.AreEqual(providedMessage, storedMessage.Text);
            Assert.AreEqual(parameter, storedMessage.FormatParameters[0]);
        }

        [Test]
        public void LogWithSeverityMessageProviderAndFormatParameters()
        {
            LogMessage storedMessage = null;
            var logger = new Mock<ILogger>();
            {
                logger.Setup(l => l.Log(It.IsAny<LogMessage>()))
                    .Callback<LogMessage>(m => storedMessage = m);
            }

            var diagnostics = new SystemDiagnostics(logger.Object, null);

            var providedLevel = LevelToLog.Error;
            var providedMessage = "This is a message";
            var provider = CultureInfo.CurrentCulture;
            var parameter = 10;
            diagnostics.Log(providedLevel, provider, providedMessage, parameter);

            Assert.AreEqual(providedLevel, storedMessage.Level);
            Assert.AreEqual(providedMessage, storedMessage.Text);
            Assert.AreEqual(provider, storedMessage.FormatProvider);
            Assert.AreEqual(parameter, storedMessage.FormatParameters[0]);
        }
    }
}
