using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFcmAndNotificationFlags : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_VerifiedByUserId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Reservations_ReservationId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_RestaurantTables_RestaurantId",
                table: "RestaurantTables");

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantId",
                table: "Reviews",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "FcmToken",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNotificationSent",
                table: "Reservations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddCheckConstraint(
                name: "CK_Review_Rating_Range",
                table: "Reviews",
                sql: "[Rating] >= 1 AND [Rating] <= 5");

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantTables_RestaurantId_TableNumber",
                table: "RestaurantTables",
                columns: new[] { "RestaurantId", "TableNumber" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_VerifiedByUserId",
                table: "Payments",
                column: "VerifiedByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Reservations_ReservationId",
                table: "Reviews",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_VerifiedByUserId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Reservations_ReservationId",
                table: "Reviews");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Review_Rating_Range",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_RestaurantTables_RestaurantId_TableNumber",
                table: "RestaurantTables");

            migrationBuilder.DropColumn(
                name: "FcmToken",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "IsNotificationSent",
                table: "Reservations");

            migrationBuilder.AlterColumn<int>(
                name: "RestaurantId",
                table: "Reviews",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RestaurantTables_RestaurantId",
                table: "RestaurantTables",
                column: "RestaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_VerifiedByUserId",
                table: "Payments",
                column: "VerifiedByUserId",
                principalTable: "Users",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Reservations_ReservationId",
                table: "Reviews",
                column: "ReservationId",
                principalTable: "Reservations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
