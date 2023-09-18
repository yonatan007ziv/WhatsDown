using System;
using WhatsDown.Core.Interfaces.Networking;

namespace WhatsDown.WPF.Interfaces;

interface INetworkClient : INetworkCommunication, IConnectionManager, IDisposable
{

}