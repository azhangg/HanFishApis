using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repositories.Migrations
{
    /// <inheritdoc />
    public partial class v0013 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Good_GoodId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_GoodId",
                table: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_Order_GoodId",
                table: "Order",
                column: "GoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Good_GoodId",
                table: "Order",
                column: "GoodId",
                principalTable: "Good",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Good_GoodId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_GoodId",
                table: "Order");

            migrationBuilder.CreateIndex(
                name: "IX_Order_GoodId",
                table: "Order",
                column: "GoodId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Good_GoodId",
                table: "Order",
                column: "GoodId",
                principalTable: "Good",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
