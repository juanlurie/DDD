﻿namespace Iris.Domain
{
    public interface IMemento
    {
        dynamic Identity { get; set; }
        int Version { get; set; }
    }
}