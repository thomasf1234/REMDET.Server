using REMDET.Server.Configuration;

namespace REMDET.Server.Services
{
  public class FeatureService : IFeatureService
  {
    private readonly IFeaturesConfiguration _featuresConfiguration;

    public FeatureService(IFeaturesConfiguration featuresConfiguration)
    {
      this._featuresConfiguration = featuresConfiguration;
    }

    public bool IsDebugEnabled()
    {
      return this._featuresConfiguration.Debug;
    }
  }
}