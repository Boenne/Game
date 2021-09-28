using System;
using Game.Model.Items.Tools;

namespace Game.Model.Workers
{
    public abstract class Worker : Identifiable
    {
        private readonly Type _toolType;

        protected Worker(int level, Type toolType)
        {
            Level = level;
            _toolType = toolType;
        }

        public int Level { get; }

        public Tool Tool { get; private set; }

        public bool CanUseTool(Tool tool)
        {
            return tool.GetType() == _toolType;
        }

        public bool SetTool(Tool tool)
        {
            lock (Lock)
            {
                if (!CanUseTool(tool) || HasTool()) return false;
                Tool = tool;
                return true;
            }
        }

        public Tool RemoveTool()
        {
            lock (Lock)
            {
                var tool = Tool.Copy();
                Tool = null;
                return tool;
            }
        }

        public bool HasTool()
        {
            lock (Lock)
            {
                return Tool != null;
            }
        }
    }
}