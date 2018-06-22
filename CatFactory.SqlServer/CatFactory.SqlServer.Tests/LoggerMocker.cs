//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;

//namespace CatFactory.SqlServer.Tests
//{
//    public static class LoggerMocker
//    {
//        public static ILogger<T> GetLogger<T>()
//        {
//            var serviceProvider = new ServiceCollection()
//                .AddLogging()
//                .BuildServiceProvider();

//            serviceProvider
//                .GetService<ILoggerFactory>()
//                .AddConsole(LogLevel.Debug)
//                .AddConsole(LogLevel.Trace)
//                .AddConsole(LogLevel.Information)
//                .AddConsole(LogLevel.Warning)
//                .AddConsole(LogLevel.Error)
//                .AddConsole(LogLevel.Critical);

//            return serviceProvider
//                .GetService<ILoggerFactory>()
//                .CreateLogger<T>();
//        }
//    }
//}
