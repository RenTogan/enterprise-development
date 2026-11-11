using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarWash.Infrastructure.PostgreSQL.Migrations;

/// <inheritdoc />
public partial class Initial : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Cars",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                LicensePlate = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                Brand = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Cars", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Clients",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Phone = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Clients", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Services",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Category = table.Column<int>(type: "integer", nullable: false),
                Price = table.Column<decimal>(type: "numeric(18,2)", precision: 18, scale: 2, nullable: false),
                DurationMinutes = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Services", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ClientCars",
            columns: table => new
            {
                ClientId = table.Column<int>(type: "integer", nullable: false),
                CarId = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ClientCars", x => new { x.ClientId, x.CarId });
                table.ForeignKey(
                    name: "FK_ClientCars_Cars_CarId",
                    column: x => x.CarId,
                    principalTable: "Cars",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
                table.ForeignKey(
                    name: "FK_ClientCars_Clients_ClientId",
                    column: x => x.ClientId,
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Orders",
            columns: table => new
            {
                Id = table.Column<int>(type: "integer", nullable: false)
                    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                ClientId = table.Column<int>(type: "integer", nullable: false),
                ServiceId = table.Column<int>(type: "integer", nullable: false),
                CarId = table.Column<int>(type: "integer", nullable: false),
                StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                BoxNumber = table.Column<int>(type: "integer", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Orders", x => x.Id);
                table.ForeignKey(
                    name: "FK_Orders_Cars_CarId",
                    column: x => x.CarId,
                    principalTable: "Cars",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Orders_Clients_ClientId",
                    column: x => x.ClientId,
                    principalTable: "Clients",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Orders_Services_ServiceId",
                    column: x => x.ServiceId,
                    principalTable: "Services",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.InsertData(
            table: "Cars",
            columns: new[] { "Id", "Brand", "LicensePlate" },
            values: new object[,]
            {
                { 1, "Lada Azimut", "А111АА163" },
                { 2, "Toyota Camry", "В222ВВ163" },
                { 3, "BELAZ", "С333СС163" },
                { 4, "BMW X6", "Е444ЕЕ163" },
                { 5, "BMW X5", "D555DD163" },
                { 6, "Kia Rio", "К666КК163" },
                { 7, "Hyundai Solaris", "М777ММ163" },
                { 8, "GAZelle", "Н888НН163" },
                { 9, "Audi A6", "О999ОО163" },
                { 10, "Renault Logan", "Р000РР163" }
            });

        migrationBuilder.InsertData(
            table: "Clients",
            columns: new[] { "Id", "FullName", "Phone" },
            values: new object[,]
            {
                { 1, "Яцентюк Данила", "+79991112233" },
                { 2, "Мансуров Роман", null },
                { 3, "Сарычев Никита", "+79993334455" },
                { 4, "Смирнова Анна", "+79995556677" },
                { 5, "Кузнецов Александр", null },
                { 6, "Кушнерев Георгий", "+79997778899" },
                { 7, "Васильев Сергей", "+79998881122" },
                { 8, "Соколов Михаил", "+79993332211" },
                { 9, "Новикова Ольга", "+79994445566" },
                { 10, "Морозов Павел", "+79990001122" }
            });

        migrationBuilder.InsertData(
            table: "Services",
            columns: new[] { "Id", "Category", "DurationMinutes", "Name", "Price" },
            values: new object[,]
            {
                { 1, 0, 20, "Экспресс мойка", 500m },
                { 2, 0, 45, "Комплексная мойка", 1200m },
                { 3, 1, 60, "Мойка грузовика", 2500m },
                { 4, 0, 120, "Химчистка салона", 5000m },
                { 5, 3, 30, "Мойка двигателя", 1500m },
                { 6, 0, 90, "Полировка кузова", 3000m },
                { 7, 0, 10, "Чернение шин", 300m },
                { 8, 3, 25, "Мойка днища", 800m },
                { 9, 0, 15, "Восковое покрытие", 700m },
                { 10, 2, 40, "Удаление битумных пятен", 1000m }
            });

        migrationBuilder.InsertData(
            table: "ClientCars",
            columns: new[] { "CarId", "ClientId" },
            values: new object[,]
            {
                { 1, 1 },
                { 2, 2 },
                { 5, 2 },
                { 3, 3 },
                { 4, 4 },
                { 6, 7 },
                { 7, 7 },
                { 8, 8 },
                { 9, 9 },
                { 10, 10 }
            });

        migrationBuilder.InsertData(
            table: "Orders",
            columns: new[] { "Id", "BoxNumber", "CarId", "ClientId", "ServiceId", "StartTime" },
            values: new object[,]
            {
                { 1, 1, 1, 1, 1, new DateTime(2026, 5, 10, 10, 0, 0, 0, DateTimeKind.Utc) },
                { 2, 1, 1, 1, 1, new DateTime(2026, 5, 10, 12, 0, 0, 0, DateTimeKind.Utc) },
                { 3, 2, 1, 1, 2, new DateTime(2026, 5, 11, 10, 0, 0, 0, DateTimeKind.Utc) },
                { 4, 1, 1, 1, 7, new DateTime(2026, 5, 12, 10, 0, 0, 0, DateTimeKind.Utc) },
                { 5, 1, 1, 1, 9, new DateTime(2026, 5, 13, 10, 0, 0, 0, DateTimeKind.Utc) },
                { 6, 2, 2, 2, 2, new DateTime(2026, 5, 10, 10, 0, 0, 0, DateTimeKind.Utc) },
                { 7, 1, 2, 2, 2, new DateTime(2026, 5, 10, 13, 0, 0, 0, DateTimeKind.Utc) },
                { 8, 3, 5, 2, 5, new DateTime(2026, 5, 11, 10, 0, 0, 0, DateTimeKind.Utc) },
                { 9, 2, 5, 2, 1, new DateTime(2026, 5, 12, 10, 0, 0, 0, DateTimeKind.Utc) },
                { 10, 3, 3, 3, 3, new DateTime(2026, 5, 10, 10, 0, 0, 0, DateTimeKind.Utc) },
                { 11, 3, 3, 3, 3, new DateTime(2026, 5, 12, 10, 0, 0, 0, DateTimeKind.Utc) },
                { 12, 3, 3, 3, 3, new DateTime(2026, 5, 14, 10, 0, 0, 0, DateTimeKind.Utc) },
                { 13, 4, 4, 4, 5, new DateTime(2026, 5, 10, 10, 10, 0, 0, DateTimeKind.Utc) },
                { 14, 1, 4, 4, 1, new DateTime(2026, 5, 11, 10, 0, 0, 0, DateTimeKind.Utc) },
                { 15, 2, 1, 5, 4, new DateTime(2026, 5, 10, 15, 0, 0, 0, DateTimeKind.Utc) },
                { 16, 2, 1, 5, 6, new DateTime(2026, 5, 13, 10, 0, 0, 0, DateTimeKind.Utc) },
                { 17, 4, 2, 6, 8, new DateTime(2026, 5, 10, 14, 0, 0, 0, DateTimeKind.Utc) },
                { 18, 5, 6, 7, 10, new DateTime(2026, 5, 10, 11, 0, 0, 0, DateTimeKind.Utc) },
                { 19, 5, 8, 8, 2, new DateTime(2026, 5, 10, 12, 0, 0, 0, DateTimeKind.Utc) },
                { 20, 5, 9, 9, 1, new DateTime(2026, 5, 10, 13, 0, 0, 0, DateTimeKind.Utc) }
            });

        migrationBuilder.CreateIndex(
            name: "IX_Cars_LicensePlate",
            table: "Cars",
            column: "LicensePlate",
            unique: true);

        migrationBuilder.CreateIndex(
            name: "IX_ClientCars_CarId",
            table: "ClientCars",
            column: "CarId");

        migrationBuilder.CreateIndex(
            name: "IX_Orders_CarId",
            table: "Orders",
            column: "CarId");

        migrationBuilder.CreateIndex(
            name: "IX_Orders_ClientId",
            table: "Orders",
            column: "ClientId");

        migrationBuilder.CreateIndex(
            name: "IX_Orders_ServiceId",
            table: "Orders",
            column: "ServiceId");
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "ClientCars");

        migrationBuilder.DropTable(
            name: "Orders");

        migrationBuilder.DropTable(
            name: "Cars");

        migrationBuilder.DropTable(
            name: "Clients");

        migrationBuilder.DropTable(
            name: "Services");
    }
}
