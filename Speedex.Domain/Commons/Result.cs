namespace Speedex.Domain.Commons;

public abstract record Result<TSuccess, TError>
{
    public TResult Match<TResult>(Func<TSuccess, TResult> onSuccess, Func<TError, TResult> onFailure) =>
        this switch
        {
            TSuccess s => onSuccess(s),
            TError f => onFailure(f),
            _ => throw new InvalidOperationException()
        };
    
    public async Task<TResult> MatchAsync<TResult>(Func<TSuccess, Task<TResult>> onSuccess, Func<TError, Task<TResult>> onFailure)
    {
        return this switch
        {
            TSuccess s => await onSuccess(s),
            TError f => await onFailure(f),
            _ => throw new InvalidOperationException()
        };
    }
}