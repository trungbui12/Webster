using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Webster.Migrations
{
    /// <inheritdoc />
    public partial class newwww : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CandidateTestSections",
                columns: table => new
                {
                    CandidateTestSectionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CandidateId = table.Column<int>(type: "int", nullable: false),
                    TestSectionId = table.Column<int>(type: "int", nullable: false),
                    IsStarted = table.Column<bool>(type: "bit", nullable: false),
                    StartedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CompletedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Score = table.Column<int>(type: "int", nullable: false),
                    IsPassed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateTestSections", x => x.CandidateTestSectionId);
                    table.ForeignKey(
                        name: "FK_CandidateTestSections_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "CandidateId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CandidateTestSections_TestSections_TestSectionId",
                        column: x => x.TestSectionId,
                        principalTable: "TestSections",
                        principalColumn: "TestSectionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CandidateTestSections_CandidateId_TestSectionId",
                table: "CandidateTestSections",
                columns: new[] { "CandidateId", "TestSectionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CandidateTestSections_TestSectionId",
                table: "CandidateTestSections",
                column: "TestSectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CandidateTestSections");
        }
    }
}
