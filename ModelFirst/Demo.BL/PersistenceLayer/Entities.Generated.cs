using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Demo.Persistence {

public class ClassEntity {

  [Required]
  public Guid Uid { get; set; } = Guid.NewGuid();

  /// <summary> *this field is optional </summary>
  public Nullable<Guid> PrimaryTeacherUid { get; set; }

  /// <summary> *this field is optional </summary>
  public Nullable<Guid> RoomUid { get; set; }

  [Required]
  public String OfficialName { get; set; } = String.Empty;

  [Required]
  public Int32 EducationLevelYear { get; set; }

  [Lookup]
  public virtual TeacherEntity PrimaryTeacher { get; set; }

  [Lookup]
  public virtual RoomEntity PrimaryRoom { get; set; }

  [Dependent]
  public virtual ObservableCollection<StudentEntity> Students { get; set; } = new ObservableCollection<StudentEntity>();

  [Dependent]
  public virtual ObservableCollection<LessonEntity> ScheduledLessons { get; set; } = new ObservableCollection<LessonEntity>();

#region Mapping

  internal static Expression<Func<Class, ClassEntity>> ClassEntitySelector = ((Class src) => new ClassEntity {
    Uid = src.Uid,
    PrimaryTeacherUid = src.PrimaryTeacherUid,
    RoomUid = src.RoomUid,
    OfficialName = src.OfficialName,
    EducationLevelYear = src.EducationLevelYear,
  });

  internal static Expression<Func<ClassEntity, Class>> ClassSelector = ((ClassEntity src) => new Class {
    Uid = src.Uid,
    PrimaryTeacherUid = src.PrimaryTeacherUid,
    RoomUid = src.RoomUid,
    OfficialName = src.OfficialName,
    EducationLevelYear = src.EducationLevelYear,
  });

  internal void CopyContentFrom(Class source, Func<String,bool> onFixedValueChangingCallback = null){
    this.Uid = source.Uid;
    this.PrimaryTeacherUid = source.PrimaryTeacherUid;
    this.RoomUid = source.RoomUid;
    this.OfficialName = source.OfficialName;
    this.EducationLevelYear = source.EducationLevelYear;
  }

  internal void CopyContentTo(Class target, Func<String,bool> onFixedValueChangingCallback = null){
    target.Uid = this.Uid;
    target.PrimaryTeacherUid = this.PrimaryTeacherUid;
    target.RoomUid = this.RoomUid;
    target.OfficialName = this.OfficialName;
    target.EducationLevelYear = this.EducationLevelYear;
  }

#endregion

}

public class TeacherEntity {

  [Required]
  public Guid Uid { get; set; } = Guid.NewGuid();

  [Required]
  public String FirstName { get; set; } = String.Empty;

  [Required]
  public String LastName { get; set; } = String.Empty;

  [Referer]
  public virtual ObservableCollection<ClassEntity> PrimaryClasses { get; set; } = new ObservableCollection<ClassEntity>();

  [Dependent]
  public virtual ObservableCollection<SubjectTeachingEntity> Teachings { get; set; } = new ObservableCollection<SubjectTeachingEntity>();

  [Referer]
  public virtual ObservableCollection<TeacherRelatedEducationItemEntity> OwnedEducationItems { get; set; } = new ObservableCollection<TeacherRelatedEducationItemEntity>();

#region Mapping

  internal static Expression<Func<Teacher, TeacherEntity>> TeacherEntitySelector = ((Teacher src) => new TeacherEntity {
    Uid = src.Uid,
    FirstName = src.FirstName,
    LastName = src.LastName,
  });

  internal static Expression<Func<TeacherEntity, Teacher>> TeacherSelector = ((TeacherEntity src) => new Teacher {
    Uid = src.Uid,
    FirstName = src.FirstName,
    LastName = src.LastName,
  });

  internal void CopyContentFrom(Teacher source, Func<String,bool> onFixedValueChangingCallback = null){
    this.Uid = source.Uid;
    this.FirstName = source.FirstName;
    this.LastName = source.LastName;
  }

  internal void CopyContentTo(Teacher target, Func<String,bool> onFixedValueChangingCallback = null){
    target.Uid = this.Uid;
    target.FirstName = this.FirstName;
    target.LastName = this.LastName;
  }

#endregion

}

public class RoomEntity {

  [Required]
  public Guid Uid { get; set; } = Guid.NewGuid();

  [Required]
  public String OfficialName { get; set; } = String.Empty;

  [Referer]
  public virtual ObservableCollection<ClassEntity> PrimaryClasses { get; set; } = new ObservableCollection<ClassEntity>();

  [Referer]
  public virtual ObservableCollection<LessonEntity> ScheduledLessons { get; set; } = new ObservableCollection<LessonEntity>();

  [Referer]
  public virtual ObservableCollection<RoomRelatedEducationItemEntity> EducationItems { get; set; } = new ObservableCollection<RoomRelatedEducationItemEntity>();

#region Mapping

  internal static Expression<Func<Room, RoomEntity>> RoomEntitySelector = ((Room src) => new RoomEntity {
    Uid = src.Uid,
    OfficialName = src.OfficialName,
  });

  internal static Expression<Func<RoomEntity, Room>> RoomSelector = ((RoomEntity src) => new Room {
    Uid = src.Uid,
    OfficialName = src.OfficialName,
  });

  internal void CopyContentFrom(Room source, Func<String,bool> onFixedValueChangingCallback = null){
    this.Uid = source.Uid;
    this.OfficialName = source.OfficialName;
  }

  internal void CopyContentTo(Room target, Func<String,bool> onFixedValueChangingCallback = null){
    target.Uid = this.Uid;
    target.OfficialName = this.OfficialName;
  }

#endregion

}

public class StudentEntity {

  [Required]
  public Guid Uid { get; set; } = Guid.NewGuid();

  [Required]
  public Guid ClassUid { get; set; }

  [Required]
  public String FirstName { get; set; } = String.Empty;

  [Required]
  public String LastName { get; set; } = String.Empty;

  [Required]
  public Int32 ScoolEntryYear { get; set; }

  [Principal]
  public virtual ClassEntity Class { get; set; }

#region Mapping

  internal static Expression<Func<Student, StudentEntity>> StudentEntitySelector = ((Student src) => new StudentEntity {
    Uid = src.Uid,
    ClassUid = src.ClassUid,
    FirstName = src.FirstName,
    LastName = src.LastName,
    ScoolEntryYear = src.ScoolEntryYear,
  });

  internal static Expression<Func<StudentEntity, Student>> StudentSelector = ((StudentEntity src) => new Student {
    Uid = src.Uid,
    ClassUid = src.ClassUid,
    FirstName = src.FirstName,
    LastName = src.LastName,
    ScoolEntryYear = src.ScoolEntryYear,
  });

  internal void CopyContentFrom(Student source, Func<String,bool> onFixedValueChangingCallback = null){
    this.Uid = source.Uid;
    this.ClassUid = source.ClassUid;
    this.FirstName = source.FirstName;
    this.LastName = source.LastName;
    this.ScoolEntryYear = source.ScoolEntryYear;
  }

  internal void CopyContentTo(Student target, Func<String,bool> onFixedValueChangingCallback = null){
    target.Uid = this.Uid;
    target.ClassUid = this.ClassUid;
    target.FirstName = this.FirstName;
    target.LastName = this.LastName;
    target.ScoolEntryYear = this.ScoolEntryYear;
  }

#endregion

}

public class LessonEntity {

  [Required]
  public Guid Uid { get; set; } = Guid.NewGuid();

  [Required]
  public Guid EducatedClassUid { get; set; }

  [Required]
  public Guid RoomUid { get; set; }

  [Required]
  public String OfficialName { get; set; } = String.Empty;

  [Required]
  public Int32 DayOfWeek { get; set; }

  [Required]
  public TimeSpan Begin { get; set; }

  [Required]
  public Decimal DurationHours { get; set; }

  [Required]
  public Guid TeacherUid { get; set; }

  [Required]
  public String SubjectOfficialName { get; set; } = String.Empty;

  [Principal]
  public virtual ClassEntity EducatedClass { get; set; }

  [Lookup]
  public virtual RoomEntity Room { get; set; }

  [Lookup]
  public virtual SubjectTeachingEntity Teaching { get; set; }

#region Mapping

  internal static Expression<Func<Lesson, LessonEntity>> LessonEntitySelector = ((Lesson src) => new LessonEntity {
    Uid = src.Uid,
    EducatedClassUid = src.EducatedClassUid,
    RoomUid = src.RoomUid,
    OfficialName = src.OfficialName,
    DayOfWeek = src.DayOfWeek,
    Begin = src.Begin,
    DurationHours = src.DurationHours,
    TeacherUid = src.TeacherUid,
    SubjectOfficialName = src.SubjectOfficialName,
  });

  internal static Expression<Func<LessonEntity, Lesson>> LessonSelector = ((LessonEntity src) => new Lesson {
    Uid = src.Uid,
    EducatedClassUid = src.EducatedClassUid,
    RoomUid = src.RoomUid,
    OfficialName = src.OfficialName,
    DayOfWeek = src.DayOfWeek,
    Begin = src.Begin,
    DurationHours = src.DurationHours,
    TeacherUid = src.TeacherUid,
    SubjectOfficialName = src.SubjectOfficialName,
  });

  internal void CopyContentFrom(Lesson source, Func<String,bool> onFixedValueChangingCallback = null){
    this.Uid = source.Uid;
    this.EducatedClassUid = source.EducatedClassUid;
    this.RoomUid = source.RoomUid;
    this.OfficialName = source.OfficialName;
    this.DayOfWeek = source.DayOfWeek;
    this.Begin = source.Begin;
    this.DurationHours = source.DurationHours;
    this.TeacherUid = source.TeacherUid;
    this.SubjectOfficialName = source.SubjectOfficialName;
  }

  internal void CopyContentTo(Lesson target, Func<String,bool> onFixedValueChangingCallback = null){
    target.Uid = this.Uid;
    target.EducatedClassUid = this.EducatedClassUid;
    target.RoomUid = this.RoomUid;
    target.OfficialName = this.OfficialName;
    target.DayOfWeek = this.DayOfWeek;
    target.Begin = this.Begin;
    target.DurationHours = this.DurationHours;
    target.TeacherUid = this.TeacherUid;
    target.SubjectOfficialName = this.SubjectOfficialName;
  }

#endregion

}

public class EducationItemPictureEntity {

  [Required]
  public Guid Uid { get; set; } = Guid.NewGuid();

  [Required]
  public Byte[] PictureData { get; set; }

  [Principal]
  public virtual EnducationItemEntity EnducationItem { get; set; }

#region Mapping

  internal static Expression<Func<EducationItemPicture, EducationItemPictureEntity>> EducationItemPictureEntitySelector = ((EducationItemPicture src) => new EducationItemPictureEntity {
    Uid = src.Uid,
    PictureData = src.PictureData,
  });

  internal static Expression<Func<EducationItemPictureEntity, EducationItemPicture>> EducationItemPictureSelector = ((EducationItemPictureEntity src) => new EducationItemPicture {
    Uid = src.Uid,
    PictureData = src.PictureData,
  });

  internal void CopyContentFrom(EducationItemPicture source, Func<String,bool> onFixedValueChangingCallback = null){
    this.Uid = source.Uid;
    this.PictureData = source.PictureData;
  }

  internal void CopyContentTo(EducationItemPicture target, Func<String,bool> onFixedValueChangingCallback = null){
    target.Uid = this.Uid;
    target.PictureData = this.PictureData;
  }

#endregion

}

public class EnducationItemEntity {

  [Required]
  public Guid Uid { get; set; } = Guid.NewGuid();

  [Required]
  public Int16 InventoryNumber { get; set; }

  [Required]
  public String Title { get; set; } = String.Empty;

  /// <summary> *this field is optional (use null as value) </summary>
  public String DedicatedToSubjectName { get; set; }

  [Dependent]
  public virtual EducationItemPictureEntity Picture { get; set; }

  [Lookup]
  public virtual SubjectEntity DedicatedToSubject { get; set; }

#region Mapping

  internal static Expression<Func<EnducationItem, EnducationItemEntity>> EnducationItemEntitySelector = ((EnducationItem src) => new EnducationItemEntity {
    Uid = src.Uid,
    InventoryNumber = src.InventoryNumber,
    Title = src.Title,
    DedicatedToSubjectName = src.DedicatedToSubjectName,
  });

  internal static Expression<Func<EnducationItemEntity, EnducationItem>> EnducationItemSelector = ((EnducationItemEntity src) => new EnducationItem {
    Uid = src.Uid,
    InventoryNumber = src.InventoryNumber,
    Title = src.Title,
    DedicatedToSubjectName = src.DedicatedToSubjectName,
  });

  internal void CopyContentFrom(EnducationItem source, Func<String,bool> onFixedValueChangingCallback = null){
    this.Uid = source.Uid;
    this.InventoryNumber = source.InventoryNumber;
    this.Title = source.Title;
    this.DedicatedToSubjectName = source.DedicatedToSubjectName;
  }

  internal void CopyContentTo(EnducationItem target, Func<String,bool> onFixedValueChangingCallback = null){
    target.Uid = this.Uid;
    target.InventoryNumber = this.InventoryNumber;
    target.Title = this.Title;
    target.DedicatedToSubjectName = this.DedicatedToSubjectName;
  }

#endregion

}

public class SubjectEntity {

  [Required]
  public String OfficialName { get; set; } = String.Empty;

  [Referer]
  public virtual ObservableCollection<EnducationItemEntity> EnducationItems { get; set; } = new ObservableCollection<EnducationItemEntity>();

  [Referer]
  public virtual ObservableCollection<SubjectTeachingEntity> Teachings { get; set; } = new ObservableCollection<SubjectTeachingEntity>();

#region Mapping

  internal static Expression<Func<Subject, SubjectEntity>> SubjectEntitySelector = ((Subject src) => new SubjectEntity {
    OfficialName = src.OfficialName,
  });

  internal static Expression<Func<SubjectEntity, Subject>> SubjectSelector = ((SubjectEntity src) => new Subject {
    OfficialName = src.OfficialName,
  });

  internal void CopyContentFrom(Subject source, Func<String,bool> onFixedValueChangingCallback = null){
    this.OfficialName = source.OfficialName;
  }

  internal void CopyContentTo(Subject target, Func<String,bool> onFixedValueChangingCallback = null){
    target.OfficialName = this.OfficialName;
  }

#endregion

}

public class SubjectTeachingEntity {

  [Required]
  public Guid TeacherUid { get; set; } = Guid.NewGuid();

  [Required]
  public String SubjectOfficialName { get; set; } = String.Empty;

  [Referer]
  public virtual ObservableCollection<LessonEntity> ScheduledLessons { get; set; } = new ObservableCollection<LessonEntity>();

  [Lookup]
  public virtual SubjectEntity Subject { get; set; }

  [Principal]
  public virtual TeacherEntity Teacher { get; set; }

  [Dependent]
  public virtual ObservableCollection<TeachingRequiredItemEntity> RequiredItems { get; set; } = new ObservableCollection<TeachingRequiredItemEntity>();

#region Mapping

  internal static Expression<Func<SubjectTeaching, SubjectTeachingEntity>> SubjectTeachingEntitySelector = ((SubjectTeaching src) => new SubjectTeachingEntity {
    TeacherUid = src.TeacherUid,
    SubjectOfficialName = src.SubjectOfficialName,
  });

  internal static Expression<Func<SubjectTeachingEntity, SubjectTeaching>> SubjectTeachingSelector = ((SubjectTeachingEntity src) => new SubjectTeaching {
    TeacherUid = src.TeacherUid,
    SubjectOfficialName = src.SubjectOfficialName,
  });

  internal void CopyContentFrom(SubjectTeaching source, Func<String,bool> onFixedValueChangingCallback = null){
    this.TeacherUid = source.TeacherUid;
    this.SubjectOfficialName = source.SubjectOfficialName;
  }

  internal void CopyContentTo(SubjectTeaching target, Func<String,bool> onFixedValueChangingCallback = null){
    target.TeacherUid = this.TeacherUid;
    target.SubjectOfficialName = this.SubjectOfficialName;
  }

#endregion

}

public class RoomRelatedEducationItemEntity : EnducationItemEntity {

  [Required]
  public Guid RoomUid { get; set; }

  [Lookup]
  public virtual RoomEntity Location { get; set; }

#region Mapping

  internal static Expression<Func<RoomRelatedEducationItem, RoomRelatedEducationItemEntity>> RoomRelatedEducationItemEntitySelector = ((RoomRelatedEducationItem src) => new RoomRelatedEducationItemEntity {
    Uid = src.Uid,
    InventoryNumber = src.InventoryNumber,
    Title = src.Title,
    DedicatedToSubjectName = src.DedicatedToSubjectName,
    RoomUid = src.RoomUid,
  });

  internal static Expression<Func<RoomRelatedEducationItemEntity, RoomRelatedEducationItem>> RoomRelatedEducationItemSelector = ((RoomRelatedEducationItemEntity src) => new RoomRelatedEducationItem {
    Uid = src.Uid,
    InventoryNumber = src.InventoryNumber,
    Title = src.Title,
    DedicatedToSubjectName = src.DedicatedToSubjectName,
    RoomUid = src.RoomUid,
  });

  internal void CopyContentFrom(RoomRelatedEducationItem source, Func<String,bool> onFixedValueChangingCallback = null){
    this.Uid = source.Uid;
    this.InventoryNumber = source.InventoryNumber;
    this.Title = source.Title;
    this.DedicatedToSubjectName = source.DedicatedToSubjectName;
    this.RoomUid = source.RoomUid;
  }

  internal void CopyContentTo(RoomRelatedEducationItem target, Func<String,bool> onFixedValueChangingCallback = null){
    target.Uid = this.Uid;
    target.InventoryNumber = this.InventoryNumber;
    target.Title = this.Title;
    target.DedicatedToSubjectName = this.DedicatedToSubjectName;
    target.RoomUid = this.RoomUid;
  }

#endregion

}

public class TeachingRequiredItemEntity {

  [Required]
  public Int32 Uid { get; set; }

  [Required]
  public Guid TeacherUid { get; set; }

  [Required]
  public String SubjectOfficialName { get; set; } = String.Empty;

  [Required]
  public Guid RequiredEducationItemUid { get; set; }

  [Principal]
  public virtual SubjectTeachingEntity Teaching { get; set; }

  [Lookup]
  public virtual TeacherRelatedEducationItemEntity RequiredItem { get; set; }

#region Mapping

  internal static Expression<Func<TeachingRequiredItem, TeachingRequiredItemEntity>> TeachingRequiredItemEntitySelector = ((TeachingRequiredItem src) => new TeachingRequiredItemEntity {
    Uid = src.Uid,
    TeacherUid = src.TeacherUid,
    SubjectOfficialName = src.SubjectOfficialName,
    RequiredEducationItemUid = src.RequiredEducationItemUid,
  });

  internal static Expression<Func<TeachingRequiredItemEntity, TeachingRequiredItem>> TeachingRequiredItemSelector = ((TeachingRequiredItemEntity src) => new TeachingRequiredItem {
    Uid = src.Uid,
    TeacherUid = src.TeacherUid,
    SubjectOfficialName = src.SubjectOfficialName,
    RequiredEducationItemUid = src.RequiredEducationItemUid,
  });

  internal void CopyContentFrom(TeachingRequiredItem source, Func<String,bool> onFixedValueChangingCallback = null){
    this.Uid = source.Uid;
    this.TeacherUid = source.TeacherUid;
    this.SubjectOfficialName = source.SubjectOfficialName;
    this.RequiredEducationItemUid = source.RequiredEducationItemUid;
  }

  internal void CopyContentTo(TeachingRequiredItem target, Func<String,bool> onFixedValueChangingCallback = null){
    target.Uid = this.Uid;
    target.TeacherUid = this.TeacherUid;
    target.SubjectOfficialName = this.SubjectOfficialName;
    target.RequiredEducationItemUid = this.RequiredEducationItemUid;
  }

#endregion

}

public class TeacherRelatedEducationItemEntity : EnducationItemEntity {

  [Required]
  public Guid TeacherUid { get; set; }

  [Lookup]
  public virtual TeacherEntity OwningTeacher { get; set; }

  [Referer]
  public virtual ObservableCollection<TeachingRequiredItemEntity> RequiredFor { get; set; } = new ObservableCollection<TeachingRequiredItemEntity>();

#region Mapping

  internal static Expression<Func<TeacherRelatedEducationItem, TeacherRelatedEducationItemEntity>> TeacherRelatedEducationItemEntitySelector = ((TeacherRelatedEducationItem src) => new TeacherRelatedEducationItemEntity {
    Uid = src.Uid,
    InventoryNumber = src.InventoryNumber,
    Title = src.Title,
    DedicatedToSubjectName = src.DedicatedToSubjectName,
    TeacherUid = src.TeacherUid,
  });

  internal static Expression<Func<TeacherRelatedEducationItemEntity, TeacherRelatedEducationItem>> TeacherRelatedEducationItemSelector = ((TeacherRelatedEducationItemEntity src) => new TeacherRelatedEducationItem {
    Uid = src.Uid,
    InventoryNumber = src.InventoryNumber,
    Title = src.Title,
    DedicatedToSubjectName = src.DedicatedToSubjectName,
    TeacherUid = src.TeacherUid,
  });

  internal void CopyContentFrom(TeacherRelatedEducationItem source, Func<String,bool> onFixedValueChangingCallback = null){
    this.Uid = source.Uid;
    this.InventoryNumber = source.InventoryNumber;
    this.Title = source.Title;
    this.DedicatedToSubjectName = source.DedicatedToSubjectName;
    this.TeacherUid = source.TeacherUid;
  }

  internal void CopyContentTo(TeacherRelatedEducationItem target, Func<String,bool> onFixedValueChangingCallback = null){
    target.Uid = this.Uid;
    target.InventoryNumber = this.InventoryNumber;
    target.Title = this.Title;
    target.DedicatedToSubjectName = this.DedicatedToSubjectName;
    target.TeacherUid = this.TeacherUid;
  }

#endregion

}

}
