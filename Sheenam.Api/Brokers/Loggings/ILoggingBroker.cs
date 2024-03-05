namespace Sheenam.Api.Brokers.Loggings
{
    public interface ILoggingBroker
    {
        void LogTrace(string message);
        void LogDebug(string message);
        void LogInfo(string message);
        void LogWarning(string message);
        void LogError(Exception exception);
        void LogCritical(Exception exception);
    }
}
