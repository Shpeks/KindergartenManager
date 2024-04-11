using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using Diplom.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;

namespace Diplom.Data
{

    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Unit> Units { get; set; }
        public DbSet<MenuFood> MenuFoods { get; set; }
        public DbSet<Menu> Menus { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<MealTime> MealTimes { get; set; }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Vault> Vaults { get; set; }
        public DbSet<VaultNote> VaultNotes { get; set; }
        
        public DbSet<ProductConsumption> ProductConsumptions { get; set; }
        public DbSet<PreviousBalance> PreviousBalances { get; set; }
        public DbSet<Arrival> Arrivals { get; set; }
        public DbSet<Food> Foods { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema(null); // Задает схему для базы данных
            builder.Entity<ApplicationUser>
                (entity =>

            /* Переименовывает таблицу пользователей из AspNetUsers в Identity.User. */

            {
                entity.ToTable(name: "User");
            });
            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });
            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });
            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });
            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");
            });
            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });
            
            builder.Entity<Menu>()
                .HasOne(v => v.ApplicationUsers)
                .WithMany(v => v.Menus)
                .HasForeignKey(v => v.IdUser)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MenuFood>()
                .HasOne(v => v.Menu)
                .WithMany(v => v.MenuFoods)
                .HasForeignKey(v => v.MenuId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MenuFood>()
                .HasOne(v => v.Unit)
                .WithMany(v => v.MenuFoods)
                .HasForeignKey(v => v.UnitId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MenuFood>()
                .HasOne(v => v.Meal)
                .WithMany(v => v.MenuFoods)
                .HasForeignKey(v => v.MealId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MenuFood>()
                .HasOne(v => v.MealTime)
                .WithMany(v => v.MenuFoods)
                .HasForeignKey(v => v.MealTimeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Vault>()
                .HasOne(v => v.ApplicationUsers)
                .WithMany(v => v.Vaults)
                .HasForeignKey(v => v.IdUser)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Vault>()
                .HasMany(c => c.VaultNotes)
                .WithOne(s => s.Vault)
                .HasForeignKey(s => s.IdVault)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Arrival>()
                .HasOne(a => a.Food)
                .WithMany(v => v.Arrivals)
                .HasForeignKey(a => a.IdFood)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PreviousBalance>()
                .HasOne(p => p.Food)
                .WithMany(v => v.PreviousBalances)
                .HasForeignKey(p => p.IdFood)
                .OnDelete(DeleteBehavior.Restrict);

            
            builder.Entity<ProductConsumption>()
                .HasOne(pc => pc.Food)
                .WithMany(v => v.ProductConsumptions)
                .HasForeignKey(pc => pc.IdFood)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Arrival>()
                .HasOne(v => v.VaultNote)
                .WithMany(v => v.Arrivals)
                .HasForeignKey(v => v.IdVaultNote)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductConsumption>()
                .HasOne(v => v.VaultNote)
                .WithMany(v => v.ProductConsumptions)
                .HasForeignKey(v => v.IdVaultNote)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<PreviousBalance>()
                .HasOne(v => v.VaultNote)
                .WithMany(v => v.PreviousBalances)
                .HasForeignKey(v => v.IdVaultNote)
                .OnDelete(DeleteBehavior.Cascade);

            


            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly()); // Применяем все конфигурации из текущей сборки

        }
        public class UnitConfiguration : IEntityTypeConfiguration<Unit>
        {
            public void Configure(EntityTypeBuilder<Unit> builder)
            {
                builder.HasData(
                    new Unit { Id = 1, Name = "Кг" },
                    new Unit { Id = 2, Name = "Л" },
                    new Unit { Id = 3, Name = "шт" }
                    );
            }

        }
        public class MealTimeConfiguration : IEntityTypeConfiguration<MealTime>
        {
            public void Configure(EntityTypeBuilder<MealTime> builder)
            {
                builder.HasData(
                    new MealTime { Id = 1, Name = "Завтрак  " },
                    new MealTime { Id = 2, Name = "2-ой завтрак" },
                    new MealTime { Id = 3, Name = "Обед" },
                    new MealTime { Id = 4, Name = "Полдник" },
                    new MealTime { Id = 5, Name = "Ужин" }
                    );
            }

        }
        public class MealConfiguration : IEntityTypeConfiguration<Meal>
        {
            public void Configure(EntityTypeBuilder<Meal> builder)
            {
                builder.HasData(
                    new Meal { Id = 1, Name = "Суп молочный с макаронными изделиями" },
                    new Meal { Id = 2, Name = "Кофейный напиток с молоком" },
                    new Meal { Id = 3, Name = "Батон с маслом (20/5)" },
                    new Meal { Id = 4, Name = "Сок фруктовый" },
                    new Meal { Id = 5, Name = "Капуста припущенная" },
                    new Meal { Id = 6, Name = "Борщ со свежей капустой, картофелем на м/к бульоне со сметаной" },
                    new Meal { Id = 7, Name = "Суфле из печени" },
                    new Meal { Id = 8, Name = "Соус красный основной" },
                    new Meal { Id = 9, Name = "Гречка отварная с маслом" },
                    new Meal { Id = 10, Name = "Компот из смеси сухофруктов" },
                    new Meal { Id = 11, Name = "Хлеб  пшеничный" },
                    new Meal { Id = 12, Name = "Хлеб   ржаной" },
                    new Meal { Id = 13, Name = "Снежок" },
                    new Meal { Id = 14, Name = "Ватрушка с повидлом" },
                    new Meal { Id = 15, Name = "Котлета/биточек рыбный" },
                    new Meal { Id = 16, Name = "Рагу овощное" },
                    new Meal { Id = 19, Name = "Чай с сахаром" }
                    );
            }

        }
        
        
        
        public class FoodConfiguration : IEntityTypeConfiguration<Food>
        {
            public void Configure(EntityTypeBuilder<Food> builder)
            {
                builder.HasData(
                    new Food { Id =  1, NameFood = "апельсины" },
                        new Food { Id =  2, NameFood = "вермешель" },
                        new Food { Id =  3, NameFood = "ванилин" },
                        new Food { Id =  4, NameFood = "вафли" },
                        new Food { Id =  5, NameFood = "геркулес" },
                        new Food { Id =  6, NameFood = "горбуша свежемороженная б/г" },
                        new Food { Id =  7, NameFood = "горох" },
                        new Food { Id =  8, NameFood = "горошек консервированный" },
                        new Food { Id =  9, NameFood = "груши" },
                        new Food { Id =  10, NameFood = "дрожжи" },
                        new Food { Id =  11, NameFood = "зефир" },
                        new Food { Id =  12, NameFood = "изюм" },
                        new Food { Id =  13, NameFood = "икра кабачковая" },
                        new Food { Id =  14, NameFood = "какао" },
                        new Food { Id =  15, NameFood = "капуста" },
                        new Food { Id =  16, NameFood = "картофель" },
                        new Food { Id =  17, NameFood = "кефир" },
                        new Food { Id =  18, NameFood = "кисель" },
                        new Food { Id =  19, NameFood = "компотная смесь (сухофрукты)" },
                        new Food { Id =  20, NameFood = "крахмал" },
                        new Food { Id =  21, NameFood = "кофейный напиток" },
                        new Food { Id =  22, NameFood = "крупа гречневая" },
                        new Food { Id =  23, NameFood = "крупа кукурузная" },
                        new Food { Id =  24, NameFood = "крупа манная" },
                        new Food { Id =  25, NameFood = "крупа перловая" },
                        new Food { Id =  26, NameFood = "крупа пшеничная" },
                        new Food { Id =  27, NameFood = "крупа ячневая" },
                        new Food { Id =  28, NameFood = "кукуруза консервированная" },
                        new Food { Id =  29, NameFood = "лимонная кислота" },
                        new Food { Id =  30, NameFood = "лавровый лист" },
                        new Food { Id =  31, NameFood = "лимоны" },
                        new Food { Id =  32, NameFood = "лук" },
                        new Food { Id =  33, NameFood = "макаронные изделия" },
                        new Food { Id =  34, NameFood = "масло растительное" },
                        new Food { Id =  35, NameFood = "масло сливочное" },
                        new Food { Id =  36, NameFood = "минтай с/м б/г" },
                        new Food { Id =  37, NameFood = "молоко свежее 2,5%" },
                        new Food { Id =  38, NameFood = "молоко сгущенное" },
                        new Food { Id =  39, NameFood = "молоко сухое" },
                        new Food { Id =  40, NameFood = "морковь" },
                        new Food { Id =  41, NameFood = "мука пшеничная" },
                        new Food { Id =  42, NameFood = "мясо говядина без кости" },
                        new Food { Id =  43, NameFood = "мясо говядина на кости" },
                        new Food { Id =  44, NameFood = "мясо птицы" },
                        new Food { Id =  45, NameFood = "огурцы соленые" },
                        new Food { Id =  46, NameFood = "паста томатная" },
                        new Food { Id =  47, NameFood = "печень говяжья" },
                        new Food { Id =  48, NameFood = "печенье" },
                        new Food { Id =  49, NameFood = "повидло" },
                        new Food { Id =  50, NameFood = "пшено" },
                        new Food { Id =  51, NameFood = "рис" },
                        new Food { Id =  52, NameFood = "ряженка" },
                        new Food { Id =  53, NameFood = "сахар" },
                        new Food { Id =  54, NameFood = "свекла" },
                        new Food { Id =  55, NameFood = "сельдь слабосоленая" },
                        new Food { Id =  56, NameFood = "сметана" },
                        new Food { Id =  57, NameFood = "снежок (кг)" },
                        new Food { Id =  58, NameFood = "сок фруктовый" },
                        new Food { Id =  59, NameFood = "с.м ягода" },
                        new Food { Id =  60, NameFood = "соль йодированная" },
                        new Food { Id =  61, NameFood = "сушки" },
                        new Food { Id =  62, NameFood = "сыр" },
                        new Food { Id =  63, NameFood = "творог" },
                        new Food { Id =  64, NameFood = "тушенка" },
                        new Food { Id =  65, NameFood = "хлеб пшеничный" },
                        new Food { Id =  66, NameFood = "хлеб ржаной" },
                        new Food { Id =  67, NameFood = "чай" },
                        new Food { Id =  68, NameFood = "чернослив" },
                        new Food { Id =  69, NameFood = "шиповник" },
                        new Food { Id =  70, NameFood = "яблоки" },
                        new Food { Id =  71, NameFood = "яйцо" }


                    );
            }

        }
        
    }

}
