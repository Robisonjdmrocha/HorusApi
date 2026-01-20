namespace HorusV2.HorusIntegration.Entities.Dispensation;

public class ProfissionalDispensador
{
    public ProfissionalDispensador()
    {
        CNES = string.Empty;
        CPF = string.Empty;
        NumeroCRF = string.Empty;
        UFCRF = string.Empty;
    }

    public ProfissionalDispensador(string cnes, string cpf, string numeroCrf, string ufcrf)
    {
        CNES = cnes;
        CPF = cpf;
        NumeroCRF = numeroCrf;
        UFCRF = ufcrf;
    }

    public string CNES { get; set; }
    public string CPF { get; set; }
    public string NumeroCRF { get; set; }
    public string UFCRF { get; set; }
}