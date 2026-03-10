using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webster.Migrations
{
    /// <inheritdoc />
    public partial class AddTextAnswerToCandidateAnswer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "AnswerId",
                table: "CandidateAnswers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "TextAnswer",
                table: "CandidateAnswers",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TextAnswer",
                table: "CandidateAnswers");

            migrationBuilder.AlterColumn<int>(
                name: "AnswerId",
                table: "CandidateAnswers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
