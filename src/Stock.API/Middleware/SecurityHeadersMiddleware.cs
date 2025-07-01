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
            string csp;
            if (context.Request.Path.StartsWithSegments("/swagger"))
            {
                // Swagger UI için daha esnek bir CSP. 'unsafe-inline' gerektirir.
                csp = "default-src 'self'; " +
                      "script-src 'self' 'unsafe-inline'; " +
                      "style-src 'self' 'unsafe-inline'; " +
                      "img-src 'self' data:; " +
                      "font-src 'self'; " +
                      "object-src 'none'; " +
                      "frame-ancestors 'none'; " +
                      "form-action 'self'; " +
                      "base-uri 'self';";
            }
            else
            {
                // Uygulamanın geri kalanı için daha kısıtlayıcı politika.
                csp = "default-src 'self'; " +
                      "script-src 'self'; " +
                      "style-src 'self' 'unsafe-inline'; " + // PrimeNG için 'unsafe-inline' gerekli olabilir
                      "img-src 'self' data:; " +
                      "font-src 'self'; " +
                      "object-src 'none'; " +
                      "frame-ancestors 'none'; " +
                      "form-action 'self'; " +
                      "base-uri 'self';";
            }
            context.Response.Headers.Append("Content-Security-Policy", csp);

            // Referrer-Policy: Referrer başlığında hangi bilgilerin gönderileceğini kontrol eder.
            context.Response.Headers.Append("Referrer-Policy", "no-referrer");

            // Permissions-Policy: Tarayıcı özelliklerinin kullanımını kontrol eder.
            context.Response.Headers.Append("Permissions-Policy", "camera=(), microphone=(), geolocation=()");

            await _next(context);
        }
    }
} 