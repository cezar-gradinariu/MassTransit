﻿namespace Contracts.Requests
{
    public class ComplexRequest : RequestBase
    {
        public string Name { get; set; }
        public string Brand { get; set; }
        public int Amount { get; set; }
    }
}