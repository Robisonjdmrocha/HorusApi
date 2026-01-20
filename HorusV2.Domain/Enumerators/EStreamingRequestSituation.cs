namespace HorusV2.Domain.Enumerators;

public enum EStreamingRequestSituation
{
    Processando = 1,
    ErroInterno = 2,
    ConflitoExterno = 3,
    TransmitidoComSucesso = 4,
    ConflitoInterno = 5
}