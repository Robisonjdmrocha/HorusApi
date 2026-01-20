using System.Data;
using Dapper;
using HorusV2.Domain.Data.Repositories;
using HorusV2.Domain.Queries.Response;
using HorusV2.Infrastructure.Data.Relational;

namespace HorusV2.Infrastructure.Data.Repositories;

public class HorusIntegrationRepository : BaseRepository, IHorusIntegrationRepository
{
    private const string GetAllDispensesFromDateQuery = @"EXEC SP_HORUS_movimentacoes
															@tipoOper = 'SEL'
															,@acao = 'BuscarDispensacao'
															,@Year = @Year
															,@Month  = @Month
															,@Day = @Day";

    private const string GetAllEntriesFromDateQuery = @"EXEC SP_HORUS_movimentacoes
															@tipoOper = 'SEL'
															,@acao = 'BuscarEntradas'
															,@Year = @Year
															,@Month  = @Month
                                                            , @Day = @Day";

    private const string GetAllExitsFromDateQuery = @"EXEC SP_HORUS_movimentacoes
															@tipoOper = 'SEL'
															,@acao = 'BuscarSaidas'
															,@Year = @Year
															,@Month  = @Month
															,@Day = @Day";

    private const string GetAllStockPositionsFromDateQuery = @"EXEC SP_HORUS_movimentacoes
																@tipoOper = 'SEL'
																,@acao = 'BuscaEstoque'																
																,@Year = @Year
																,@Month  = @Month
                                                                ,@Day = @Day";


	public HorusIntegrationRepository(RelationalDatabaseContext relationalDatabaseContext,
        IDbTransaction databaseTransaction) : base(relationalDatabaseContext, databaseTransaction)
    {
    }

    public async Task<IEnumerable<DispensationByDateQueryResponse>> GetAllDispensationsByDate(int year, int month, int day)
    {
        DynamicParameters parameters = new();

        parameters.Add("@Year", year);
        parameters.Add("@Month", month);
        parameters.Add("@Day", day);

        return await QueryAsync<DispensationByDateQueryResponse>(GetAllDispensesFromDateQuery,
            parameters);
    }
    
    public async Task<IEnumerable<EntriesByDateQueryResponse>> GetAllEntriesByDate(int year, int month, int day)
    {
	    DynamicParameters parameters = new();

	    parameters.Add("@Year", year);
	    parameters.Add("@Month", month);
	    parameters.Add("@Day", day);


	    return await QueryAsync<EntriesByDateQueryResponse>(GetAllEntriesFromDateQuery,
		    parameters);
    }
    
    public async Task<IEnumerable<ExitsByDateQueryResponse>> GetAllExitsByDate(int year, int month,int day)
    {
	    DynamicParameters parameters = new();

	    parameters.Add("@Year", year);
	    parameters.Add("@Month", month);
	    parameters.Add("@Day", day);

	    return await QueryAsync<ExitsByDateQueryResponse>(GetAllExitsFromDateQuery,
		    parameters);
    }
    
    public async Task<IEnumerable<StockPositionsByDateQueryResponse>> GetAllStockPositionsByDate(int year, int month, int day)
    {
	    DynamicParameters parameters = new();

	    parameters.Add("@Year", year);
	    parameters.Add("@Month", month);
	    parameters.Add("@Day", day);

	    return await QueryAsync<StockPositionsByDateQueryResponse>(GetAllStockPositionsFromDateQuery,
		    parameters);
    }
}