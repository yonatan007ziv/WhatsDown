using WhatsDown.Core.Models;

namespace WhatsDown.Core.Interfaces;

public interface IChatUserMessageSerializer
	: ISerializer<IEnumerable<ChatModel>>
	, ISerializer<ChatModel>
	, ISerializer<UserModel>
	, ISerializer<MessageModel>
{

}