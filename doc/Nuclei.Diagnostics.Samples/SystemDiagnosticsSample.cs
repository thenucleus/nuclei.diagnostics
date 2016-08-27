//-----------------------------------------------------------------------
// <copyright company="TheNucleus">
// Copyright (c) TheNucleus. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENCE.md file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
using Moq;
using Nuclei.Diagnostics.Logging;
using NUnit.Framework;

namespace Nuclei.Diagnostics.Samples
{
    [TestFixture]
    public sealed class SystemDiagnosticsSample
    {
        [Test]
        public void Log()
        {
            LogMessage storedMessage = null;
            var mockLogger = new Mock<ILogger>();
            {
                mockLogger.Setup(l => l.Log(It.IsAny<LogMessage>()))
                    .Callback<LogMessage>(m => storedMessage = m);
            }

            var logger = mockLogger.Object;

            var diagnostics = new SystemDiagnostics(logger, null);

            var providedLevel = LevelToLog.Error;
            var providedMessage = "This is a message";
            diagnostics.Log(providedLevel, providedMessage);

            Assert.AreEqual(providedLevel, storedMessage.Level);
            Assert.AreEqual(providedMessage, storedMessage.Text);
        }
    }
}
