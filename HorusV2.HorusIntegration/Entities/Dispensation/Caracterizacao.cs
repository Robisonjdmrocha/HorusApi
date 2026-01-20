namespace HorusV2.HorusIntegration.Entities.Dispensation;

public class Caracterizacao
{
    public Caracterizacao()
    {
    }

    public Caracterizacao(string codigoOrigem, string dataDispensacao)
    {
        CodigoOrigem = codigoOrigem;
        DataDispensacao = dataDispensacao;
    }

    public string CodigoOrigem { get; set; }
    public string DataDispensacao { get; set; }
}