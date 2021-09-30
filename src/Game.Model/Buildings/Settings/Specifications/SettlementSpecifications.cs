namespace Game.Model.Buildings.Settings.Specifications
{
    public static class SettlementSpecifications
    {
        public static SettlementSpecification Level1 = new SettlementSpecification
        {
            MaximumNumberOfWorkers = 100,
            CarrierResourceLimit = 20,
            NumberOfCarriers = 20
        };

        public static SettlementSpecification Level2 = new SettlementSpecification
        {
            MaximumNumberOfWorkers = 200,
            CarrierResourceLimit = 40,
            NumberOfCarriers = 50
        };

        public static SettlementSpecification GetSpecifications(int level)
        {
            switch (level)
            {
                case 2: return Level2;
                default: return Level1;
            }
        }
    }

    public class SettlementSpecification
    {
        public int MaximumNumberOfWorkers { get; set; }
        public int NumberOfCarriers { get; set; }
        public int CarrierResourceLimit { get; set; }
    }
}