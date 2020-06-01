using System;

using Newtonsoft.Json;

namespace REMDET.Server.Models
{
    public class MeasurementEvent : Event
    {
        [JsonProperty("measurement")]
        public string Measurement { get; set; }

        [JsonProperty("value")]
        public double Value { get; set; }

        [JsonProperty("recorded_at")]
        public DateTime RecordedAt { get; set; }

        public MeasurementEvent() { }

        public MeasurementEvent(string _measurement, double _value, DateTime _recordedAt)
        {
            this.Measurement = _measurement;
            this.Value = _value;
            this.RecordedAt = _recordedAt;
        }      
    }
}
