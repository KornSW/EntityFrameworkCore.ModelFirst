using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

namespace Demo {

public class Class {

  [Required]
  public Guid Uid { get; set; } = Guid.NewGuid();

  /// <summary> *this field is optional </summary>
  public Nullable<Guid> PrimaryTeacherUid { get; set; }

  /// <summary> *this field is optional </summary>
  public Nullable<Guid> RoomUid { get; set; }

  [Required]
  public String OfficialName { get; set; }

  [Required]
  public Int32 EducationLevelYear { get; set; }

}

public class Teacher {

  [Required]
  public Guid Uid { get; set; } = Guid.NewGuid();

  [Required]
  public String FirstName { get; set; }

  [Required]
  public String LastName { get; set; }

}

public class Room {

  [Required]
  public Guid Uid { get; set; } = Guid.NewGuid();

  [Required]
  public String OfficialName { get; set; }

}

public class Student {

  [Required]
  public Guid Uid { get; set; } = Guid.NewGuid();

  [Required]
  public Guid ClassUid { get; set; }

  [Required]
  public String FirstName { get; set; }

  [Required]
  public String LastName { get; set; }

  [Required]
  public Int32 ScoolEntryYear { get; set; }

}

public class Lesson {

  [Required]
  public Guid Uid { get; set; } = Guid.NewGuid();

  [Required]
  public Guid EducatedClassUid { get; set; }

  [Required]
  public Guid RoomUid { get; set; }

  [Required]
  public String OfficialName { get; set; }

  [Required]
  public Int32 DayOfWeek { get; set; }

  [Required]
  public TimeSpan Begin { get; set; }

  [Required]
  public Decimal DurationHours { get; set; }

  [Required]
  public Guid TeacherUid { get; set; }

  [Required]
  public String SubjectOfficialName { get; set; }

}

public class EducationItemPicture {

  [Required]
  public Guid Uid { get; set; } = Guid.NewGuid();

  [Required]
  public Byte[] PictureData { get; set; }

}

public class EnducationItem {

  [Required]
  public Guid Uid { get; set; } = Guid.NewGuid();

  [Required]
  public Int16 InventoryNumber { get; set; }

  [Required]
  public String Title { get; set; }

  /// <summary> *this field is optional (use null as value) </summary>
  public String DedicatedToSubjectName { get; set; }

}

public class Subject {

  [Required]
  public String OfficialName { get; set; }

}

public class SubjectTeaching {

  [Required]
  public Guid TeacherUid { get; set; } = Guid.NewGuid();

  [Required]
  public String SubjectOfficialName { get; set; }

}

public class RoomRelatedEducationItem : EnducationItem {

  [Required]
  public Guid RoomUid { get; set; }

}

public class TeachingRequiredItem {

  [Required]
  public Int32 Uid { get; set; }

  [Required]
  public Guid TeacherUid { get; set; }

  [Required]
  public String SubjectOfficialName { get; set; }

  [Required]
  public Guid RequiredEducationItemUid { get; set; }

}

public class TeacherRelatedEducationItem : EnducationItem {

  [Required]
  public Guid TeacherUid { get; set; }

}

}
