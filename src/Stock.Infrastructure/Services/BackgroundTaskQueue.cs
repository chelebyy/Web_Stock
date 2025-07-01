using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System;
using Stock.Application.Common.Interfaces;

namespace Stock.Infrastructure.Services
{
    /// <summary>
    /// IBackgroundTaskQueue arayüzünün somut implementasyonu.
    /// Thread-safe bir kuyruk ve sinyal mekanizması kullanır.
    /// </summary>
    public class BackgroundTaskQueue : IBackgroundTaskQueue
    {
        private readonly ConcurrentQueue<Func<CancellationToken, Task>> _workItems = new();
        private readonly SemaphoreSlim _signal = new(0);

        /// <summary>
        /// Kuyruktan bir iş öğesini alır. Eğer kuyruk boşsa, yeni bir iş eklenene kadar bekler.
        /// </summary>
        public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
        {
            await _signal.WaitAsync(cancellationToken);
            _workItems.TryDequeue(out var workItem);
            return workItem;
        }

        /// <summary>
        /// Kuyruğa yeni bir iş öğesi ekler ve bekleyen bir Dequeue işlemini serbest bırakır.
        /// </summary>
        public void QueueBackgroundWorkItem(Func<CancellationToken, Task> workItem)
        {
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            _workItems.Enqueue(workItem);
            _signal.Release();
        }
    }
} 