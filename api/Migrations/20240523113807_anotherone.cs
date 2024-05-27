using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class anotherone : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Stocks_StockId",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "StockId",
                table: "Comments",
                newName: "StockID");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_StockId",
                table: "Comments",
                newName: "IX_Comments_StockID");

            migrationBuilder.AlterColumn<Guid>(
                name: "StockID",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Stocks_StockID",
                table: "Comments",
                column: "StockID",
                principalTable: "Stocks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Stocks_StockID",
                table: "Comments");

            migrationBuilder.RenameColumn(
                name: "StockID",
                table: "Comments",
                newName: "StockId");

            migrationBuilder.RenameIndex(
                name: "IX_Comments_StockID",
                table: "Comments",
                newName: "IX_Comments_StockId");

            migrationBuilder.AlterColumn<Guid>(
                name: "StockId",
                table: "Comments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Stocks_StockId",
                table: "Comments",
                column: "StockId",
                principalTable: "Stocks",
                principalColumn: "Id");
        }
    }
}
