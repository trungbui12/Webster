using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webster.Migrations
{
    /// <inheritdoc />
    public partial class New : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CandidateAnswers_CandidateId",
                table: "CandidateAnswers");

            migrationBuilder.AddColumn<int>(
                name: "TotalQuestions",
                table: "TestSections",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Score",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "Questions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Managers",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Candidates",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Candidates",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_PassedCandidates_CandidateId",
                table: "PassedCandidates",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Managers_Username",
                table: "Managers",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_Email",
                table: "Candidates",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CandidateAnswers_AnswerId",
                table: "CandidateAnswers",
                column: "AnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateAnswers_CandidateId_QuestionId",
                table: "CandidateAnswers",
                columns: new[] { "CandidateId", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CandidateAnswers_QuestionId",
                table: "CandidateAnswers",
                column: "QuestionId");

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateAnswers_Answers_AnswerId",
                table: "CandidateAnswers",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "AnswerId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateAnswers_Questions_QuestionId",
                table: "CandidateAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PassedCandidates_Candidates_CandidateId",
                table: "PassedCandidates",
                column: "CandidateId",
                principalTable: "Candidates",
                principalColumn: "CandidateId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateAnswers_Answers_AnswerId",
                table: "CandidateAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_CandidateAnswers_Questions_QuestionId",
                table: "CandidateAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_PassedCandidates_Candidates_CandidateId",
                table: "PassedCandidates");

            migrationBuilder.DropIndex(
                name: "IX_PassedCandidates_CandidateId",
                table: "PassedCandidates");

            migrationBuilder.DropIndex(
                name: "IX_Managers_Username",
                table: "Managers");

            migrationBuilder.DropIndex(
                name: "IX_Candidates_Email",
                table: "Candidates");

            migrationBuilder.DropIndex(
                name: "IX_CandidateAnswers_AnswerId",
                table: "CandidateAnswers");

            migrationBuilder.DropIndex(
                name: "IX_CandidateAnswers_CandidateId_QuestionId",
                table: "CandidateAnswers");

            migrationBuilder.DropIndex(
                name: "IX_CandidateAnswers_QuestionId",
                table: "CandidateAnswers");

            migrationBuilder.DropColumn(
                name: "TotalQuestions",
                table: "TestSections");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Questions");

            migrationBuilder.AlterColumn<int>(
                name: "Score",
                table: "Questions",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Managers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Username",
                table: "Candidates",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Candidates",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateAnswers_CandidateId",
                table: "CandidateAnswers",
                column: "CandidateId");
        }
    }
}
