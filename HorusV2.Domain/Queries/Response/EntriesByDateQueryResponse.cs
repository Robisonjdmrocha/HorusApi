namespace HorusV2.Domain.Queries.Response;

public class EntriesByDateQueryResponse
{
    public string CNESEstabelecimento { get; set; }
    public string TipoEstabelecimento { get; set; }
    public int CodigoOrigemEntrada { get; set; }
    public DateTime DataEntrada { get; set; }
    public string IdentificacaoDistribuidor { get; set; }
    public string TipoEntrada { get; set; }
    public string TipoProduto { get; set; }
    public int CodigoOrigemProduto { get; set; }
    public string NumeroProdutoCATMAT { get; set; }
    public string NumeroLote { get; set; }
    public DateTime DataValidade { get; set; }
    public int Quantidade { get; set; }
    public string NomeFabricante { get; set; }

    public string NumeroDocumento { get; set; }

    public decimal ValorUnitario {  get; set; }

    public EntriesByDateQueryResponse()
    {
    }

    public EntriesByDateQueryResponse(string cnesEstabelecimento, string tipoEstabelecimento, int codigoOrigemEntrada, DateTime dataEntrada, string identificacaoDistribuidor, string tipoEntrada, int codigoOrigemProduto, string numeroProdutoCatmat, string numeroLote, DateTime dataValidade, int quantidade, string nomeFabricante)
    {
        CNESEstabelecimento = cnesEstabelecimento;
        TipoEstabelecimento = tipoEstabelecimento;
        CodigoOrigemEntrada = codigoOrigemEntrada;
        DataEntrada = dataEntrada;
        IdentificacaoDistribuidor = identificacaoDistribuidor;
        TipoEntrada = tipoEntrada;
        CodigoOrigemProduto = codigoOrigemProduto;
        NumeroProdutoCATMAT = numeroProdutoCatmat;
        NumeroLote = numeroLote;
        DataValidade = dataValidade;
        Quantidade = quantidade;
        NomeFabricante = nomeFabricante;
    }
}