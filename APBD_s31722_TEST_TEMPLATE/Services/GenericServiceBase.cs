using APBD_s31722_8_API.Utils;
using APBD_s31722_TEST_TEMPLATE.Exceptions;
using APBD_s31722_TEST_TEMPLATE.Interfaces;
using Microsoft.Data.SqlClient;

namespace APBD_s31722_TEST_TEMPLATE.Services;

public abstract class GenericServiceBase<TDto, TKey> : IGenericService<TDto, TKey>
{
    protected readonly IDbClient _dbClient;
    
    protected abstract string InsertQuery { get; }
    protected abstract Func<TDto, Dictionary<string, object>> InsertParams { get; }

    protected abstract string UpdateQuery  { get; }
    protected abstract Func<TKey, TDto, Dictionary<string, object>> UpdateParams { get; }

    protected abstract string DeleteQuery  { get; }
    protected abstract Func<TKey, Dictionary<string, object>> DeleteParams { get; }
    
    protected abstract string SelectAllQuery { get; }
    protected abstract string SelectByIdQuery { get; }
    protected abstract Func<SqlDataReader, TDto> Mapper { get; }

    protected GenericServiceBase(IDbClient dbClient)
    {
        _dbClient = dbClient;
    }

    public virtual Task<List<TDto>> GetAllAsync()
        => _dbClient.ReadDataAsync<TDto>(SelectAllQuery, Mapper)
            .ToListAsync();

    public virtual async Task<TDto> GetByIdAsync(TKey id)
    {
        var list = await _dbClient.ReadDataAsync<TDto>(
            SelectByIdQuery,
            reader => Mapper(reader),
            new Dictionary<string, object> { ["id"] = id }).ToListAsync();
        if (list.Count == 0) throw new ApiExceptions(ErrorStatusCode.NotFound, $"No data found for id: {id}");
        return list[0];
    }
    
    public virtual async Task<TDto> CreateAsync(TDto dto)
    {
        var newId = await _dbClient.ReadScalarAsync<TKey>(InsertQuery, InsertParams(dto));
        return await GetByIdAsync(newId);
    }

    public virtual async Task<TDto> UpdateAsync(TKey id, TDto dto)
    {
        var affectedRows = await _dbClient.ExecuteNonQueryAsync(UpdateQuery,UpdateParams(id,dto));
        if(affectedRows == 0) throw new ApiExceptions(ErrorStatusCode.NotFound, $"No data found for id: {id}");
        return await GetByIdAsync(id);
    }

    public virtual async Task DeleteAsync(TKey id)
    {
        var affectedRows = await _dbClient.ExecuteNonQueryAsync(DeleteQuery,DeleteParams(id));
        if(affectedRows == 0) throw new ApiExceptions(ErrorStatusCode.NotFound, $"No data found for id: {id}");
    }
}