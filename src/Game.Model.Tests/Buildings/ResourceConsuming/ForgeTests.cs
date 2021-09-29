using System;
using System.Linq;
using System.Threading.Tasks;
using Game.Model.Buildings.ResourceConsuming;
using Game.Model.Buildings.Settings;
using Game.Model.Buildings.Settlement;
using Game.Model.Factories;
using Game.Model.Items.Tools;
using Game.Model.Resources;
using Moq;
using Shouldly;
using Xunit;

namespace Game.Model.Tests.Buildings.ResourceConsuming
{
    public class ForgeTests
    {
        public ForgeTests()
        {
            _toolFactoryService = new Mock<IToolFactoryService>();
        }

        private readonly Mock<IToolFactoryService> _toolFactoryService;

        [Fact]
        public void AddTool_AddsTool()
        {
            var storage = new Storage(0, 0);
            var forge = new Forge(storage, _toolFactoryService.Object);
            var pickaxe = new Pickaxe("", 0, 0);

            forge.AddTool(pickaxe);

            forge.Tools.Count.ShouldBe(1);
        }

        [Fact]
        public void GetTool_CorrectId_ReturnsTool()
        {
            var storage = new Storage(0, 0);
            var forge = new Forge(storage, _toolFactoryService.Object);
            var pickaxe = new Pickaxe("", 0, 0);
            forge.AddTool(pickaxe);

            var tool = forge.GetTool(pickaxe.Id);

            tool.ShouldBe(pickaxe);
            forge.Tools.ShouldBeEmpty();
        }

        [Fact]
        public void GetTool_IncorrectId_ReturnsNull()
        {
            var storage = new Storage(0, 0);
            var forge = new Forge(storage, _toolFactoryService.Object);
            var pickaxe = new Pickaxe("", 0, 0);
            forge.AddTool(pickaxe);

            var tool = forge.GetTool(Guid.Empty);

            tool.ShouldBeNull();
            forge.Tools.Count.ShouldBe(1);
        }

        [Fact]
        public async Task CraftHammer_EnoughResources_CreatesHammer()
        {
            var hammer = new Hammer("", 0, 0);
            _toolFactoryService.Setup(x => x.CreateHammer(1)).Returns(hammer);
            StartingResources.Stone = new Stone(10);
            StartingResources.Lumber = new Lumber(10);
            var storage = new Storage(0, 0);
            var forge = new Forge(storage, _toolFactoryService.Object);

            var wasCrafted = await forge.CraftHammer(1);

            wasCrafted.ShouldBeTrue();
            forge.Tools.Count.ShouldBe(1);
            forge.Tools.First().ShouldBe(hammer);
        }

        [Fact]
        public async Task CraftHammer_NotEnoughResources_DoesNothing()
        {
            StartingResources.Stone = new Stone(0);
            var storage = new Storage(0, 0);
            var forge = new Forge(storage, _toolFactoryService.Object);

            var wasCrafted = await forge.CraftHammer(1);

            wasCrafted.ShouldBeFalse();
            forge.Tools.ShouldBeEmpty();
        }

        [Fact]
        public async Task CraftPickaxe_EnoughResources_CreatesPickaxe()
        {
            var pickaxe = new Pickaxe("", 0, 0);
            _toolFactoryService.Setup(x => x.CreatePickaxe(1)).Returns(pickaxe);
            StartingResources.Stone = new Stone(10);
            StartingResources.Lumber = new Lumber(10);
            var storage = new Storage(0, 0);
            var forge = new Forge(storage, _toolFactoryService.Object);

            var wasCrafted = await forge.CraftPickaxe(1);

            wasCrafted.ShouldBeTrue();
            forge.Tools.Count.ShouldBe(1);
            forge.Tools.First().ShouldBe(pickaxe);
        }

        [Fact]
        public async Task CraftPickaxe_NotEnoughResources_DoesNothing()
        {
            StartingResources.Stone = new Stone(0);
            var storage = new Storage(0, 0);
            var forge = new Forge(storage, _toolFactoryService.Object);

            var wasCrafted = await forge.CraftPickaxe(1);

            wasCrafted.ShouldBeFalse();
            forge.Tools.ShouldBeEmpty();
        }

        [Fact]
        public async Task CraftHatchet_EnoughResources_CreatesHatchet()
        {
            var hatchet = new Hatchet("", 0, 0);
            _toolFactoryService.Setup(x => x.CreateHatchet(1)).Returns(hatchet);
            StartingResources.Stone = new Stone(10);
            StartingResources.Lumber = new Lumber(10);
            var storage = new Storage(0, 0);
            var forge = new Forge(storage, _toolFactoryService.Object);

            var wasCrafted = await forge.CraftHatchet(1);

            wasCrafted.ShouldBeTrue();
            forge.Tools.Count.ShouldBe(1);
            forge.Tools.First().ShouldBe(hatchet);
        }

        [Fact]
        public async Task CraftHatchet_NotEnoughResources_DoesNothing()
        {
            StartingResources.Stone = new Stone(0);
            var storage = new Storage(0, 0);
            var forge = new Forge(storage, _toolFactoryService.Object);

            var wasCrafted = await forge.CraftHatchet(1);

            wasCrafted.ShouldBeFalse();
            forge.Tools.ShouldBeEmpty();
        }

        [Fact]
        public async Task CraftRake_EnoughResources_CreatesRake()
        {
            var rake = new Rake("", 0, 0);
            _toolFactoryService.Setup(x => x.CreateRake(1)).Returns(rake);
            StartingResources.Lumber = new Lumber(20);
            var storage = new Storage(0, 0);
            var forge = new Forge(storage, _toolFactoryService.Object);

            var wasCrafted = await forge.CraftRake(1);

            wasCrafted.ShouldBeTrue();
            forge.Tools.Count.ShouldBe(1);
            forge.Tools.First().ShouldBe(rake);
        }

        [Fact]
        public async Task CraftRake_NotEnoughResources_DoesNothing()
        {
            StartingResources.Lumber = new Lumber(0);
            var storage = new Storage(0, 0);
            var forge = new Forge(storage, _toolFactoryService.Object);

            var wasCrafted = await forge.CraftRake(1);

            wasCrafted.ShouldBeFalse();
            forge.Tools.ShouldBeEmpty();
        }
    }
}