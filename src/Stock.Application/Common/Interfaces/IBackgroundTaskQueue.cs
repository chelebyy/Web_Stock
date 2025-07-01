using System;
using System.Threading;
using System.Threading.Tasks;

namespace Stock.Application.Common.Interfaces
{
    /// <summary>
    /// Arka planda çalıştırılacak görevler için bir kuyruk mekanizması tanımlar.
    /// </summary>
    public interface IBackgroundTaskQueue
    {
        /// <summary>
        /// Kuyruğa yeni bir iş öğesi ekler.
        /// </summary>
        /// <param name="workItem">Çalıştırılacak işi temsil eden fonksiyon.</param>
        void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem);

        /// <summary>
        /// Kuyruktan bir iş öğesini alır (dequeue).
        /// </summary>
        /// <param name="cancellationToken">İptal token'ı.</param>
        /// <returns>Kuyruktan alınan iş öğesi.</returns>
        Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
    }
} 