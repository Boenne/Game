﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Game.Model.Buildings.MainBuildings;
using Game.Model.Factories;
using Game.Model.Items.Settings.Costs;
using Game.Model.Items.Tools;
using Game.Model.Workers.ResourceConsuming;
using Newtonsoft.Json;

namespace Game.Model.Buildings.ResourceConsuming
{
    public class Forge : ResourceConsumingBuilding<Blacksmith>
    {
        private readonly Storage _storage;
        private readonly IToolFactoryService _toolFactoryService;

        public Forge(Storage storage, IToolFactoryService toolFactoryService)
        {
            _storage = storage;
            _toolFactoryService = toolFactoryService;
        }

        public Forge()
        {
            
        }

        [JsonProperty]
        public List<Tool> Tools { get; private set; } = new List<Tool>();

        public async Task<bool> CraftHammer(int level)
        {
            if (!_storage.Consume(ToolCosts.Hammer.GetCosts(level))) return false;

            var hammer = _toolFactoryService.CreateHammer(level);
            await Task.Delay(CraftingTime());
            lock (Lock)
            {
                Tools.Add(hammer);
            }

            return true;
        }

        public async Task<bool> CraftPickaxe(int level)
        {
            if (!_storage.Consume(ToolCosts.Pickaxe.GetCosts(level))) return false;

            var pickaxe = _toolFactoryService.CreatePickaxe(level);
            await Task.Delay(CraftingTime());
            lock (Lock)
            {
                Tools.Add(pickaxe);
            }

            return true;
        }

        public async Task<bool> CraftHatchet(int level)
        {
            if (!_storage.Consume(ToolCosts.Hatchet.GetCosts(level))) return false;

            var hatchet = _toolFactoryService.CreateHatchet(level);
            await Task.Delay(CraftingTime());
            lock (Lock)
            {
                Tools.Add(hatchet);
            }

            return true;
        }

        public async Task<bool> CraftRake(int level)
        {
            if (!_storage.Consume(ToolCosts.Rake.GetCosts(level))) return false;

            var rake = _toolFactoryService.CreateRake(level);
            await Task.Delay(CraftingTime());
            lock (Lock)
            {
                Tools.Add(rake);
            }

            return true;
        }

        public Tool GetTool(Urn id)
        {
            lock (Lock)
            {
                var tool = Tools.FirstOrDefault(x => Equals(x.Id, id) );
                if (tool != null)
                    Tools.Remove(tool);
                return tool;
            }
        }

        public void AddTool(Tool tool)
        {
            lock (Lock)
            {
                Tools.Add(tool);
            }
        }

        private int CraftingTime()
        {
            lock (Lock) { 
                return ExecutionTimes.ToolCraftingTime - Workers.Sum(x => x.TotalCraftingSpeedRecution());
            }
        }
    }
}