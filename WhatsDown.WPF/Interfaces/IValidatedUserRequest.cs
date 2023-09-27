using System.Collections.Generic;
using System.Threading.Tasks;
using WhatsDown.Core.Models;

namespace WhatsDown.WPF.Interfaces;

interface IValidatedUserRequest
{
	Task<IEnumerable<ChatModel>> GetChats();
}