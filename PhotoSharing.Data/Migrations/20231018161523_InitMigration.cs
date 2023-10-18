using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PhotoSharing.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "common");

            migrationBuilder.EnsureSchema(
                name: "auth");

            migrationBuilder.CreateTable(
                name: "roles",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                schema: "auth",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: true),
                    email = table.Column<string>(type: "text", nullable: true),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    refresh_token = table.Column<string>(type: "text", nullable: true),
                    token_created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    token_expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    role_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                    table.ForeignKey(
                        name: "FK_users_roles_role_id",
                        column: x => x.role_id,
                        principalSchema: "auth",
                        principalTable: "roles",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "file_infos",
                schema: "common",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    file_name = table.Column<string>(type: "text", nullable: true),
                    mime_type = table.Column<byte>(type: "smallint", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    modified = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_file_infos", x => x.id);
                    table.ForeignKey(
                        name: "FK_file_infos_users_user_id",
                        column: x => x.user_id,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Friendships",
                columns: table => new
                {
                    user_first_id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_second_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Friendships", x => new { x.user_first_id, x.user_second_id });
                    table.ForeignKey(
                        name: "FK_Friendships_users_user_first_id",
                        column: x => x.user_first_id,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Friendships_users_user_second_id",
                        column: x => x.user_second_id,
                        principalSchema: "auth",
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "roles",
                columns: new[] { "id", "created", "modified", "name" },
                values: new object[,]
                {
                    { new Guid("31e53e1e-a297-4b88-8f06-bcd8aa120a43"), null, null, "user" },
                    { new Guid("ddd42cbe-45e3-42f4-975e-cb393c6849d2"), null, null, "admin" }
                });

            migrationBuilder.InsertData(
                schema: "auth",
                table: "users",
                columns: new[] { "id", "created", "email", "modified", "name", "password_hash", "refresh_token", "token_created", "token_expires", "role_id" },
                values: new object[,]
                {
                    { new Guid("31e53e1e-a297-4b88-8f06-bcd8aa120a43"), null, "user@mail.ru", null, null, "AQAAAAIAAYagAAAAEPo832kV1urxkBmYo2kDrEpCAe6ZAu5Nl00vkU5susJOicekTQyk/8QKLWdVUjtiAg==", null, null, null, new Guid("31e53e1e-a297-4b88-8f06-bcd8aa120a43") },
                    { new Guid("ddd42cbe-45e3-42f4-975e-cb393c6849d2"), null, "admin@mail.ru", null, null, "AQAAAAIAAYagAAAAEHj+dVtu6yhxua4sIZqEA4EDQNSMuk2/N2piziPI/Be57MO4YB1zE0XJ/Ck9N/VHbQ==", null, null, null, new Guid("ddd42cbe-45e3-42f4-975e-cb393c6849d2") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_file_infos_user_id",
                schema: "common",
                table: "file_infos",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_Friendships_user_second_id",
                table: "Friendships",
                column: "user_second_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                schema: "auth",
                table: "users",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "file_infos",
                schema: "common");

            migrationBuilder.DropTable(
                name: "Friendships");

            migrationBuilder.DropTable(
                name: "users",
                schema: "auth");

            migrationBuilder.DropTable(
                name: "roles",
                schema: "auth");
        }
    }
}
