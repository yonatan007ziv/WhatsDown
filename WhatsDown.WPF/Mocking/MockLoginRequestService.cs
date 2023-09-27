using System;
using System.Threading.Tasks;
using WhatsDown.Core.CommunicationProtocol;
using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.Core.Models;
using WhatsDown.WPF.Interfaces.Aliases;

namespace WhatsDown.WPF.Mocking;

class MockLoginRequestService : ILoginRequest
{
	public Task<LoginResult> Procedure(CredentialsModel model)
	{
		return Task.FromResult(LoginResult.Success);
	}

	public Task PostRequest(CredentialsModel post)
	{
		throw new NotImplementedException();
	}

	public Task<MessagePacket> GetResponse()
	{
		throw new NotImplementedException();
	}

	public int GetTimeoutPercentage()
	{
		return -1;
	}

	public void StopProcedure()
	{

	}
}