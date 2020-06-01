using System;
using System.Collections.Generic; 

using REMDET.Server.Models;

namespace REMDET.Server.Services
{
    public interface ISensorService
    {
      void Record(List<MeasurementEvent> measurementEvents);
    }
}
