using WhatsDown.Core.CommunicationProtocol;

namespace WhatsDown.Core.Interfaces;

public interface IRequestResponseProcedure<TPost, TResult>
	where TPost : class
	where TResult : Enum
{
	Task<TResult> Procedure(TPost model);
	void StopProcedure();
	Task PostRequest(TPost post);
	Task<MessagePacket> GetResponse();
	int GetTimeoutPercentage();
}