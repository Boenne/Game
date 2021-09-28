﻿using System;

namespace Game.Model
{
    public abstract class Identifiable
    {
        protected Identifiable()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }

        protected static readonly object Lock = new object();
    }
}