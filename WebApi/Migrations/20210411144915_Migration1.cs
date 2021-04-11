using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApi.Migrations
{
    public partial class Migration1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CreditCardDetails",
                columns: table => new
                {
                    CreditCardNumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CardHolder = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SecurityCode = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCardDetails", x => x.CreditCardNumber);
                });

            migrationBuilder.CreateTable(
                name: "PaymentState",
                columns: table => new
                {
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StateOfTransation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreditCardNo = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentState", x => x.TransactionId);
                    table.ForeignKey(
                        name: "FK_PaymentState_CreditCardDetails_CreditCardNo",
                        column: x => x.CreditCardNo,
                        principalTable: "CreditCardDetails",
                        principalColumn: "CreditCardNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentState_CreditCardNo",
                table: "PaymentState",
                column: "CreditCardNo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentState");

            migrationBuilder.DropTable(
                name: "CreditCardDetails");
        }
    }
}
