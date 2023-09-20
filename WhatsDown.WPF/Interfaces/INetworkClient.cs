using System;
using System.Threading.Tasks;
using WhatsDown.Core.Interfaces.Networking;

namespace WhatsDown.WPF.Interfaces;

interface INetworkClient : INetworkCommunication, IConnectionManager, IDisposable
{
	public string IntegrityToken { get; set; }

	Task<bool> ValidateToken();
}