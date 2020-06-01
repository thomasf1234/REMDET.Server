using System;
using System.Collections.Generic; 

using InfluxDB.Client;
using InfluxDB.Client.Api.Domain;
using InfluxDB.Client.Writes;

using REMDET.Server.Models;

namespace REMDET.Server.Repositories
{
    public class SensorRepository : ISensorRepository, IDisposable
    {
        private readonly InfluxDBClient _influxDBClient;
        
        public SensorRepository(InfluxDBClient influxDBClient) 
        {
            this._influxDBClient = influxDBClient;
        }

        public void Write(List<MeasurementEvent> measurementEvents)
        {
            WriteOptions writeOptions = WriteOptions.CreateNew().BatchSize(100).Build();

            using (var writeApi = this._influxDBClient.GetWriteApi(writeOptions))
            {
                List<PointData> points = new List<PointData>();

                foreach (var me in measurementEvents)
                {
                    PointData point = PointData.Measurement(me.Measurement)
                    .Field("value", me.Value)
                    .Timestamp(me.RecordedAt.ToUniversalTime(), WritePrecision.Ns); 

                    points.Add(point);
                }

                writeApi.EventHandler += (sender, eventArgs) =>
                {
                    if (eventArgs is WriteErrorEvent @event)
                    {
                        var exception = @event.Exception;

                        throw exception;
                        
                        // Notify exception to error catching system such as Airbrake 
                        // Publish failed write event that could trigger a retry mechanism or notify a support operator
                    }
                };
  
                writeApi.WritePoints(points);
                writeApi.Flush();
            }
        }

        public void Dispose()
        {
            this._influxDBClient.Dispose(); 
        }
    }
}
