using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class KeyPair : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "KeyBundles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentityKey = table.Column<string>(type: "text", nullable: false),
                    RegistrationId = table.Column<int>(type: "integer", nullable: false),
                    PreKey_KeyId = table.Column<int>(type: "integer", nullable: false),
                    PreKey_PublicKey = table.Column<string>(type: "text", nullable: false),
                    SignedPreKey_KeyId = table.Column<int>(type: "integer", nullable: false),
                    SignedPreKey_PublicKey = table.Column<string>(type: "text", nullable: false),
                    SignedPreKey_Signature = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KeyBundles", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "KeyBundles");
        }
    }
}
