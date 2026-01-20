using System.Collections.Concurrent;
using System.Text.RegularExpressions;
using HorusV2.Core.Helpers;
using HorusV2.Domain.Queries.Response;
using HorusV2.HorusIntegration.DTO;
using HorusV2.HorusIntegration.Entities;
using HorusV2.HorusIntegration.Entities.Dispensation;

namespace HorusV2.HorusIntegration.Factories;

public static class RequestDtoFactory
{
    public static TransmitDispensingBatchRequestDTO ConvertToIntegrationDto(
        this IEnumerable<DispensationByDateQueryResponse> source)
    {
        DispensationByDateQueryResponse firstElement = source.First();

        ConcurrentBag<Item> dispensationItems = new();

        source.ForEach(query =>
        {
            Item item = new()
            {
                CodigoOrigem = query.CodigoOrigemDispensacao.ToString(),
                Lote = query.NumeroLote,
                Quantidade = query.Quantidade,
                Numero = query.NumeroProdutoCATMAT,
                DataValidade = query.DataValidade.ToString("yyyy-MM-dd"),
                TipoProduto = query.TipoProduto
            };

            dispensationItems.Add(item);
        });

        return new TransmitDispensingBatchRequestDTO
        {
            Caracterizacao = new Caracterizacao
            {
                CodigoOrigem = firstElement.CodigoOrigemDispensacao.ToString(),
                DataDispensacao = firstElement.DataDispensacao.ToString("yyyy-MM-dd")
            },
            UsuarioSus = new UsuarioSus(firstElement.DocumentoUsuarioSus),
            EstabelecimentoDispensador = new EstabelecimentoDispensador
            {
                CNES = firstElement.CNESEstabelecimento
            },
            Itens = dispensationItems
        };
    }
    
    public static TransmitEntriesBatchRequestDTO ConvertToIntegrationDto(
        this IEnumerable<EntriesByDateQueryResponse> source)
    {
        EntriesByDateQueryResponse firstElement = source.First();

        ConcurrentBag<EntradaItem> entryItens = new();

        source.ForEach(query =>
        {
            EntradaItem item = new()
            {
                CodigoOrigem = query.CodigoOrigemEntrada.ToString(),
                Numero = query.NumeroProdutoCATMAT,
                TipoProduto = query.TipoProduto,
                Lote = query.NumeroLote,
                Quantidade = query.Quantidade,
                DataValidade = query.DataValidade.ToString("yyyy-MM-dd"),
                CnpjFabricante = query.IdentificacaoDistribuidor.GetNumbers().PadLeft(14, '0'),
                ValorUnitario = query.ValorUnitario
            };

            entryItens.Add(item);
        });

        return new TransmitEntriesBatchRequestDTO
        {
            Caracterizacao = new EntradaCaracterizacao
            {

                CodigoOrigem = firstElement.CodigoOrigemEntrada.ToString(),
                DataEntrada = firstElement.DataEntrada.ToString("yyyy-MM-dd"),
                CnesCnpjDistribuidor = firstElement.IdentificacaoDistribuidor.GetNumbers(),
                NumeroDocumento = firstElement.NumeroDocumento,
                TipoEntrada = firstElement.TipoEntrada[..firstElement.TipoEntrada.IndexOf(' ')]
            },
            Estabelecimento = new EntradaEstabelecimento()
            {
                Cnes = firstElement.CNESEstabelecimento,
                Tipo = firstElement.TipoEstabelecimento
            },
            Itens = entryItens
        };
    }
    
    public static TransmitExitBatchRequestDTO ConvertToIntegrationDto(
        this IEnumerable<ExitsByDateQueryResponse> source)
    {
        ExitsByDateQueryResponse firstElement = source.First();

        ConcurrentBag<SaidaItem> exitItens = new();

        source.ForEach(query =>
        {
            SaidaItem item = new()
            {
                //CnpjFabricante = query.IdentificacaoFabricante.GetNumbers(),                
                DataValidade = query.DataValidade.ToString("yyyy-MM-dd"),
                CodigoOrigem = query.CodigoOrigemSaida.ToString(),
                Numero = query.NumeroProdutoCATMAT,
                TipoProduto = query.TipoProduto,
                Lote = query.NumeroLote,
                NomeFabricanteInternacional = query.IdentificacaoFabricante.ToString(),
                Quantidade = query.Quantidade
            };

            exitItens.Add(item);
        });

        return new TransmitExitBatchRequestDTO
        {
            Caracterizacao = new SaidaCaracterizacao
            {
                CodigoOrigem = firstElement.CodigoOrigemSaida.ToString(),
                DataSaida = firstElement.DataSaida.ToString("yyyy-MM-dd"),
                EstabelecimentoDestino = firstElement.DocumentoEstabelecimentoDestino,
                TipoSaida = firstElement.TipoSaida
            },
            Estabelecimento = new SaidaEstabelecimento()
            {
                Cnes = firstElement.CNESEstabelecimento,
                Tipo = firstElement.TipoEstabelecimento
            },
            Itens = exitItens
        };
    }
    
    public static TransmitStockPositionBatchRequestDTO ConvertToIntegrationDto(
        this IEnumerable<StockPositionsByDateQueryResponse> source)
    {
        StockPositionsByDateQueryResponse firstElement = source.First();

        ConcurrentBag<PosicaoEstoqueItem> stockPositionItems = new();

        source.ForEach(query =>
        {
            PosicaoEstoqueItem item = new()
            {
                //CnpjFabricante = query.IdentificacaoFabricante,
                DataValidade = query.DataValidade.ToString("yyyy-MM-dd"),
                CodigoOrigem = query.CodigoOrigemProduto.ToString(),
                Numero = query.NumeroProdutoCATMAT,
                TipoProduto = query.TipoProduto,
                Lote = query.NumeroLote,
                Quantidade = query.Quantidade,
                NomeFabricanteInternacional = query.IdentificacaoFabricante.ToString()
            };

            stockPositionItems.Add(item);
        });

        return new TransmitStockPositionBatchRequestDTO
        {
            Caracterizacao = new PosicaoEstoqueCaracterizacao
            {
                CodigoOrigem = firstElement.CodigoOrigemPosicaoEstoque.ToString(),
                DataPosicaoEstoque = firstElement.DataPosicao.ToString("yyyy-MM-dd")
            },
            Estabelecimento = new PosicaoEstoqueEstabelecimento
            {
                Cnes = firstElement.CNESEstabelecimento,
                Tipo = firstElement.TipoEstabelecimento
            },
            Itens = stockPositionItems
        };
    }
}