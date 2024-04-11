using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Diplom.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Foods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NameFood = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Foods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Meals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealTimes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Units",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Units", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UsernameChangeLimit = table.Column<int>(type: "int", nullable: false),
                    ProfilePicture = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_UserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_UserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RoleClaims_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Role_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Menus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChildHouse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChildCount = table.Column<int>(type: "int", nullable: false),
                    IdUser = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Menus", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Menus_User_IdUser",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vaults",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DateEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdUser = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vaults_User_IdUser",
                        column: x => x.IdUser,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuFoods",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountPerUnit = table.Column<double>(type: "float", nullable: false),
                    Supply = table.Column<double>(type: "float", nullable: false),
                    Code = table.Column<int>(type: "int", nullable: false),
                    MealId = table.Column<int>(type: "int", nullable: false),
                    MealTimeId = table.Column<int>(type: "int", nullable: false),
                    UnitId = table.Column<int>(type: "int", nullable: false),
                    MenuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuFoods", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuFoods_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuFoods_MealTimes_MealTimeId",
                        column: x => x.MealTimeId,
                        principalTable: "MealTimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuFoods_Menus_MenuId",
                        column: x => x.MenuId,
                        principalTable: "Menus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MenuFoods_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VaultNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChildCount = table.Column<int>(type: "int", nullable: false),
                    KidCount = table.Column<int>(type: "int", nullable: false),
                    IdVault = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VaultNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VaultNotes_Vaults_IdVault",
                        column: x => x.IdVault,
                        principalTable: "Vaults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Arrivals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdFood = table.Column<int>(type: "int", nullable: false),
                    FoodCount = table.Column<double>(type: "float", nullable: false),
                    IdVaultNote = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Arrivals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Arrivals_Foods_IdFood",
                        column: x => x.IdFood,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Arrivals_VaultNotes_IdVaultNote",
                        column: x => x.IdVaultNote,
                        principalTable: "VaultNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PreviousBalances",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StartBalance = table.Column<double>(type: "float", nullable: false),
                    EndBalance = table.Column<double>(type: "float", nullable: true),
                    IdVaultNote = table.Column<int>(type: "int", nullable: false),
                    IdFood = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PreviousBalances_Foods_IdFood",
                        column: x => x.IdFood,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PreviousBalances_VaultNotes_IdVaultNote",
                        column: x => x.IdVaultNote,
                        principalTable: "VaultNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductConsumptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FoodCountChild = table.Column<double>(type: "float", nullable: false),
                    FoodCountKid = table.Column<double>(type: "float", nullable: false),
                    IdFood = table.Column<int>(type: "int", nullable: false),
                    IdVaultNote = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductConsumptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductConsumptions_Foods_IdFood",
                        column: x => x.IdFood,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductConsumptions_VaultNotes_IdVaultNote",
                        column: x => x.IdVaultNote,
                        principalTable: "VaultNotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Foods",
                columns: new[] { "Id", "NameFood" },
                values: new object[,]
                {
                    { 1, "апельсины" },
                    { 53, "сахар" },
                    { 52, "ряженка" },
                    { 51, "рис" },
                    { 50, "пшено" },
                    { 49, "повидло" },
                    { 47, "печень говяжья" },
                    { 54, "свекла" },
                    { 46, "паста томатная" },
                    { 44, "мясо птицы" },
                    { 43, "мясо говядина на кости" },
                    { 42, "мясо говядина без кости" },
                    { 41, "мука пшеничная" },
                    { 40, "морковь" },
                    { 39, "молоко сухое" },
                    { 45, "огурцы соленые" },
                    { 38, "молоко сгущенное" },
                    { 55, "сельдь слабосоленая" },
                    { 57, "снежок (кг)" },
                    { 71, "яйцо" },
                    { 70, "яблоки" },
                    { 69, "шиповник" },
                    { 68, "чернослив" },
                    { 67, "чай" },
                    { 66, "хлеб ржаной" },
                    { 56, "сметана" },
                    { 65, "хлеб пшеничный" },
                    { 63, "творог" },
                    { 62, "сыр" },
                    { 61, "сушки" },
                    { 60, "соль йодированная" },
                    { 59, "с.м ягода" },
                    { 58, "сок фруктовый" },
                    { 64, "тушенка" },
                    { 37, "молоко свежее 2,5%" },
                    { 48, "печенье" },
                    { 35, "масло сливочное" },
                    { 16, "картофель" },
                    { 15, "капуста" },
                    { 14, "какао" },
                    { 13, "икра кабачковая" },
                    { 12, "изюм" }
                });

            migrationBuilder.InsertData(
                table: "Foods",
                columns: new[] { "Id", "NameFood" },
                values: new object[,]
                {
                    { 11, "зефир" },
                    { 10, "дрожжи" },
                    { 9, "груши" },
                    { 8, "горошек консервированный" },
                    { 36, "минтай с/м б/г" },
                    { 6, "горбуша свежемороженная б/г" },
                    { 5, "геркулес" },
                    { 4, "вафли" },
                    { 3, "ванилин" },
                    { 2, "вермешель" },
                    { 17, "кефир" },
                    { 18, "кисель" },
                    { 7, "горох" },
                    { 20, "крахмал" },
                    { 19, "компотная смесь (сухофрукты)" },
                    { 34, "масло растительное" },
                    { 33, "макаронные изделия" },
                    { 31, "лимоны" },
                    { 30, "лавровый лист" },
                    { 29, "лимонная кислота" },
                    { 28, "кукуруза консервированная" },
                    { 32, "лук" },
                    { 26, "крупа пшеничная" },
                    { 25, "крупа перловая" },
                    { 21, "кофейный напиток" },
                    { 24, "крупа манная" },
                    { 23, "крупа кукурузная" },
                    { 22, "крупа гречневая" },
                    { 27, "крупа ячневая" }
                });

            migrationBuilder.InsertData(
                table: "MealTimes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Завтрак  " },
                    { 2, "2-ой завтрак" },
                    { 3, "Обед" },
                    { 5, "Ужин" },
                    { 4, "Полдник" }
                });

            migrationBuilder.InsertData(
                table: "Meals",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 13, "Снежок" },
                    { 19, "Чай с сахаром" },
                    { 16, "Рагу овощное" },
                    { 15, "Котлета/биточек рыбный" },
                    { 14, "Ватрушка с повидлом" },
                    { 12, "Хлеб   ржаной" },
                    { 5, "Капуста припущенная" },
                    { 10, "Компот из смеси сухофруктов" }
                });

            migrationBuilder.InsertData(
                table: "Meals",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 9, "Гречка отварная с маслом" },
                    { 8, "Соус красный основной" },
                    { 7, "Суфле из печени" },
                    { 6, "Борщ со свежей капустой, картофелем на м/к бульоне со сметаной" },
                    { 4, "Сок фруктовый" },
                    { 3, "Батон с маслом (20/5)" },
                    { 2, "Кофейный напиток с молоком" },
                    { 1, "Суп молочный с макаронными изделиями" },
                    { 11, "Хлеб  пшеничный" }
                });

            migrationBuilder.InsertData(
                table: "Units",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 2, "Л" },
                    { 1, "Кг" },
                    { 3, "шт" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Arrivals_IdFood",
                table: "Arrivals",
                column: "IdFood");

            migrationBuilder.CreateIndex(
                name: "IX_Arrivals_IdVaultNote",
                table: "Arrivals",
                column: "IdVaultNote");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MenuFoods_MealId",
                table: "MenuFoods",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuFoods_MealTimeId",
                table: "MenuFoods",
                column: "MealTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuFoods_MenuId",
                table: "MenuFoods",
                column: "MenuId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuFoods_UnitId",
                table: "MenuFoods",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Menus_IdUser",
                table: "Menus",
                column: "IdUser");

            migrationBuilder.CreateIndex(
                name: "IX_PreviousBalances_IdFood",
                table: "PreviousBalances",
                column: "IdFood");

            migrationBuilder.CreateIndex(
                name: "IX_PreviousBalances_IdVaultNote",
                table: "PreviousBalances",
                column: "IdVaultNote");

            migrationBuilder.CreateIndex(
                name: "IX_ProductConsumptions_IdFood",
                table: "ProductConsumptions",
                column: "IdFood");

            migrationBuilder.CreateIndex(
                name: "IX_ProductConsumptions_IdVaultNote",
                table: "ProductConsumptions",
                column: "IdVaultNote");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "Role",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_RoleClaims_RoleId",
                table: "RoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_UserClaims_UserId",
                table: "UserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogins_UserId",
                table: "UserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_VaultNotes_IdVault",
                table: "VaultNotes",
                column: "IdVault");

            migrationBuilder.CreateIndex(
                name: "IX_Vaults_IdUser",
                table: "Vaults",
                column: "IdUser");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Arrivals");

            migrationBuilder.DropTable(
                name: "MenuFoods");

            migrationBuilder.DropTable(
                name: "PreviousBalances");

            migrationBuilder.DropTable(
                name: "ProductConsumptions");

            migrationBuilder.DropTable(
                name: "RoleClaims");

            migrationBuilder.DropTable(
                name: "UserClaims");

            migrationBuilder.DropTable(
                name: "UserLogins");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.DropTable(
                name: "MealTimes");

            migrationBuilder.DropTable(
                name: "Menus");

            migrationBuilder.DropTable(
                name: "Units");

            migrationBuilder.DropTable(
                name: "Foods");

            migrationBuilder.DropTable(
                name: "VaultNotes");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Vaults");

            migrationBuilder.DropTable(
                name: "User");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
