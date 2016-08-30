//-----------------------------------------------------------------------
// <copyright company="TheNucleus">
// Copyright (c) TheNucleus. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENCE.md file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using NUnit.Framework;

namespace Nuclei.Diagnostics.Logging.NLog
{
    [TestFixture]
    [SuppressMessage(
        "Microsoft.StyleCop.CSharp.DocumentationRules",
        "SA1600:ElementsMustBeDocumented",
        Justification = "Unit tests do not need documentation.")]
    public sealed class LogMessageTest
    {
        [Test]
        public void Create()
        {
            var level = LevelToLog.Debug;
            var text = "text";

            var message = new LogMessage(level, text);

            Assert.AreEqual(0, message.FormatParameters.Length);

            Assert.IsNotNull(message.FormatProvider);
            Assert.AreEqual(CultureInfo.InvariantCulture, message.FormatProvider);

            Assert.IsFalse(message.HasAdditionalInformation);

            Assert.AreEqual(level, message.Level);

            Assert.AreEqual(0, message.Properties.Count);

            Assert.AreEqual(text, message.Text);
        }

        [Test]
        public void CreateWithFormatParameters()
        {
            var level = LevelToLog.Debug;
            var text = "text";
            var parameter = 10;

            var message = new LogMessage(level, text, parameter);

            Assert.AreEqual(1, message.FormatParameters.Length);
            Assert.AreEqual(parameter, message.FormatParameters[0]);

            Assert.IsNotNull(message.FormatProvider);
            Assert.AreEqual(CultureInfo.InvariantCulture, message.FormatProvider);

            Assert.IsFalse(message.HasAdditionalInformation);

            Assert.AreEqual(level, message.Level);

            Assert.AreEqual(0, message.Properties.Count);

            Assert.AreEqual(text, message.Text);
        }

        [Test]
        public void CreateWithFormatProviderAndParameters()
        {
            var level = LevelToLog.Debug;
            var text = "text";
            var parameter = 10;
            var provider = CultureInfo.CurrentUICulture;

            var message = new LogMessage(level, provider, text, parameter);

            Assert.AreEqual(1, message.FormatParameters.Length);
            Assert.AreEqual(parameter, message.FormatParameters[0]);

            Assert.IsNotNull(message.FormatProvider);
            Assert.AreEqual(provider, message.FormatProvider);

            Assert.IsFalse(message.HasAdditionalInformation);

            Assert.AreEqual(level, message.Level);

            Assert.AreEqual(0, message.Properties.Count);

            Assert.AreEqual(text, message.Text);
        }

        [Test]
        public void CreateWithProperties()
        {
            var level = LevelToLog.Debug;
            var text = "text";
            var properties = new Dictionary<string, object>
                {
                    { "Key", 10 }
                };

            var message = new LogMessage(level, properties, text);

            Assert.AreEqual(0, message.FormatParameters.Length);

            Assert.IsNotNull(message.FormatProvider);
            Assert.AreEqual(CultureInfo.InvariantCulture, message.FormatProvider);

            Assert.IsTrue(message.HasAdditionalInformation);

            Assert.AreEqual(level, message.Level);

            Assert.That(message.Properties, Is.EquivalentTo(properties));

            Assert.AreEqual(text, message.Text);
        }
    }
}
