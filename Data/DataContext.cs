namespace ASP_P42.Data
{
    public class DataContext
    {
    }
}

/*
Entity Framework - інструмент спрощеної роботи з БД
Надає засоби для уніфікації - для роботи з БД вживаються
команди основної мови проєкту (C#), однакові (або еквівалентні)
для різних СУБД.

Для підключення EF додаємо пакети NuGet:
- загальні інтерфейси -- Microsoft.EntityFrameworkCore
- їх імплементація під конкретну БД -- Microsoft.EntityFrameworkCore.SqlServer
- інструментарій командного рядка (зокрема, міграції) -- Microsoft.EntityFrameworkCore.Tools
 
Створюємо директорію (шар) проєкту Data
Entities - відбивають структуру таблиць БД. Для частини "Users":

[UsersData]        [UserAccesses]     [UserRoles]
[Id]      ---\     [Id]          /--- [Id]
[FullName]    \----[UsedId]     /     [Name]
[Email]            [RoleId] ---/      [CreateLevel]
[Phone]            [Login]            [ReadLevel]
[Birthdate]        [Salt]             [UpdateLevel]
[RegisteredAt]     [Dk]               [DeleteLevel]
[DeletedAt]
 */