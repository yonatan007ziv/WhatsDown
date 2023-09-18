using System.Threading.Tasks;

namespace WhatsDown.WPF.Interfaces.RequestResponse;

internal interface IRequestResponseHandler<TPost, TResult>
{
    Task PostRequest(TPost post);
    Task<TResult> GetResponse();
}