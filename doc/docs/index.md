# Nuclei.Diagnostics


## DistributedLogger

The `DistributedLogger` class provides an `ILogger` implementation that can send the log messages to multiple sub-loggers. The 
level at which the `DistributedLogger` starts logging is determined by the sub-loggers, a message will be logged if there is a
`ILogger` implementation that will take the message. This allows for instance having a file logger that logs all messages while
having an eventlog logger which only logs the fatal messages.

[!code-csharp[DistributedLogger.Level](..\..\Nuclei.Diagnostics.Samples\DistributedLoggerSample.cs?range=30-39)]

[!code-csharp[DistributedLogger.ShouldLog](..\..\Nuclei.Diagnostics.Samples\DistributedLoggerSample.cs?range=70-79,81-83,85-87)]

[!code-csharp[DistributedLogger.Log](..\..\Nuclei.Diagnostics.Samples\DistributedLoggerSample.cs?range=46-64)]


## SystemDiagnostics

The `SystemDiagnostics` class provides a convenient way to distribute references to both `Ilogger` and `IMetricsCollector` instances.

[!code-csharp[SystemDiagnostics.Log](..\..\Nuclei.Diagnostics.Samples\SystemDiagnosticsSample.cs?range=29-35)]