namespace HorusV2.Domain.Queries.Response;

public class StockPositionsByDateQueryResponse
{
    public StockPositionsByDateQueryResponse() {}

    public StockPositionsByDateQueryResponse(string cnesEstabelecimento, string tipoEstabelecimento, string codigoOrigemPosicaoEstoque, DateTime dataPosicao, int codigoOrigemProduto, string tipoProduto, string numeroProdutoCatmat, string numeroLote, int quantidade, DateTime dataValidade, string identificacaoFabricante, int ordemEnvio)
    {
        CNESEstabelecimento = cnesEstabelecimento;
        TipoEstabelecimento = tipoEstabelecimento;
        CodigoOrigemPosicaoEstoque = codigoOrigemPosicaoEstoque;
        DataPosicao = dataPosicao;
        CodigoOrigemProduto = codigoOrigemProduto;
        TipoProduto = tipoProduto;
        NumeroProdutoCATMAT = numeroProdutoCatmat;
        NumeroLote = numeroLote;
        Quantidade = quantidade;
        DataValidade = dataValidade;
        IdentificacaoFabricante = identificacaoFabricante;
        OrdemEnvio = ordemEnvio;
    }

    public string CNESEstabelecimento { get; set; }
    public string TipoEstabelecimento { get; set; }
    public string CodigoOrigemPosicaoEstoque { get; set; }
    public DateTime DataPosicao { get; set; }
    public int CodigoOrigemProduto { get; set; }
    public string TipoProduto { get; set; }
    public string NumeroProdutoCATMAT { get; set; }
    public string NumeroLote { get; set; }
    public int Quantidade { get; set; }
    public DateTime DataValidade { get; set; }
    public string IdentificacaoFabricante { get; set; }
    public int OrdemEnvio { get; set; } 
}
