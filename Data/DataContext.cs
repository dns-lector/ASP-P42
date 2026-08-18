using Microsoft.EntityFrameworkCore;

namespace ASP_P42.Data
{
    public class DataContext : DbContext
    {
        public DbSet<Entities.UserData> UsersData { get; set; }
        public DbSet<Entities.UserRole> UserRoles { get; set; }
        public DbSet<Entities.UserAccess> UserAccesses { get; set; }

        // Конструювання контексту налаштовується з Program.cs
        // відповідно, на час проєктування делегується конструктор
        // з параметрами підключення.
        public DataContext(DbContextOptions options) : base(options)
        {           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Налаштування, що виконуються під час першого завантаження
            // контексту даних, зокрема, зв'язки між таблицями, унікальність
            // тощо
            modelBuilder.Entity<Entities.UserAccess>()
                .HasIndex(ua => ua.Login)
                .IsUnique();
        }
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

Д.З. Створити сторінку з описом дій для долучення
бази даних з Entity Framework до проєкту ASP
* з означенням термінології БД

 */