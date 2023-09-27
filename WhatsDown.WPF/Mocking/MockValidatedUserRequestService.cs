using System.Collections.Generic;
using System.Threading.Tasks;
using WhatsDown.Core.Models;
using WhatsDown.WPF.Interfaces;

namespace WhatsDown.WPF.Mocking;

class MockValidatedUserRequestService : IValidatedUserRequest
{
	public Task<IEnumerable<ChatModel>> GetChats()
	{
		IEnumerable<ChatModel> chats = new List<ChatModel>
		{
			new ChatModel
			{
				Title = "Title 1"
			},
			new ChatModel
			{
				Title = "Title 2",
			},
			new ChatModel
			{
				Title = "Title 3"
			},
			new ChatModel
			{
				Title = "Title 4"
			},
			new ChatModel
			{
				Title = "Title 5",
			},
			new ChatModel
			{
				Title = "Title 6"
			},
			new ChatModel
			{
				Title = "Title 7"
			},
			new ChatModel
			{
				Title = "Title 8",
			},
			new ChatModel
			{
				Title = "Title 9"
			},
			new ChatModel
			{
				Title = "Title 10"
			},
			new ChatModel
			{
				Title = "Title 11",
			},
			new ChatModel
			{
				Title = "Title 12"
			},
		};
		return Task.FromResult(chats);
	}
}