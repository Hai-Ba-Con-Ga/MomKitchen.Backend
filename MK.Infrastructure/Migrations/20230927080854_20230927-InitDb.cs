using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MK.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class _20230927InitDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "location",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    lat = table.Column<double>(type: "double precision", nullable: false),
                    lng = table.Column<double>(type: "double precision", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_location", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payment_type",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    provider = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payment_type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "area",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    north_id = table.Column<Guid>(type: "uuid", nullable: false),
                    south_id = table.Column<Guid>(type: "uuid", nullable: false),
                    east_id = table.Column<Guid>(type: "uuid", nullable: false),
                    west_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_area", x => x.id);
                    table.ForeignKey(
                        name: "fk_area_location_east_id",
                        column: x => x.east_id,
                        principalTable: "location",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_area_location_north_id",
                        column: x => x.north_id,
                        principalTable: "location",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_area_location_south_id",
                        column: x => x.south_id,
                        principalTable: "location",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_area_location_west_id",
                        column: x => x.west_id,
                        principalTable: "location",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    password = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    credential = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    birthday = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    avatar_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    fullname = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_user", x => x.id);
                    table.ForeignKey(
                        name: "fk_user_role_role_id",
                        column: x => x.role_id,
                        principalTable: "role",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customer",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customer", x => x.id);
                    table.ForeignKey(
                        name: "fk_customer_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "kitchen",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    address = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    location_id = table.Column<Guid>(type: "uuid", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    area_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_kitchen", x => x.id);
                    table.ForeignKey(
                        name: "fk_kitchen_area_area_id",
                        column: x => x.area_id,
                        principalTable: "area",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_kitchen_location_location_id",
                        column: x => x.location_id,
                        principalTable: "location",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_kitchen_user_owner_id",
                        column: x => x.owner_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    notification_type = table.Column<string>(type: "text", nullable: false),
                    sent_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    receiver_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_notification", x => x.id);
                    table.ForeignKey(
                        name: "fk_notification_user_receiver_id",
                        column: x => x.receiver_id,
                        principalTable: "user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dish",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    image_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    kitchen_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_dish", x => x.id);
                    table.ForeignKey(
                        name: "fk_dish_kitchen_kitchen_id",
                        column: x => x.kitchen_id,
                        principalTable: "kitchen",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "favourite_kitchen",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    kitchen_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_favourite_kitchen", x => x.id);
                    table.ForeignKey(
                        name: "fk_favourite_kitchen_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_favourite_kitchen_kitchen_kitchen_id",
                        column: x => x.kitchen_id,
                        principalTable: "kitchen",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dish_tray",
                columns: table => new
                {
                    dishies_id = table.Column<Guid>(type: "uuid", nullable: false),
                    trays_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_dish_tray", x => new { x.dishies_id, x.trays_id });
                    table.ForeignKey(
                        name: "fk_dish_tray_dish_dishies_id",
                        column: x => x.dishies_id,
                        principalTable: "dish",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "feedback",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    rating = table.Column<float>(type: "real", nullable: false),
                    img_url = table.Column<string>(type: "text", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_feedback", x => x.id);
                    table.ForeignKey(
                        name: "fk_feedback_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "meal",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    service_from = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    service_to = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    service_quantity = table.Column<int>(type: "integer", nullable: false),
                    close_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    tray_id = table.Column<Guid>(type: "uuid", nullable: false),
                    kitchen_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_meal", x => x.id);
                    table.ForeignKey(
                        name: "fk_meal_kitchen_kitchen_id",
                        column: x => x.kitchen_id,
                        principalTable: "kitchen",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "order",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    total_price = table.Column<double>(type: "double precision", nullable: false),
                    total_quantity = table.Column<int>(type: "integer", nullable: false),
                    surcharge = table.Column<decimal>(type: "numeric", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    meal_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_customer_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customer",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_meal_meal_id",
                        column: x => x.meal_id,
                        principalTable: "meal",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tray",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    img_url = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    price = table.Column<decimal>(type: "numeric", nullable: false),
                    kitchen_id = table.Column<Guid>(type: "uuid", nullable: false),
                    meal_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tray", x => x.id);
                    table.ForeignKey(
                        name: "fk_tray_kitchen_kitchen_id",
                        column: x => x.kitchen_id,
                        principalTable: "kitchen",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_tray_meal_meal_id",
                        column: x => x.meal_id,
                        principalTable: "meal",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "order_payment",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: false),
                    payment_type_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    updated_date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_payment", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_payment_order_order_id",
                        column: x => x.order_id,
                        principalTable: "order",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_order_payment_payment_type_payment_type_id",
                        column: x => x.payment_type_id,
                        principalTable: "payment_type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_area_east_id",
                table: "area",
                column: "east_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_area_north_id",
                table: "area",
                column: "north_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_area_south_id",
                table: "area",
                column: "south_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_area_west_id",
                table: "area",
                column: "west_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_customer_user_id",
                table: "customer",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_dish_kitchen_id",
                table: "dish",
                column: "kitchen_id");

            migrationBuilder.CreateIndex(
                name: "ix_dish_tray_trays_id",
                table: "dish_tray",
                column: "trays_id");

            migrationBuilder.CreateIndex(
                name: "ix_favourite_kitchen_customer_id",
                table: "favourite_kitchen",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_favourite_kitchen_kitchen_id",
                table: "favourite_kitchen",
                column: "kitchen_id");

            migrationBuilder.CreateIndex(
                name: "ix_feedback_customer_id",
                table: "feedback",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_feedback_order_id",
                table: "feedback",
                column: "order_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_kitchen_area_id",
                table: "kitchen",
                column: "area_id");

            migrationBuilder.CreateIndex(
                name: "ix_kitchen_location_id",
                table: "kitchen",
                column: "location_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_kitchen_owner_id",
                table: "kitchen",
                column: "owner_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_meal_kitchen_id",
                table: "meal",
                column: "kitchen_id");

            migrationBuilder.CreateIndex(
                name: "ix_meal_tray_id",
                table: "meal",
                column: "tray_id");

            migrationBuilder.CreateIndex(
                name: "ix_notification_receiver_id",
                table: "notification",
                column: "receiver_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_customer_id",
                table: "order",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_meal_id",
                table: "order",
                column: "meal_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_payment_order_id",
                table: "order_payment",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_payment_payment_type_id",
                table: "order_payment",
                column: "payment_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_tray_kitchen_id",
                table: "tray",
                column: "kitchen_id");

            migrationBuilder.CreateIndex(
                name: "ix_tray_meal_id",
                table: "tray",
                column: "meal_id");

            migrationBuilder.CreateIndex(
                name: "ix_user_role_id",
                table: "user",
                column: "role_id");

            migrationBuilder.AddForeignKey(
                name: "fk_dish_tray_tray_trays_id",
                table: "dish_tray",
                column: "trays_id",
                principalTable: "tray",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_feedback_order_order_id",
                table: "feedback",
                column: "order_id",
                principalTable: "order",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_meal_tray_tray_id",
                table: "meal",
                column: "tray_id",
                principalTable: "tray",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_area_location_east_id",
                table: "area");

            migrationBuilder.DropForeignKey(
                name: "fk_area_location_north_id",
                table: "area");

            migrationBuilder.DropForeignKey(
                name: "fk_area_location_south_id",
                table: "area");

            migrationBuilder.DropForeignKey(
                name: "fk_area_location_west_id",
                table: "area");

            migrationBuilder.DropForeignKey(
                name: "fk_kitchen_location_location_id",
                table: "kitchen");

            migrationBuilder.DropForeignKey(
                name: "fk_kitchen_user_owner_id",
                table: "kitchen");

            migrationBuilder.DropForeignKey(
                name: "fk_meal_kitchen_kitchen_id",
                table: "meal");

            migrationBuilder.DropForeignKey(
                name: "fk_tray_kitchen_kitchen_id",
                table: "tray");

            migrationBuilder.DropForeignKey(
                name: "fk_meal_tray_tray_id",
                table: "meal");

            migrationBuilder.DropTable(
                name: "dish_tray");

            migrationBuilder.DropTable(
                name: "favourite_kitchen");

            migrationBuilder.DropTable(
                name: "feedback");

            migrationBuilder.DropTable(
                name: "notification");

            migrationBuilder.DropTable(
                name: "order_payment");

            migrationBuilder.DropTable(
                name: "dish");

            migrationBuilder.DropTable(
                name: "order");

            migrationBuilder.DropTable(
                name: "payment_type");

            migrationBuilder.DropTable(
                name: "customer");

            migrationBuilder.DropTable(
                name: "location");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "kitchen");

            migrationBuilder.DropTable(
                name: "area");

            migrationBuilder.DropTable(
                name: "tray");

            migrationBuilder.DropTable(
                name: "meal");
        }
    }
}
