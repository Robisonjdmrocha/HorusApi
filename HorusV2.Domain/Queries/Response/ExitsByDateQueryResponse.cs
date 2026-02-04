namespace HorusV2.Domain.Queries.Response;

public class ExitsByDateQueryResponse
{
    public ExitsByDateQueryResponse() {}
    public ExitsByDateQueryResponse(string cnesEstabelecimento, string tipoEstabelecimento, int codigoOrigemSaida, DateTime dataSaida, int codigoOrigemProduto, string documentoEstabelecimentoDestino, string tipoSaida, string tipoProduto, string numeroProdutoCatmat, string numeroLote, int quantidade, DateTime dataValidade, string documentoUsuarioSus, string identificacaoFabricante, decimal valorUnitario)
    {
        CNESEstabelecimento = cnesEstabelecimento;
        TipoEstabelecimento = tipoEstabelecimento;
        CodigoOrigemSaida = codigoOrigemSaida;
        DataSaida = dataSaida;
        CodigoOrigemProduto = codigoOrigemProduto;
        DocumentoEstabelecimentoDestino = documentoEstabelecimentoDestino;
        TipoSaida = tipoSaida;
        TipoProduto = tipoProduto;
        NumeroProdutoCATMAT = numeroProdutoCatmat;
        NumeroLote = numeroLote;
        Quantidade = quantidade;
        DataValidade = dataValidade;
        DocumentoUsuarioSus = documentoUsuarioSus;
        IdentificacaoFabricante = identificacaoFabricante;
        ValorUnitario = valorUnitario;
    }

    public string CNESEstabelecimento { get; set; }
    public string TipoEstabelecimento { get; set; }
    public int CodigoOrigemSaida { get; set; }
    public DateTime DataSaida { get; set; }
    public int CodigoOrigemProduto { get; set; }
    public string DocumentoEstabelecimentoDestino { get; set; }
    public string TipoSaida { get; set; }
    public string TipoProduto { get; set; }
    public string NumeroProdutoCATMAT { get; set; }
    public string NumeroLote { get; set; }
    public int Quantidade { get; set; }
    public DateTime DataValidade { get; set; }
    public string DocumentoUsuarioSus { get; set; }
    public string IdentificacaoFabricante { get; set; }
    public decimal ValorUnitario { get; set; }
}
