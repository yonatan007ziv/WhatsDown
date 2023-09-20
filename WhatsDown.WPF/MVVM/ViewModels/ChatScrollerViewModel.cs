using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WhatsDown.Core.CommunicationProtocol;
using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Models;
using WhatsDown.WPF.Interfaces;
using WhatsDown.WPF.MVVM.MVVMCore;

namespace WhatsDown.WPF.MVVM.ViewModels;

class ChatScrollerViewModel : BaseViewModel
{
	private readonly ISerializer<IEnumerable<ChatModel>> chatUserMessageSerializer;
	private readonly INetworkClient client;
	private readonly ILogger logger;

	public IEnumerable<ChatModel> chats = new List<ChatModel>();

	public ChatScrollerViewModel(IChatUserMessageSerializer chatUserMessageSerializer, INetworkClient client, ILogger logger)
	{
		this.chatUserMessageSerializer = chatUserMessageSerializer;
		this.client = client;
		this.logger = logger;
	}

	public override async void Enter()
	{
		await StartCommunication();
		IEnumerable<ChatModel> chats = await GetChats();

		StringBuilder strBuilder = new StringBuilder("\n");
		foreach (ChatModel chat in chats)
		{
			strBuilder.Append("Titles: ").AppendLine(chat.Title);

			if (chat.Participants != null)
			{
				strBuilder.AppendLine("Participants:");
				foreach (UserModel user in chat.Participants)
					strBuilder.Append(user.DisplayName).Append("\n");
				strBuilder.Append("\n");
			}

			if (chat.Messages != null)
			{
				strBuilder.AppendLine("Messages:");
				foreach (MessageModel msg in chat.Messages)
					strBuilder.Append(msg.DisplayName).Append(" Sent: ").Append(msg.Content).Append("\n");
				strBuilder.Append("\n");
				strBuilder.Append("\n");
			}
		}

		logger.LogSuccess(strBuilder.ToString());
	}

	public override void Exit()
	{
		
	}

	private async Task StartCommunication()
	{
		await client.Connect(IPAddress.Parse("127.0.0.1"), 7777);
		await client.ValidateToken();
	}

	private async Task<IEnumerable<ChatModel>> GetChats()
	{
		await client.WriteMessage(new MessagePacket(CommunicationType.ChatsRequest));
		MessagePacket received = await client.ReadMessage();
		if (received.Result == CommunicationValid.No)
			return new List<ChatModel>();

		IEnumerable<ChatModel> chats = chatUserMessageSerializer.Deserialize(received.Params[0])
											?? new List<ChatModel>();
		return chats;
	}
}