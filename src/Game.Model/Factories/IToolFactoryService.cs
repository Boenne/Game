using Game.Model.Items.Tools;

namespace Game.Model.Factories
{
    public interface IToolFactoryService
    {
        Hammer CreateHammer(int level);
        Pickaxe CreatePickaxe(int level);
        Hatchet CreateHatchet(int level);
        Rake CreateRake(int level);
    }
}