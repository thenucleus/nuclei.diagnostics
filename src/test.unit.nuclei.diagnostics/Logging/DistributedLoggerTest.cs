//-----------------------------------------------------------------------
// <copyright company="TheNucleus">
// Copyright (c) TheNucleus. All rights reserved.
// Licensed under the Apache License, Version 2.0 license. See LICENCE.md file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;
using Moq;
using NUnit.Framework;

namespace Nuclei.Diagnostics.Logging
{
    [TestFixture]

    public sealed class DistributedLoggerTest
    {
        [Test]
        public void ChangeLevelWithMultipleLoggers()
        {
            var subLogger1 = new Mock<ILogger>();
            subLogger1.SetupAllProperties();

            var subLogger2 = new Mock<ILogger>();
            subLogger2.SetupAllProperties();

            var logger = new DistributedLogger(new ILogger[] { subLogger1.Object, subLogger2.Object });
            logger.Level = LevelToLog.Info;

            Assert.AreEqual(subLogger1.Object.Level, LevelToLog.Info);
            Assert.AreEqual(subLogger2.Object.Level, LevelToLog.Info);
        }

        [Test]
        public void ChangeLevelWithSingleLogger()
        {
            var subLogger = new Mock<ILogger>();
            subLogger.SetupAllProperties();

            var logger = new DistributedLogger(new ILogger[] { subLogger.Object });
            logger.Level = LevelToLog.Info;

            Assert.AreEqual(subLogger.Object.Level, LevelToLog.Info);
        }

        [Test]
        [SuppressMessage(
            "Microsoft.Usage",
            "CA1806:DoNotIgnoreMethodResults",
            MessageId = "Nuclei.Diagnostics.Logging.DistributedLogger",
            Justification = "Testing that the constructor throws if provided with a null collection.")]
        public void CreateWithNullCollection()
        {
            Assert.Throws<ArgumentNullException>(() => new DistributedLogger(null));
        }

        [Test]
        public void LevelWithMultipleLoggers()
        {
            var level = LevelToLog.Debug;
            var subLogger1 = new Mock<ILogger>();
            {
                subLogger1.Setup(l => l.Level)
                    .Returns(level);
            }

            var subLogger2 = new Mock<ILogger>();
            {
                subLogger2.Setup(l => l.Level)
                    .Returns(level);
            }

            var logger = new DistributedLogger(new ILogger[] { subLogger1.Object, subLogger2.Object });

            Assert.AreEqual(level, logger.Level);
        }

        [Test]
        public void LevelWithMultipleLoggersWithDifferentLevels()
        {
            var logger1Level = LevelToLog.Debug;
            var subLogger1 = new Mock<ILogger>();
            {
                subLogger1.Setup(l => l.Level)
                    .Returns(logger1Level);
            }

            var logger2Level = LevelToLog.Info;
            var subLogger2 = new Mock<ILogger>();
            {
                subLogger2.Setup(l => l.Level)
                    .Returns(logger2Level);
            }

            var logger = new DistributedLogger(new ILogger[] { subLogger1.Object, subLogger2.Object });

            Assert.AreEqual(logger1Level, logger.Level);
        }

        [Test]
        public void LevelWithSingleLogger()
        {
            var level = LevelToLog.Debug;
            var subLogger = new Mock<ILogger>();
            {
                subLogger.Setup(l => l.Level)
                    .Returns(level);
            }

            var logger = new DistributedLogger(new ILogger[] { subLogger.Object });

            Assert.AreEqual(level, logger.Level);
        }

        [Test]
        public void LogWithMultipleLoggers()
        {
            var logger1Level = LevelToLog.Info;
            LogMessage logger1Message = null;
            var subLogger1 = new Mock<ILogger>();
            {
                subLogger1.Setup(l => l.Level)
                    .Returns(logger1Level);
                subLogger1.Setup(l => l.Log(It.IsAny<LogMessage>()))
                    .Callback<LogMessage>(m => logger1Message = m);
                subLogger1.Setup(l => l.ShouldLog(It.IsAny<LogMessage>()))
                    .Returns<LogMessage>(m => m.Level >= logger1Level);
            }

            var logger2Level = LevelToLog.Info;
            LogMessage logger2Message = null;
            var subLogger2 = new Mock<ILogger>();
            {
                subLogger2.Setup(l => l.Level)
                    .Returns(logger2Level);
                subLogger2.Setup(l => l.Log(It.IsAny<LogMessage>()))
                    .Callback<LogMessage>(m => logger2Message = m);
                subLogger2.Setup(l => l.ShouldLog(It.IsAny<LogMessage>()))
                    .Returns<LogMessage>(m => m.Level >= logger2Level);
            }

            var logger = new DistributedLogger(new ILogger[] { subLogger1.Object, subLogger2.Object });

            var messageToLog = new LogMessage(LevelToLog.Debug, "a");
            logger.Log(messageToLog);
            Assert.IsNull(logger1Message);
            Assert.IsNull(logger2Message);

            messageToLog = new LogMessage(LevelToLog.Info, "c");
            logger.Log(messageToLog);
            Assert.AreSame(messageToLog, logger1Message);
            Assert.AreSame(messageToLog, logger2Message);
        }

        [Test]
        public void LogWithMultipleLoggersAtDifferentLevels()
        {
            var logger1Level = LevelToLog.Debug;
            LogMessage logger1Message = null;
            var subLogger1 = new Mock<ILogger>();
            {
                subLogger1.Setup(l => l.Level)
                    .Returns(logger1Level);
                subLogger1.Setup(l => l.Log(It.IsAny<LogMessage>()))
                    .Callback<LogMessage>(m => logger1Message = m);
                subLogger1.Setup(l => l.ShouldLog(It.IsAny<LogMessage>()))
                    .Returns<LogMessage>(m => m.Level >= logger1Level);
            }

            var logger2Level = LevelToLog.Info;
            LogMessage logger2Message = null;
            var subLogger2 = new Mock<ILogger>();
            {
                subLogger2.Setup(l => l.Level)
                    .Returns(logger2Level);
                subLogger2.Setup(l => l.Log(It.IsAny<LogMessage>()))
                    .Callback<LogMessage>(m => logger2Message = m);
                subLogger2.Setup(l => l.ShouldLog(It.IsAny<LogMessage>()))
                    .Returns<LogMessage>(m => m.Level >= logger2Level);
            }

            var logger = new DistributedLogger(new ILogger[] { subLogger1.Object, subLogger2.Object });

            var messageToLog = new LogMessage(LevelToLog.Trace, "a");
            logger.Log(messageToLog);
            Assert.IsNull(logger1Message);
            Assert.IsNull(logger2Message);

            messageToLog = new LogMessage(LevelToLog.Debug, "b");
            logger.Log(messageToLog);
            Assert.AreSame(messageToLog, logger1Message);
            Assert.IsNull(logger2Message);

            messageToLog = new LogMessage(LevelToLog.Info, "c");
            logger.Log(messageToLog);
            Assert.AreSame(messageToLog, logger1Message);
            Assert.AreSame(messageToLog, logger2Message);
        }

        [Test]
        public void LogWithMultipleLoggersWithMultipleThrowing()
        {
            var logger1Level = LevelToLog.Debug;
            var logger1Exception = new LoggingException();
            var subLogger1 = new Mock<ILogger>();
            {
                subLogger1.Setup(l => l.Level)
                    .Returns(logger1Level);
                subLogger1.Setup(l => l.Log(It.IsAny<LogMessage>()))
                    .Throws(logger1Exception);
                subLogger1.Setup(l => l.ShouldLog(It.IsAny<LogMessage>()))
                    .Returns<LogMessage>(m => m.Level >= logger1Level);
            }

            var logger2Level = LevelToLog.Info;
            var logger2Exception = new LoggingException();
            var subLogger2 = new Mock<ILogger>();
            {
                subLogger2.Setup(l => l.Level)
                    .Returns(logger2Level);
                subLogger2.Setup(l => l.Log(It.IsAny<LogMessage>()))
                    .Throws(logger2Exception);
                subLogger2.Setup(l => l.ShouldLog(It.IsAny<LogMessage>()))
                    .Returns<LogMessage>(m => m.Level >= logger2Level);
            }

            var logger = new DistributedLogger(new ILogger[] { subLogger1.Object, subLogger2.Object });

            var messageToLog = new LogMessage(LevelToLog.Info, "a");
            try
            {
                logger.Log(messageToLog);
            }
            catch (LoggingException e)
            {
                Assert.AreEqual(2, e.InnerExceptions.Count);
                Assert.AreSame(logger1Exception, e.InnerExceptions[0]);
                Assert.AreSame(logger2Exception, e.InnerExceptions[1]);
            }
        }

        [Test]
        public void LogWithMultipleLoggersWithOneThrowing()
        {
            var logger1Level = LevelToLog.Debug;
            var logger1Exception = new LoggingException();
            var subLogger1 = new Mock<ILogger>();
            {
                subLogger1.Setup(l => l.Level)
                    .Returns(logger1Level);
                subLogger1.Setup(l => l.Log(It.IsAny<LogMessage>()))
                    .Throws(logger1Exception);
                subLogger1.Setup(l => l.ShouldLog(It.IsAny<LogMessage>()))
                    .Returns<LogMessage>(m => m.Level >= logger1Level);
            }

            var logger2Level = LevelToLog.Info;
            LogMessage logger2Message = null;
            var subLogger2 = new Mock<ILogger>();
            {
                subLogger2.Setup(l => l.Level)
                    .Returns(logger2Level);
                subLogger2.Setup(l => l.Log(It.IsAny<LogMessage>()))
                    .Callback<LogMessage>(m => logger2Message = m);
                subLogger2.Setup(l => l.ShouldLog(It.IsAny<LogMessage>()))
                    .Returns<LogMessage>(m => m.Level >= logger2Level);
            }

            var logger = new DistributedLogger(new ILogger[] { subLogger1.Object, subLogger2.Object });

            var messageToLog = new LogMessage(LevelToLog.Info, "a");
            try
            {
                logger.Log(messageToLog);
            }
            catch (LoggingException e)
            {
                Assert.AreEqual(1, e.InnerExceptions.Count);
                Assert.AreSame(logger1Exception, e.InnerExceptions[0]);
            }

            Assert.AreSame(messageToLog, logger2Message);
        }

        [Test]
        public void LogWithNullMessage()
        {
            var logger1Level = LevelToLog.Info;
            LogMessage logger1Message = null;
            var subLogger1 = new Mock<ILogger>();
            {
                subLogger1.Setup(l => l.Level)
                    .Returns(logger1Level);
                subLogger1.Setup(l => l.Log(It.IsAny<LogMessage>()))
                    .Callback<LogMessage>(m => logger1Message = m);
                subLogger1.Setup(l => l.ShouldLog(It.IsAny<LogMessage>()))
                    .Returns<LogMessage>(m => m.Level >= logger1Level);
            }

            var logger2Level = LevelToLog.Info;
            LogMessage logger2Message = null;
            var subLogger2 = new Mock<ILogger>();
            {
                subLogger2.Setup(l => l.Level)
                    .Returns(logger2Level);
                subLogger2.Setup(l => l.Log(It.IsAny<LogMessage>()))
                    .Callback<LogMessage>(m => logger2Message = m);
                subLogger2.Setup(l => l.ShouldLog(It.IsAny<LogMessage>()))
                    .Returns<LogMessage>(m => m.Level >= logger2Level);
            }

            var logger = new DistributedLogger(new ILogger[] { subLogger1.Object, subLogger2.Object });

            logger.Log(null);
            Assert.IsNull(logger1Message);
            Assert.IsNull(logger2Message);
        }

        [Test]
        public void LogWithSingleLogger()
        {
            var level = LevelToLog.Info;
            LogMessage message = null;
            var subLogger = new Mock<ILogger>();
            {
                subLogger.Setup(l => l.Level)
                    .Returns(level);
                subLogger.Setup(l => l.Log(It.IsAny<LogMessage>()))
                    .Callback<LogMessage>(m => message = m);
                subLogger.Setup(l => l.ShouldLog(It.IsAny<LogMessage>()))
                    .Returns<LogMessage>(m => m.Level >= level);
            }

            var logger = new DistributedLogger(new ILogger[] { subLogger.Object });

            var messageToLog = new LogMessage(LevelToLog.Debug, "a");
            logger.Log(messageToLog);
            Assert.IsNull(message);

            messageToLog = new LogMessage(LevelToLog.Info, "a");
            logger.Log(messageToLog);
            Assert.AreSame(messageToLog, message);
        }

        [Test]
        public void LogWithSingleLoggerThrowing()
        {
            var level = LevelToLog.Info;
            var exception = new LoggingException();
            var subLogger = new Mock<ILogger>();
            {
                subLogger.Setup(l => l.Level)
                    .Returns(level);
                subLogger.Setup(l => l.Log(It.IsAny<LogMessage>()))
                    .Throws(exception);
                subLogger.Setup(l => l.ShouldLog(It.IsAny<LogMessage>()))
                    .Returns<LogMessage>(m => m.Level >= level);
            }

            var logger = new DistributedLogger(new ILogger[] { subLogger.Object });

            try
            {
                logger.Log(new LogMessage(LevelToLog.Debug, "a"));
            }
            catch (LoggingException e)
            {
                Assert.AreEqual(1, e.InnerExceptions.Count);
                Assert.AreSame(exception, e.InnerExceptions[0]);
            }
        }

        [Test]
        public void ShouldLogWithMessageWithLevelSetToNone()
        {
            var level = LevelToLog.None;
            var subLogger = new Mock<ILogger>();
            {
                subLogger.Setup(l => l.Level)
                    .Returns(level);
            }

            var logger = new DistributedLogger(new ILogger[] { subLogger.Object });

            Assert.IsFalse(logger.ShouldLog(new LogMessage(LevelToLog.Debug, "a")));
        }

        [Test]
        public void ShouldLogWithMultipleLoggersAtDifferentLevels()
        {
            var logger1Level = LevelToLog.Debug;
            var subLogger1 = new Mock<ILogger>();
            {
                subLogger1.Setup(l => l.Level)
                    .Returns(logger1Level);
            }

            var logger2Level = LevelToLog.Info;
            var subLogger2 = new Mock<ILogger>();
            {
                subLogger2.Setup(l => l.Level)
                    .Returns(logger2Level);
            }

            var logger = new DistributedLogger(new ILogger[] { subLogger1.Object, subLogger2.Object });

            Assert.IsFalse(logger.ShouldLog(new LogMessage(LevelToLog.Trace, "a")));
            Assert.IsTrue(logger.ShouldLog(new LogMessage(LevelToLog.Debug, "b")));
            Assert.IsTrue(logger.ShouldLog(new LogMessage(LevelToLog.Info, "c")));
        }

        [Test]
        public void ShouldLogWithMultipleLoggersAtSameLevel()
        {
            var logger1Level = LevelToLog.Info;
            var subLogger1 = new Mock<ILogger>();
            {
                subLogger1.Setup(l => l.Level)
                    .Returns(logger1Level);
            }

            var logger2Level = LevelToLog.Info;
            var subLogger2 = new Mock<ILogger>();
            {
                subLogger2.Setup(l => l.Level)
                    .Returns(logger2Level);
            }

            var logger = new DistributedLogger(new ILogger[] { subLogger1.Object, subLogger2.Object });

            Assert.IsFalse(logger.ShouldLog(new LogMessage(LevelToLog.Trace, "a")));
            Assert.IsFalse(logger.ShouldLog(new LogMessage(LevelToLog.Debug, "b")));
            Assert.IsTrue(logger.ShouldLog(new LogMessage(LevelToLog.Info, "c")));
        }

        [Test]
        public void ShouldLogWithNullMessage()
        {
            var level = LevelToLog.Info;
            var subLogger = new Mock<ILogger>();
            {
                subLogger.Setup(l => l.Level)
                    .Returns(level);
            }

            var logger = new DistributedLogger(new ILogger[] { subLogger.Object });

            Assert.IsFalse(logger.ShouldLog(null));
        }

        [Test]
        public void ShouldLogWithSingleLogger()
        {
            var level = LevelToLog.Info;
            var subLogger = new Mock<ILogger>();
            {
                subLogger.Setup(l => l.Level)
                    .Returns(level);
            }

            var logger = new DistributedLogger(new ILogger[] { subLogger.Object });

            Assert.IsFalse(logger.ShouldLog(new LogMessage(LevelToLog.Debug, "a")));
            Assert.IsTrue(logger.ShouldLog(new LogMessage(LevelToLog.Info, "b")));
        }
    }
}
