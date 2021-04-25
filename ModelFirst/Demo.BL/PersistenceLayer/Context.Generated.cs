using System;
using Microsoft.EntityFrameworkCore;

namespace Demo.Persistence.EF {

  /// <summary> EntityFramework DbContext (based on schema version '1.3.0') </summary>
  public partial class DemoDbContext : DbContext{

    public const String SchemaVersion = "1.3.0";

    public DbSet<ClassEntity> Classes { get; set; }

    public DbSet<TeacherEntity> Teachers { get; set; }

    public DbSet<RoomEntity> Rooms { get; set; }

    public DbSet<StudentEntity> Students { get; set; }

    public DbSet<LessonEntity> Lessons { get; set; }

    public DbSet<EducationItemPictureEntity> EducationItemPictures { get; set; }

    public DbSet<EnducationItemEntity> EnducationItems { get; set; }

    public DbSet<SubjectEntity> Subjects { get; set; }

    public DbSet<SubjectTeachingEntity> SubjectTeachings { get; set; }

    public DbSet<TeachingRequiredItemEntity> TeachingRequiredItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
      base.OnModelCreating(modelBuilder);

#region Mapping

      //////////////////////////////////////////////////////////////////////////////////////
      // Class
      //////////////////////////////////////////////////////////////////////////////////////

      var cfgClass = modelBuilder.Entity<ClassEntity>();
      cfgClass.ToTable("mfdClasses");
      cfgClass.HasKey((e) => e.Uid);

      // LOOKUP: >>> Teacher
      cfgClass
        .HasOne((lcl) => lcl.PrimaryTeacher )
        .WithMany((rem) => rem.PrimaryClasses )
        .HasForeignKey(nameof(ClassEntity.PrimaryTeacherUid))
        .OnDelete(DeleteBehavior.Restrict);

      // LOOKUP: >>> Room
      cfgClass
        .HasOne((lcl) => lcl.PrimaryRoom )
        .WithMany((rem) => rem.PrimaryClasses )
        .HasForeignKey(nameof(ClassEntity.RoomUid))
        .OnDelete(DeleteBehavior.Restrict);

      //////////////////////////////////////////////////////////////////////////////////////
      // Teacher
      //////////////////////////////////////////////////////////////////////////////////////

      var cfgTeacher = modelBuilder.Entity<TeacherEntity>();
      cfgTeacher.ToTable("mfdTeachers");
      cfgTeacher.HasKey((e) => e.Uid);

      //////////////////////////////////////////////////////////////////////////////////////
      // Room
      //////////////////////////////////////////////////////////////////////////////////////

      var cfgRoom = modelBuilder.Entity<RoomEntity>();
      cfgRoom.ToTable("mfdRooms");
      cfgRoom.HasKey((e) => e.Uid);

      //////////////////////////////////////////////////////////////////////////////////////
      // Student
      //////////////////////////////////////////////////////////////////////////////////////

      var cfgStudent = modelBuilder.Entity<StudentEntity>();
      cfgStudent.ToTable("mfdStudents");
      cfgStudent.HasKey((e) => e.Uid);

      // PRINCIPAL: >>> Class
      cfgStudent
        .HasOne((lcl) => lcl.Class )
        .WithMany((rem) => rem.Students )
        .HasForeignKey(nameof(StudentEntity.ClassUid))
        .OnDelete(DeleteBehavior.Cascade);

      //////////////////////////////////////////////////////////////////////////////////////
      // Lesson
      //////////////////////////////////////////////////////////////////////////////////////

      var cfgLesson = modelBuilder.Entity<LessonEntity>();
      cfgLesson.ToTable("mfdLessons");
      cfgLesson.HasKey((e) => e.Uid);

      // PRINCIPAL: >>> Class
      cfgLesson
        .HasOne((lcl) => lcl.EducatedClass )
        .WithMany((rem) => rem.ScheduledLessons )
        .HasForeignKey(nameof(LessonEntity.EducatedClassUid))
        .OnDelete(DeleteBehavior.Cascade);

      // LOOKUP: >>> Room
      cfgLesson
        .HasOne((lcl) => lcl.Room )
        .WithMany((rem) => rem.ScheduledLessons )
        .HasForeignKey(nameof(LessonEntity.RoomUid))
        .OnDelete(DeleteBehavior.Restrict);

      // LOOKUP: >>> SubjectTeaching
      cfgLesson
        .HasOne((lcl) => lcl.Teaching )
        .WithMany((rem) => rem.ScheduledLessons )
        .HasForeignKey(nameof(LessonEntity.TeacherUid), nameof(LessonEntity.SubjectOfficialName))
        .OnDelete(DeleteBehavior.Restrict);

      //////////////////////////////////////////////////////////////////////////////////////
      // EducationItemPicture
      //////////////////////////////////////////////////////////////////////////////////////

      var cfgEducationItemPicture = modelBuilder.Entity<EducationItemPictureEntity>();
      cfgEducationItemPicture.ToTable("mfdEducationItemPictures");
      cfgEducationItemPicture.HasKey((e) => e.Uid);

      // PRINCIPAL: >>> EnducationItem
      cfgEducationItemPicture
        .HasOne((lcl) => lcl.EnducationItem )
        .WithOne((rem) => rem.Picture )
        .HasForeignKey(typeof(EducationItemPictureEntity), nameof(EducationItemPictureEntity.Uid))
        .OnDelete(DeleteBehavior.Cascade);

      //////////////////////////////////////////////////////////////////////////////////////
      // EnducationItem
      //////////////////////////////////////////////////////////////////////////////////////

      var cfgEnducationItem = modelBuilder.Entity<EnducationItemEntity>();
      cfgEnducationItem.ToTable("mfdEnducationItems");
      cfgEnducationItem.HasKey((e) => e.Uid);

      // LOOKUP: >>> Subject
      cfgEnducationItem
        .HasOne((lcl) => lcl.DedicatedToSubject )
        .WithMany((rem) => rem.EnducationItems )
        .HasForeignKey(nameof(EnducationItemEntity.DedicatedToSubjectName))
        .OnDelete(DeleteBehavior.Restrict);

      //////////////////////////////////////////////////////////////////////////////////////
      // Subject
      //////////////////////////////////////////////////////////////////////////////////////

      var cfgSubject = modelBuilder.Entity<SubjectEntity>();
      cfgSubject.ToTable("mfdSubjects");
      cfgSubject.HasKey((e) => e.OfficialName);

      //////////////////////////////////////////////////////////////////////////////////////
      // SubjectTeaching
      //////////////////////////////////////////////////////////////////////////////////////

      var cfgSubjectTeaching = modelBuilder.Entity<SubjectTeachingEntity>();
      cfgSubjectTeaching.ToTable("mfdSubjectTeachings");
      cfgSubjectTeaching.HasKey((e) => new {e.TeacherUid, e.SubjectOfficialName});

      // LOOKUP: >>> Subject
      cfgSubjectTeaching
        .HasOne((lcl) => lcl.Subject )
        .WithMany((rem) => rem.Teachings )
        .HasForeignKey(nameof(SubjectTeachingEntity.SubjectOfficialName))
        .OnDelete(DeleteBehavior.Restrict);

      // PRINCIPAL: >>> Teacher
      cfgSubjectTeaching
        .HasOne((lcl) => lcl.Teacher )
        .WithMany((rem) => rem.Teachings )
        .HasForeignKey(nameof(SubjectTeachingEntity.TeacherUid))
        .OnDelete(DeleteBehavior.Cascade);

      //////////////////////////////////////////////////////////////////////////////////////
      // RoomRelatedEducationItem
      //////////////////////////////////////////////////////////////////////////////////////

      var cfgRoomRelatedEducationItem = modelBuilder.Entity<RoomRelatedEducationItemEntity>();
      cfgRoomRelatedEducationItem.ToTable("mfdEnducationItemsOfRoomRelatedEducationItem");
      cfgRoomRelatedEducationItem.HasBaseType<EnducationItemEntity>();

      // LOOKUP: >>> Room
      cfgRoomRelatedEducationItem
        .HasOne((lcl) => lcl.Location )
        .WithMany((rem) => rem.EducationItems )
        .HasForeignKey(nameof(RoomRelatedEducationItemEntity.RoomUid))
        .OnDelete(DeleteBehavior.Restrict);

      //////////////////////////////////////////////////////////////////////////////////////
      // TeachingRequiredItem
      //////////////////////////////////////////////////////////////////////////////////////

      var cfgTeachingRequiredItem = modelBuilder.Entity<TeachingRequiredItemEntity>();
      cfgTeachingRequiredItem.ToTable("mfdTeachingRequiredItems");
      cfgTeachingRequiredItem.HasKey((e) => e.Uid);
      cfgTeachingRequiredItem.Property((e) => e.Uid).UseIdentityColumn();

      // PRINCIPAL: >>> SubjectTeaching
      cfgTeachingRequiredItem
        .HasOne((lcl) => lcl.Teaching )
        .WithMany((rem) => rem.RequiredItems )
        .HasForeignKey(nameof(TeachingRequiredItemEntity.TeacherUid), nameof(TeachingRequiredItemEntity.SubjectOfficialName))
        .OnDelete(DeleteBehavior.Cascade);

      // LOOKUP: >>> TeacherRelatedEducationItem
      cfgTeachingRequiredItem
        .HasOne((lcl) => lcl.RequiredItem )
        .WithMany((rem) => rem.RequiredFor )
        .HasForeignKey(nameof(TeachingRequiredItemEntity.RequiredEducationItemUid))
        .OnDelete(DeleteBehavior.Restrict);

      //////////////////////////////////////////////////////////////////////////////////////
      // TeacherRelatedEducationItem
      //////////////////////////////////////////////////////////////////////////////////////

      var cfgTeacherRelatedEducationItem = modelBuilder.Entity<TeacherRelatedEducationItemEntity>();
      cfgTeacherRelatedEducationItem.ToTable("mfdEnducationItemsOfTeacherRelatedEducationItem");
      cfgTeacherRelatedEducationItem.HasBaseType<EnducationItemEntity>();

      // LOOKUP: >>> Teacher
      cfgTeacherRelatedEducationItem
        .HasOne((lcl) => lcl.OwningTeacher )
        .WithMany((rem) => rem.OwnedEducationItems )
        .HasForeignKey(nameof(TeacherRelatedEducationItemEntity.TeacherUid))
        .OnDelete(DeleteBehavior.Restrict);

#endregion

      this.OnModelCreatingCustom(modelBuilder);
    }

    partial void OnModelCreatingCustom(ModelBuilder modelBuilder);

    protected override void OnConfiguring(DbContextOptionsBuilder options) {

      //reqires separate nuget-package Microsoft.EntityFrameworkCore.SqlServer
      options.UseSqlServer(_ConnectionString);

      //reqires separate nuget-package Microsoft.EntityFrameworkCore.Proxies
      options.UseLazyLoadingProxies();

      this.OnConfiguringCustom(options);
    }

    partial void OnConfiguringCustom(DbContextOptionsBuilder options);

    public static void Migrate() {
      if (!_Migrated) {
        DemoDbContext c = new DemoDbContext();
        c.Database.Migrate();
        _Migrated = true;
        c.Dispose();
      }
    }

   private static bool _Migrated = false;

    private static String _ConnectionString = "Server=(localdb)\\MsSqlLocalDb;Database=EfCoreModelFirstDemoDbContext;Integrated Security=True;Persist Security Info=True;MultipleActiveResultSets=True;";
    public static String ConnectionString {
      set{ _ConnectionString = value;  }
    }

  }

}
