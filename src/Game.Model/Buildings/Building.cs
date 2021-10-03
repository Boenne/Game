using System.Collections.Generic;
using System.Linq;
using Game.Model.Workers;
using Newtonsoft.Json;

namespace Game.Model.Buildings
{
    public abstract class Building<T> : Identifiable where T : Worker
    {
        [JsonProperty] public int Level { get; protected set; }

        [JsonProperty] public List<T> Workers { get; private set; } = new List<T>();

        public int NumberOfWorkers()
        {
            lock (Lock)
            {
                return Workers.Count;
            }
        }

        public void AddWorker(T worker)
        {
            lock (Lock)
            {
                Workers.Add(worker);
            }
        }

        public List<T> RemoveWorker(params Urn[] ids)
        {
            lock (Lock)
            {
                var workers = Workers.Where(x => ids.Contains(x.Id)).ToList();
                foreach (var worker in workers) Workers.Remove(worker);

                return workers;
            }
        }
    }
}