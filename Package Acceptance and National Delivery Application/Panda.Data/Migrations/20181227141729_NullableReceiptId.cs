namespace Panda.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class NullableReceiptId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Receipts_ReceiptId",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_ReceiptId",
                table: "Packages");

            migrationBuilder.AlterColumn<int>(
                name: "ReceiptId",
                table: "Packages",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Packages_ReceiptId",
                table: "Packages",
                column: "ReceiptId",
                unique: true,
                filter: "[ReceiptId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Receipts_ReceiptId",
                table: "Packages",
                column: "ReceiptId",
                principalTable: "Receipts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Receipts_ReceiptId",
                table: "Packages");

            migrationBuilder.DropIndex(
                name: "IX_Packages_ReceiptId",
                table: "Packages");

            migrationBuilder.AlterColumn<int>(
                name: "ReceiptId",
                table: "Packages",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Packages_ReceiptId",
                table: "Packages",
                column: "ReceiptId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Receipts_ReceiptId",
                table: "Packages",
                column: "ReceiptId",
                principalTable: "Receipts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}