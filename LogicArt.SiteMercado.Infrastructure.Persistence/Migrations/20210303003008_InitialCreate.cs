using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LogicArt.SiteMercado.Infrastructure.Persistence.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "__auth_token",
                columns: table => new
                {
                    auth_token_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    client_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    expiration = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    subject_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    modified_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK___auth_token", x => x.auth_token_id);
                });

            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    product_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    image = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    modified_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.product_id);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    user_name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    modified_at = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                    table.UniqueConstraint("AK_user_user_name", x => x.user_name);
                });

            migrationBuilder.CreateIndex(
                name: "IX___auth_token_subject_id_client_id_type_expiration",
                table: "__auth_token",
                columns: new[] { "subject_id", "client_id", "type", "expiration" });

            migrationBuilder.CreateIndex(
                name: "IX_product_created_at_modified_at",
                table: "product",
                columns: new[] { "created_at", "modified_at" });

            migrationBuilder.CreateIndex(
                name: "IX_product_product_id_version",
                table: "product",
                columns: new[] { "product_id", "version" });

            migrationBuilder.CreateIndex(
                name: "IX_user_created_at_modified_at",
                table: "user",
                columns: new[] { "created_at", "modified_at" });

            migrationBuilder.CreateIndex(
                name: "IX_user_user_id_version",
                table: "user",
                columns: new[] { "user_id", "version" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "__auth_token");

            migrationBuilder.DropTable(
                name: "product");

            migrationBuilder.DropTable(
                name: "user");
        }
    }
}
