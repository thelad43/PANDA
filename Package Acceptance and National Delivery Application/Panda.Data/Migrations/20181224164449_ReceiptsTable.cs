namespace Panda.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Metadata;
    using Microsoft.EntityFrameworkCore.Migrations;
    using System;

    public partial class ReceiptsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ReceiptId",
                table: "Packages",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Receipts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Fee = table.Column<decimal>(nullable: false),
                    IssuedOn = table.Column<DateTime>(nullable: false),
                    RecipientId = table.Column<string>(nullable: true),
                    PackageId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Receipts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Receipts_AspNetUsers_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Packages_ReceiptId",
                table: "Packages",
                column: "ReceiptId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_RecipientId",
                table: "Receipts",
                column: "RecipientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Receipts_ReceiptId",
                table: "Packages",
                column: "ReceiptId",
                principalTable: "Receipts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Receipts_ReceiptId",
                table: "Packages");

            migrationBuilder.DropTable(
                name: "Receipts");

            migrationBuilder.DropIndex(
                name: "IX_Packages_ReceiptId",
                table: "Packages");

            migrationBuilder.DropColumn(
                name: "ReceiptId",
                table: "Packages");
        }
    }
}