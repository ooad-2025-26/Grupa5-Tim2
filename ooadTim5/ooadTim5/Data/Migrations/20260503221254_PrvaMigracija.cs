using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ooadTim5.Data.Migrations
{
    /// <inheritdoc />
    public partial class PrvaMigracija : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClanskaKartica",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BrojKartice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumIzdavanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClanstvoVaziDo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Aktivan = table.Column<bool>(type: "bit", nullable: false),
                    KorisnikId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClanskaKartica", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Dobavljac",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KontaktEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KontaktTelefon = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Adresa = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dobavljac", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Knjiga",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Naziv = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Autor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ISBN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Kategorija = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GodinaIzdanja = table.Column<int>(type: "int", nullable: false),
                    BrojStranica = table.Column<int>(type: "int", nullable: false),
                    Izdavac = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Naslovnica = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Knjiga", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NabavkaKnjiga",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KnjigaId = table.Column<int>(type: "int", nullable: false),
                    DobavljacId = table.Column<int>(type: "int", nullable: false),
                    Kolicina = table.Column<int>(type: "int", nullable: false),
                    DatumNarudzbe = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OcekivaniDatumIsporuke = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NabavkaKnjiga", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NabavkaKnjiga_Dobavljac_DobavljacId",
                        column: x => x.DobavljacId,
                        principalTable: "Dobavljac",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NabavkaKnjiga_Knjiga_KnjigaId",
                        column: x => x.KnjigaId,
                        principalTable: "Knjiga",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Posudba",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KnjigaId = table.Column<int>(type: "int", nullable: false),
                    ClanId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatumPosudbe = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OcekivaniDatumVracanja = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DatumVracanja = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Napomena = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posudba", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posudba_Knjiga_KnjigaId",
                        column: x => x.KnjigaId,
                        principalTable: "Knjiga",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ZahtjevZaPosudbu",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    KorisnikId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    KnjigaId = table.Column<int>(type: "int", nullable: false),
                    DatumZahtjeva = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RazlogOdbijanja = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ZahtjevZaPosudbu", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ZahtjevZaPosudbu_Knjiga_KnjigaId",
                        column: x => x.KnjigaId,
                        principalTable: "Knjiga",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NabavkaKnjiga_DobavljacId",
                table: "NabavkaKnjiga",
                column: "DobavljacId");

            migrationBuilder.CreateIndex(
                name: "IX_NabavkaKnjiga_KnjigaId",
                table: "NabavkaKnjiga",
                column: "KnjigaId");

            migrationBuilder.CreateIndex(
                name: "IX_Posudba_KnjigaId",
                table: "Posudba",
                column: "KnjigaId");

            migrationBuilder.CreateIndex(
                name: "IX_ZahtjevZaPosudbu_KnjigaId",
                table: "ZahtjevZaPosudbu",
                column: "KnjigaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClanskaKartica");

            migrationBuilder.DropTable(
                name: "NabavkaKnjiga");

            migrationBuilder.DropTable(
                name: "Posudba");

            migrationBuilder.DropTable(
                name: "ZahtjevZaPosudbu");

            migrationBuilder.DropTable(
                name: "Dobavljac");

            migrationBuilder.DropTable(
                name: "Knjiga");
        }
    }
}
