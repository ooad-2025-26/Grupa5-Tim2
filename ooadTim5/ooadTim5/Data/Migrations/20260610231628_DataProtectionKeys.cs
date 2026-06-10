using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ooadTim5.Data.Migrations
{
    /// <inheritdoc />
    public partial class DataProtectionKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NabavkaKnjiga_Knjiga_KnjigaId",
                table: "NabavkaKnjiga");

            migrationBuilder.DropIndex(
                name: "IX_NabavkaKnjiga_KnjigaId",
                table: "NabavkaKnjiga");

            migrationBuilder.DropColumn(
                name: "KnjigaId",
                table: "NabavkaKnjiga");

            migrationBuilder.AddColumn<int>(
                name: "BrojStranica",
                table: "NabavkaKnjiga",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GodinaIzdanja",
                table: "NabavkaKnjiga",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ISBN",
                table: "NabavkaKnjiga",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Izdavac",
                table: "NabavkaKnjiga",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Kategorija",
                table: "NabavkaKnjiga",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Naslovnica",
                table: "NabavkaKnjiga",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DataProtectionKeys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FriendlyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Xml = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DataProtectionKeys");

            migrationBuilder.DropColumn(
                name: "BrojStranica",
                table: "NabavkaKnjiga");

            migrationBuilder.DropColumn(
                name: "GodinaIzdanja",
                table: "NabavkaKnjiga");

            migrationBuilder.DropColumn(
                name: "ISBN",
                table: "NabavkaKnjiga");

            migrationBuilder.DropColumn(
                name: "Izdavac",
                table: "NabavkaKnjiga");

            migrationBuilder.DropColumn(
                name: "Kategorija",
                table: "NabavkaKnjiga");

            migrationBuilder.DropColumn(
                name: "Naslovnica",
                table: "NabavkaKnjiga");

            migrationBuilder.AddColumn<int>(
                name: "KnjigaId",
                table: "NabavkaKnjiga",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_NabavkaKnjiga_KnjigaId",
                table: "NabavkaKnjiga",
                column: "KnjigaId");

            migrationBuilder.AddForeignKey(
                name: "FK_NabavkaKnjiga_Knjiga_KnjigaId",
                table: "NabavkaKnjiga",
                column: "KnjigaId",
                principalTable: "Knjiga",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
