using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Common.CQRS
{
    /// <summary>
    /// Command arayüzü - sistemde değişiklik yapan işlemleri temsil eder
    /// </summary>
    /// <typeparam name="TResponse">Command'in döndüreceği yanıt tipi</typeparam>
    public interface ICommand<TResponse>
    {
    }

    /// <summary>
    /// Yanıt döndürmeyen command'ler için arayüz
    /// </summary>
    public interface ICommand : ICommand<Unit>
    {
    }
} 