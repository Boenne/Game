using System;
using System.Collections.Generic;
using System.Linq;
using Game.Model.Workers;

namespace Game.Model.Buildings
{
    public abstract class Building<T> : Identifiable where T : Worker
    {
        public int Level { get; protected set; }
        public List<T> Workers { get; } = new List<T>();
        public int NumberOfWorkers => Workers.Count;

        public void AddWorker(T worker)
        {
            lock (Lock)
            {
                Workers.Add(worker);
            }
        }

        public List<T> RemoveWorker(params Guid[] ids)
        {
            lock (Lock)
            {
                var workers = Workers.Where(x => ids.Contains(x.Id)).ToList();
                foreach (var worker in workers)
                {
                    Workers.Remove(worker);
                }

                return workers;
            }
        }
    }
}