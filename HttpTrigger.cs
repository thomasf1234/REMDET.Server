using System;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic; 

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using REMDET.Server.Models;
using REMDET.Server.Services; 

namespace REMDET.Server
{
    public class HttpTrigger
    {
        private readonly IFeatureService _featureService;
        private readonly ISensorService _sensorService;

        public HttpTrigger(IFeatureService featureService, ISensorService sensorService)
        {
            this._featureService = featureService;
            this._sensorService = sensorService;
        }

        // Client throttling handled in an API Management Service
        [FunctionName("Measurements")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = null)] HttpRequest req,
            ILogger logger
            )
        {
            logger.LogInformation("C# HTTP trigger function processed a request.");
            var requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            
            // Validate and parse request JSON body
            IsoDateTimeConverter recordedAtConverter = new IsoDateTimeConverter { DateTimeFormat = "%Y-%m-%dT%H:%M:%S.%L%Z" };
            List<MeasurementEvent> measurementEvents = JsonConvert.DeserializeObject<List<MeasurementEvent>>(requestBody, recordedAtConverter);

            if (this._featureService.IsDebugEnabled())
            {
                foreach (var me in measurementEvents)
                {
                    logger.LogInformation($"Received ({me.Measurement}, {me.Value}, {me.RecordedAt})");    
                }
            }
        
            // Issue asyncronous data write
            this._sensorService.Record(measurementEvents);

            var result = new ObjectResult("Your sync has been accepted and should reflect on your account shortly");
            result.StatusCode = StatusCodes.Status202Accepted;
            
            return result;
        }
    }
}
