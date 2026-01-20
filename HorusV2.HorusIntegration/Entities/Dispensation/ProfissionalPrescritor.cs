namespace HorusV2.HorusIntegration.Entities.Dispensation;

public class ProfissionalPrescritor
{
    public ProfissionalPrescritor()
    {
    }

    public ProfissionalPrescritor(string cnesEstabelecimentoPrescritor, string cns, string cpf, string numeroCrm,
        string ufcrm)
    {
        CnesEstabelecimentoPrescritor = cnesEstabelecimentoPrescritor;
        Cns = cns;
        Cpf = cpf;
        NumeroCrm = numeroCrm;
        Ufcrm = ufcrm;
    }

    public string CnesEstabelecimentoPrescritor { get; set; }
    public string Cns { get; set; }
    public string Cpf { get; set; }
    public string NumeroCrm { get; set; }
    public string Ufcrm { get; set; }
}