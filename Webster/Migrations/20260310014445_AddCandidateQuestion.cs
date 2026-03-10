using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webster.Migrations
{
    /// <inheritdoc />
    public partial class AddCandidateQuestion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CandidateQuestions",
                columns: table => new
                {
                    CandidateQuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidateId = table.Column<int>(type: "int", nullable: false),
                    TestSectionId = table.Column<int>(type: "int", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false),
                    AssignedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateQuestions", x => x.CandidateQuestionId);
                    table.ForeignKey(
                        name: "FK_CandidateQuestions_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "CandidateId");
                    table.ForeignKey(
                        name: "FK_CandidateQuestions_Questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "Questions",
                        principalColumn: "QuestionId");
                    table.ForeignKey(
                        name: "FK_CandidateQuestions_TestSections_TestSectionId",
                        column: x => x.TestSectionId,
                        principalTable: "TestSections",
                        principalColumn: "TestSectionId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CandidateQuestions_CandidateId",
                table: "CandidateQuestions",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateQuestions_QuestionId",
                table: "CandidateQuestions",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_CandidateQuestions_TestSectionId",
                table: "CandidateQuestions",
                column: "TestSectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandidateQuestions");
        }
    }
}
