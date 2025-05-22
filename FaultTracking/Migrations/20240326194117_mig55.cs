using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FaultTracking.Migrations
{
    /// <inheritdoc />
    public partial class mig55 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmployeeMail",
                table: "Form",
                newName: "studentNumber");

            migrationBuilder.AddColumn<int>(
                name: "AuthorizedPersonId",
                table: "Form",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FaultTypeId",
                table: "Form",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "UserMail",
                table: "Form",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FaultType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaultType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Form_AuthorizedPersonId",
                table: "Form",
                column: "AuthorizedPersonId");

            migrationBuilder.CreateIndex(
                name: "IX_Form_FaultTypeId",
                table: "Form",
                column: "FaultTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Form_AuthorizedPerson_AuthorizedPersonId",
                table: "Form",
                column: "AuthorizedPersonId",
                principalTable: "AuthorizedPerson",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Form_FaultType_FaultTypeId",
                table: "Form",
                column: "FaultTypeId",
                principalTable: "FaultType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Form_AuthorizedPerson_AuthorizedPersonId",
                table: "Form");

            migrationBuilder.DropForeignKey(
                name: "FK_Form_FaultType_FaultTypeId",
                table: "Form");

            migrationBuilder.DropTable(
                name: "FaultType");

            migrationBuilder.DropIndex(
                name: "IX_Form_AuthorizedPersonId",
                table: "Form");

            migrationBuilder.DropIndex(
                name: "IX_Form_FaultTypeId",
                table: "Form");

            migrationBuilder.DropColumn(
                name: "AuthorizedPersonId",
                table: "Form");

            migrationBuilder.DropColumn(
                name: "FaultTypeId",
                table: "Form");

            migrationBuilder.DropColumn(
                name: "UserMail",
                table: "Form");

            migrationBuilder.RenameColumn(
                name: "studentNumber",
                table: "Form",
                newName: "EmployeeMail");
        }
    }
}
