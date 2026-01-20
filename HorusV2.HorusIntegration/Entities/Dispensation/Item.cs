namespace HorusV2.HorusIntegration.Entities.Dispensation;

public class Item
{
    public Item()
    {
    }

    public Item(string cnpjFabricante,
        string dataValidade,
        List<IUM> iums,
        string lote,
        string nomeFabricanteInternacional,
        string numero,
        string codigoOrigem,
        string notificacao,
        string cid10,
        Posologia posologia,
        int quantidade,
        string siglaProgramaSaude,
        string dataCompetenciaDispensacao,
        string tipoProduto,
        ProfissionalPrescritor profissionalPrescritor,
        ProfissionalDispensador profissionalDispensador)
    {
        CnpjFabricante = cnpjFabricante;
        DataValidade = dataValidade;
        Iums = iums;
        Lote = lote;
        NomeFabricanteInternacional = nomeFabricanteInternacional;
        Numero = numero;
        CodigoOrigem = codigoOrigem;
        Notificacao = notificacao;
        Cid10 = cid10;
        Posologia = posologia;
        Quantidade = quantidade;
        SiglaProgramaSaude = siglaProgramaSaude;
        DataCompetenciaDispensacao = dataCompetenciaDispensacao;
        TipoProduto = tipoProduto;
        ProfissionalPrescritor = profissionalPrescritor;
        ProfissionalDispensador = profissionalDispensador;
    }

    public string CnpjFabricante { get; set; }
    public string DataValidade { get; set; }
    public List<IUM> Iums { get; set; }
    public string Lote { get; set; }
    public string NomeFabricanteInternacional { get; set; }
    public string Numero { get; set; }
    public string CodigoOrigem { get; set; }
    public string Notificacao { get; set; }
    public string Cid10 { get; set; }
    public Posologia Posologia { get; set; }
    public int Quantidade { get; set; }
    public string SiglaProgramaSaude { get; set; }
    public string DataCompetenciaDispensacao { get; set; }
    public string TipoProduto { get; set; }
    public ProfissionalPrescritor ProfissionalPrescritor { get; set; }
    public ProfissionalDispensador ProfissionalDispensador { get; set; }
}