namespace PulseApp.Web.Api;

/// <summary>
/// Класс точка входа
/// </summary>
internal static class Program
{
    /// <summary>
    /// Запуск сервера
    /// </summary>
    public static void Main(string[] args)
    {
        CreateHostBuilder(args)
            .Build()
            .Run();
    }

    /// <summary>
    /// Инициализация сервера
    /// </summary>
    private static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
