using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoffeeMate.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "TEXT", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    ProfileImage = table.Column<string>(type: "TEXT", nullable: true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "TEXT", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityStamp = table.Column<string>(type: "TEXT", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "INTEGER", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "TEXT", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "INTEGER", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Coffees",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Origin = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coffees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    ClaimType = table.Column<string>(type: "TEXT", nullable: true),
                    ClaimValue = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderKey = table.Column<string>(type: "TEXT", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "TEXT", nullable: true),
                    UserId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    RoleId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "TEXT", nullable: false),
                    LoginProvider = table.Column<string>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Value = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Recipes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    CoffeeId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Recipes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Recipes_Coffees_CoffeeId",
                        column: x => x.CoffeeId,
                        principalTable: "Coffees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CoffeeId = table.Column<Guid>(type: "TEXT", nullable: false),
                    CreatedByUserId = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sessions_AspNetUsers_CreatedByUserId",
                        column: x => x.CreatedByUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Sessions_Coffees_CoffeeId",
                        column: x => x.CoffeeId,
                        principalTable: "Coffees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Steps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    DurationSeconds = table.Column<int>(type: "INTEGER", nullable: false),
                    RecipeId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Steps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Steps_Recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuestTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Token = table.Column<string>(type: "TEXT", nullable: false),
                    AssignedUsername = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    SessionId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuestTokens_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionSteps",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Order = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    ClaimedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CompletedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ClaimedBy = table.Column<string>(type: "TEXT", nullable: true),
                    SessionId = table.Column<Guid>(type: "TEXT", nullable: false),
                    RowVersion = table.Column<byte[]>(type: "BLOB", rowVersion: true, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionSteps_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Collaborators",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    DisplayName = table.Column<string>(type: "TEXT", nullable: false),
                    JoinedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UserId = table.Column<string>(type: "TEXT", nullable: true),
                    GuestTokenId = table.Column<Guid>(type: "TEXT", nullable: true),
                    SessionId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collaborators", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Collaborators_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Collaborators_GuestTokens_GuestTokenId",
                        column: x => x.GuestTokenId,
                        principalTable: "GuestTokens",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Collaborators_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Coffees",
                columns: new[] { "Id", "Description", "ImageUrl", "Name", "Origin" },
                values: new object[,]
                {
                    { new Guid("a1b2c3d4-0001-0001-0001-000000000001"), "A concentrated coffee brewed by forcing hot water under pressure through finely-ground beans. Bold, rich, and the foundation of most coffee drinks.", null, "Espresso", "Italy" },
                    { new Guid("a1b2c3d4-0001-0001-0001-000000000002"), "Espresso combined with steamed milk and a thin layer of foam. Smooth, creamy, and perfect for those who enjoy a milder coffee flavor.", null, "Latte", "Italy" },
                    { new Guid("a1b2c3d4-0001-0001-0001-000000000003"), "Equal parts espresso, steamed milk, and milk foam. A well-balanced drink with a velvety texture and strong coffee presence.", null, "Cappuccino", "Italy" },
                    { new Guid("a1b2c3d4-0001-0001-0001-000000000004"), "Espresso diluted with hot water, producing a coffee similar in strength to drip brew but with a distinctly different flavor profile.", null, "Americano", "United States" },
                    { new Guid("a1b2c3d4-0001-0001-0001-000000000005"), "A chocolate-flavored variant of the latte, combining espresso, steamed milk, and chocolate syrup topped with whipped cream.", null, "Mocha", "Yemen" }
                });

            migrationBuilder.InsertData(
                table: "Recipes",
                columns: new[] { "Id", "CoffeeId", "Description", "Title" },
                values: new object[,]
                {
                    { new Guid("b2c3d4e5-0002-0002-0002-000000000001"), new Guid("a1b2c3d4-0001-0001-0001-000000000001"), "A single shot of pure espresso.", "Classic Espresso" },
                    { new Guid("b2c3d4e5-0002-0002-0002-000000000002"), new Guid("a1b2c3d4-0001-0001-0001-000000000002"), "Smooth espresso with steamed milk and light foam.", "Classic Latte" },
                    { new Guid("b2c3d4e5-0002-0002-0002-000000000003"), new Guid("a1b2c3d4-0001-0001-0001-000000000003"), "Balanced espresso with equal parts steamed milk and foam.", "Classic Cappuccino" },
                    { new Guid("b2c3d4e5-0002-0002-0002-000000000004"), new Guid("a1b2c3d4-0001-0001-0001-000000000004"), "Espresso lengthened with hot water.", "Classic Americano" },
                    { new Guid("b2c3d4e5-0002-0002-0002-000000000005"), new Guid("a1b2c3d4-0001-0001-0001-000000000005"), "Chocolate espresso drink topped with whipped cream.", "Classic Mocha" }
                });

            migrationBuilder.InsertData(
                table: "Steps",
                columns: new[] { "Id", "Description", "DurationSeconds", "Name", "Order", "RecipeId" },
                values: new object[,]
                {
                    { new Guid("c3d4e5f6-0003-0001-0001-000000000001"), "Grind 18g of coffee beans to a fine consistency.", 30, "Grind Beans", 1, new Guid("b2c3d4e5-0002-0002-0002-000000000001") },
                    { new Guid("c3d4e5f6-0003-0001-0001-000000000002"), "Distribute and tamp the grounds evenly in the portafilter with ~30 lbs of pressure.", 15, "Tamp Grounds", 2, new Guid("b2c3d4e5-0002-0002-0002-000000000001") },
                    { new Guid("c3d4e5f6-0003-0001-0001-000000000003"), "Lock the portafilter and extract for 25-30 seconds until you get ~36ml of espresso.", 30, "Pull Shot", 3, new Guid("b2c3d4e5-0002-0002-0002-000000000001") },
                    { new Guid("c3d4e5f6-0003-0001-0001-000000000004"), "Pour the espresso into a pre-warmed demitasse cup and serve immediately.", 10, "Serve", 4, new Guid("b2c3d4e5-0002-0002-0002-000000000001") },
                    { new Guid("c3d4e5f6-0003-0002-0001-000000000001"), "Grind 18g of coffee beans to a fine consistency.", 30, "Grind Beans", 1, new Guid("b2c3d4e5-0002-0002-0002-000000000002") },
                    { new Guid("c3d4e5f6-0003-0002-0001-000000000002"), "Extract a double shot of espresso (~36ml) into your cup.", 30, "Pull Espresso Shot", 2, new Guid("b2c3d4e5-0002-0002-0002-000000000002") },
                    { new Guid("c3d4e5f6-0003-0002-0001-000000000003"), "Steam 240ml of milk to 65°C with a thin layer of microfoam.", 45, "Steam Milk", 3, new Guid("b2c3d4e5-0002-0002-0002-000000000002") },
                    { new Guid("c3d4e5f6-0003-0002-0001-000000000004"), "Pour the steamed milk over the espresso in a steady stream, finishing with a thin foam layer.", 15, "Pour Milk", 4, new Guid("b2c3d4e5-0002-0002-0002-000000000002") },
                    { new Guid("c3d4e5f6-0003-0002-0001-000000000005"), "Serve immediately in a large cup or glass.", 10, "Serve", 5, new Guid("b2c3d4e5-0002-0002-0002-000000000002") },
                    { new Guid("c3d4e5f6-0003-0003-0001-000000000001"), "Grind 18g of coffee beans to a fine consistency.", 30, "Grind Beans", 1, new Guid("b2c3d4e5-0002-0002-0002-000000000003") },
                    { new Guid("c3d4e5f6-0003-0003-0001-000000000002"), "Extract a double shot of espresso (~36ml) into a cappuccino cup.", 30, "Pull Espresso Shot", 2, new Guid("b2c3d4e5-0002-0002-0002-000000000003") },
                    { new Guid("c3d4e5f6-0003-0003-0001-000000000003"), "Steam 120ml of milk to 65°C, creating a thick and airy foam (more foam than a latte).", 45, "Steam & Froth Milk", 3, new Guid("b2c3d4e5-0002-0002-0002-000000000003") },
                    { new Guid("c3d4e5f6-0003-0003-0001-000000000004"), "Pour steamed milk into the espresso, then spoon the thick foam on top.", 20, "Pour & Spoon Foam", 4, new Guid("b2c3d4e5-0002-0002-0002-000000000003") },
                    { new Guid("c3d4e5f6-0003-0003-0001-000000000005"), "Optionally dust with cocoa powder or cinnamon and serve.", 10, "Dust & Serve", 5, new Guid("b2c3d4e5-0002-0002-0002-000000000003") },
                    { new Guid("c3d4e5f6-0003-0004-0001-000000000001"), "Grind 18g of coffee beans to a fine consistency.", 30, "Grind Beans", 1, new Guid("b2c3d4e5-0002-0002-0002-000000000004") },
                    { new Guid("c3d4e5f6-0003-0004-0001-000000000002"), "Extract a double shot of espresso (~36ml).", 30, "Pull Espresso Shot", 2, new Guid("b2c3d4e5-0002-0002-0002-000000000004") },
                    { new Guid("c3d4e5f6-0003-0004-0001-000000000003"), "Heat 180ml of fresh water to approximately 90°C.", 60, "Heat Water", 3, new Guid("b2c3d4e5-0002-0002-0002-000000000004") },
                    { new Guid("c3d4e5f6-0003-0004-0001-000000000004"), "Pour the hot water into the cup first, then add the espresso on top to preserve the crema.", 15, "Combine & Serve", 4, new Guid("b2c3d4e5-0002-0002-0002-000000000004") },
                    { new Guid("c3d4e5f6-0003-0005-0001-000000000001"), "Grind 18g of coffee beans to a fine consistency.", 30, "Grind Beans", 1, new Guid("b2c3d4e5-0002-0002-0002-000000000005") },
                    { new Guid("c3d4e5f6-0003-0005-0001-000000000002"), "Add 30ml of chocolate syrup or 2 tbsp of cocoa powder to the cup.", 10, "Add Chocolate", 2, new Guid("b2c3d4e5-0002-0002-0002-000000000005") },
                    { new Guid("c3d4e5f6-0003-0005-0001-000000000003"), "Extract a double shot of espresso (~36ml) directly over the chocolate.", 30, "Pull Espresso Shot", 3, new Guid("b2c3d4e5-0002-0002-0002-000000000005") },
                    { new Guid("c3d4e5f6-0003-0005-0001-000000000004"), "Stir the espresso and chocolate together until fully combined.", 15, "Stir Base", 4, new Guid("b2c3d4e5-0002-0002-0002-000000000005") },
                    { new Guid("c3d4e5f6-0003-0005-0001-000000000005"), "Steam 200ml of milk to 65°C with light foam.", 45, "Steam Milk", 5, new Guid("b2c3d4e5-0002-0002-0002-000000000005") },
                    { new Guid("c3d4e5f6-0003-0005-0001-000000000006"), "Pour steamed milk into the chocolate-espresso base, top with whipped cream.", 20, "Pour & Top", 6, new Guid("b2c3d4e5-0002-0002-0002-000000000005") },
                    { new Guid("c3d4e5f6-0003-0005-0001-000000000007"), "Drizzle chocolate syrup on top and serve immediately.", 10, "Garnish & Serve", 7, new Guid("b2c3d4e5-0002-0002-0002-000000000005") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_GuestTokenId",
                table: "Collaborators",
                column: "GuestTokenId");

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_SessionId",
                table: "Collaborators",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Collaborators_UserId",
                table: "Collaborators",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestTokens_SessionId",
                table: "GuestTokens",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestTokens_Token",
                table: "GuestTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Recipes_CoffeeId",
                table: "Recipes",
                column: "CoffeeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_CoffeeId",
                table: "Sessions",
                column: "CoffeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_CreatedByUserId",
                table: "Sessions",
                column: "CreatedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionSteps_SessionId",
                table: "SessionSteps",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Steps_RecipeId",
                table: "Steps",
                column: "RecipeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "Collaborators");

            migrationBuilder.DropTable(
                name: "SessionSteps");

            migrationBuilder.DropTable(
                name: "Steps");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "GuestTokens");

            migrationBuilder.DropTable(
                name: "Recipes");

            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Coffees");
        }
    }
}
