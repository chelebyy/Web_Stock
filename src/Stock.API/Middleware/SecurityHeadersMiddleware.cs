using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Stock.API.Middleware
{
    public class SecurityHeadersMiddleware
    {
        private readonly RequestDelegate _next;

        public SecurityHeadersMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // X-Content-Type-Options: Tarayıcının, belirtilen içerik türünden farklı bir türde içeriği "koklamasını" (MIME-sniffing) engeller.
            context.Response.Headers.Append("X-Content-Type-Options", "nosniff");

            // X-Frame-Options: Sayfanın bir <frame>, <iframe>, <embed> veya <object> içinde görüntülenip görüntülenemeyeceğini kontrol eder. Clickjacking saldırılarını önler.
            context.Response.Headers.Append("X-Frame-Options", "DENY");

            // Content-Security-Policy (CSP): XSS gibi belirli saldırı türlerini tespit etmeye ve azaltmaya yardımcı olan ek bir güvenlik katmanıdır.
            // Başlangıç için oldukça kısıtlayıcı ama güvenli bir politika belirliyoruz.
            // Gerekirse, script-src, style-src gibi direktifler daha sonra gevşetilebilir.
            context.Response.Headers.Append("Content-Security-Policy", 
                "default-src 'self'; " +
                "script-src 'self' http://me.kis.v2.scr.kaspersky-labs.com ws://me.kis.v2.scr.kaspersky-labs.com 'sha256-Tui7QoFlnLXkJCSl1/JvEZdIXTmBttnWNxzJpXomQjg=' 'sha256-REyj5JBcqxKpuKFViQ3K6WDPCDFGIqvAEX3ZHrORrtw='; " + // Kaspersky ve Swagger UI için gerekli kaynaklar eklendi.
                "style-src 'self' 'unsafe-inline'; " + // PrimeNG gibi kütüphaneler için 'unsafe-inline' gerekebilir.
                "img-src 'self' data:; " +
                "font-src 'self'; " +
                "object-src 'none'; " +
                "frame-ancestors 'none'; " +
                "form-action 'self'; " +
                "base-uri 'self';"
            );

            // Referrer-Policy: Referrer başlığında hangi bilgilerin gönderileceğini kontrol eder.
            context.Response.Headers.Append("Referrer-Policy", "no-referrer");

            // Permissions-Policy: Tarayıcı özelliklerinin kullanımını kontrol eder.
            context.Response.Headers.Append("Permissions-Policy", "camera=(), microphone=(), geolocation=()");

            await _next(context);
        }
    }
} 