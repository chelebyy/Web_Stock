using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Common.CQRS
{
    /// <summary>
    /// Query arayüzü - sistemden veri almak için kullanılır
    /// </summary>
    /// <typeparam name="TResponse">Query'nin döndüreceği yanıt tipi</typeparam>
    public interface IQuery<TResponse>
    {
    }
} 