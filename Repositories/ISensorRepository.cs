using System;
using System.Collections.Generic; 

using REMDET.Server.Models;

namespace REMDET.Server.Repositories
{
    public interface ISensorRepository
    {
      void Write(List<MeasurementEvent> measurementEvents);
    }
}
