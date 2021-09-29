using Game.Model.Factories;
using Game.Model.Items.Settings.Specifications;
using Game.Model.Items.Tools;

namespace Game.Application.Factories
{
    public class ToolFactoryService : IToolFactoryService
    {
        public Hammer CreateHammer(int level)
        {
            var hammerSpecifications = ToolSpecifications.Hammer.GetSpecification(level);
            return new Hammer(hammerSpecifications.Name, hammerSpecifications.Modifier, level);
        }

        public Pickaxe CreatePickaxe(int level)
        {
            var pickaxeSpecifications = ToolSpecifications.Pickaxe.GetSpecification(level);
            return new Pickaxe(pickaxeSpecifications.Name, pickaxeSpecifications.Modifier, level);
        }

        public Hatchet CreateHatchet(int level)
        {
            var hatchetSpecifications = ToolSpecifications.Hatchet.GetSpecification(level);
            return new Hatchet(hatchetSpecifications.Name, hatchetSpecifications.Modifier, level);
        }

        public Rake CreateRake(int level)
        {
            var rakeSpecifications = ToolSpecifications.Rake.GetSpecification(level);
            return new Rake(rakeSpecifications.Name, rakeSpecifications.Modifier, level);
        }
    }
}