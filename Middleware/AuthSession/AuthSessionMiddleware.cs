using ASP_P42.Data;
using ASP_P42.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace ASP_P42.Middleware.AuthSession
{
    // Класи Middleware повинні мати певну структуру:
    // - конструктор має приймати посилання на наступний Middleware - next
    // - для подальшого використання посилання має бути збережене
    // - за умовами (або безумовно) робота має передаватись далі
    public class AuthSessionMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public async Task InvokeAsync(
            HttpContext context,       // інжекція через метод
            DataContext dataContext    // порядок ролі не грає, тільки тип
        )
        {
            // context, що передається параметром, це той самий 
            // HttpContext, що доступний з контролерів
            // Відповідно, до нього можна закласти дані, що можуть
            // бути використані як у контролерах, так і у представленнях
            context.Items.Add("itemKey", "Item Value");

            // перевіряємо, чи є у сесії елемент з ключем "userAccessId"
            String authKey = "userAccessId";
            if (context.Session.Keys.Contains(authKey))
            {
                String userAccessId = context.Session.GetString(authKey)!;
                // це має бути валідний рядок з БД - перевіряємо...
                UserAccess? userAccess = dataContext
                    .UserAccesses
                    .Include(ua => ua.UserData)  // інструкція для заповнення
                    .Include(ua => ua.UserRole)  // навігаційних властивостей
                    .AsNoTracking()              // Вимкнення стеження змін
                    .FirstOrDefault(ua => ua.Id.ToString() == userAccessId);
                if (userAccess != null)
                {
                    // знайдено підтвердження допуску, передаємо до контексту
                    context.Items.Add(authKey, userAccess);
                }
            }

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