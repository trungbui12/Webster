using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webster.Migrations
{
    /// <inheritdoc />
    public partial class FixCascadeDelete : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CandidateAnswers_Answers_AnswerId",
                table: "CandidateAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_CandidateAnswers_Questions_QuestionId",
                table: "CandidateAnswers");

            migrationBuilder.DropIndex(
                name: "IX_CandidateQuestions_CandidateId",
                table: "CandidateQuestions");

            migrationBuilder.DropIndex(
                name: "IX_CandidateAnswers_CandidateId_AnswerId",
                table: "CandidateAnswers");

            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                table: "CandidateQuestions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CandidateQuestions_CandidateId_TestSectionId_QuestionId",
                table: "CandidateQuestions",
                columns: new[] { "CandidateId", "TestSectionId", "QuestionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CandidateAnswers_CandidateId_QuestionId",
                table: "CandidateAnswers",
                columns: new[] { "CandidateId", "QuestionId" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateAnswers_Answers_AnswerId",
                table: "CandidateAnswers",
                column: "AnswerId",
                principalTable: "Answers",
                principalColumn: "AnswerId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CandidateAnswers_Questions_QuestionId",
                table: "CandidateAnswers",
                column: "QuestionId",
                principalTable: "Questions",
                principalColumn: "QuestionId",
                onDelete: ReferentialAction.Cascade);
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

            migrationBuilder.DropIndex(
                name: "IX_CandidateQuestions_CandidateId_TestSectionId_QuestionId",
                table: "CandidateQuestions");

            migrationBuilder.DropIndex(
                name: "IX_CandidateAnswers_CandidateId_QuestionId",
                table: "CandidateAnswers");

            migrationBuilder.DropColumn(
                name: "OrderIndex",
                table: "CandidateQuestions");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateQuestions_CandidateId",
                table: "CandidateQuestions",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateAnswers_CandidateId_AnswerId",
                table: "CandidateAnswers",
                columns: new[] { "CandidateId", "AnswerId" },
                unique: true,
                filter: "[AnswerId] IS NOT NULL");

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
        }
    }
}
