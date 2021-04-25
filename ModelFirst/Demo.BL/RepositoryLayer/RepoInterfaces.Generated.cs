using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;

namespace Demo {

/// <summary> Provides CRUD access to stored Classes (based on schema version '1.3.0') </summary>
public partial interface IClassRepository {

  //// <summary> Returns an info object, which specifies the possible operations (accessor specific permissions) regarding Classes.</summary>
  //AccessSpecs GetAccessSpecs();

  /// <summary> Loads a specific Class addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="uid"> Represents the primary identity of a Class </param>
  Class GetClassByUid(Guid uid);

  /// <summary> Loads Classes. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of Classes which should be returned </param>
  Class[] GetClasses(int page = 1, int pageSize = 20);

  /// <summary> Loads Classes where values matching to ALL fields of the given 'filterValues' object.</summary>
    /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of Classes which should be returned</param>
  Class[] SearchClasses(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20);

  /// <summary> Adds a new Class and returns its primary identifier (or null on failure). </summary>
  /// <param name="class"> Class containing the new values </param>
  bool AddNewClass(Class @class);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Class addressed by the primary identifier fields within the given Class. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="class"> Class containing the new values (the primary identifier fields within the given Class will be used to address the target record) </param>
  bool UpdateClass(Class @class);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Class addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a Class </param>
  /// <param name="class"> Class containing the new values (the primary identifier fields within the given Class will be ignored) </param>
  bool UpdateClassByUid(Guid uid, Class @class);

  /// <summary> Deletes a specific Class addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a Class </param>
  bool DeleteClassByUid(Guid uid);

}

/// <summary> Provides CRUD access to stored Teachers (based on schema version '1.3.0') </summary>
public partial interface ITeacherRepository {

  //// <summary> Returns an info object, which specifies the possible operations (accessor specific permissions) regarding Teachers.</summary>
  //AccessSpecs GetAccessSpecs();

  /// <summary> Loads a specific Teacher addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="uid"> Represents the primary identity of a Teacher </param>
  Teacher GetTeacherByUid(Guid uid);

  /// <summary> Loads Teachers. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of Teachers which should be returned </param>
  Teacher[] GetTeachers(int page = 1, int pageSize = 20);

  /// <summary> Loads Teachers where values matching to ALL fields of the given 'filterValues' object.</summary>
    /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of Teachers which should be returned</param>
  Teacher[] SearchTeachers(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20);

  /// <summary> Adds a new Teacher and returns its primary identifier (or null on failure). </summary>
  /// <param name="teacher"> Teacher containing the new values </param>
  bool AddNewTeacher(Teacher teacher);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Teacher addressed by the primary identifier fields within the given Teacher. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="teacher"> Teacher containing the new values (the primary identifier fields within the given Teacher will be used to address the target record) </param>
  bool UpdateTeacher(Teacher teacher);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Teacher addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a Teacher </param>
  /// <param name="teacher"> Teacher containing the new values (the primary identifier fields within the given Teacher will be ignored) </param>
  bool UpdateTeacherByUid(Guid uid, Teacher teacher);

  /// <summary> Deletes a specific Teacher addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a Teacher </param>
  bool DeleteTeacherByUid(Guid uid);

}

/// <summary> Provides CRUD access to stored Rooms (based on schema version '1.3.0') </summary>
public partial interface IRoomRepository {

  //// <summary> Returns an info object, which specifies the possible operations (accessor specific permissions) regarding Rooms.</summary>
  //AccessSpecs GetAccessSpecs();

  /// <summary> Loads a specific Room addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="uid"> Represents the primary identity of a Room </param>
  Room GetRoomByUid(Guid uid);

  /// <summary> Loads Rooms. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of Rooms which should be returned </param>
  Room[] GetRooms(int page = 1, int pageSize = 20);

  /// <summary> Loads Rooms where values matching to ALL fields of the given 'filterValues' object.</summary>
    /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of Rooms which should be returned</param>
  Room[] SearchRooms(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20);

  /// <summary> Adds a new Room and returns its primary identifier (or null on failure). </summary>
  /// <param name="room"> Room containing the new values </param>
  bool AddNewRoom(Room room);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Room addressed by the primary identifier fields within the given Room. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="room"> Room containing the new values (the primary identifier fields within the given Room will be used to address the target record) </param>
  bool UpdateRoom(Room room);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Room addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a Room </param>
  /// <param name="room"> Room containing the new values (the primary identifier fields within the given Room will be ignored) </param>
  bool UpdateRoomByUid(Guid uid, Room room);

  /// <summary> Deletes a specific Room addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a Room </param>
  bool DeleteRoomByUid(Guid uid);

}

/// <summary> Provides CRUD access to stored Students (based on schema version '1.3.0') </summary>
public partial interface IStudentRepository {

  //// <summary> Returns an info object, which specifies the possible operations (accessor specific permissions) regarding Students.</summary>
  //AccessSpecs GetAccessSpecs();

  /// <summary> Loads a specific Student addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="uid"> Represents the primary identity of a Student </param>
  Student GetStudentByUid(Guid uid);

  /// <summary> Loads Students. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of Students which should be returned </param>
  Student[] GetStudents(int page = 1, int pageSize = 20);

  /// <summary> Loads Students where values matching to ALL fields of the given 'filterValues' object.</summary>
    /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of Students which should be returned</param>
  Student[] SearchStudents(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20);

  /// <summary> Adds a new Student and returns its primary identifier (or null on failure). </summary>
  /// <param name="student"> Student containing the new values </param>
  bool AddNewStudent(Student student);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Student addressed by the primary identifier fields within the given Student. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="student"> Student containing the new values (the primary identifier fields within the given Student will be used to address the target record) </param>
  bool UpdateStudent(Student student);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Student addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a Student </param>
  /// <param name="student"> Student containing the new values (the primary identifier fields within the given Student will be ignored) </param>
  bool UpdateStudentByUid(Guid uid, Student student);

  /// <summary> Deletes a specific Student addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a Student </param>
  bool DeleteStudentByUid(Guid uid);

}

/// <summary> Provides CRUD access to stored Lessons (based on schema version '1.3.0') </summary>
public partial interface ILessonRepository {

  //// <summary> Returns an info object, which specifies the possible operations (accessor specific permissions) regarding Lessons.</summary>
  //AccessSpecs GetAccessSpecs();

  /// <summary> Loads a specific Lesson addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="uid"> Represents the primary identity of a Lesson </param>
  Lesson GetLessonByUid(Guid uid);

  /// <summary> Loads Lessons. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of Lessons which should be returned </param>
  Lesson[] GetLessons(int page = 1, int pageSize = 20);

  /// <summary> Loads Lessons where values matching to ALL fields of the given 'filterValues' object.</summary>
    /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of Lessons which should be returned</param>
  Lesson[] SearchLessons(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20);

  /// <summary> Adds a new Lesson and returns its primary identifier (or null on failure). </summary>
  /// <param name="lesson"> Lesson containing the new values </param>
  bool AddNewLesson(Lesson lesson);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Lesson addressed by the primary identifier fields within the given Lesson. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="lesson"> Lesson containing the new values (the primary identifier fields within the given Lesson will be used to address the target record) </param>
  bool UpdateLesson(Lesson lesson);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Lesson addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a Lesson </param>
  /// <param name="lesson"> Lesson containing the new values (the primary identifier fields within the given Lesson will be ignored) </param>
  bool UpdateLessonByUid(Guid uid, Lesson lesson);

  /// <summary> Deletes a specific Lesson addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a Lesson </param>
  bool DeleteLessonByUid(Guid uid);

}

/// <summary> Provides CRUD access to stored EducationItemPictures (based on schema version '1.3.0') </summary>
public partial interface IEducationItemPictureRepository {

  //// <summary> Returns an info object, which specifies the possible operations (accessor specific permissions) regarding EducationItemPictures.</summary>
  //AccessSpecs GetAccessSpecs();

  /// <summary> Loads a specific EducationItemPicture addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="uid"> Represents the primary identity of a EducationItemPicture </param>
  EducationItemPicture GetEducationItemPictureByUid(Guid uid);

  /// <summary> Loads EducationItemPictures. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of EducationItemPictures which should be returned </param>
  EducationItemPicture[] GetEducationItemPictures(int page = 1, int pageSize = 20);

  /// <summary> Loads EducationItemPictures where values matching to ALL fields of the given 'filterValues' object.</summary>
    /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of EducationItemPictures which should be returned</param>
  EducationItemPicture[] SearchEducationItemPictures(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20);

  /// <summary> Adds a new EducationItemPicture and returns its primary identifier (or null on failure). </summary>
  /// <param name="educationItemPicture"> EducationItemPicture containing the new values </param>
  bool AddNewEducationItemPicture(EducationItemPicture educationItemPicture);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given EducationItemPicture addressed by the primary identifier fields within the given EducationItemPicture. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="educationItemPicture"> EducationItemPicture containing the new values (the primary identifier fields within the given EducationItemPicture will be used to address the target record) </param>
  bool UpdateEducationItemPicture(EducationItemPicture educationItemPicture);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given EducationItemPicture addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a EducationItemPicture </param>
  /// <param name="educationItemPicture"> EducationItemPicture containing the new values (the primary identifier fields within the given EducationItemPicture will be ignored) </param>
  bool UpdateEducationItemPictureByUid(Guid uid, EducationItemPicture educationItemPicture);

  /// <summary> Deletes a specific EducationItemPicture addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a EducationItemPicture </param>
  bool DeleteEducationItemPictureByUid(Guid uid);

}

/// <summary> Provides CRUD access to stored EnducationItems (based on schema version '1.3.0') </summary>
public partial interface IEnducationItemRepository {

  //// <summary> Returns an info object, which specifies the possible operations (accessor specific permissions) regarding EnducationItems.</summary>
  //AccessSpecs GetAccessSpecs();

  /// <summary> Loads a specific EnducationItem addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="uid"> Represents the primary identity of a EnducationItem </param>
  EnducationItem GetEnducationItemByUid(Guid uid);

  /// <summary> Loads EnducationItems. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of EnducationItems which should be returned </param>
  EnducationItem[] GetEnducationItems(int page = 1, int pageSize = 20);

  /// <summary> Loads EnducationItems where values matching to ALL fields of the given 'filterValues' object.</summary>
    /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of EnducationItems which should be returned</param>
  EnducationItem[] SearchEnducationItems(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20);

  /// <summary> Adds a new EnducationItem and returns its primary identifier (or null on failure). </summary>
  /// <param name="enducationItem"> EnducationItem containing the new values </param>
  bool AddNewEnducationItem(EnducationItem enducationItem);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given EnducationItem addressed by the primary identifier fields within the given EnducationItem. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="enducationItem"> EnducationItem containing the new values (the primary identifier fields within the given EnducationItem will be used to address the target record) </param>
  bool UpdateEnducationItem(EnducationItem enducationItem);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given EnducationItem addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a EnducationItem </param>
  /// <param name="enducationItem"> EnducationItem containing the new values (the primary identifier fields within the given EnducationItem will be ignored) </param>
  bool UpdateEnducationItemByUid(Guid uid, EnducationItem enducationItem);

  /// <summary> Deletes a specific EnducationItem addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a EnducationItem </param>
  bool DeleteEnducationItemByUid(Guid uid);

}

/// <summary> Provides CRUD access to stored Subjects (based on schema version '1.3.0') </summary>
public partial interface ISubjectRepository {

  //// <summary> Returns an info object, which specifies the possible operations (accessor specific permissions) regarding Subjects.</summary>
  //AccessSpecs GetAccessSpecs();

  /// <summary> Loads a specific Subject addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="officialName"> Represents the primary identity of a Subject </param>
  Subject GetSubjectByOfficialName(String officialName);

  /// <summary> Loads Subjects. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of Subjects which should be returned </param>
  Subject[] GetSubjects(int page = 1, int pageSize = 20);

  /// <summary> Loads Subjects where values matching to ALL fields of the given 'filterValues' object.</summary>
    /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of Subjects which should be returned</param>
  Subject[] SearchSubjects(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20);

  /// <summary> Adds a new Subject and returns its primary identifier (or null on failure). </summary>
  /// <param name="subject"> Subject containing the new values </param>
  bool AddNewSubject(Subject subject);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Subject addressed by the primary identifier fields within the given Subject. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="subject"> Subject containing the new values (the primary identifier fields within the given Subject will be used to address the target record) </param>
  bool UpdateSubject(Subject subject);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Subject addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="officialName"> Represents the primary identity of a Subject </param>
  /// <param name="subject"> Subject containing the new values (the primary identifier fields within the given Subject will be ignored) </param>
  bool UpdateSubjectByOfficialName(String officialName, Subject subject);

  /// <summary> Deletes a specific Subject addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="officialName"> Represents the primary identity of a Subject </param>
  bool DeleteSubjectByOfficialName(String officialName);

}

/// <summary> Provides CRUD access to stored SubjectTeachings (based on schema version '1.3.0') </summary>
public partial interface ISubjectTeachingRepository {

  //// <summary> Returns an info object, which specifies the possible operations (accessor specific permissions) regarding SubjectTeachings.</summary>
  //AccessSpecs GetAccessSpecs();

  /// <summary> Loads a specific SubjectTeaching addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="subjectTeachingIdentity"> Composite Key, which represents the primary identity of a SubjectTeaching </param>
  SubjectTeaching GetSubjectTeachingBySubjectTeachingIdentity(SubjectTeachingIdentity subjectTeachingIdentity);

  /// <summary> Loads SubjectTeachings. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of SubjectTeachings which should be returned </param>
  SubjectTeaching[] GetSubjectTeachings(int page = 1, int pageSize = 20);

  /// <summary> Loads SubjectTeachings where values matching to ALL fields of the given 'filterValues' object.</summary>
    /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of SubjectTeachings which should be returned</param>
  SubjectTeaching[] SearchSubjectTeachings(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20);

  /// <summary> Adds a new SubjectTeaching and returns its primary identifier (or null on failure). </summary>
  /// <param name="subjectTeaching"> SubjectTeaching containing the new values </param>
  bool AddNewSubjectTeaching(SubjectTeaching subjectTeaching);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given SubjectTeaching addressed by the primary identifier fields within the given SubjectTeaching. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="subjectTeaching"> SubjectTeaching containing the new values (the primary identifier fields within the given SubjectTeaching will be used to address the target record) </param>
  bool UpdateSubjectTeaching(SubjectTeaching subjectTeaching);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given SubjectTeaching addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="subjectTeachingIdentity"> Composite Key, which represents the primary identity of a SubjectTeaching </param>
  /// <param name="subjectTeaching"> SubjectTeaching containing the new values (the primary identifier fields within the given SubjectTeaching will be ignored) </param>
  bool UpdateSubjectTeachingBySubjectTeachingIdentity(SubjectTeachingIdentity subjectTeachingIdentity, SubjectTeaching subjectTeaching);

  /// <summary> Deletes a specific SubjectTeaching addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="subjectTeachingIdentity"> Composite Key, which represents the primary identity of a SubjectTeaching </param>
  bool DeleteSubjectTeachingBySubjectTeachingIdentity(SubjectTeachingIdentity subjectTeachingIdentity);

}

/// <summary> Composite Key, which represents the primary identity of a SubjectTeaching </summary>
public class SubjectTeachingIdentity {
  public Guid TeacherUid;
  public String SubjectOfficialName;
}

/// <summary> Provides CRUD access to stored EnducationItemsOfRoomRelatedEducationItem (based on schema version '1.3.0') </summary>
public partial interface IRoomRelatedEducationItemRepository {

  //// <summary> Returns an info object, which specifies the possible operations (accessor specific permissions) regarding EnducationItemsOfRoomRelatedEducationItem.</summary>
  //AccessSpecs GetAccessSpecs();

  /// <summary> Loads a specific RoomRelatedEducationItem addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="uid"> Represents the primary identity of a RoomRelatedEducationItem </param>
  RoomRelatedEducationItem GetRoomRelatedEducationItemByUid(Guid uid);

  /// <summary> Loads EnducationItemsOfRoomRelatedEducationItem. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of EnducationItemsOfRoomRelatedEducationItem which should be returned </param>
  RoomRelatedEducationItem[] GetEnducationItemsOfRoomRelatedEducationItem(int page = 1, int pageSize = 20);

  /// <summary> Loads EnducationItemsOfRoomRelatedEducationItem where values matching to ALL fields of the given 'filterValues' object.</summary>
    /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of EnducationItemsOfRoomRelatedEducationItem which should be returned</param>
  RoomRelatedEducationItem[] SearchEnducationItemsOfRoomRelatedEducationItem(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20);

  /// <summary> Adds a new RoomRelatedEducationItem and returns its primary identifier (or null on failure). </summary>
  /// <param name="roomRelatedEducationItem"> RoomRelatedEducationItem containing the new values </param>
  bool AddNewRoomRelatedEducationItem(RoomRelatedEducationItem roomRelatedEducationItem);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given RoomRelatedEducationItem addressed by the primary identifier fields within the given RoomRelatedEducationItem. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="roomRelatedEducationItem"> RoomRelatedEducationItem containing the new values (the primary identifier fields within the given RoomRelatedEducationItem will be used to address the target record) </param>
  bool UpdateRoomRelatedEducationItem(RoomRelatedEducationItem roomRelatedEducationItem);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given RoomRelatedEducationItem addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a RoomRelatedEducationItem </param>
  /// <param name="roomRelatedEducationItem"> RoomRelatedEducationItem containing the new values (the primary identifier fields within the given RoomRelatedEducationItem will be ignored) </param>
  bool UpdateRoomRelatedEducationItemByUid(Guid uid, RoomRelatedEducationItem roomRelatedEducationItem);

  /// <summary> Deletes a specific RoomRelatedEducationItem addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a RoomRelatedEducationItem </param>
  bool DeleteRoomRelatedEducationItemByUid(Guid uid);

}

/// <summary> Provides CRUD access to stored TeachingRequiredItems (based on schema version '1.3.0') </summary>
public partial interface ITeachingRequiredItemRepository {

  //// <summary> Returns an info object, which specifies the possible operations (accessor specific permissions) regarding TeachingRequiredItems.</summary>
  //AccessSpecs GetAccessSpecs();

  /// <summary> Loads a specific TeachingRequiredItem addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="uid"> Represents the primary identity of a TeachingRequiredItem </param>
  TeachingRequiredItem GetTeachingRequiredItemByUid(Int32 uid);

  /// <summary> Loads TeachingRequiredItems. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of TeachingRequiredItems which should be returned </param>
  TeachingRequiredItem[] GetTeachingRequiredItems(int page = 1, int pageSize = 20);

  /// <summary> Loads TeachingRequiredItems where values matching to ALL fields of the given 'filterValues' object.</summary>
    /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of TeachingRequiredItems which should be returned</param>
  TeachingRequiredItem[] SearchTeachingRequiredItems(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20);

  /// <summary> Adds a new TeachingRequiredItem and returns its primary identifier (or null on failure). </summary>
  /// <param name="teachingRequiredItem"> TeachingRequiredItem containing the new values </param>
  bool AddNewTeachingRequiredItem(TeachingRequiredItem teachingRequiredItem);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given TeachingRequiredItem addressed by the primary identifier fields within the given TeachingRequiredItem. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="teachingRequiredItem"> TeachingRequiredItem containing the new values (the primary identifier fields within the given TeachingRequiredItem will be used to address the target record) </param>
  bool UpdateTeachingRequiredItem(TeachingRequiredItem teachingRequiredItem);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given TeachingRequiredItem addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a TeachingRequiredItem </param>
  /// <param name="teachingRequiredItem"> TeachingRequiredItem containing the new values (the primary identifier fields within the given TeachingRequiredItem will be ignored) </param>
  bool UpdateTeachingRequiredItemByUid(Int32 uid, TeachingRequiredItem teachingRequiredItem);

  /// <summary> Deletes a specific TeachingRequiredItem addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a TeachingRequiredItem </param>
  bool DeleteTeachingRequiredItemByUid(Int32 uid);

}

/// <summary> Provides CRUD access to stored EnducationItemsOfTeacherRelatedEducationItem (based on schema version '1.3.0') </summary>
public partial interface ITeacherRelatedEducationItemRepository {

  //// <summary> Returns an info object, which specifies the possible operations (accessor specific permissions) regarding EnducationItemsOfTeacherRelatedEducationItem.</summary>
  //AccessSpecs GetAccessSpecs();

  /// <summary> Loads a specific TeacherRelatedEducationItem addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="uid"> Represents the primary identity of a TeacherRelatedEducationItem </param>
  TeacherRelatedEducationItem GetTeacherRelatedEducationItemByUid(Guid uid);

  /// <summary> Loads EnducationItemsOfTeacherRelatedEducationItem. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of EnducationItemsOfTeacherRelatedEducationItem which should be returned </param>
  TeacherRelatedEducationItem[] GetEnducationItemsOfTeacherRelatedEducationItem(int page = 1, int pageSize = 20);

  /// <summary> Loads EnducationItemsOfTeacherRelatedEducationItem where values matching to ALL fields of the given 'filterValues' object.</summary>
    /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of EnducationItemsOfTeacherRelatedEducationItem which should be returned</param>
  TeacherRelatedEducationItem[] SearchEnducationItemsOfTeacherRelatedEducationItem(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20);

  /// <summary> Adds a new TeacherRelatedEducationItem and returns its primary identifier (or null on failure). </summary>
  /// <param name="teacherRelatedEducationItem"> TeacherRelatedEducationItem containing the new values </param>
  bool AddNewTeacherRelatedEducationItem(TeacherRelatedEducationItem teacherRelatedEducationItem);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given TeacherRelatedEducationItem addressed by the primary identifier fields within the given TeacherRelatedEducationItem. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="teacherRelatedEducationItem"> TeacherRelatedEducationItem containing the new values (the primary identifier fields within the given TeacherRelatedEducationItem will be used to address the target record) </param>
  bool UpdateTeacherRelatedEducationItem(TeacherRelatedEducationItem teacherRelatedEducationItem);

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given TeacherRelatedEducationItem addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a TeacherRelatedEducationItem </param>
  /// <param name="teacherRelatedEducationItem"> TeacherRelatedEducationItem containing the new values (the primary identifier fields within the given TeacherRelatedEducationItem will be ignored) </param>
  bool UpdateTeacherRelatedEducationItemByUid(Guid uid, TeacherRelatedEducationItem teacherRelatedEducationItem);

  /// <summary> Deletes a specific TeacherRelatedEducationItem addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a TeacherRelatedEducationItem </param>
  bool DeleteTeacherRelatedEducationItemByUid(Guid uid);

}

}
