namespace HorusV2.HorusIntegration.Entities.Dispensation;

public class EstabelecimentoDispensador
{
    public EstabelecimentoDispensador()
    {
    }

    public EstabelecimentoDispensador(string cNES)
    {
        CNES = cNES;
    }

    public string CNES { get; set; }
}