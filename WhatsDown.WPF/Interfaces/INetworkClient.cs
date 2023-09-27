using System.Threading.Tasks;
using WhatsDown.Core.Interfaces.Networking;

namespace WhatsDown.WPF.Interfaces;

interface INetworkClient : IBaseNetworkCommunication, Core.Interfaces.Networking.INetworkClient
{
	public string IntegrityToken { get; set; }

	Task<bool> ValidateToken();
}