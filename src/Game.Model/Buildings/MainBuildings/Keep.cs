using System.Collections.Generic;
using System.Linq;
using Game.Model.Workers;
using Newtonsoft.Json;

namespace Game.Model.Buildings.MainBuildings
{
    public class Keep : Identifiable
    {
        [JsonProperty] public List<Worker> AvailableWorkers { get; private set; } = new List<Worker>();

        public void AddWorker(Worker worker)
        {
            lock (Lock)
            {
                AvailableWorkers.Add(worker);
            }
        }

        public List<Worker> GetWorkers(params Urn[] workerIds)
        {
            lock (Lock)
            {
                var workers = AvailableWorkers.Where(x => workerIds.Contains(x.Id)).ToList();
                foreach (var worker in workers) AvailableWorkers.Remove(worker);
                return workers;
            }
        }

        public int NumberOfIdleWorkers()
        {
            lock (Lock)
            {
                return AvailableWorkers.Count;
            }
        }
    }
}