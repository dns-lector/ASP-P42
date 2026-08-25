using System.Globalization;

namespace ASP_P42.Middleware.AuthSession
{
    // Класи Middleware повинні мати певну структуру:
    // - конструктор має приймати посилання на наступний Middleware - next
    // - для подальшого використання посилання має бути збережене
    // - за умовами (або безумовно) робота має передаватись далі
    public class AuthSessionMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            // context, що передається параметром, це той самий 
            // HttpContext, що доступний з контролерів
            // Відповідно, до нього можна закласти дані, що можуть
            // бути використані як у контролерах, так і у представленнях
            context.Items.Add("itemKey", "Item Value");

            // передача роботи до наступного Middleware
            await _next(context);
        }
    }
}
/* Д.З. Вивести на головній сторінці сайту дані,
 * що закладені до контексту запиту в AuthSessionMiddleware
 * (context.Items.Add("itemKey", "Item Value");)
 * * відключити midleware, переконатись, що дані зникають
 */