namespace WhatsDown.Core.CommunicationProtocol.Enums;

public enum CommunicationType
{
	// Token Validation
	TokenValidationRequest,
	TokenValidationResponse,

	// Login Related
	LoginRequest,
	LoginResponse,

	// Register Related
	RegisterRequest,
	RegisterResponse,

	ChatsRequest,
	ChatsResponse,
}