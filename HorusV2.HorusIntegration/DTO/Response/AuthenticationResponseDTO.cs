namespace HorusV2.HorusIntegration.DTO.Response;

public class AuthenticationResponseDTO
{
    public AuthenticationResponseDTO()
    {
    }

    public AuthenticationResponseDTO(string access_Token, int expires_In)
    {
        access_token = access_Token;
        expires_in = expires_In;
    }

    public string access_token { get; set; }
    public int expires_in { get; set; }
}