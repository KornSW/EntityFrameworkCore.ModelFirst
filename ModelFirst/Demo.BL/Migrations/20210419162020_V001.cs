using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Demo.Migrations
{
    public partial class V001 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mfdRooms",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfficialName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mfdRooms", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "mfdSubjects",
                columns: table => new
                {
                    OfficialName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mfdSubjects", x => x.OfficialName);
                });

            migrationBuilder.CreateTable(
                name: "mfdTeachers",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mfdTeachers", x => x.Uid);
                });

            migrationBuilder.CreateTable(
                name: "mfdEnducationItems",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InventoryNumber = table.Column<short>(type: "smallint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DedicatedToSubjectName = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mfdEnducationItems", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_mfdEnducationItems_mfdSubjects_DedicatedToSubjectName",
                        column: x => x.DedicatedToSubjectName,
                        principalTable: "mfdSubjects",
                        principalColumn: "OfficialName",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mfdClasses",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrimaryTeacherUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RoomUid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OfficialName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EducationLevelYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mfdClasses", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_mfdClasses_mfdRooms_RoomUid",
                        column: x => x.RoomUid,
                        principalTable: "mfdRooms",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mfdClasses_mfdTeachers_PrimaryTeacherUid",
                        column: x => x.PrimaryTeacherUid,
                        principalTable: "mfdTeachers",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mfdSubjectTeachings",
                columns: table => new
                {
                    TeacherUid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectOfficialName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mfdSubjectTeachings", x => new { x.TeacherUid, x.SubjectOfficialName });
                    table.ForeignKey(
                        name: "FK_mfdSubjectTeachings_mfdSubjects_SubjectOfficialName",
                        column: x => x.SubjectOfficialName,
                        principalTable: "mfdSubjects",
                        principalColumn: "OfficialName",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mfdSubjectTeachings_mfdTeachers_TeacherUid",
                        column: x => x.TeacherUid,
                        principalTable: "mfdTeachers",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mfdEducationItemPictures",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PictureData = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mfdEducationItemPictures", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_mfdEducationItemPictures_mfdEnducationItems_Uid",
                        column: x => x.Uid,
                        principalTable: "mfdEnducationItems",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mfdEnducationItemsOfRoomRelatedEducationItem",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomUid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mfdEnducationItemsOfRoomRelatedEducationItem", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_mfdEnducationItemsOfRoomRelatedEducationItem_mfdEnducationItems_Uid",
                        column: x => x.Uid,
                        principalTable: "mfdEnducationItems",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mfdEnducationItemsOfRoomRelatedEducationItem_mfdRooms_RoomUid",
                        column: x => x.RoomUid,
                        principalTable: "mfdRooms",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mfdEnducationItemsOfTeacherRelatedEducationItem",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TeacherUid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mfdEnducationItemsOfTeacherRelatedEducationItem", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_mfdEnducationItemsOfTeacherRelatedEducationItem_mfdEnducationItems_Uid",
                        column: x => x.Uid,
                        principalTable: "mfdEnducationItems",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mfdEnducationItemsOfTeacherRelatedEducationItem_mfdTeachers_TeacherUid",
                        column: x => x.TeacherUid,
                        principalTable: "mfdTeachers",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mfdStudents",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClassUid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ScoolEntryYear = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mfdStudents", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_mfdStudents_mfdClasses_ClassUid",
                        column: x => x.ClassUid,
                        principalTable: "mfdClasses",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mfdLessons",
                columns: table => new
                {
                    Uid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EducatedClassUid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomUid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfficialName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    Begin = table.Column<TimeSpan>(type: "time", nullable: false),
                    DurationHours = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TeacherUid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectOfficialName = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mfdLessons", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_mfdLessons_mfdClasses_EducatedClassUid",
                        column: x => x.EducatedClassUid,
                        principalTable: "mfdClasses",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_mfdLessons_mfdRooms_RoomUid",
                        column: x => x.RoomUid,
                        principalTable: "mfdRooms",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mfdLessons_mfdSubjectTeachings_TeacherUid_SubjectOfficialName",
                        columns: x => new { x.TeacherUid, x.SubjectOfficialName },
                        principalTable: "mfdSubjectTeachings",
                        principalColumns: new[] { "TeacherUid", "SubjectOfficialName" },
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "mfdTeachingRequiredItems",
                columns: table => new
                {
                    Uid = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeacherUid = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SubjectOfficialName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RequiredEducationItemUid = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mfdTeachingRequiredItems", x => x.Uid);
                    table.ForeignKey(
                        name: "FK_mfdTeachingRequiredItems_mfdEnducationItemsOfTeacherRelatedEducationItem_RequiredEducationItemUid",
                        column: x => x.RequiredEducationItemUid,
                        principalTable: "mfdEnducationItemsOfTeacherRelatedEducationItem",
                        principalColumn: "Uid",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_mfdTeachingRequiredItems_mfdSubjectTeachings_TeacherUid_SubjectOfficialName",
                        columns: x => new { x.TeacherUid, x.SubjectOfficialName },
                        principalTable: "mfdSubjectTeachings",
                        principalColumns: new[] { "TeacherUid", "SubjectOfficialName" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_mfdClasses_PrimaryTeacherUid",
                table: "mfdClasses",
                column: "PrimaryTeacherUid");

            migrationBuilder.CreateIndex(
                name: "IX_mfdClasses_RoomUid",
                table: "mfdClasses",
                column: "RoomUid");

            migrationBuilder.CreateIndex(
                name: "IX_mfdEnducationItems_DedicatedToSubjectName",
                table: "mfdEnducationItems",
                column: "DedicatedToSubjectName");

            migrationBuilder.CreateIndex(
                name: "IX_mfdEnducationItemsOfRoomRelatedEducationItem_RoomUid",
                table: "mfdEnducationItemsOfRoomRelatedEducationItem",
                column: "RoomUid");

            migrationBuilder.CreateIndex(
                name: "IX_mfdEnducationItemsOfTeacherRelatedEducationItem_TeacherUid",
                table: "mfdEnducationItemsOfTeacherRelatedEducationItem",
                column: "TeacherUid");

            migrationBuilder.CreateIndex(
                name: "IX_mfdLessons_EducatedClassUid",
                table: "mfdLessons",
                column: "EducatedClassUid");

            migrationBuilder.CreateIndex(
                name: "IX_mfdLessons_RoomUid",
                table: "mfdLessons",
                column: "RoomUid");

            migrationBuilder.CreateIndex(
                name: "IX_mfdLessons_TeacherUid_SubjectOfficialName",
                table: "mfdLessons",
                columns: new[] { "TeacherUid", "SubjectOfficialName" });

            migrationBuilder.CreateIndex(
                name: "IX_mfdStudents_ClassUid",
                table: "mfdStudents",
                column: "ClassUid");

            migrationBuilder.CreateIndex(
                name: "IX_mfdSubjectTeachings_SubjectOfficialName",
                table: "mfdSubjectTeachings",
                column: "SubjectOfficialName");

            migrationBuilder.CreateIndex(
                name: "IX_mfdTeachingRequiredItems_RequiredEducationItemUid",
                table: "mfdTeachingRequiredItems",
                column: "RequiredEducationItemUid");

            migrationBuilder.CreateIndex(
                name: "IX_mfdTeachingRequiredItems_TeacherUid_SubjectOfficialName",
                table: "mfdTeachingRequiredItems",
                columns: new[] { "TeacherUid", "SubjectOfficialName" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mfdEducationItemPictures");

            migrationBuilder.DropTable(
                name: "mfdEnducationItemsOfRoomRelatedEducationItem");

            migrationBuilder.DropTable(
                name: "mfdLessons");

            migrationBuilder.DropTable(
                name: "mfdStudents");

            migrationBuilder.DropTable(
                name: "mfdTeachingRequiredItems");

            migrationBuilder.DropTable(
                name: "mfdClasses");

            migrationBuilder.DropTable(
                name: "mfdEnducationItemsOfTeacherRelatedEducationItem");

            migrationBuilder.DropTable(
                name: "mfdSubjectTeachings");

            migrationBuilder.DropTable(
                name: "mfdRooms");

            migrationBuilder.DropTable(
                name: "mfdEnducationItems");

            migrationBuilder.DropTable(
                name: "mfdTeachers");

            migrationBuilder.DropTable(
                name: "mfdSubjects");
        }
    }
}
