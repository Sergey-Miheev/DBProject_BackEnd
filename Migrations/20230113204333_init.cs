using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace placeBookingAPI.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cinema",
                columns: table => new
                {
                    idcinema = table.Column<int>(name: "id_cinema", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    name = table.Column<string>(type: "character varying(44)", maxLength: 44, nullable: false),
                    cityname = table.Column<string>(name: "city_name", type: "character varying(100)", maxLength: 100, nullable: false),
                    address = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("Cinema_pkey", x => x.idcinema);
                });

            migrationBuilder.CreateTable(
                name: "film",
                columns: table => new
                {
                    idfilm = table.Column<int>(name: "id_film", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    duration = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "character varying(113)", maxLength: 113, nullable: false),
                    agerating = table.Column<short>(name: "age_rating", type: "smallint", nullable: false),
                    description = table.Column<string>(type: "character varying(1600)", maxLength: 1600, nullable: false),
                    idimage = table.Column<int>(name: "id_image", type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("film_pkey", x => x.idfilm);
                });

            migrationBuilder.CreateTable(
                name: "image",
                columns: table => new
                {
                    idimage = table.Column<int>(name: "id_image", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    url = table.Column<string>(type: "character varying(5000)", maxLength: 5000, nullable: false),
                    type = table.Column<short>(type: "smallint", nullable: false),
                    identity = table.Column<int>(name: "id_entity", type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("image_pkey", x => x.idimage);
                });

            migrationBuilder.CreateTable(
                name: "hall",
                columns: table => new
                {
                    idhall = table.Column<int>(name: "id_hall", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    idcinema = table.Column<int>(name: "id_cinema", type: "integer", nullable: false),
                    number = table.Column<short>(type: "smallint", nullable: false),
                    type = table.Column<short>(type: "smallint", nullable: false),
                    capacity = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("hall_pkey", x => x.idhall);
                    table.ForeignKey(
                        name: "hall_id_cinema_fkey",
                        column: x => x.idcinema,
                        principalTable: "cinema",
                        principalColumn: "id_cinema",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    idaccount = table.Column<int>(name: "id_account", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    email = table.Column<string>(type: "character varying(80)", maxLength: 80, nullable: false),
                    password = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    dateofbirthday = table.Column<string>(name: "date_of_birthday", type: "text", nullable: false),
                    idimage = table.Column<int>(name: "id_image", type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("user_pkey", x => x.idaccount);
                    table.ForeignKey(
                        name: "account_id_image_fkey",
                        column: x => x.idimage,
                        principalTable: "image",
                        principalColumn: "id_image",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "actor",
                columns: table => new
                {
                    idactor = table.Column<int>(name: "id_actor", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    idimage = table.Column<int>(name: "id_image", type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("actor_pkey", x => x.idactor);
                    table.ForeignKey(
                        name: "actor_id_image_fkey",
                        column: x => x.idimage,
                        principalTable: "image",
                        principalColumn: "id_image",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "place",
                columns: table => new
                {
                    idplace = table.Column<int>(name: "id_place", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    idhall = table.Column<int>(name: "id_hall", type: "integer", nullable: false),
                    row = table.Column<short>(type: "smallint", nullable: false),
                    place = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("place_pkey", x => x.idplace);
                    table.ForeignKey(
                        name: "place_id_hall_fkey",
                        column: x => x.idhall,
                        principalTable: "hall",
                        principalColumn: "id_hall",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "session",
                columns: table => new
                {
                    idsession = table.Column<int>(name: "id_session", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    idhall = table.Column<int>(name: "id_hall", type: "integer", nullable: false),
                    idfilm = table.Column<int>(name: "id_film", type: "integer", nullable: false),
                    datetime = table.Column<DateTimeOffset>(name: "date_time", type: "time with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("session_pkey", x => x.idsession);
                    table.ForeignKey(
                        name: "session_id_film_fkey",
                        column: x => x.idfilm,
                        principalTable: "film",
                        principalColumn: "id_film",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "session_id_hall_fkey",
                        column: x => x.idhall,
                        principalTable: "hall",
                        principalColumn: "id_hall",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    idrole = table.Column<int>(name: "id_role", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    idactor = table.Column<int>(name: "id_actor", type: "integer", nullable: false),
                    idfilm = table.Column<int>(name: "id_film", type: "integer", nullable: false),
                    namepersonage = table.Column<string>(name: "name_personage", type: "character varying(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("role_pkey", x => x.idrole);
                    table.ForeignKey(
                        name: "role_id_actor_fkey",
                        column: x => x.idactor,
                        principalTable: "actor",
                        principalColumn: "id_actor",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "role_id_film_fkey",
                        column: x => x.idfilm,
                        principalTable: "film",
                        principalColumn: "id_film",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "booking",
                columns: table => new
                {
                    idbooking = table.Column<int>(name: "id_booking", type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityAlwaysColumn),
                    idsession = table.Column<int>(name: "id_session", type: "integer", nullable: false),
                    idplace = table.Column<int>(name: "id_place", type: "integer", nullable: false),
                    idaccount = table.Column<int>(name: "id_account", type: "integer", nullable: false),
                    bookingcode = table.Column<int>(name: "booking_code", type: "integer", nullable: false),
                    datetime = table.Column<string>(name: "date_time", type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("booking_pkey", x => x.idbooking);
                    table.ForeignKey(
                        name: "booking_id_account_fkey",
                        column: x => x.idaccount,
                        principalTable: "account",
                        principalColumn: "id_account",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "booking_id_place_fkey",
                        column: x => x.idplace,
                        principalTable: "place",
                        principalColumn: "id_place",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "booking_id_session_fkey",
                        column: x => x.idsession,
                        principalTable: "session",
                        principalColumn: "id_session",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_id_image",
                table: "account",
                column: "id_image");

            migrationBuilder.CreateIndex(
                name: "IX_actor_id_image",
                table: "actor",
                column: "id_image");

            migrationBuilder.CreateIndex(
                name: "IX_booking_id_account",
                table: "booking",
                column: "id_account");

            migrationBuilder.CreateIndex(
                name: "IX_booking_id_place",
                table: "booking",
                column: "id_place");

            migrationBuilder.CreateIndex(
                name: "IX_booking_id_session",
                table: "booking",
                column: "id_session");

            migrationBuilder.CreateIndex(
                name: "IX_hall_id_cinema",
                table: "hall",
                column: "id_cinema");

            migrationBuilder.CreateIndex(
                name: "IX_place_id_hall",
                table: "place",
                column: "id_hall");

            migrationBuilder.CreateIndex(
                name: "IX_role_id_actor",
                table: "role",
                column: "id_actor");

            migrationBuilder.CreateIndex(
                name: "IX_role_id_film",
                table: "role",
                column: "id_film");

            migrationBuilder.CreateIndex(
                name: "IX_session_id_film",
                table: "session",
                column: "id_film");

            migrationBuilder.CreateIndex(
                name: "IX_session_id_hall",
                table: "session",
                column: "id_hall");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "booking");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "place");

            migrationBuilder.DropTable(
                name: "session");

            migrationBuilder.DropTable(
                name: "actor");

            migrationBuilder.DropTable(
                name: "film");

            migrationBuilder.DropTable(
                name: "hall");

            migrationBuilder.DropTable(
                name: "image");

            migrationBuilder.DropTable(
                name: "cinema");
        }
    }
}
