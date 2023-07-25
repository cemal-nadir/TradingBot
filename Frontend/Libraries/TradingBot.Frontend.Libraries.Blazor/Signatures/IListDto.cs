namespace TradingBot.Frontend.Libraries.Blazor.Signatures;

public interface IListDto
{
}
public interface IListDto<TKey> : IListDto
{
    public TKey Id { get; set; }
}