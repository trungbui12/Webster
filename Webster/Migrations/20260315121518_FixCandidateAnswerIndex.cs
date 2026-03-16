using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webster.Migrations
{
    /// <inheritdoc />
    public partial class FixCandidateAnswerIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CandidateAnswers_CandidateId_QuestionId",
                table: "CandidateAnswers");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateAnswers_CandidateId_QuestionId_AnswerId",
                table: "CandidateAnswers",
                columns: new[] { "CandidateId", "QuestionId", "AnswerId" },
                unique: true,
                filter: "[AnswerId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CandidateAnswers_CandidateId_QuestionId_AnswerId",
                table: "CandidateAnswers");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateAnswers_CandidateId_QuestionId",
                table: "CandidateAnswers",
                columns: new[] { "CandidateId", "QuestionId" },
                unique: true);
        }
    }
}
