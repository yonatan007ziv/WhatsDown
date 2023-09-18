using System.Threading.Tasks;
using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.WPF.MVVM.Models;

namespace WhatsDown.WPF.Interfaces.RequestResponse;

internal interface IRegisterRequestResponseHandler : IRequestResponseHandler<RegisterModel, RegisterResult>
{
    Task<RegisterResult> RegisterProcedure(RegisterModel model);
}