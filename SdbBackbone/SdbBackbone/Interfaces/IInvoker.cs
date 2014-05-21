namespace SdbBackbone.Interfaces
{
    public interface IInvoker<in TRequest, out TResponse>
    {
        TResponse Invoke(TRequest request);
    }
}