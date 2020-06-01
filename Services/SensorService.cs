using System;
using System.Collections.Generic; 

using REMDET.Server.Models;
using REMDET.Server.Repositories;

namespace REMDET.Server.Services
{
    public class SensorService : ISensorService
    {
        private readonly ISensorRepository _sensorRepository;

        public SensorService(ISensorRepository sensorRepository) 
        {
            this._sensorRepository = sensorRepository;
        }

        public void Record(List<MeasurementEvent> measurementEvents)
        {
            this._sensorRepository.Write(measurementEvents); 
        }
    }
}
