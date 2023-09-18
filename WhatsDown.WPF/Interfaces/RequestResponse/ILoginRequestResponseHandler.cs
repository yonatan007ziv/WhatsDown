using System.Threading.Tasks;
using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.WPF.MVVM.Models;

namespace WhatsDown.WPF.Interfaces.RequestResponse;

internal interface ILoginRequestResponseHandler : IRequestResponseHandler<LoginModel, LoginResult>
{
    Task<LoginResult> LoginProcedure(LoginModel model);
}