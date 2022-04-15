using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecipePortal.Db.Context.Migrations
{
    public partial class _01_new_rules_OnDelete_in_tables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recipe_composition_fields_ingredients_IngredientId",
                table: "recipe_composition_fields");

            migrationBuilder.DropForeignKey(
                name: "FK_recipe_composition_fields_recipes_RecipeId",
                table: "recipe_composition_fields");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "recipe_composition_fields",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "AuthorId",
                table: "comments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_recipe_composition_fields_ingredients_IngredientId",
                table: "recipe_composition_fields",
                column: "IngredientId",
                principalTable: "ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_recipe_composition_fields_recipes_RecipeId",
                table: "recipe_composition_fields",
                column: "RecipeId",
                principalTable: "recipes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_recipe_composition_fields_ingredients_IngredientId",
                table: "recipe_composition_fields");

            migrationBuilder.DropForeignKey(
                name: "FK_recipe_composition_fields_recipes_RecipeId",
                table: "recipe_composition_fields");

            migrationBuilder.AlterColumn<int>(
                name: "RecipeId",
                table: "recipe_composition_fields",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "AuthorId",
                table: "comments",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_recipe_composition_fields_ingredients_IngredientId",
                table: "recipe_composition_fields",
                column: "IngredientId",
                principalTable: "ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_recipe_composition_fields_recipes_RecipeId",
                table: "recipe_composition_fields",
                column: "RecipeId",
                principalTable: "recipes",
                principalColumn: "Id");
        }
    }
}
