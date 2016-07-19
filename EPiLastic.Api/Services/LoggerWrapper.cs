using log4net;
using System;

namespace EPiLastic.Api.Services
{
    public interface ILoggerWrapper
    {
        void LogError<T>(string message, Exception e);   
    }

    public class LoggerWrapper : ILoggerWrapper
    {
        public void LogError<T>(string message, Exception e)
        {
            var logger = LogManager.GetLogger(typeof(T));
            logger.Error(message, e);
        }
    }
}