using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profiles.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameAccoundIdToAccountId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccoundId",
                table: "DoctorProfiles",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "AccoundId",
                table: "PatientProfiles",
                newName: "AccountId");

            migrationBuilder.RenameColumn(
                name: "AccoundId",
                table: "ReceptionistProfiles",
                newName: "AccountId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "DoctorProfiles",
                newName: "AccoundId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "PatientProfiles",
                newName: "AccoundId");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "ReceptionistProfiles",
                newName: "AccoundId");
        }
    }
}
