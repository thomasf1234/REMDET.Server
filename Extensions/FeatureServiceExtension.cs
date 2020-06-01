using Microsoft.Extensions.DependencyInjection;

using REMDET.Server.Services;

namespace REMDET.Server.Extensions
{
  public static class FeatureServiceExtension
  {
    public static void AddFeatureService(this IServiceCollection services)
    {
      services.AddScoped<IFeatureService, FeatureService>();
    }
  }
}