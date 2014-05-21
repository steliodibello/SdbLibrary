namespace SdbBackbone.Interfaces
{
    public interface IQuery<in TRequest, out TResponse> : IInvoker<TRequest, TResponse>
    {
    }
}