namespace REMDET.Server.Configuration
{
  public interface IInfluxDBConfiguration
  {
    string Url { get; set; }
    string Token { get; set; }
    string Bucket { get; set; }
    string Organisation { get; set; }
  }
}