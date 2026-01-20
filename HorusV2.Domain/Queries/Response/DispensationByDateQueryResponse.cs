namespace HorusV2.Domain.Queries.Response;

public class DispensationByDateQueryResponse
{
    public DispensationByDateQueryResponse()
    {
    }

    public DispensationByDateQueryResponse(string cNESEstabelecimento,
        int codigoOrigemDispensacao,
        DateTime dataDispensacao,
        int codigoOrigemProduto,
        string numeroProdutoCATMAT,
        string numeroLote,
        int quantidade,
        DateTime dataValidade,
        string documentoUsuarioSus)
    {
        CNESEstabelecimento = cNESEstabelecimento;
        CodigoOrigemDispensacao = codigoOrigemDispensacao;
        DataDispensacao = dataDispensacao;
        CodigoOrigemProduto = codigoOrigemProduto;
        NumeroProdutoCATMAT = numeroProdutoCATMAT;
        NumeroLote = numeroLote;
        Quantidade = quantidade;
        DataValidade = dataValidade;
        DocumentoUsuarioSus = documentoUsuarioSus;
    }

    public string CNESEstabelecimento { get; set; }
    public int CodigoOrigemDispensacao { get; set; }
    public DateTime DataDispensacao { get; set; }
    public int CodigoOrigemProduto { get; set; }
    public string TipoProduto { get; set; }
    public string NumeroProdutoCATMAT { get; set; }
    public string NumeroLote { get; set; }
    public int Quantidade { get; set; }
    public DateTime DataValidade { get; set; }
    public string DocumentoUsuarioSus { get; set; }
}