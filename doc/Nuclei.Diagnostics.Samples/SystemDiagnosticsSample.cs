//-----------------------------------------------------------------------
// <copyright company="TheNucleus">
// Copyright (c) TheNucleus. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENCE.md file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Globalization;
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
