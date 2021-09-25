using System.Collections.Generic;
using Game.Model.Workers;

namespace Game.Model.Buildings
{
    public abstract class Building<T> where T : Worker
    {
        public int Level { get; protected set; }
        public List<T> Workers { get; } = new List<T>();
        public int NumberOfWorkers => Workers.Count;

        public void AddWorker(T worker)
        {
            Workers.Add(worker);
        }

        public void RemoveWorker(T worker)
        {
            Workers.Remove(worker);
        }
    }
}