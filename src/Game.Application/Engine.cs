using System.IO;
using System.Threading.Tasks;
using Game.Model;
using Game.Model.Buildings.MainBuildings;
using Game.Model.Factories;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Game.Application
{
    public class Engine
    {
        private readonly IBuildingFactoryService _buildingFactoryService;
        private readonly IWorkerFactoryService _workerFactoryService;
        private bool _produceResources = true;

        public Engine(IBuildingFactoryService buildingFactoryService, IWorkerFactoryService workerFactoryService)
        {
            _buildingFactoryService = buildingFactoryService;
            _workerFactoryService = workerFactoryService;
        }

        public void StartResourceProduction(Settlement settlement)
        {
            Task.Run(async () =>
            {
                while (_produceResources)
                {
                    await Task.Delay(ExecutionTimes.ResourceProductionTime);
                    if (!_produceResources) break;
                    settlement.ProduceResources();
                }
            });
        }

        public void StopResourceProduction()
        {
            _produceResources = false;
        }

        public void SaveGame(Settlement settlement)
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new JavaScriptDateTimeConverter());
            serializer.NullValueHandling = NullValueHandling.Ignore;
            serializer.TypeNameHandling = TypeNameHandling.Auto;
            serializer.Formatting = Formatting.Indented;

            using (var sw = new StreamWriter(File.Create("savedgame2.json")))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, settlement, typeof(Settlement));
            }
        }
    }
}