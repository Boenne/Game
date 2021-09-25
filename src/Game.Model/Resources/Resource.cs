namespace Game.Model.Resources
{
    public abstract class Resource
    {
        public string Name { get; protected set; }
        public int Quantity { get; set; }
    }
}