using Microsoft.Extensions.DependencyInjection;

using REMDET.Server.Services;

namespace REMDET.Server.Extensions
{
  public static class SensorServiceExtension
  {
    public static void AddSensorService(this IServiceCollection services)
    {
      services.AddScoped<ISensorService, SensorService>();
    }
  }
}