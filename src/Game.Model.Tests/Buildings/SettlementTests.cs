using Game.Model.Buildings.Settlement;
using Shouldly;
using Xunit;

namespace Game.Model.Tests.Buildings
{
    public class SettlementTests
    {
        [Fact]
        public void MaximumNumberOfWorkersIsOne_NoCurrentWorkers_CanAddWorker()
        {
            var settlement = new Settlement(1, 0, 0, 0, 0);

            settlement.CanAddWorker().ShouldBeTrue();
        }
    }
}