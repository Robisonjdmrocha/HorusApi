using System.Data;
using Dapper;
using HorusV2.Domain.Data.Repositories;
using HorusV2.Domain.Entities;
using HorusV2.Domain.Queries.Parameters;
using HorusV2.Domain.Queries.Response;
using HorusV2.Infrastructure.Data.Relational;

namespace HorusV2.Infrastructure.Data.Repositories;

public class StreamingRequestHistoryRepository : BaseRepository, IStreamingRequestHistoryRepository
{
    public StreamingRequestHistoryRepository(RelationalDatabaseContext relationalDatabaseContext,
        IDbTransaction databaseTransaction) : base(relationalDatabaseContext, databaseTransaction)
    {
    }

    public async Task Add(StreamingRequestHistory streamingRequestHistory)
    {
        streamingRequestHistory.Id = await ExecuteScalarAsync(_addScript, streamingRequestHistory);
    }

    public async Task Update(StreamingRequestHistory streamingRequestHistory)
    {
        await ExecuteScalarAsync(_updateStatusScript, streamingRequestHistory);
    }

    public async Task<IEnumerable<StreamingRequestHistory>> GetByMonthAndYear(int day, int month, int year)
    {
        DynamicParameters parameters = new();

        parameters.Add("@Day", day);
        parameters.Add("@Month", month);
        parameters.Add("@Year", year);

        return await QueryAsync<StreamingRequestHistory>(_getByMonthAndYearScript, parameters);
    }

    public async Task<IEnumerable<SearchStreamingRequestsQueryResponse>> Search(SearchStreamingRequestQuery searchQuery)
    {
        DynamicParameters parameters = new();

        parameters.Add("@StreamingDay", searchQuery.Day);
        parameters.Add("@StreamingMonth", searchQuery.Month);
        parameters.Add("@StreamingYear", searchQuery.Year);
        parameters.Add("@Offset", searchQuery.Offset);
        parameters.Add("@Take", searchQuery.Take);

        return await QueryAsync<SearchStreamingRequestsQueryResponse>(_searchScript, parameters);
    }

    #region SqlScripts

    private const string _addScript = @"
                                        INSERT INTO Historico_Solicitacoes_Transmissao
                                        (
                                            identificador_unico, sigsm_id_usuario, 
                                            data_requisicao, data_atualizacao, id_situacao_requisicao, 
                                            situacao_requisicao, mensagem_auxiliar,
                                            dia_solicitacao,mes_solicitado, ano_solicitado
                                        ) OUTPUT Inserted.id
                                        VALUES
                                        (
                                            @UniqueIdentifier, @SigsmUserId, 
                                            @RequestDate, null, @RequestSituationId,
                                            @RequestSituation, @AuxiliaryMessage,
                                            @StreamingDay,@StreamingMonth, @StreamingYear
                                        );";

    private const string _getByMonthAndYearScript = @"
									                    SELECT 
                                                            H.id AS Id,
	                                                        H.identificador_unico AS UniqueIdentifier,
	                                                        H.sigsm_id_usuario AS SigsmUserId,
	                                                        H.data_requisicao AS RequestDate,
	                                                        H.data_atualizacao AS UpdateDate,
	                                                        H.id_situacao_requisicao AS RequestSituationId,
	                                                        H.situacao_requisicao AS RequestSituation,
	                                                        H.mensagem_auxiliar AS AuxiliaryMessage,
                                                            H.dia_solicitacao AS StreamingDay,
                                                            H.mes_solicitado AS StreamingMonth,
                                                            H.ano_solicitado AS StreamingYear
                                                        FROM
                                                            historico_solicitacoes_transmissao H
                                                        WHERE 
                                                            H.dia_solicitacao = @Day AND
                                                            H.mes_solicitado = @Month AND
                                                            H.ano_solicitado = @Year";

    private const string _searchScript = @"SELECT
                                                H.identificador_unico AS RequestUID,
	                                            H.sigsm_id_usuario AS SigsmUserId,
	                                            H.data_requisicao AS RequestDate,
                                                H.data_atualizacao AS UpdateDate,
	                                            H.id_situacao_requisicao AS RequestSituationId,
	                                            H.situacao_requisicao AS RequestSituation,
	                                            H.mensagem_auxiliar AS AuxiliaryMessage,
                                                H.dia_solicitacao AS StreamingDay,
                                                H.mes_solicitado AS StreamingMonth,
                                                H.ano_solicitado AS StreamingYear,
	                                            U.Nome AS SigsmUsername,
                                                COUNT(H.Id) OVER() AS TotalItens
                                            FROM
                                                historico_solicitacoes_transmissao H
                                            INNER JOIN ASSMED_USUARIO U 
	                                            ON H.sigsm_id_usuario = U.CodUsu
                                            WHERE	
	                                            (@StreamingDay IS NULL OR H.dia_solicitacao = @StreamingDay)	
	                                            AND (@StreamingMonth IS NULL OR H.mes_solicitado = @StreamingMonth)	
	                                            AND (@StreamingYear IS NULL OR H.ano_solicitado = @StreamingYear)	
                                            ORDER BY RequestDate DESC
                                            OFFSET @Offset ROWS FETCH NEXT @Take ROWS ONLY";

    private const string _updateStatusScript = @"UPDATE Historico_Solicitacoes_Transmissao SET 
                                                     data_atualizacao = @UpdateDate,
                                                     id_situacao_requisicao = @RequestSituationId,
                                                     situacao_requisicao = @RequestSituation,
                                                     mensagem_auxiliar = @AuxiliaryMessage
	                                                 WHERE id = @Id;";

    #endregion
}