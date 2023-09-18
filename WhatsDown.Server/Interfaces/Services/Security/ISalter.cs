namespace WhatsDown.Server.Interfaces.Services.Security;

internal interface ISalter
{
    string GenerateSalt();
}