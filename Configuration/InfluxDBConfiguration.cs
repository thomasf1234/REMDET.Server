namespace REMDET.Server.Configuration
{
  public class InfluxDBConfiguration : IInfluxDBConfiguration
  {
    public string Url { get; set; }
    public string Token { get; set; }
    public string Bucket { get; set; }
    public string Organisation { get; set; }
  }
}
