using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.DataBase.Migrations
{
    public partial class ProductsSeed : Migration
    {
        private static readonly string[] columns = new[] { "id", "name", "description", "price", "stock" };

        /// <summary>
        /// Inserts seed data into the "products" table. This includes 10 predefined products with
        /// specified id, name, description, price, and stock values.
        /// </summary>
        /// <param name="migrationBuilder">The <see cref="MigrationBuilder"/> used to construct operations.</param>
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "products",
                columns: columns,
                values: new object[,]
                {
                    { 1, "Product 1", "Description for product 1", 10.99m, 100 },
                    { 2, "Product 2", "Description for product 2", 12.49m, 150 },
                    { 3, "Product 3", "Description for product 3", 8.75m, 200 },
                    { 4, "Product 4", "Description for product 4", 15.00m, 80 },
                    { 5, "Product 5", "Description for product 5", 9.99m, 60 },
                    { 6, "Product 6", "Description for product 6", 20.00m, 50 },
                    { 7, "Product 7", "Description for product 7", 14.30m, 120 },
                    { 8, "Product 8", "Description for product 8", 7.99m, 90 },
                    { 9, "Product 9", "Description for product 9", 11.50m, 110 },
                    { 10, "Product 10", "Description for product 10", 13.75m, 130 }
                }
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "products",
                keyColumn: "id",
                keyValues: new object[] { 1,2,3,4,5,6,7,8,9,10 }
            );
        }
    }
}
