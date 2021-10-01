using System;

namespace Game.Model
{
    public abstract class Identifiable
    {
        public Guid BaseId { get; set; }

        protected Identifiable()
        {
            BaseId = Guid.NewGuid();
        }

        public Urn Id => $"urn:{GetType().Name}:{BaseId}";

        protected static readonly object Lock = new object();
    }
}