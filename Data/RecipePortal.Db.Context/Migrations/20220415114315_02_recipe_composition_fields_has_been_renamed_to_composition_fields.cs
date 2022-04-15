using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipePortal.Db.Context.Migrations
{
    public partial class _02_recipe_composition_fields_has_been_renamed_to_composition_fields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "recipe_composition_fields");

            migrationBuilder.CreateTable(
                name: "composition_fields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    IngredientId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_composition_fields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_composition_fields_ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_composition_fields_recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_composition_fields_IngredientId",
                table: "composition_fields",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_composition_fields_RecipeId",
                table: "composition_fields",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_composition_fields_Uid",
                table: "composition_fields",
                column: "Uid",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "composition_fields");

            migrationBuilder.CreateTable(
                name: "recipe_composition_fields",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IngredientId = table.Column<int>(type: "int", nullable: false),
                    RecipeId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipe_composition_fields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_recipe_composition_fields_ingredients_IngredientId",
                        column: x => x.IngredientId,
                        principalTable: "ingredients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_recipe_composition_fields_recipes_RecipeId",
                        column: x => x.RecipeId,
                        principalTable: "recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_recipe_composition_fields_IngredientId",
                table: "recipe_composition_fields",
                column: "IngredientId");

            migrationBuilder.CreateIndex(
                name: "IX_recipe_composition_fields_RecipeId",
                table: "recipe_composition_fields",
                column: "RecipeId");

            migrationBuilder.CreateIndex(
                name: "IX_recipe_composition_fields_Uid",
                table: "recipe_composition_fields",
                column: "Uid",
                unique: true);
        }
    }
}
