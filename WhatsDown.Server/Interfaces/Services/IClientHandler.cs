﻿namespace WhatsDown.Server.Interfaces.Services;

internal interface IClientHandler
{
    Task ReadMessageLoop();
}