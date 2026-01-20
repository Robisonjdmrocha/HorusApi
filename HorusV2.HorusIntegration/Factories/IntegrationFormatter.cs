using HorusV2.Domain.Queries.Response;
using HorusV2.HorusIntegration.DTO;

namespace HorusV2.HorusIntegration.Factories;

public static class IntegrationFormatter
{
    /// <summary>
    ///     Agrupar todas as dispensações da lista informada por usuário (Documento usuario SUS - CPF ou Carteirinha), data de
    ///     dispensação e unidade (Número do CNES)
    ///     Para cada grupo, prepara a lista de dispensações no formato do Hórus
    ///     Após preparar a lista, sera novamente agrupado por unidade e por data para gerar um protocolo para cada grupo
    /// </summary>
    /// <param name="dispensations">Lista de dispensações à serem transmitidas</param>
    /// <returns>Grupos de dispensação no formato de transmissão do Hórus</returns>
    public static IEnumerable<IGrouping<dynamic, TransmitDispensingBatchRequestDTO>> ConvertToIntegrationFormat(
        this IEnumerable<DispensationByDateQueryResponse> dispensations)
    {
        /*
         * 1º Passo:
         *      Agrupar todas as dispensações por usuário (Documento usuario SUS - CPF ou Carteirinha), data de dispensação e unidade (Número do CNES)
         */
        IEnumerable<IGrouping<dynamic, DispensationByDateQueryResponse>> dispensationGroups = dispensations.GroupBy(
            dispensation =>
                new
                {
                    dispensation.DocumentoUsuarioSus, 
                    dispensation.DataDispensacao.Date,
                    dispensation.CNESEstabelecimento
                });

        /*
         *  2º Passo:
         *      Para cada grupo, prepara a lista de dispensações no formato do Hórus
         */ 
        IEnumerable<TransmitDispensingBatchRequestDTO> dispensationListForIntegration = dispensationGroups
            .SelectMany(dispensationGroup =>
            {
                var dispensationList = dispensationGroup.AsEnumerable().ToList(); //Obtém lista de entradas da dispensação
                var batchCount = (int)Math.Ceiling(dispensationList.Count / 20.0); //Obtém o tamanho da listan 

                return Enumerable.Range(0, batchCount).Select(batchIndex =>
                {
                    var batchItems = dispensationList.Skip(batchIndex * 20).Take(20).ToList();
                    return batchItems.ConvertToIntegrationDto(); // Converte para o formato da integração
                });
            });

        //IEnumerable<TransmitDispensingBatchRequestDTO> dispensationListForIntegration = dispensationGroups
        //    .Select(dispensationGroup => dispensationGroup.AsEnumerable()) //Obtém lista de dispensações do grupo
        //    .Select(dispensationList => dispensationList.ConvertToIntegrationDto()); //Converte par ao formato da integração
        /*
         *  3º Passo:
         *      Agora com a estrutura da integração montada, agrupar por Unidade e por Data
         *      Será gerado um protocolo por agrupamento
         */ 
         
        return dispensationListForIntegration
            .Select((dispensation, index) => new { dispensation, index }) // Adiciona um índice para cada item
            .GroupBy(x => new
            {
                x.dispensation.Caracterizacao.DataDispensacao,
                x.dispensation.EstabelecimentoDispensador.CNES,
                BatchNumber = x.index / 1000 // Divide o índice por 100 para criar grupos de 100 itens
            })
            .Select(g => new
            {
                g.Key.DataDispensacao,
                g.Key.CNES,
                Items = g.Select(x => x.dispensation)
            })
            .SelectMany(g => g.Items.GroupBy(item => new
            {
                g.DataDispensacao,
                g.CNES
            }));
    }
    
    /// <summary>
    ///     Agrupar todas as entradas da lista informada por data de
    ///     entrada e unidade (Número do CNES)
    ///     Para cada grupo, prepara a lista de entrada no formato do Hórus
    ///     Após preparar a lista, sera novamente agrupado por unidade e por data para gerar um protocolo para cada grupo
    /// </summary>
    /// <param name="entries">Lista de entradas à serem transmitidas</param>
    /// <returns>Grupos de entrada no formato de transmissão do Hórus</returns>
    public static IEnumerable<IGrouping<dynamic, TransmitEntriesBatchRequestDTO>> ConvertToIntegrationFormat(
        this IEnumerable<EntriesByDateQueryResponse> entries)
    {
        /*
         * 1º Passo:
         *      Agrupar todas as entradas data de dispensação e unidade (Número do CNES)
         */
        IEnumerable<IGrouping<dynamic, EntriesByDateQueryResponse>> entriesGroups = entries.GroupBy(
            entry => new {
                entry.CNESEstabelecimento, 
                entry.DataEntrada.Date, 
                entry.TipoEntrada, 
                entry.IdentificacaoDistribuidor
            }
        );

        /*
         *  2º Passo:
         *      Para cada grupo, prepara a lista de entradas no formato do Hórus
         */
        IEnumerable<TransmitEntriesBatchRequestDTO> entryListForIntegration = entriesGroups
            //.Select(entryGroup => entryGroup.AsEnumerable()) //Obtém lista de entradas do grupo
            //.Select(entryList => entryList.ConvertToIntegrationDto()) //Converte par ao formato da integração
            .SelectMany(entriesGroup =>
            {
                var entryList = entriesGroup.AsEnumerable().ToList();
                var batchCount = (int)Math.Ceiling(entryList.Count / 60.0); //Obtém o tamanho da listan S

                return Enumerable.Range(0, batchCount).Select(batchIndex =>
                {
                    var batchItems = entryList.Skip(batchIndex * 60).Take(60).ToList();
                    return batchItems.ConvertToIntegrationDto(); // Converte para o formato da integraçãoz'
                });

            });

        /*
         *  3º Passo:
         *      Agora com a estrutura da integração montada, agrupar por Unidade e por Data
         *      Será gerado um protocolo por agrupamento
         */
        return entryListForIntegration
            .Select((entry, index) => new { entry, index }) // Adiciona um índice para cada item
            .GroupBy(x => new
            {
                x.entry.Caracterizacao.DataEntrada,
                x.entry.Estabelecimento.Cnes,
                BatchNumber = x.index / 1000 // Divide o índice por 100 para criar grupos de 100 itens
            })
            .Select(g => new
            {
                g.Key.DataEntrada,
                g.Key.Cnes,
                Items = g.Select(x => x.entry)
            })
            .SelectMany(g => g.Items.GroupBy(item => new
            {
                g.DataEntrada,
                g.Cnes
            }));
    }
    
    /// <summary>
    ///     Agrupar todas as saídas da lista informada
    ///     Para cada grupo, prepara a lista de saídas no formato do Hórus
    ///     Após preparar a lista, sera novamente agrupado por unidade e por data para gerar um protocolo para cada grupo
    /// </summary>
    /// <param name="exits">Lista de saídas à serem transmitidas</param>
    /// <returns>Grupos de saídas no formato de transmissão do Hórus</returns>
    public static IEnumerable<IGrouping<dynamic, TransmitExitBatchRequestDTO>> ConvertToIntegrationFormat(
        this IEnumerable<ExitsByDateQueryResponse> exits)
    {
        /*
         * 1º Passo:
         *      Agrupar todas as saídas
         */
        IEnumerable<IGrouping<dynamic, ExitsByDateQueryResponse>> exitsGroups = exits.GroupBy(
            exit =>
                new
                {
                    exit.CNESEstabelecimento, 
                    exit.DataSaida.Date,
                    exit.TipoSaida, 
                    exit.DocumentoEstabelecimentoDestino
                });

        /*
         *  2º Passo:
         *      Para cada grupo, prepara a lista de saídas no formato do Hórus
         */
        IEnumerable<TransmitExitBatchRequestDTO> exitListForIntegration = exitsGroups
            .SelectMany(exitsGroup =>
            {
                var exitsGroupList = exitsGroup.AsEnumerable().ToList();
                var batchCount = (int)Math.Ceiling(exitsGroupList.Count / 60.0); //Obtém o tamanho da listan 

                return Enumerable.Range(0, batchCount).Select(batchIndex =>
                {
                    var batchItems = exitsGroupList.Skip(batchIndex * 60).Take(60).ToList();
                    return batchItems.ConvertToIntegrationDto(); // Converte para o formato da integração
                });

            }); 
        /*
         *  3º Passo:
         *      Agora com a estrutura da integração montada, agrupar por Unidade e por Data
         *      Será gerado um protocolo por agrupamento
         */
        return exitListForIntegration
            .Select((exit, index) => new { exit, index }) // Adiciona um índice para cada item
            .GroupBy(x => new
            {
                x.exit.Caracterizacao.DataSaida,
                x.exit.Estabelecimento.Cnes,
                BatchNumber = x.index / 1000 // Divide o índice por 100 para criar grupos de 100 itens
            })
            .Select(g => new
            {
                g.Key.DataSaida,
                g.Key.Cnes,
                Items = g.Select(x => x.exit)
            })
            .SelectMany(g => g.Items.GroupBy(item => new
            {
                g.DataSaida,
                g.Cnes
            })); 
    }
    
    /// <summary>
    ///     Agrupar todas as entradas da lista informada...
    ///     Para cada grupo, prepara a lista de posições de estoque no formato do Hórus
    ///     Após preparar a lista, sera novamente agrupado por unidade e por data para gerar um protocolo para cada grupo
    /// </summary>
    /// <param name="stockPositions">Lista de posições de estoque à serem transmitidas</param>
    /// <returns>Grupos de saídas no formato de transmissão do Hórus</returns>
    public static IEnumerable<IGrouping<dynamic, TransmitStockPositionBatchRequestDTO>> ConvertToIntegrationFormat(
        this IEnumerable<StockPositionsByDateQueryResponse> stockPositions)
    {
        /*
         * 1º Passo:
         *      Agrupar todas as posições de estoque
         */
        IEnumerable<IGrouping<dynamic, StockPositionsByDateQueryResponse>> stockPositionGroups = stockPositions.GroupBy(
            entry =>
                new
                {
                    entry.CNESEstabelecimento, 
                    entry.DataPosicao.Date 
                }
        );

        /*
         *  2º Passo:
         *      Para cada grupo, prepara a lista de posições de estoque no formato do Hórus
         */
        IEnumerable<TransmitStockPositionBatchRequestDTO> stockPositionListForIntegration = stockPositionGroups
            .SelectMany(stockPositionGroup =>
            {
                var stockPositionList = stockPositionGroup.AsEnumerable().ToList();
                var batchCount = (int)Math.Ceiling(stockPositionList.Count / 60.0); //Obtém o tamanho da listan 

                return Enumerable.Range(0, batchCount).Select(batchIndex =>
                {
                    var batchItems = stockPositionList.Skip(batchIndex * 60).Take(60).ToList();
                    return batchItems.ConvertToIntegrationDto(); // Converte para o formato da integração
                });

            });           

        /*
         *  3º Passo:
         *      Agora com a estrutura da integração montada, agrupar por Unidade e por Data
         *      Será gerado um protocolo por agrupamento
         */
        return stockPositionListForIntegration
            .Select((entry, index) => new { entry, index }) // Adiciona um índice para cada item
            .GroupBy(x => new
            {
                x.entry.Caracterizacao.DataPosicaoEstoque,
                x.entry.Estabelecimento.Cnes,
                BatchNumber = x.index / 1000 // Divide o índice por 100 para criar grupos de 100 itens
            })
            .Select(g => new
            {
                g.Key.DataPosicaoEstoque,
                g.Key.Cnes,
                Items = g.Select(x => x.entry)
            })
            .SelectMany(g => g.Items.GroupBy(item => new
            {
                g.DataPosicaoEstoque,
                g.Cnes
            })); 
    }
}