﻿namespace CmdCoffee.Cli
{
    public interface IAppSettings
    {
        ShippingAddress ShippingAddress { get; set; }
        string ContactEmail { get; }
    }
}