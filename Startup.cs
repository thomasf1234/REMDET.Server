using System;

using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;

using REMDET.Server.Configuration;
using REMDET.Server.Repositories;
using REMDET.Server.Services;
using REMDET.Server.Extensions;

[assembly: FunctionsStartup(typeof(REMDET.Server.Startup))]

namespace REMDET.Server
{
    public class Startup : FunctionsStartup
    {

        public override void Configure(IFunctionsHostBuilder builder)
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .SetBasePath(Environment.CurrentDirectory)
                .AddJsonFile("local.settings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            IConfigurationSection influxDBConfigurationSection = configuration.GetSection("InfluxDB");
            IConfigurationSection featuresConfigurationSection = configuration.GetSection("Features");

            IInfluxDBConfiguration influxDBConfiguration = new InfluxDBConfiguration();
            IFeaturesConfiguration featuresConfiguration = new FeaturesConfiguration();

            influxDBConfigurationSection.Bind(influxDBConfiguration);
            featuresConfigurationSection.Bind(featuresConfiguration);

            InfluxDBClientOptions influxDBClientOptions = InfluxDBClientOptions.Builder
              .CreateNew()
              .Url(influxDBConfiguration.Url)
              .AuthenticateToken(influxDBConfiguration.Token.ToCharArray())
              .Org(influxDBConfiguration.Organisation)
              .Bucket(influxDBConfiguration.Bucket)
              .Build();

            InfluxDBClient influxDBClient = InfluxDBClientFactory.Create(influxDBClientOptions);
            ISensorRepository sensorRepository = new SensorRepository(influxDBClient);

            builder.Services.AddSingleton<IFeaturesConfiguration>(featuresConfiguration);
            builder.Services.AddSingleton<ISensorRepository>(sensorRepository);
            builder.Services.AddSensorService();
            builder.Services.AddFeatureService();
        }
    }
}