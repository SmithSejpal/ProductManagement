using Microsoft.Extensions.Logging;
using ProductManagement.Core.Interfaces;

namespace ProductManagement.Infrastructure.Logging
{
    /// <summary>
    /// Provides a generic logging adapter that wraps <see cref="ILogger{T}"/> 
    /// to allow dependency injection of logging functionality throughout the application.
    /// </summary>
    /// <typeparam name="T">The type for which logs are being written.</typeparam>
    public class LoggerAdapter<T> : IAppLogger<T>
    {
        private readonly ILogger<T> _logger;

        public LoggerAdapter(ILogger<T> logger)
        {
            _logger = logger;
        }

        public void LogTrace(string message, params object[] args) =>
            _logger.LogTrace(message, args);

        public void LogDebug(string message, params object[] args) =>
            _logger.LogDebug(message, args);

        public void LogInformation(string message, params object[] args) =>
            _logger.LogInformation(message, args);

        public void LogWarning(string message, params object[] args) =>
            _logger.LogWarning(message, args);

        public void LogError(string message, params object[] args) =>
            _logger.LogError(message, args);

        public void LogError(Exception ex, string message, params object[] args) =>
            _logger.LogError(ex, message, args);
    }
}