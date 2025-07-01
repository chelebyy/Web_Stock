using Microsoft.AspNetCore.Mvc;
using Stock.Application.Common.Interfaces;
using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace Stock.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Bu endpoint'e sadece yetkili kullanıcıların erişebilmesini sağla
    public class ReportsController : ControllerBase
    {
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly IServiceProvider _serviceProvider;

        public ReportsController(IBackgroundTaskQueue backgroundTaskQueue, IServiceProvider serviceProvider)
        {
            _backgroundTaskQueue = backgroundTaskQueue;
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Ürün listesi raporu oluşturma işlemini arka planda başlatır.
        /// </summary>
        /// <returns>İsteğin kabul edildiğini ve işlemin arka planda başladığını belirten 202 Accepted yanıtı.</returns>
        [HttpPost("generate-products-report")]
        [ProducesResponseType(StatusCodes.Status202Accepted)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GenerateProductsReport()
        {
            _backgroundTaskQueue.QueueBackgroundWorkItem(async token =>
            {
                // Arka plan görevleri genellikle singleton olan bir hosted service içinde çalışır,
                // ancak IReportService gibi servisler 'scoped' olabilir.
                // Bu nedenle, her görev için manuel olarak yeni bir scope oluşturmak en iyi pratiktir.
                using (var scope = _serviceProvider.CreateScope())
                {
                    var reportService = scope.ServiceProvider.GetRequiredService<IReportService>();
                    await reportService.GenerateProductsReportAsync(token);
                }
            });

            return Accepted();
        }
    }
} 