using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using WhatsDown.Core.CommunicationProtocol;
using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Models;
using WhatsDown.WPF.Interfaces;

namespace WhatsDown.WPF.DI.Services.RequestResponseServices;

class ValidatedUserRequestService : IValidatedUserRequest
{
	private readonly INetworkClient client;
	private readonly ISerializer serializer;

	private readonly string ip;
	private readonly int port;

	public ValidatedUserRequestService(INetworkClient client, IConfigurationFetcher configFetcher, ISerializer serializer)
	{
		this.client = client;
		this.serializer = serializer;

		ip = configFetcher.GetStringAttribute("ServerEndpoint:Ip");
		port = configFetcher.GetIntAttribute("ServerEndpoint:Port");
	}

	public async Task<IEnumerable<ChatModel>> GetChats()
	{
		await client.Connect(IPAddress.Parse(ip), port);
		await client.ValidateToken();

		await client.WriteMessage(new MessagePacket(CommunicationType.ChatsRequest));
		MessagePacket received = await client.ReadMessage();
		if (received.InvalidPacket)
			return new List<ChatModel>();

		IEnumerable<ChatModel> chats = serializer.Deserialize<List<ChatModel>>(received.Params[0])
											?? new List<ChatModel>();
		return chats;
	}
}