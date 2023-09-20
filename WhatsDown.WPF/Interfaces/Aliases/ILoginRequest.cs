﻿using WhatsDown.Core.CommunicationProtocol.Enums;
using WhatsDown.Core.Interfaces;
using WhatsDown.Core.Models;

namespace WhatsDown.WPF.Interfaces.Aliases;

internal interface ILoginRequest
    : IRequestResponseProcedure<CredentialsModel, LoginResult>
{

}