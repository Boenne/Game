using System.Collections.Generic;
using Game.Model.Workers;

namespace Game.Model.Buildings.Settlement
{
    public class Keep
    {
        private readonly object _lockObject = new object();
        private readonly int _trainingTime;

        public Keep(int trainingTime)
        {
            _trainingTime = trainingTime;
        }

        public List<Worker> AvailableWorkers { get; } = new List<Worker>();

        public void AddWorker(Worker worker)
        {
            AvailableWorkers.Add(worker);
        }
    }
}