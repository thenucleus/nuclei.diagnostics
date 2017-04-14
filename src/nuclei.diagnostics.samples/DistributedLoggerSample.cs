//-----------------------------------------------------------------------
// <copyright company="TheNucleus">
// Copyright (c) TheNucleus. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENCE.md file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using Moq;
using Nuclei.Diagnostics.Logging;
using NUnit.Framework;

namespace Nuclei.Diagnostics.Samples
{
    [TestFixture]
    public sealed class DistributedLoggerSample
    {
        private ILogger NewLogger()
        {
            var result = new Mock<ILogger>();
            {
                result.SetupAllProperties();
            }

            return result.Object;
        }

        [Test]
        public void LevelWithMultipleLoggersWithDifferentLevels()
        {
            var logger1 = NewLogger();
            logger1.Level = LevelToLog.Debug;

            var logger2 = NewLogger();
            logger2.Level = LevelToLog.Warn;

            var logger = new DistributedLogger(new ILogger[] { logger1, logger2 });

            // The level is the lowest of the two log levels, in this case LevelToLog.Debug
            var level = logger.Level;
            Assert.AreEqual(LevelToLog.Debug, level);
        }

        [Test]
        public void LogWithMultipleLoggersAtDifferentLevels()
        {
            var logger1 = NewLogger();
            logger1.Level = LevelToLog.Debug;

            var logger2 = NewLogger();
            logger2.Level = LevelToLog.Info;

            var logger = new DistributedLogger(new ILogger[] { logger1, logger2 });

            // This message should not be logged by either logger
            var messageToLog = new LogMessage(LevelToLog.Trace, "a");
            logger.Log(messageToLog);

            // This message should only be logged by loger1
            messageToLog = new LogMessage(LevelToLog.Debug, "b");
            logger.Log(messageToLog);

            // This message should be logged by both loggers
            messageToLog = new LogMessage(LevelToLog.Info, "c");
            logger.Log(messageToLog);
        }

        [Test]
        public void ShouldLogWithMultipleLoggersAtDifferentLevels()
        {
            var logger1 = NewLogger();
            logger1.Level = LevelToLog.Debug;

            var logger2 = NewLogger();
            logger2.Level = LevelToLog.Info;

            var logger = new DistributedLogger(new ILogger[] { logger1, logger2 });

            // Returns false
            var shouldLog = logger.ShouldLog(new LogMessage(LevelToLog.Trace, "a"));
            Assert.IsFalse(shouldLog);

            // Returns true because logger1 will accept this message
            shouldLog = logger.ShouldLog(new LogMessage(LevelToLog.Debug, "b"));
            Assert.IsTrue(shouldLog);

            // Returns true because both loggers will accept this message
            shouldLog = logger.ShouldLog(new LogMessage(LevelToLog.Info, "c"));
            Assert.IsTrue(shouldLog);
        }
    }
}
