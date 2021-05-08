using Demo.Persistence;
using Demo.Persistence.EF;
using System;
using System.Data;
using System.Data.AccessControl;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;

namespace Demo {

/// <summary> Provides CRUD access to stored Classes (based on schema version '1.3.0') </summary>
public partial class ClassStore : IClassRepository {

  /// <summary> Loads a specific Class addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="uid"> Represents the primary identity of a Class </param>
  public Class GetClassByUid(Guid uid){
    var mac = AccessControlContext.Current;

    Class result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.Classes.AsNoTracking().AccessScopeFiltered().Select(ClassEntity.ClassSelector);

      query = query.Where((e)=>e.Uid == uid);

      //materialization / load
      result = query.SingleOrDefault();

    }

    return result;
  }

  /// <summary> Loads Classes. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of Classes which should be returned </param>
  public Class[] GetClasses(int page = 1, int pageSize = 20){
    return this.SearchClasses(null, null, page, pageSize); 
  }

  private static String[] _ExactMatchPropNames = new String[] {};
  private static String[] _FreetextPropNames = new String[] {"OfficialName"};

  /// <summary> Loads Classes where values matching to the given filterExpression</summary>
  /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of Classes which should be returned</param>
  public Class[] SearchClasses(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20){
    var mac = AccessControlContext.Current;

    Class[] result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.Classes
        .AsNoTracking()
        .AccessScopeFiltered()
        .Select(ClassEntity.ClassSelector)
      ;
      
      //apply filter (if given)
      if(filterExpression != null) {
        //just if the filterExpression isnt already a valid expression, treat it as a freetext seach string and transform it to a valid expression
        filterExpression = DynamicLinqExtensions.FreetextSearchStringToFilterExpression(filterExpression, _ExactMatchPropNames, _FreetextPropNames);
      }
      if(filterExpression != null) {
        query = query.DynamicallyFiltered(filterExpression);
      }

      //apply sorting
      if(String.IsNullOrWhiteSpace(sortingExpression)) {
        query = query.DynamicallySorted("Uid");
      }
      else{
        query = query.DynamicallySorted(sortingExpression + ", Uid");
      }

      //apply pagination
      query = query.Skip(pageSize * (page - 1)).Take(pageSize);

      //materialization / load
      result = query.ToArray();

    }

    return result;
  }

  /// <summary> Adds a new Class and returns success. </summary>
  /// <param name="class"> Class containing the new values </param>
  public bool AddNewClass(Class @class){
    var mac = AccessControlContext.Current;

    ClassEntity newEntity = new ClassEntity();
    newEntity.CopyContentFrom(@class);

    using (DemoDbContext db = new DemoDbContext()) {

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(newEntity, db)){
        return false;
      }

      if (db.Classes.Where((e)=>e.Uid == newEntity.Uid).Any()) {
        return false;
      }

      db.Classes.Add(newEntity);

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Class addressed by the primary identifier fields within the given Class. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="class"> Class containing the new values (the primary identifier fields within the given Class will be used to address the target record) </param>
  public bool UpdateClass(Class @class){
    return this.UpdateClassByUid(@class.Uid, @class);
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Class addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a Class </param>
  /// <param name="class"> Class containing the new values (the primary identifier fields within the given Class will be ignored) </param>
  public bool UpdateClassByUid(Guid uid, Class @class){
    var mac = AccessControlContext.Current;

    ClassEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.Classes.AsNoTracking();

      query = query.Where((e)=>e.Uid == uid).AccessScopeFiltered();

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if(existingEntity == null) {
        return false;
      }

      bool changeAttemptOnFixedField = false;
      existingEntity.CopyContentFrom(@class, (name) => changeAttemptOnFixedField = true);

      if (changeAttemptOnFixedField) {
        return false;
      }

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(existingEntity, db)){
        return false;
      }

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Deletes a specific Class addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a Class </param>
  public bool DeleteClassByUid(Guid uid){
    var mac = AccessControlContext.Current;

    ClassEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.Classes.AsNoTracking().AccessScopeFiltered();

      query = query.Where((e)=>e.Uid == uid);

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if (existingEntity == null) {
        return false;
      }

      db.Classes.Remove(existingEntity);

      db.SaveChanges();
    }

    return true;
  }

  private bool PreValidateAccessControlScope(ClassEntity entity, DemoDbContext db){

    var filterExpression = EntityAccessControl.BuildExpressionForLocalEntity<ClassEntity>(AccessControlContext.Current);
    if(!filterExpression.Compile().Invoke(entity)) {
      return false;
    }

    if(!db.Teachers.Where((tgt)=> tgt.Uid==entity.PrimaryTeacherUid ).AccessScopeFiltered().Any()) {
      return false;
    }
    if(!db.Rooms.Where((tgt)=> tgt.Uid==entity.RoomUid ).AccessScopeFiltered().Any()) {
      return false;
    }
    return true;
  }

}

/// <summary> Provides CRUD access to stored Teachers (based on schema version '1.3.0') </summary>
public partial class TeacherStore : ITeacherRepository {

  /// <summary> Loads a specific Teacher addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="uid"> Represents the primary identity of a Teacher </param>
  public Teacher GetTeacherByUid(Guid uid){
    var mac = AccessControlContext.Current;

    Teacher result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.Teachers.AsNoTracking().AccessScopeFiltered().Select(TeacherEntity.TeacherSelector);

      query = query.Where((e)=>e.Uid == uid);

      //materialization / load
      result = query.SingleOrDefault();

    }

    return result;
  }

  /// <summary> Loads Teachers. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of Teachers which should be returned </param>
  public Teacher[] GetTeachers(int page = 1, int pageSize = 20){
    return this.SearchTeachers(null, null, page, pageSize); 
  }

  private static String[] _ExactMatchPropNames = new String[] {};
  private static String[] _FreetextPropNames = new String[] {"FirstName", "LastName"};

  /// <summary> Loads Teachers where values matching to the given filterExpression</summary>
  /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of Teachers which should be returned</param>
  public Teacher[] SearchTeachers(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20){
    var mac = AccessControlContext.Current;

    Teacher[] result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.Teachers
        .AsNoTracking()
        .AccessScopeFiltered()
        .Select(TeacherEntity.TeacherSelector)
      ;
      
      //apply filter (if given)
      if(filterExpression != null) {
        //just if the filterExpression isnt already a valid expression, treat it as a freetext seach string and transform it to a valid expression
        filterExpression = DynamicLinqExtensions.FreetextSearchStringToFilterExpression(filterExpression, _ExactMatchPropNames, _FreetextPropNames);
      }
      if(filterExpression != null) {
        query = query.DynamicallyFiltered(filterExpression);
      }

      //apply sorting
      if(String.IsNullOrWhiteSpace(sortingExpression)) {
        query = query.DynamicallySorted("Uid");
      }
      else{
        query = query.DynamicallySorted(sortingExpression + ", Uid");
      }

      //apply pagination
      query = query.Skip(pageSize * (page - 1)).Take(pageSize);

      //materialization / load
      result = query.ToArray();

    }

    return result;
  }

  /// <summary> Adds a new Teacher and returns success. </summary>
  /// <param name="teacher"> Teacher containing the new values </param>
  public bool AddNewTeacher(Teacher teacher){
    var mac = AccessControlContext.Current;

    TeacherEntity newEntity = new TeacherEntity();
    newEntity.CopyContentFrom(teacher);

    using (DemoDbContext db = new DemoDbContext()) {

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(newEntity, db)){
        return false;
      }

      if (db.Teachers.Where((e)=>e.Uid == newEntity.Uid).Any()) {
        return false;
      }

      db.Teachers.Add(newEntity);

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Teacher addressed by the primary identifier fields within the given Teacher. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="teacher"> Teacher containing the new values (the primary identifier fields within the given Teacher will be used to address the target record) </param>
  public bool UpdateTeacher(Teacher teacher){
    return this.UpdateTeacherByUid(teacher.Uid, teacher);
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Teacher addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a Teacher </param>
  /// <param name="teacher"> Teacher containing the new values (the primary identifier fields within the given Teacher will be ignored) </param>
  public bool UpdateTeacherByUid(Guid uid, Teacher teacher){
    var mac = AccessControlContext.Current;

    TeacherEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.Teachers.AsNoTracking();

      query = query.Where((e)=>e.Uid == uid).AccessScopeFiltered();

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if(existingEntity == null) {
        return false;
      }

      bool changeAttemptOnFixedField = false;
      existingEntity.CopyContentFrom(teacher, (name) => changeAttemptOnFixedField = true);

      if (changeAttemptOnFixedField) {
        return false;
      }

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(existingEntity, db)){
        return false;
      }

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Deletes a specific Teacher addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a Teacher </param>
  public bool DeleteTeacherByUid(Guid uid){
    var mac = AccessControlContext.Current;

    TeacherEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.Teachers.AsNoTracking().AccessScopeFiltered();

      query = query.Where((e)=>e.Uid == uid);

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if (existingEntity == null) {
        return false;
      }

      db.Teachers.Remove(existingEntity);

      db.SaveChanges();
    }

    return true;
  }

  private bool PreValidateAccessControlScope(TeacherEntity entity, DemoDbContext db){

    var filterExpression = EntityAccessControl.BuildExpressionForLocalEntity<TeacherEntity>(AccessControlContext.Current);
    if(!filterExpression.Compile().Invoke(entity)) {
      return false;
    }

    return true;
  }

}

/// <summary> Provides CRUD access to stored Rooms (based on schema version '1.3.0') </summary>
public partial class RoomStore : IRoomRepository {

  /// <summary> Loads a specific Room addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="uid"> Represents the primary identity of a Room </param>
  public Room GetRoomByUid(Guid uid){
    var mac = AccessControlContext.Current;

    Room result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.Rooms.AsNoTracking().AccessScopeFiltered().Select(RoomEntity.RoomSelector);

      query = query.Where((e)=>e.Uid == uid);

      //materialization / load
      result = query.SingleOrDefault();

    }

    return result;
  }

  /// <summary> Loads Rooms. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of Rooms which should be returned </param>
  public Room[] GetRooms(int page = 1, int pageSize = 20){
    return this.SearchRooms(null, null, page, pageSize); 
  }

  private static String[] _ExactMatchPropNames = new String[] {};
  private static String[] _FreetextPropNames = new String[] {"OfficialName"};

  /// <summary> Loads Rooms where values matching to the given filterExpression</summary>
  /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of Rooms which should be returned</param>
  public Room[] SearchRooms(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20){
    var mac = AccessControlContext.Current;

    Room[] result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.Rooms
        .AsNoTracking()
        .AccessScopeFiltered()
        .Select(RoomEntity.RoomSelector)
      ;
      
      //apply filter (if given)
      if(filterExpression != null) {
        //just if the filterExpression isnt already a valid expression, treat it as a freetext seach string and transform it to a valid expression
        filterExpression = DynamicLinqExtensions.FreetextSearchStringToFilterExpression(filterExpression, _ExactMatchPropNames, _FreetextPropNames);
      }
      if(filterExpression != null) {
        query = query.DynamicallyFiltered(filterExpression);
      }

      //apply sorting
      if(String.IsNullOrWhiteSpace(sortingExpression)) {
        query = query.DynamicallySorted("Uid");
      }
      else{
        query = query.DynamicallySorted(sortingExpression + ", Uid");
      }

      //apply pagination
      query = query.Skip(pageSize * (page - 1)).Take(pageSize);

      //materialization / load
      result = query.ToArray();

    }

    return result;
  }

  /// <summary> Adds a new Room and returns success. </summary>
  /// <param name="room"> Room containing the new values </param>
  public bool AddNewRoom(Room room){
    var mac = AccessControlContext.Current;

    RoomEntity newEntity = new RoomEntity();
    newEntity.CopyContentFrom(room);

    using (DemoDbContext db = new DemoDbContext()) {

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(newEntity, db)){
        return false;
      }

      if (db.Rooms.Where((e)=>e.Uid == newEntity.Uid).Any()) {
        return false;
      }

      db.Rooms.Add(newEntity);

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Room addressed by the primary identifier fields within the given Room. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="room"> Room containing the new values (the primary identifier fields within the given Room will be used to address the target record) </param>
  public bool UpdateRoom(Room room){
    return this.UpdateRoomByUid(room.Uid, room);
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Room addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a Room </param>
  /// <param name="room"> Room containing the new values (the primary identifier fields within the given Room will be ignored) </param>
  public bool UpdateRoomByUid(Guid uid, Room room){
    var mac = AccessControlContext.Current;

    RoomEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.Rooms.AsNoTracking();

      query = query.Where((e)=>e.Uid == uid).AccessScopeFiltered();

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if(existingEntity == null) {
        return false;
      }

      bool changeAttemptOnFixedField = false;
      existingEntity.CopyContentFrom(room, (name) => changeAttemptOnFixedField = true);

      if (changeAttemptOnFixedField) {
        return false;
      }

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(existingEntity, db)){
        return false;
      }

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Deletes a specific Room addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a Room </param>
  public bool DeleteRoomByUid(Guid uid){
    var mac = AccessControlContext.Current;

    RoomEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.Rooms.AsNoTracking().AccessScopeFiltered();

      query = query.Where((e)=>e.Uid == uid);

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if (existingEntity == null) {
        return false;
      }

      db.Rooms.Remove(existingEntity);

      db.SaveChanges();
    }

    return true;
  }

  private bool PreValidateAccessControlScope(RoomEntity entity, DemoDbContext db){

    var filterExpression = EntityAccessControl.BuildExpressionForLocalEntity<RoomEntity>(AccessControlContext.Current);
    if(!filterExpression.Compile().Invoke(entity)) {
      return false;
    }

    return true;
  }

}

/// <summary> Provides CRUD access to stored Students (based on schema version '1.3.0') </summary>
public partial class StudentStore : IStudentRepository {

  /// <summary> Loads a specific Student addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="uid"> Represents the primary identity of a Student </param>
  public Student GetStudentByUid(Guid uid){
    var mac = AccessControlContext.Current;

    Student result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.Students.AsNoTracking().AccessScopeFiltered().Select(StudentEntity.StudentSelector);

      query = query.Where((e)=>e.Uid == uid);

      //materialization / load
      result = query.SingleOrDefault();

    }

    return result;
  }

  /// <summary> Loads Students. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of Students which should be returned </param>
  public Student[] GetStudents(int page = 1, int pageSize = 20){
    return this.SearchStudents(null, null, page, pageSize); 
  }

  private static String[] _ExactMatchPropNames = new String[] {};
  private static String[] _FreetextPropNames = new String[] {"FirstName", "LastName"};

  /// <summary> Loads Students where values matching to the given filterExpression</summary>
  /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of Students which should be returned</param>
  public Student[] SearchStudents(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20){
    var mac = AccessControlContext.Current;

    Student[] result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.Students
        .AsNoTracking()
        .AccessScopeFiltered()
        .Select(StudentEntity.StudentSelector)
      ;
      
      //apply filter (if given)
      if(filterExpression != null) {
        //just if the filterExpression isnt already a valid expression, treat it as a freetext seach string and transform it to a valid expression
        filterExpression = DynamicLinqExtensions.FreetextSearchStringToFilterExpression(filterExpression, _ExactMatchPropNames, _FreetextPropNames);
      }
      if(filterExpression != null) {
        query = query.DynamicallyFiltered(filterExpression);
      }

      //apply sorting
      if(String.IsNullOrWhiteSpace(sortingExpression)) {
        query = query.DynamicallySorted("Uid");
      }
      else{
        query = query.DynamicallySorted(sortingExpression + ", Uid");
      }

      //apply pagination
      query = query.Skip(pageSize * (page - 1)).Take(pageSize);

      //materialization / load
      result = query.ToArray();

    }

    return result;
  }

  /// <summary> Adds a new Student and returns success. </summary>
  /// <param name="student"> Student containing the new values </param>
  public bool AddNewStudent(Student student){
    var mac = AccessControlContext.Current;

    StudentEntity newEntity = new StudentEntity();
    newEntity.CopyContentFrom(student);

    using (DemoDbContext db = new DemoDbContext()) {

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(newEntity, db)){
        return false;
      }

      if (db.Students.Where((e)=>e.Uid == newEntity.Uid).Any()) {
        return false;
      }

      db.Students.Add(newEntity);

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Student addressed by the primary identifier fields within the given Student. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="student"> Student containing the new values (the primary identifier fields within the given Student will be used to address the target record) </param>
  public bool UpdateStudent(Student student){
    return this.UpdateStudentByUid(student.Uid, student);
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Student addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a Student </param>
  /// <param name="student"> Student containing the new values (the primary identifier fields within the given Student will be ignored) </param>
  public bool UpdateStudentByUid(Guid uid, Student student){
    var mac = AccessControlContext.Current;

    StudentEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.Students.AsNoTracking();

      query = query.Where((e)=>e.Uid == uid).AccessScopeFiltered();

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if(existingEntity == null) {
        return false;
      }

      bool changeAttemptOnFixedField = false;
      existingEntity.CopyContentFrom(student, (name) => changeAttemptOnFixedField = true);

      if (changeAttemptOnFixedField) {
        return false;
      }

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(existingEntity, db)){
        return false;
      }

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Deletes a specific Student addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a Student </param>
  public bool DeleteStudentByUid(Guid uid){
    var mac = AccessControlContext.Current;

    StudentEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.Students.AsNoTracking().AccessScopeFiltered();

      query = query.Where((e)=>e.Uid == uid);

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if (existingEntity == null) {
        return false;
      }

      db.Students.Remove(existingEntity);

      db.SaveChanges();
    }

    return true;
  }

  private bool PreValidateAccessControlScope(StudentEntity entity, DemoDbContext db){

    var filterExpression = EntityAccessControl.BuildExpressionForLocalEntity<StudentEntity>(AccessControlContext.Current);
    if(!filterExpression.Compile().Invoke(entity)) {
      return false;
    }

    if(!db.Classes.Where((tgt)=> tgt.Uid==entity.ClassUid ).AccessScopeFiltered().Any()) {
      return false;
    }
    return true;
  }

}

/// <summary> Provides CRUD access to stored Lessons (based on schema version '1.3.0') </summary>
public partial class LessonStore : ILessonRepository {

  /// <summary> Loads a specific Lesson addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="uid"> Represents the primary identity of a Lesson </param>
  public Lesson GetLessonByUid(Guid uid){
    var mac = AccessControlContext.Current;

    Lesson result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.Lessons.AsNoTracking().AccessScopeFiltered().Select(LessonEntity.LessonSelector);

      query = query.Where((e)=>e.Uid == uid);

      //materialization / load
      result = query.SingleOrDefault();

    }

    return result;
  }

  /// <summary> Loads Lessons. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of Lessons which should be returned </param>
  public Lesson[] GetLessons(int page = 1, int pageSize = 20){
    return this.SearchLessons(null, null, page, pageSize); 
  }

  private static String[] _ExactMatchPropNames = new String[] {"SubjectOfficialName"};
  private static String[] _FreetextPropNames = new String[] {"OfficialName"};

  /// <summary> Loads Lessons where values matching to the given filterExpression</summary>
  /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of Lessons which should be returned</param>
  public Lesson[] SearchLessons(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20){
    var mac = AccessControlContext.Current;

    Lesson[] result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.Lessons
        .AsNoTracking()
        .AccessScopeFiltered()
        .Select(LessonEntity.LessonSelector)
      ;
      
      //apply filter (if given)
      if(filterExpression != null) {
        //just if the filterExpression isnt already a valid expression, treat it as a freetext seach string and transform it to a valid expression
        filterExpression = DynamicLinqExtensions.FreetextSearchStringToFilterExpression(filterExpression, _ExactMatchPropNames, _FreetextPropNames);
      }
      if(filterExpression != null) {
        query = query.DynamicallyFiltered(filterExpression);
      }

      //apply sorting
      if(String.IsNullOrWhiteSpace(sortingExpression)) {
        query = query.DynamicallySorted("Uid");
      }
      else{
        query = query.DynamicallySorted(sortingExpression + ", Uid");
      }

      //apply pagination
      query = query.Skip(pageSize * (page - 1)).Take(pageSize);

      //materialization / load
      result = query.ToArray();

    }

    return result;
  }

  /// <summary> Adds a new Lesson and returns success. </summary>
  /// <param name="lesson"> Lesson containing the new values </param>
  public bool AddNewLesson(Lesson lesson){
    var mac = AccessControlContext.Current;

    LessonEntity newEntity = new LessonEntity();
    newEntity.CopyContentFrom(lesson);

    using (DemoDbContext db = new DemoDbContext()) {

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(newEntity, db)){
        return false;
      }

      if (db.Lessons.Where((e)=>e.Uid == newEntity.Uid).Any()) {
        return false;
      }

      db.Lessons.Add(newEntity);

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Lesson addressed by the primary identifier fields within the given Lesson. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="lesson"> Lesson containing the new values (the primary identifier fields within the given Lesson will be used to address the target record) </param>
  public bool UpdateLesson(Lesson lesson){
    return this.UpdateLessonByUid(lesson.Uid, lesson);
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Lesson addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a Lesson </param>
  /// <param name="lesson"> Lesson containing the new values (the primary identifier fields within the given Lesson will be ignored) </param>
  public bool UpdateLessonByUid(Guid uid, Lesson lesson){
    var mac = AccessControlContext.Current;

    LessonEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.Lessons.AsNoTracking();

      query = query.Where((e)=>e.Uid == uid).AccessScopeFiltered();

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if(existingEntity == null) {
        return false;
      }

      bool changeAttemptOnFixedField = false;
      existingEntity.CopyContentFrom(lesson, (name) => changeAttemptOnFixedField = true);

      if (changeAttemptOnFixedField) {
        return false;
      }

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(existingEntity, db)){
        return false;
      }

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Deletes a specific Lesson addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a Lesson </param>
  public bool DeleteLessonByUid(Guid uid){
    var mac = AccessControlContext.Current;

    LessonEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.Lessons.AsNoTracking().AccessScopeFiltered();

      query = query.Where((e)=>e.Uid == uid);

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if (existingEntity == null) {
        return false;
      }

      db.Lessons.Remove(existingEntity);

      db.SaveChanges();
    }

    return true;
  }

  private bool PreValidateAccessControlScope(LessonEntity entity, DemoDbContext db){

    var filterExpression = EntityAccessControl.BuildExpressionForLocalEntity<LessonEntity>(AccessControlContext.Current);
    if(!filterExpression.Compile().Invoke(entity)) {
      return false;
    }

    if(!db.Classes.Where((tgt)=> tgt.Uid==entity.EducatedClassUid ).AccessScopeFiltered().Any()) {
      return false;
    }
    if(!db.Rooms.Where((tgt)=> tgt.Uid==entity.RoomUid ).AccessScopeFiltered().Any()) {
      return false;
    }
    if(!db.SubjectTeachings.Where((tgt)=> tgt.TeacherUid==entity.TeacherUid && tgt.SubjectOfficialName==entity.SubjectOfficialName ).AccessScopeFiltered().Any()) {
      return false;
    }
    return true;
  }

}

/// <summary> Provides CRUD access to stored EducationItemPictures (based on schema version '1.3.0') </summary>
public partial class EducationItemPictureStore : IEducationItemPictureRepository {

  /// <summary> Loads a specific EducationItemPicture addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="uid"> Represents the primary identity of a EducationItemPicture </param>
  public EducationItemPicture GetEducationItemPictureByUid(Guid uid){
    var mac = AccessControlContext.Current;

    EducationItemPicture result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.EducationItemPictures.AsNoTracking().AccessScopeFiltered().Select(EducationItemPictureEntity.EducationItemPictureSelector);

      query = query.Where((e)=>e.Uid == uid);

      //materialization / load
      result = query.SingleOrDefault();

    }

    return result;
  }

  /// <summary> Loads EducationItemPictures. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of EducationItemPictures which should be returned </param>
  public EducationItemPicture[] GetEducationItemPictures(int page = 1, int pageSize = 20){
    return this.SearchEducationItemPictures(null, null, page, pageSize); 
  }

  private static String[] _ExactMatchPropNames = new String[] {};
  private static String[] _FreetextPropNames = new String[] {};

  /// <summary> Loads EducationItemPictures where values matching to the given filterExpression</summary>
  /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of EducationItemPictures which should be returned</param>
  public EducationItemPicture[] SearchEducationItemPictures(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20){
    var mac = AccessControlContext.Current;

    EducationItemPicture[] result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.EducationItemPictures
        .AsNoTracking()
        .AccessScopeFiltered()
        .Select(EducationItemPictureEntity.EducationItemPictureSelector)
      ;
      
      //apply filter (if given)
      if(filterExpression != null) {
        //just if the filterExpression isnt already a valid expression, treat it as a freetext seach string and transform it to a valid expression
        filterExpression = DynamicLinqExtensions.FreetextSearchStringToFilterExpression(filterExpression, _ExactMatchPropNames, _FreetextPropNames);
      }
      if(filterExpression != null) {
        query = query.DynamicallyFiltered(filterExpression);
      }

      //apply sorting
      if(String.IsNullOrWhiteSpace(sortingExpression)) {
        query = query.DynamicallySorted("Uid");
      }
      else{
        query = query.DynamicallySorted(sortingExpression + ", Uid");
      }

      //apply pagination
      query = query.Skip(pageSize * (page - 1)).Take(pageSize);

      //materialization / load
      result = query.ToArray();

    }

    return result;
  }

  /// <summary> Adds a new EducationItemPicture and returns success. </summary>
  /// <param name="educationItemPicture"> EducationItemPicture containing the new values </param>
  public bool AddNewEducationItemPicture(EducationItemPicture educationItemPicture){
    var mac = AccessControlContext.Current;

    EducationItemPictureEntity newEntity = new EducationItemPictureEntity();
    newEntity.CopyContentFrom(educationItemPicture);

    using (DemoDbContext db = new DemoDbContext()) {

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(newEntity, db)){
        return false;
      }

      if (db.EducationItemPictures.Where((e)=>e.Uid == newEntity.Uid).Any()) {
        return false;
      }

      db.EducationItemPictures.Add(newEntity);

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given EducationItemPicture addressed by the primary identifier fields within the given EducationItemPicture. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="educationItemPicture"> EducationItemPicture containing the new values (the primary identifier fields within the given EducationItemPicture will be used to address the target record) </param>
  public bool UpdateEducationItemPicture(EducationItemPicture educationItemPicture){
    return this.UpdateEducationItemPictureByUid(educationItemPicture.Uid, educationItemPicture);
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given EducationItemPicture addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a EducationItemPicture </param>
  /// <param name="educationItemPicture"> EducationItemPicture containing the new values (the primary identifier fields within the given EducationItemPicture will be ignored) </param>
  public bool UpdateEducationItemPictureByUid(Guid uid, EducationItemPicture educationItemPicture){
    var mac = AccessControlContext.Current;

    EducationItemPictureEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.EducationItemPictures.AsNoTracking();

      query = query.Where((e)=>e.Uid == uid).AccessScopeFiltered();

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if(existingEntity == null) {
        return false;
      }

      bool changeAttemptOnFixedField = false;
      existingEntity.CopyContentFrom(educationItemPicture, (name) => changeAttemptOnFixedField = true);

      if (changeAttemptOnFixedField) {
        return false;
      }

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(existingEntity, db)){
        return false;
      }

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Deletes a specific EducationItemPicture addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a EducationItemPicture </param>
  public bool DeleteEducationItemPictureByUid(Guid uid){
    var mac = AccessControlContext.Current;

    EducationItemPictureEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.EducationItemPictures.AsNoTracking().AccessScopeFiltered();

      query = query.Where((e)=>e.Uid == uid);

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if (existingEntity == null) {
        return false;
      }

      db.EducationItemPictures.Remove(existingEntity);

      db.SaveChanges();
    }

    return true;
  }

  private bool PreValidateAccessControlScope(EducationItemPictureEntity entity, DemoDbContext db){

    var filterExpression = EntityAccessControl.BuildExpressionForLocalEntity<EducationItemPictureEntity>(AccessControlContext.Current);
    if(!filterExpression.Compile().Invoke(entity)) {
      return false;
    }

    if(!db.EnducationItems.Where((tgt)=> tgt.Uid==entity.Uid ).AccessScopeFiltered().Any()) {
      return false;
    }
    return true;
  }

}

/// <summary> Provides CRUD access to stored EnducationItems (based on schema version '1.3.0') </summary>
public partial class EnducationItemStore : IEnducationItemRepository {

  /// <summary> Loads a specific EnducationItem addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="uid"> Represents the primary identity of a EnducationItem </param>
  public EnducationItem GetEnducationItemByUid(Guid uid){
    var mac = AccessControlContext.Current;

    EnducationItem result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.EnducationItems.AsNoTracking().AccessScopeFiltered().Select(EnducationItemEntity.EnducationItemSelector);

      query = query.Where((e)=>e.Uid == uid);

      //materialization / load
      result = query.SingleOrDefault();

    }

    return result;
  }

  /// <summary> Loads EnducationItems. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of EnducationItems which should be returned </param>
  public EnducationItem[] GetEnducationItems(int page = 1, int pageSize = 20){
    return this.SearchEnducationItems(null, null, page, pageSize); 
  }

  private static String[] _ExactMatchPropNames = new String[] {"DedicatedToSubjectName"};
  private static String[] _FreetextPropNames = new String[] {"Title"};

  /// <summary> Loads EnducationItems where values matching to the given filterExpression</summary>
  /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of EnducationItems which should be returned</param>
  public EnducationItem[] SearchEnducationItems(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20){
    var mac = AccessControlContext.Current;

    EnducationItem[] result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.EnducationItems
        .AsNoTracking()
        .AccessScopeFiltered()
        .Select(EnducationItemEntity.EnducationItemSelector)
      ;
      
      //apply filter (if given)
      if(filterExpression != null) {
        //just if the filterExpression isnt already a valid expression, treat it as a freetext seach string and transform it to a valid expression
        filterExpression = DynamicLinqExtensions.FreetextSearchStringToFilterExpression(filterExpression, _ExactMatchPropNames, _FreetextPropNames);
      }
      if(filterExpression != null) {
        query = query.DynamicallyFiltered(filterExpression);
      }

      //apply sorting
      if(String.IsNullOrWhiteSpace(sortingExpression)) {
        query = query.DynamicallySorted("Uid");
      }
      else{
        query = query.DynamicallySorted(sortingExpression + ", Uid");
      }

      //apply pagination
      query = query.Skip(pageSize * (page - 1)).Take(pageSize);

      //materialization / load
      result = query.ToArray();

    }

    return result;
  }

  /// <summary> Adds a new EnducationItem and returns success. </summary>
  /// <param name="enducationItem"> EnducationItem containing the new values </param>
  public bool AddNewEnducationItem(EnducationItem enducationItem){
    var mac = AccessControlContext.Current;

    EnducationItemEntity newEntity = new EnducationItemEntity();
    newEntity.CopyContentFrom(enducationItem);

    using (DemoDbContext db = new DemoDbContext()) {

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(newEntity, db)){
        return false;
      }

      if (db.EnducationItems.Where((e)=>e.Uid == newEntity.Uid).Any()) {
        return false;
      }

      db.EnducationItems.Add(newEntity);

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given EnducationItem addressed by the primary identifier fields within the given EnducationItem. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="enducationItem"> EnducationItem containing the new values (the primary identifier fields within the given EnducationItem will be used to address the target record) </param>
  public bool UpdateEnducationItem(EnducationItem enducationItem){
    return this.UpdateEnducationItemByUid(enducationItem.Uid, enducationItem);
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given EnducationItem addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a EnducationItem </param>
  /// <param name="enducationItem"> EnducationItem containing the new values (the primary identifier fields within the given EnducationItem will be ignored) </param>
  public bool UpdateEnducationItemByUid(Guid uid, EnducationItem enducationItem){
    var mac = AccessControlContext.Current;

    EnducationItemEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.EnducationItems.AsNoTracking();

      query = query.Where((e)=>e.Uid == uid).AccessScopeFiltered();

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if(existingEntity == null) {
        return false;
      }

      bool changeAttemptOnFixedField = false;
      existingEntity.CopyContentFrom(enducationItem, (name) => changeAttemptOnFixedField = true);

      if (changeAttemptOnFixedField) {
        return false;
      }

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(existingEntity, db)){
        return false;
      }

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Deletes a specific EnducationItem addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a EnducationItem </param>
  public bool DeleteEnducationItemByUid(Guid uid){
    var mac = AccessControlContext.Current;

    EnducationItemEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.EnducationItems.AsNoTracking().AccessScopeFiltered();

      query = query.Where((e)=>e.Uid == uid);

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if (existingEntity == null) {
        return false;
      }

      db.EnducationItems.Remove(existingEntity);

      db.SaveChanges();
    }

    return true;
  }

  private bool PreValidateAccessControlScope(EnducationItemEntity entity, DemoDbContext db){

    var filterExpression = EntityAccessControl.BuildExpressionForLocalEntity<EnducationItemEntity>(AccessControlContext.Current);
    if(!filterExpression.Compile().Invoke(entity)) {
      return false;
    }

    if(!db.Subjects.Where((tgt)=> tgt.OfficialName==entity.DedicatedToSubjectName ).AccessScopeFiltered().Any()) {
      return false;
    }
    return true;
  }

}

/// <summary> Provides CRUD access to stored Subjects (based on schema version '1.3.0') </summary>
public partial class SubjectStore : ISubjectRepository {

  /// <summary> Loads a specific Subject addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="officialName"> Represents the primary identity of a Subject </param>
  public Subject GetSubjectByOfficialName(String officialName){
    var mac = AccessControlContext.Current;

    Subject result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.Subjects.AsNoTracking().AccessScopeFiltered().Select(SubjectEntity.SubjectSelector);

      query = query.Where((e)=>e.OfficialName == officialName);

      //materialization / load
      result = query.SingleOrDefault();

    }

    return result;
  }

  /// <summary> Loads Subjects. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of Subjects which should be returned </param>
  public Subject[] GetSubjects(int page = 1, int pageSize = 20){
    return this.SearchSubjects(null, null, page, pageSize); 
  }

  private static String[] _ExactMatchPropNames = new String[] {"OfficialName"};
  private static String[] _FreetextPropNames = new String[] {};

  /// <summary> Loads Subjects where values matching to the given filterExpression</summary>
  /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of Subjects which should be returned</param>
  public Subject[] SearchSubjects(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20){
    var mac = AccessControlContext.Current;

    Subject[] result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.Subjects
        .AsNoTracking()
        .AccessScopeFiltered()
        .Select(SubjectEntity.SubjectSelector)
      ;
      
      //apply filter (if given)
      if(filterExpression != null) {
        //just if the filterExpression isnt already a valid expression, treat it as a freetext seach string and transform it to a valid expression
        filterExpression = DynamicLinqExtensions.FreetextSearchStringToFilterExpression(filterExpression, _ExactMatchPropNames, _FreetextPropNames);
      }
      if(filterExpression != null) {
        query = query.DynamicallyFiltered(filterExpression);
      }

      //apply sorting
      if(String.IsNullOrWhiteSpace(sortingExpression)) {
        query = query.DynamicallySorted("OfficialName");
      }
      else{
        query = query.DynamicallySorted(sortingExpression + ", OfficialName");
      }

      //apply pagination
      query = query.Skip(pageSize * (page - 1)).Take(pageSize);

      //materialization / load
      result = query.ToArray();

    }

    return result;
  }

  /// <summary> Adds a new Subject and returns success. </summary>
  /// <param name="subject"> Subject containing the new values </param>
  public bool AddNewSubject(Subject subject){
    var mac = AccessControlContext.Current;

    SubjectEntity newEntity = new SubjectEntity();
    newEntity.CopyContentFrom(subject);

    using (DemoDbContext db = new DemoDbContext()) {

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(newEntity, db)){
        return false;
      }

      if (db.Subjects.Where((e)=>e.OfficialName == newEntity.OfficialName).Any()) {
        return false;
      }

      db.Subjects.Add(newEntity);

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Subject addressed by the primary identifier fields within the given Subject. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="subject"> Subject containing the new values (the primary identifier fields within the given Subject will be used to address the target record) </param>
  public bool UpdateSubject(Subject subject){
    return this.UpdateSubjectByOfficialName(subject.OfficialName, subject);
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given Subject addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="officialName"> Represents the primary identity of a Subject </param>
  /// <param name="subject"> Subject containing the new values (the primary identifier fields within the given Subject will be ignored) </param>
  public bool UpdateSubjectByOfficialName(String officialName, Subject subject){
    var mac = AccessControlContext.Current;

    SubjectEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.Subjects.AsNoTracking();

      query = query.Where((e)=>e.OfficialName == officialName).AccessScopeFiltered();

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if(existingEntity == null) {
        return false;
      }

      bool changeAttemptOnFixedField = false;
      existingEntity.CopyContentFrom(subject, (name) => changeAttemptOnFixedField = true);

      if (changeAttemptOnFixedField) {
        return false;
      }

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(existingEntity, db)){
        return false;
      }

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Deletes a specific Subject addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="officialName"> Represents the primary identity of a Subject </param>
  public bool DeleteSubjectByOfficialName(String officialName){
    var mac = AccessControlContext.Current;

    SubjectEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.Subjects.AsNoTracking().AccessScopeFiltered();

      query = query.Where((e)=>e.OfficialName == officialName);

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if (existingEntity == null) {
        return false;
      }

      db.Subjects.Remove(existingEntity);

      db.SaveChanges();
    }

    return true;
  }

  private bool PreValidateAccessControlScope(SubjectEntity entity, DemoDbContext db){

    var filterExpression = EntityAccessControl.BuildExpressionForLocalEntity<SubjectEntity>(AccessControlContext.Current);
    if(!filterExpression.Compile().Invoke(entity)) {
      return false;
    }

    return true;
  }

}

/// <summary> Provides CRUD access to stored SubjectTeachings (based on schema version '1.3.0') </summary>
public partial class SubjectTeachingStore : ISubjectTeachingRepository {

  /// <summary> Loads a specific SubjectTeaching addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="subjectTeachingIdentity"> Composite Key, which represents the primary identity of a SubjectTeaching </param>
  public SubjectTeaching GetSubjectTeachingBySubjectTeachingIdentity(SubjectTeachingIdentity subjectTeachingIdentity){
    var mac = AccessControlContext.Current;

    SubjectTeaching result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.SubjectTeachings.AsNoTracking().AccessScopeFiltered().Select(SubjectTeachingEntity.SubjectTeachingSelector);

      query = query.Where((e)=> e.TeacherUid == subjectTeachingIdentity.TeacherUid && e.SubjectOfficialName == subjectTeachingIdentity.SubjectOfficialName);

      //materialization / load
      result = query.SingleOrDefault();

    }

    return result;
  }

  /// <summary> Loads SubjectTeachings. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of SubjectTeachings which should be returned </param>
  public SubjectTeaching[] GetSubjectTeachings(int page = 1, int pageSize = 20){
    return this.SearchSubjectTeachings(null, null, page, pageSize); 
  }

  private static String[] _ExactMatchPropNames = new String[] {"SubjectOfficialName", "SubjectOfficialName"};
  private static String[] _FreetextPropNames = new String[] {};

  /// <summary> Loads SubjectTeachings where values matching to the given filterExpression</summary>
  /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of SubjectTeachings which should be returned</param>
  public SubjectTeaching[] SearchSubjectTeachings(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20){
    var mac = AccessControlContext.Current;

    SubjectTeaching[] result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.SubjectTeachings
        .AsNoTracking()
        .AccessScopeFiltered()
        .Select(SubjectTeachingEntity.SubjectTeachingSelector)
      ;
      
      //apply filter (if given)
      if(filterExpression != null) {
        //just if the filterExpression isnt already a valid expression, treat it as a freetext seach string and transform it to a valid expression
        filterExpression = DynamicLinqExtensions.FreetextSearchStringToFilterExpression(filterExpression, _ExactMatchPropNames, _FreetextPropNames);
      }
      if(filterExpression != null) {
        query = query.DynamicallyFiltered(filterExpression);
      }

      //apply sorting
      if(String.IsNullOrWhiteSpace(sortingExpression)) {
        query = query.DynamicallySorted("TeacherUid, SubjectOfficialName");
      }
      else{
        query = query.DynamicallySorted(sortingExpression + ", TeacherUid, SubjectOfficialName");
      }

      //apply pagination
      query = query.Skip(pageSize * (page - 1)).Take(pageSize);

      //materialization / load
      result = query.ToArray();

    }

    return result;
  }

  /// <summary> Adds a new SubjectTeaching and returns success. </summary>
  /// <param name="subjectTeaching"> SubjectTeaching containing the new values </param>
  public bool AddNewSubjectTeaching(SubjectTeaching subjectTeaching){
    var mac = AccessControlContext.Current;

    SubjectTeachingEntity newEntity = new SubjectTeachingEntity();
    newEntity.CopyContentFrom(subjectTeaching);

    using (DemoDbContext db = new DemoDbContext()) {

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(newEntity, db)){
        return false;
      }

      if (db.SubjectTeachings.Where((e)=> e.TeacherUid == newEntity.TeacherUid && e.SubjectOfficialName == newEntity.SubjectOfficialName).Any()) {
        return false;
      }

      db.SubjectTeachings.Add(newEntity);

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given SubjectTeaching addressed by the primary identifier fields within the given SubjectTeaching. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="subjectTeaching"> SubjectTeaching containing the new values (the primary identifier fields within the given SubjectTeaching will be used to address the target record) </param>
  public bool UpdateSubjectTeaching(SubjectTeaching subjectTeaching){
    return this.UpdateSubjectTeachingBySubjectTeachingIdentity(new SubjectTeachingIdentity { TeacherUid=subjectTeaching.TeacherUid, SubjectOfficialName=subjectTeaching.SubjectOfficialName }, subjectTeaching);
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given SubjectTeaching addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="subjectTeachingIdentity"> Composite Key, which represents the primary identity of a SubjectTeaching </param>
  /// <param name="subjectTeaching"> SubjectTeaching containing the new values (the primary identifier fields within the given SubjectTeaching will be ignored) </param>
  public bool UpdateSubjectTeachingBySubjectTeachingIdentity(SubjectTeachingIdentity subjectTeachingIdentity, SubjectTeaching subjectTeaching){
    var mac = AccessControlContext.Current;

    SubjectTeachingEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.SubjectTeachings.AsNoTracking();

      query = query.Where((e)=> e.TeacherUid == subjectTeachingIdentity.TeacherUid && e.SubjectOfficialName == subjectTeachingIdentity.SubjectOfficialName).AccessScopeFiltered();

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if(existingEntity == null) {
        return false;
      }

      bool changeAttemptOnFixedField = false;
      existingEntity.CopyContentFrom(subjectTeaching, (name) => changeAttemptOnFixedField = true);

      if (changeAttemptOnFixedField) {
        return false;
      }

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(existingEntity, db)){
        return false;
      }

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Deletes a specific SubjectTeaching addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="subjectTeachingIdentity"> Composite Key, which represents the primary identity of a SubjectTeaching </param>
  public bool DeleteSubjectTeachingBySubjectTeachingIdentity(SubjectTeachingIdentity subjectTeachingIdentity){
    var mac = AccessControlContext.Current;

    SubjectTeachingEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.SubjectTeachings.AsNoTracking().AccessScopeFiltered();

      query = query.Where((e)=> e.TeacherUid == subjectTeachingIdentity.TeacherUid && e.SubjectOfficialName == subjectTeachingIdentity.SubjectOfficialName);

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if (existingEntity == null) {
        return false;
      }

      db.SubjectTeachings.Remove(existingEntity);

      db.SaveChanges();
    }

    return true;
  }

  private bool PreValidateAccessControlScope(SubjectTeachingEntity entity, DemoDbContext db){

    var filterExpression = EntityAccessControl.BuildExpressionForLocalEntity<SubjectTeachingEntity>(AccessControlContext.Current);
    if(!filterExpression.Compile().Invoke(entity)) {
      return false;
    }

    if(!db.Subjects.Where((tgt)=> tgt.OfficialName==entity.SubjectOfficialName ).AccessScopeFiltered().Any()) {
      return false;
    }
    if(!db.Teachers.Where((tgt)=> tgt.Uid==entity.TeacherUid ).AccessScopeFiltered().Any()) {
      return false;
    }
    return true;
  }

}

/// <summary> Provides CRUD access to stored EnducationItemsOfRoomRelatedEducationItem (based on schema version '1.3.0') </summary>
public partial class RoomRelatedEducationItemStore : IRoomRelatedEducationItemRepository {

  /// <summary> Loads a specific RoomRelatedEducationItem addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="uid"> Represents the primary identity of a RoomRelatedEducationItem </param>
  public RoomRelatedEducationItem GetRoomRelatedEducationItemByUid(Guid uid){
    var mac = AccessControlContext.Current;

    RoomRelatedEducationItem result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.EnducationItems.AsNoTracking().OfType<RoomRelatedEducationItemEntity>().AccessScopeFiltered().Select(RoomRelatedEducationItemEntity.RoomRelatedEducationItemSelector);

      query = query.Where((e)=>e.Uid == uid);

      //materialization / load
      result = query.SingleOrDefault();

    }

    return result;
  }

  /// <summary> Loads EnducationItemsOfRoomRelatedEducationItem. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of EnducationItemsOfRoomRelatedEducationItem which should be returned </param>
  public RoomRelatedEducationItem[] GetEnducationItemsOfRoomRelatedEducationItem(int page = 1, int pageSize = 20){
    return this.SearchEnducationItemsOfRoomRelatedEducationItem(null, null, page, pageSize); 
  }

  private static String[] _ExactMatchPropNames = new String[] {};
  private static String[] _FreetextPropNames = new String[] {};

  /// <summary> Loads EnducationItemsOfRoomRelatedEducationItem where values matching to the given filterExpression</summary>
  /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of EnducationItemsOfRoomRelatedEducationItem which should be returned</param>
  public RoomRelatedEducationItem[] SearchEnducationItemsOfRoomRelatedEducationItem(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20){
    var mac = AccessControlContext.Current;

    RoomRelatedEducationItem[] result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.EnducationItems
        .AsNoTracking().OfType<RoomRelatedEducationItemEntity>()
        .AccessScopeFiltered()
        .Select(RoomRelatedEducationItemEntity.RoomRelatedEducationItemSelector)
      ;
      
      //apply filter (if given)
      if(filterExpression != null) {
        //just if the filterExpression isnt already a valid expression, treat it as a freetext seach string and transform it to a valid expression
        filterExpression = DynamicLinqExtensions.FreetextSearchStringToFilterExpression(filterExpression, _ExactMatchPropNames, _FreetextPropNames);
      }
      if(filterExpression != null) {
        query = query.DynamicallyFiltered(filterExpression);
      }

      //apply sorting
      if(String.IsNullOrWhiteSpace(sortingExpression)) {
        query = query.DynamicallySorted("Uid");
      }
      else{
        query = query.DynamicallySorted(sortingExpression + ", Uid");
      }

      //apply pagination
      query = query.Skip(pageSize * (page - 1)).Take(pageSize);

      //materialization / load
      result = query.ToArray();

    }

    return result;
  }

  /// <summary> Adds a new RoomRelatedEducationItem and returns success. </summary>
  /// <param name="roomRelatedEducationItem"> RoomRelatedEducationItem containing the new values </param>
  public bool AddNewRoomRelatedEducationItem(RoomRelatedEducationItem roomRelatedEducationItem){
    var mac = AccessControlContext.Current;

    RoomRelatedEducationItemEntity newEntity = new RoomRelatedEducationItemEntity();
    newEntity.CopyContentFrom(roomRelatedEducationItem);

    using (DemoDbContext db = new DemoDbContext()) {

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(newEntity, db)){
        return false;
      }

      if (db.EnducationItems.OfType<RoomRelatedEducationItemEntity>().Where((e)=>e.Uid == newEntity.Uid).Any()) {
        return false;
      }

      db.EnducationItems.Add(newEntity);

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given RoomRelatedEducationItem addressed by the primary identifier fields within the given RoomRelatedEducationItem. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="roomRelatedEducationItem"> RoomRelatedEducationItem containing the new values (the primary identifier fields within the given RoomRelatedEducationItem will be used to address the target record) </param>
  public bool UpdateRoomRelatedEducationItem(RoomRelatedEducationItem roomRelatedEducationItem){
    return this.UpdateRoomRelatedEducationItemByUid(roomRelatedEducationItem.Uid, roomRelatedEducationItem);
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given RoomRelatedEducationItem addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a RoomRelatedEducationItem </param>
  /// <param name="roomRelatedEducationItem"> RoomRelatedEducationItem containing the new values (the primary identifier fields within the given RoomRelatedEducationItem will be ignored) </param>
  public bool UpdateRoomRelatedEducationItemByUid(Guid uid, RoomRelatedEducationItem roomRelatedEducationItem){
    var mac = AccessControlContext.Current;

    RoomRelatedEducationItemEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.EnducationItems.AsNoTracking().OfType<RoomRelatedEducationItemEntity>();

      query = query.Where((e)=>e.Uid == uid).AccessScopeFiltered();

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if(existingEntity == null) {
        return false;
      }

      bool changeAttemptOnFixedField = false;
      existingEntity.CopyContentFrom(roomRelatedEducationItem, (name) => changeAttemptOnFixedField = true);

      if (changeAttemptOnFixedField) {
        return false;
      }

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(existingEntity, db)){
        return false;
      }

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Deletes a specific RoomRelatedEducationItem addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a RoomRelatedEducationItem </param>
  public bool DeleteRoomRelatedEducationItemByUid(Guid uid){
    var mac = AccessControlContext.Current;

    RoomRelatedEducationItemEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.EnducationItems.AsNoTracking().OfType<RoomRelatedEducationItemEntity>().AccessScopeFiltered();

      query = query.Where((e)=>e.Uid == uid);

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if (existingEntity == null) {
        return false;
      }

      db.EnducationItems.Remove(existingEntity);

      db.SaveChanges();
    }

    return true;
  }

  private bool PreValidateAccessControlScope(RoomRelatedEducationItemEntity entity, DemoDbContext db){

    var filterExpression = EntityAccessControl.BuildExpressionForLocalEntity<RoomRelatedEducationItemEntity>(AccessControlContext.Current);
    if(!filterExpression.Compile().Invoke(entity)) {
      return false;
    }

    if(!db.Rooms.Where((tgt)=> tgt.Uid==entity.RoomUid ).AccessScopeFiltered().Any()) {
      return false;
    }
    return true;
  }

}

/// <summary> Provides CRUD access to stored TeachingRequiredItems (based on schema version '1.3.0') </summary>
public partial class TeachingRequiredItemStore : ITeachingRequiredItemRepository {

  /// <summary> Loads a specific TeachingRequiredItem addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="uid"> Represents the primary identity of a TeachingRequiredItem </param>
  public TeachingRequiredItem GetTeachingRequiredItemByUid(Int32 uid){
    var mac = AccessControlContext.Current;

    TeachingRequiredItem result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.TeachingRequiredItems.AsNoTracking().AccessScopeFiltered().Select(TeachingRequiredItemEntity.TeachingRequiredItemSelector);

      query = query.Where((e)=>e.Uid == uid);

      //materialization / load
      result = query.SingleOrDefault();

    }

    return result;
  }

  /// <summary> Loads TeachingRequiredItems. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of TeachingRequiredItems which should be returned </param>
  public TeachingRequiredItem[] GetTeachingRequiredItems(int page = 1, int pageSize = 20){
    return this.SearchTeachingRequiredItems(null, null, page, pageSize); 
  }

  private static String[] _ExactMatchPropNames = new String[] {"SubjectOfficialName"};
  private static String[] _FreetextPropNames = new String[] {};

  /// <summary> Loads TeachingRequiredItems where values matching to the given filterExpression</summary>
  /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of TeachingRequiredItems which should be returned</param>
  public TeachingRequiredItem[] SearchTeachingRequiredItems(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20){
    var mac = AccessControlContext.Current;

    TeachingRequiredItem[] result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.TeachingRequiredItems
        .AsNoTracking()
        .AccessScopeFiltered()
        .Select(TeachingRequiredItemEntity.TeachingRequiredItemSelector)
      ;
      
      //apply filter (if given)
      if(filterExpression != null) {
        //just if the filterExpression isnt already a valid expression, treat it as a freetext seach string and transform it to a valid expression
        filterExpression = DynamicLinqExtensions.FreetextSearchStringToFilterExpression(filterExpression, _ExactMatchPropNames, _FreetextPropNames);
      }
      if(filterExpression != null) {
        query = query.DynamicallyFiltered(filterExpression);
      }

      //apply sorting
      if(String.IsNullOrWhiteSpace(sortingExpression)) {
        query = query.DynamicallySorted("Uid");
      }
      else{
        query = query.DynamicallySorted(sortingExpression + ", Uid");
      }

      //apply pagination
      query = query.Skip(pageSize * (page - 1)).Take(pageSize);

      //materialization / load
      result = query.ToArray();

    }

    return result;
  }

  /// <summary> Adds a new TeachingRequiredItem and returns success. </summary>
  /// <param name="teachingRequiredItem"> TeachingRequiredItem containing the new values </param>
  public bool AddNewTeachingRequiredItem(TeachingRequiredItem teachingRequiredItem){
    var mac = AccessControlContext.Current;

    TeachingRequiredItemEntity newEntity = new TeachingRequiredItemEntity();
    newEntity.CopyContentFrom(teachingRequiredItem);

    using (DemoDbContext db = new DemoDbContext()) {

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(newEntity, db)){
        return false;
      }

      if (db.TeachingRequiredItems.Where((e)=>e.Uid == newEntity.Uid).Any()) {
        return false;
      }

      db.TeachingRequiredItems.Add(newEntity);

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given TeachingRequiredItem addressed by the primary identifier fields within the given TeachingRequiredItem. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="teachingRequiredItem"> TeachingRequiredItem containing the new values (the primary identifier fields within the given TeachingRequiredItem will be used to address the target record) </param>
  public bool UpdateTeachingRequiredItem(TeachingRequiredItem teachingRequiredItem){
    return this.UpdateTeachingRequiredItemByUid(teachingRequiredItem.Uid, teachingRequiredItem);
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given TeachingRequiredItem addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a TeachingRequiredItem </param>
  /// <param name="teachingRequiredItem"> TeachingRequiredItem containing the new values (the primary identifier fields within the given TeachingRequiredItem will be ignored) </param>
  public bool UpdateTeachingRequiredItemByUid(Int32 uid, TeachingRequiredItem teachingRequiredItem){
    var mac = AccessControlContext.Current;

    TeachingRequiredItemEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.TeachingRequiredItems.AsNoTracking();

      query = query.Where((e)=>e.Uid == uid).AccessScopeFiltered();

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if(existingEntity == null) {
        return false;
      }

      bool changeAttemptOnFixedField = false;
      existingEntity.CopyContentFrom(teachingRequiredItem, (name) => changeAttemptOnFixedField = true);

      if (changeAttemptOnFixedField) {
        return false;
      }

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(existingEntity, db)){
        return false;
      }

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Deletes a specific TeachingRequiredItem addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a TeachingRequiredItem </param>
  public bool DeleteTeachingRequiredItemByUid(Int32 uid){
    var mac = AccessControlContext.Current;

    TeachingRequiredItemEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.TeachingRequiredItems.AsNoTracking().AccessScopeFiltered();

      query = query.Where((e)=>e.Uid == uid);

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if (existingEntity == null) {
        return false;
      }

      db.TeachingRequiredItems.Remove(existingEntity);

      db.SaveChanges();
    }

    return true;
  }

  private bool PreValidateAccessControlScope(TeachingRequiredItemEntity entity, DemoDbContext db){

    var filterExpression = EntityAccessControl.BuildExpressionForLocalEntity<TeachingRequiredItemEntity>(AccessControlContext.Current);
    if(!filterExpression.Compile().Invoke(entity)) {
      return false;
    }

    if(!db.SubjectTeachings.Where((tgt)=> tgt.TeacherUid==entity.TeacherUid && tgt.SubjectOfficialName==entity.SubjectOfficialName ).AccessScopeFiltered().Any()) {
      return false;
    }
    if(!db.EnducationItems.OfType<TeacherRelatedEducationItemEntity>().Where((tgt)=> tgt.Uid==entity.RequiredEducationItemUid ).AccessScopeFiltered().Any()) {
      return false;
    }
    return true;
  }

}

/// <summary> Provides CRUD access to stored EnducationItemsOfTeacherRelatedEducationItem (based on schema version '1.3.0') </summary>
public partial class TeacherRelatedEducationItemStore : ITeacherRelatedEducationItemRepository {

  /// <summary> Loads a specific TeacherRelatedEducationItem addressed by the given primary identifier. Returns null on failure, or if no record exists with the given identity.</summary>
  /// <param name="uid"> Represents the primary identity of a TeacherRelatedEducationItem </param>
  public TeacherRelatedEducationItem GetTeacherRelatedEducationItemByUid(Guid uid){
    var mac = AccessControlContext.Current;

    TeacherRelatedEducationItem result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.EnducationItems.AsNoTracking().OfType<TeacherRelatedEducationItemEntity>().AccessScopeFiltered().Select(TeacherRelatedEducationItemEntity.TeacherRelatedEducationItemSelector);

      query = query.Where((e)=>e.Uid == uid);

      //materialization / load
      result = query.SingleOrDefault();

    }

    return result;
  }

  /// <summary> Loads EnducationItemsOfTeacherRelatedEducationItem. </summary>
  /// <param name="page">Number of the page, which should be returned </param>
  /// <param name="pageSize">Max count of EnducationItemsOfTeacherRelatedEducationItem which should be returned </param>
  public TeacherRelatedEducationItem[] GetEnducationItemsOfTeacherRelatedEducationItem(int page = 1, int pageSize = 20){
    return this.SearchEnducationItemsOfTeacherRelatedEducationItem(null, null, page, pageSize); 
  }

  private static String[] _ExactMatchPropNames = new String[] {};
  private static String[] _FreetextPropNames = new String[] {};

  /// <summary> Loads EnducationItemsOfTeacherRelatedEducationItem where values matching to the given filterExpression</summary>
  /// <param name="filterExpression">a filter expression like '((FieldName1 == "ABC" &amp;&amp; FieldName2 &gt; 12) || FieldName2 != "")'</param>
  /// <param name="sortingExpression">one or more property names which are used as sort order (before pagination)</param>
  /// <param name="page">Number of the page, which should be returned</param>
  /// <param name="pageSize">Max count of EnducationItemsOfTeacherRelatedEducationItem which should be returned</param>
  public TeacherRelatedEducationItem[] SearchEnducationItemsOfTeacherRelatedEducationItem(String filterExpression, String sortingExpression = null, int page = 1, int pageSize = 20){
    var mac = AccessControlContext.Current;

    TeacherRelatedEducationItem[] result;
    using (DemoDbContext db = new DemoDbContext()) {

      //select models, bacause we dont want to return types with navigation-properties!
      var query = db.EnducationItems
        .AsNoTracking().OfType<TeacherRelatedEducationItemEntity>()
        .AccessScopeFiltered()
        .Select(TeacherRelatedEducationItemEntity.TeacherRelatedEducationItemSelector)
      ;
      
      //apply filter (if given)
      if(filterExpression != null) {
        //just if the filterExpression isnt already a valid expression, treat it as a freetext seach string and transform it to a valid expression
        filterExpression = DynamicLinqExtensions.FreetextSearchStringToFilterExpression(filterExpression, _ExactMatchPropNames, _FreetextPropNames);
      }
      if(filterExpression != null) {
        query = query.DynamicallyFiltered(filterExpression);
      }

      //apply sorting
      if(String.IsNullOrWhiteSpace(sortingExpression)) {
        query = query.DynamicallySorted("Uid");
      }
      else{
        query = query.DynamicallySorted(sortingExpression + ", Uid");
      }

      //apply pagination
      query = query.Skip(pageSize * (page - 1)).Take(pageSize);

      //materialization / load
      result = query.ToArray();

    }

    return result;
  }

  /// <summary> Adds a new TeacherRelatedEducationItem and returns success. </summary>
  /// <param name="teacherRelatedEducationItem"> TeacherRelatedEducationItem containing the new values </param>
  public bool AddNewTeacherRelatedEducationItem(TeacherRelatedEducationItem teacherRelatedEducationItem){
    var mac = AccessControlContext.Current;

    TeacherRelatedEducationItemEntity newEntity = new TeacherRelatedEducationItemEntity();
    newEntity.CopyContentFrom(teacherRelatedEducationItem);

    using (DemoDbContext db = new DemoDbContext()) {

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(newEntity, db)){
        return false;
      }

      if (db.EnducationItems.OfType<TeacherRelatedEducationItemEntity>().Where((e)=>e.Uid == newEntity.Uid).Any()) {
        return false;
      }

      db.EnducationItems.Add(newEntity);

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given TeacherRelatedEducationItem addressed by the primary identifier fields within the given TeacherRelatedEducationItem. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="teacherRelatedEducationItem"> TeacherRelatedEducationItem containing the new values (the primary identifier fields within the given TeacherRelatedEducationItem will be used to address the target record) </param>
  public bool UpdateTeacherRelatedEducationItem(TeacherRelatedEducationItem teacherRelatedEducationItem){
    return this.UpdateTeacherRelatedEducationItemByUid(teacherRelatedEducationItem.Uid, teacherRelatedEducationItem);
  }

  /// <summary> Updates all values (which are not "FixedAfterCreation") of the given TeacherRelatedEducationItem addressed by the supplementary given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a TeacherRelatedEducationItem </param>
  /// <param name="teacherRelatedEducationItem"> TeacherRelatedEducationItem containing the new values (the primary identifier fields within the given TeacherRelatedEducationItem will be ignored) </param>
  public bool UpdateTeacherRelatedEducationItemByUid(Guid uid, TeacherRelatedEducationItem teacherRelatedEducationItem){
    var mac = AccessControlContext.Current;

    TeacherRelatedEducationItemEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.EnducationItems.AsNoTracking().OfType<TeacherRelatedEducationItemEntity>();

      query = query.Where((e)=>e.Uid == uid).AccessScopeFiltered();

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if(existingEntity == null) {
        return false;
      }

      bool changeAttemptOnFixedField = false;
      existingEntity.CopyContentFrom(teacherRelatedEducationItem, (name) => changeAttemptOnFixedField = true);

      if (changeAttemptOnFixedField) {
        return false;
      }

      //checks, that the new values are within the access control scope
      if(!this.PreValidateAccessControlScope(existingEntity, db)){
        return false;
      }

      db.SaveChanges();
    }

    return true;
  }

  /// <summary> Deletes a specific TeacherRelatedEducationItem addressed by the given primary identifier. Returns false on failure or if no target record was found, otherwise true.</summary>
  /// <param name="uid"> Represents the primary identity of a TeacherRelatedEducationItem </param>
  public bool DeleteTeacherRelatedEducationItemByUid(Guid uid){
    var mac = AccessControlContext.Current;

    TeacherRelatedEducationItemEntity existingEntity;
    using (DemoDbContext db = new DemoDbContext()) {

      var query = db.EnducationItems.AsNoTracking().OfType<TeacherRelatedEducationItemEntity>().AccessScopeFiltered();

      query = query.Where((e)=>e.Uid == uid);

      //materialization / load
      existingEntity = query.SingleOrDefault();

      if (existingEntity == null) {
        return false;
      }

      db.EnducationItems.Remove(existingEntity);

      db.SaveChanges();
    }

    return true;
  }

  private bool PreValidateAccessControlScope(TeacherRelatedEducationItemEntity entity, DemoDbContext db){

    var filterExpression = EntityAccessControl.BuildExpressionForLocalEntity<TeacherRelatedEducationItemEntity>(AccessControlContext.Current);
    if(!filterExpression.Compile().Invoke(entity)) {
      return false;
    }

    if(!db.Teachers.Where((tgt)=> tgt.Uid==entity.TeacherUid ).AccessScopeFiltered().Any()) {
      return false;
    }
    return true;
  }

}

}
