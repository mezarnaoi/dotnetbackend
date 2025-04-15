using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MobyLabWebProgramming.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreatev4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Place_Category_CategoryId",
                table: "Place");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaceTag_Place_PlaceId",
                table: "PlaceTag");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaceTag_Tag_TagId",
                table: "PlaceTag");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Place_PlaceId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaceTag",
                table: "PlaceTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Place",
                table: "Place");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "PlaceTag",
                newName: "PlaceTags");

            migrationBuilder.RenameTable(
                name: "Place",
                newName: "Places");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categories");

            migrationBuilder.RenameIndex(
                name: "IX_PlaceTag_TagId",
                table: "PlaceTags",
                newName: "IX_PlaceTags_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_Place_CategoryId",
                table: "Places",
                newName: "IX_Places_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaceTags",
                table: "PlaceTags",
                columns: new[] { "PlaceId", "TagId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Places",
                table: "Places",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_Categories_CategoryId",
                table: "Places",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaceTags_Places_PlaceId",
                table: "PlaceTags",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaceTags_Tag_TagId",
                table: "PlaceTags",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Places_PlaceId",
                table: "Review",
                column: "PlaceId",
                principalTable: "Places",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_Categories_CategoryId",
                table: "Places");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaceTags_Places_PlaceId",
                table: "PlaceTags");

            migrationBuilder.DropForeignKey(
                name: "FK_PlaceTags_Tag_TagId",
                table: "PlaceTags");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Places_PlaceId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlaceTags",
                table: "PlaceTags");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Places",
                table: "Places");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "PlaceTags",
                newName: "PlaceTag");

            migrationBuilder.RenameTable(
                name: "Places",
                newName: "Place");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Category");

            migrationBuilder.RenameIndex(
                name: "IX_PlaceTags_TagId",
                table: "PlaceTag",
                newName: "IX_PlaceTag_TagId");

            migrationBuilder.RenameIndex(
                name: "IX_Places_CategoryId",
                table: "Place",
                newName: "IX_Place_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlaceTag",
                table: "PlaceTag",
                columns: new[] { "PlaceId", "TagId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Place",
                table: "Place",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Place_Category_CategoryId",
                table: "Place",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaceTag_Place_PlaceId",
                table: "PlaceTag",
                column: "PlaceId",
                principalTable: "Place",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlaceTag_Tag_TagId",
                table: "PlaceTag",
                column: "TagId",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Place_PlaceId",
                table: "Review",
                column: "PlaceId",
                principalTable: "Place",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
