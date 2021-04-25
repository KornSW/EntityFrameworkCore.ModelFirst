# EF-Core ModelFirst Demo

|          | Info                                    |
|----------|-----------------------------------------|
|author:   |github.com/[KornSW](https://github.com/KornSW/EntityFrameworkCore.ModelFirst)|
|license:  |[Apache-2](https://choosealicense.com/licenses/apache-2.0/)|
|version:  |1.3.0|
|timestamp:|2021-04-21 16:47|

### Contents

  * .  [Class](#Class)
  * ........\  [Lesson](#Lesson)
  * ........\  [Student](#Student)
  * .  [EnducationItem](#EnducationItem)
  * ........^ **(spec.)**  [RoomRelatedEducationItem](#RoomRelatedEducationItem)
  * ........^ **(spec.)**  [TeacherRelatedEducationItem](#TeacherRelatedEducationItem)
  * ........\  [EducationItemPicture](#EducationItemPicture)
  * .  [Room](#Room)
  * .  [Subject](#Subject)
  * .  [Teacher](#Teacher)
  * ........\  [SubjectTeaching](#SubjectTeaching)
  * ................\  [TeachingRequiredItem](#TeachingRequiredItem)

# Model:



## Class


### Fields

| Name | Type | Required | Fix |
| ---- | ---- | -------- | --- |
| [Uid](#ClassUid-Field) **(PK)** | *guid* | YES | no |
| [PrimaryTeacherUid](#ClassPrimaryTeacherUid-Field) (FK) | *guid* | no | no |
| [RoomUid](#ClassRoomUid-Field) (FK) | *guid* | no | no |
| OfficialName | *string* | YES | no |
| EducationLevelYear | *int32* | YES | no |
##### Unique Keys
* Uid **(primary)**
##### Class.**Uid** (Field)
* this field represents the identity (PK) of the record
##### Class.**PrimaryTeacherUid** (Field)
* this field is optional, so that '*null*' values are supported
* this field is used as foreign key to address the related 'PrimaryTeacher'
##### Class.**RoomUid** (Field)
* this field is optional, so that '*null*' values are supported
* this field is used as foreign key to address the related 'PrimaryRoom'


### Relations

| Navigation-Name | Role | Target-Type | Target-Multiplicity |
| --------------- | ----------- | ------------------- |
| [PrimaryTeacher](#PrimaryTeacher-lookup-from-this-Class) | Lookup | [Teacher](#Teacher) | 1 (required) |
| [PrimaryRoom](#PrimaryRoom-lookup-from-this-Class) | Lookup | [Room](#Room) | 1 (required) |
| [Students](#Students-childs-of-this-Class) | Childs | [Student](#Student) | * (multiple) |
| [ScheduledLessons](#ScheduledLessons-childs-of-this-Class) | Childs | [Lesson](#Lesson) | * (multiple) |

##### **PrimaryTeacher** (lookup from this Class)
Target Type: [Teacher](#Teacher)
Addressed by: [PrimaryTeacherUid](#ClassPrimaryTeacherUid-Field).
##### **PrimaryRoom** (lookup from this Class)
Target Type: [Room](#Room)
Addressed by: [RoomUid](#ClassRoomUid-Field).
##### **Students** (childs of this Class)
Target: [Student](#Student)
##### **ScheduledLessons** (childs of this Class)
Target: [Lesson](#Lesson)




## Lesson


### Fields

| Name | Type | Required | Fix |
| ---- | ---- | -------- | --- |
| [Uid](#LessonUid-Field) **(PK)** | *guid* | YES | no |
| [EducatedClassUid](#LessonEducatedClassUid-Field) (FK) | *guid* | YES | no |
| [RoomUid](#LessonRoomUid-Field) (FK) | *guid* | YES | no |
| OfficialName | *string* | YES | no |
| DayOfWeek | *int32* | YES | no |
| Begin | *time* | YES | no |
| DurationHours | *decimal* | YES | no |
| [TeacherUid](#LessonTeacherUid-Field) (FK) | *guid* | YES | no |
| [SubjectOfficialName](#LessonSubjectOfficialName-Field) (FK) | *string* | YES | no |
##### Unique Keys
* Uid **(primary)**
##### Lesson.**Uid** (Field)
* this field represents the identity (PK) of the record
##### Lesson.**EducatedClassUid** (Field)
* this field is used as foreign key to address the related 'EducatedClass'
##### Lesson.**RoomUid** (Field)
* this field is used as foreign key to address the related 'Room'
##### Lesson.**TeacherUid** (Field)
* this field is used as foreign key to address the related 'Teaching'
##### Lesson.**SubjectOfficialName** (Field)
* this field is used as foreign key to address the related 'Teaching'


### Relations

| Navigation-Name | Role | Target-Type | Target-Multiplicity |
| --------------- | ----------- | ------------------- |
| [EducatedClass](#EducatedClass-parent-of-this-Lesson) | Parent | [Class](#Class) | 0/1 (optional) |
| [Room](#Room-lookup-from-this-Lesson) | Lookup | [Room](#Room) | 0/1 (optional) |
| [Teaching](#Teaching-lookup-from-this-Lesson) | Lookup | [SubjectTeaching](#SubjectTeaching) | 0/1 (optional) |

##### **EducatedClass** (parent of this Lesson)
Target Type: [Class](#Class)
Addressed by: [EducatedClassUid](#LessonEducatedClassUid-Field).
##### **Room** (lookup from this Lesson)
Target Type: [Room](#Room)
Addressed by: [RoomUid](#LessonRoomUid-Field).
##### **Teaching** (lookup from this Lesson)
Target Type: [SubjectTeaching](#SubjectTeaching)
Addressed by: [TeacherUid](#LessonTeacherUid-Field), [SubjectOfficialName](#LessonSubjectOfficialName-Field).




## Student


### Fields

| Name | Type | Required | Fix |
| ---- | ---- | -------- | --- |
| [Uid](#StudentUid-Field) **(PK)** | *guid* | YES | no |
| [ClassUid](#StudentClassUid-Field) (FK) | *guid* | YES | no |
| FirstName | *string* | YES | no |
| LastName | *string* | YES | no |
| ScoolEntryYear | *int32* | YES | no |
##### Unique Keys
* Uid **(primary)**
##### Student.**Uid** (Field)
* this field represents the identity (PK) of the record
##### Student.**ClassUid** (Field)
* this field is used as foreign key to address the related 'Class'


### Relations

| Navigation-Name | Role | Target-Type | Target-Multiplicity |
| --------------- | ----------- | ------------------- |
| [Class](#Class-parent-of-this-Student) | Parent | [Class](#Class) | 0/1 (optional) |

##### **Class** (parent of this Student)
Target Type: [Class](#Class)
Addressed by: [ClassUid](#StudentClassUid-Field).




## EnducationItem


### Fields

| Name | Type | Required | Fix |
| ---- | ---- | -------- | --- |
| [Uid](#EnducationItemUid-Field) **(PK)** | *guid* | YES | no |
| InventoryNumber | *int16* | YES | no |
| Title | *string* | YES | no |
| [DedicatedToSubjectName](#EnducationItemDedicatedToSubjectName-Field) (FK) | *string* | no | no |
##### Unique Keys
* Uid **(primary)**
##### EnducationItem.**Uid** (Field)
* this field represents the identity (PK) of the record
##### EnducationItem.**DedicatedToSubjectName** (Field)
* this field is optional, so that '*null*' values are supported
* this field is used as foreign key to address the related 'DedicatedToSubject'


### Relations

| Navigation-Name | Role | Target-Type | Target-Multiplicity |
| --------------- | ----------- | ------------------- |
| [Picture](#Picture-child-of-this-EnducationItem) | Child | [EducationItemPicture](#EducationItemPicture) | 0/1 (single) |
| [DedicatedToSubject](#DedicatedToSubject-lookup-from-this-EnducationItem) | Lookup | [Subject](#Subject) | 1 (required) |

##### **Picture** (child of this EnducationItem)
Target: [EducationItemPicture](#EducationItemPicture)
##### **DedicatedToSubject** (lookup from this EnducationItem)
Target Type: [Subject](#Subject)
Addressed by: [DedicatedToSubjectName](#EnducationItemDedicatedToSubjectName-Field).




## RoomRelatedEducationItem


### Inheritance
This type is a specialization of [EnducationItem](#EnducationItem)
### Fields

| Name | Type | Required | Fix |
| ---- | ---- | -------- | --- |
| [RoomUid](#RoomRelatedEducationItemRoomUid-Field) (FK) | *guid* | YES | no |
##### Unique Keys
##### RoomRelatedEducationItem.**RoomUid** (Field)
* this field is used as foreign key to address the related 'Location'


### Relations

| Navigation-Name | Role | Target-Type | Target-Multiplicity |
| --------------- | ----------- | ------------------- |
| [Location](#Location-lookup-from-this-RoomRelatedEducationItem) | Lookup | [Room](#Room) | 0/1 (optional) |

##### **Location** (lookup from this RoomRelatedEducationItem)
Target Type: [Room](#Room)
Addressed by: [RoomUid](#RoomRelatedEducationItemRoomUid-Field).




## TeacherRelatedEducationItem


### Inheritance
This type is a specialization of [EnducationItem](#EnducationItem)
### Fields

| Name | Type | Required | Fix |
| ---- | ---- | -------- | --- |
| [TeacherUid](#TeacherRelatedEducationItemTeacherUid-Field) (FK) | *guid* | YES | no |
##### Unique Keys
##### TeacherRelatedEducationItem.**TeacherUid** (Field)
* this field is used as foreign key to address the related 'OwningTeacher'


### Relations

| Navigation-Name | Role | Target-Type | Target-Multiplicity |
| --------------- | ----------- | ------------------- |
| [OwningTeacher](#OwningTeacher-lookup-from-this-TeacherRelatedEducationItem) | Lookup | [Teacher](#Teacher) | 0/1 (optional) |

##### **OwningTeacher** (lookup from this TeacherRelatedEducationItem)
Target Type: [Teacher](#Teacher)
Addressed by: [TeacherUid](#TeacherRelatedEducationItemTeacherUid-Field).




## EducationItemPicture


### Fields

| Name | Type | Required | Fix |
| ---- | ---- | -------- | --- |
| [Uid](#EducationItemPictureUid-Field) **(PK)** (FK) | *guid* | YES | no |
| PictureData | *byte[]* | YES | no |
##### Unique Keys
* Uid **(primary)**
##### EducationItemPicture.**Uid** (Field)
* this field represents the identity (PK) of the record
* this field is used as foreign key to address the related 'EnducationItem'


### Relations

| Navigation-Name | Role | Target-Type | Target-Multiplicity |
| --------------- | ----------- | ------------------- |
| [EnducationItem](#EnducationItem-parent-of-this-EducationItemPicture) | Parent | [EnducationItem](#EnducationItem) | 0/1 (optional) |

##### **EnducationItem** (parent of this EducationItemPicture)
Target Type: [EnducationItem](#EnducationItem)
Addressed by: [Uid](#EducationItemPictureUid-Field).




## Room


### Fields

| Name | Type | Required | Fix |
| ---- | ---- | -------- | --- |
| [Uid](#RoomUid-Field) **(PK)** | *guid* | YES | no |
| OfficialName | *string* | YES | no |
##### Unique Keys
* Uid **(primary)**
##### Room.**Uid** (Field)
* this field represents the identity (PK) of the record


### Relations

| Navigation-Name | Role | Target-Type | Target-Multiplicity |
| --------------- | ----------- | ------------------- |
| [PrimaryClasses](#PrimaryClasses-refering-to-this-Room) | Referers | [Class](#Class) | * (multiple) |
| [ScheduledLessons](#ScheduledLessons-refering-to-this-Room) | Referers | [Lesson](#Lesson) | * (multiple) |
| [EducationItems](#EducationItems-refering-to-this-Room) | Referers | [RoomRelatedEducationItem](#RoomRelatedEducationItem) | * (multiple) |

##### **PrimaryClasses** (refering to this Room)
Target: [Class](#Class)
##### **ScheduledLessons** (refering to this Room)
Target: [Lesson](#Lesson)
##### **EducationItems** (refering to this Room)
Target: [RoomRelatedEducationItem](#RoomRelatedEducationItem)




## Subject


### Fields

| Name | Type | Required | Fix |
| ---- | ---- | -------- | --- |
| [OfficialName](#SubjectOfficialName-Field) **(PK)** | *string* | YES | no |
##### Unique Keys
* OfficialName **(primary)**
##### Subject.**OfficialName** (Field)
* this field represents the identity (PK) of the record


### Relations

| Navigation-Name | Role | Target-Type | Target-Multiplicity |
| --------------- | ----------- | ------------------- |
| [EnducationItems](#EnducationItems-refering-to-this-Subject) | Referers | [EnducationItem](#EnducationItem) | * (multiple) |
| [Teachings](#Teachings-refering-to-this-Subject) | Referers | [SubjectTeaching](#SubjectTeaching) | * (multiple) |

##### **EnducationItems** (refering to this Subject)
Target: [EnducationItem](#EnducationItem)
##### **Teachings** (refering to this Subject)
Target: [SubjectTeaching](#SubjectTeaching)




## Teacher


### Fields

| Name | Type | Required | Fix |
| ---- | ---- | -------- | --- |
| [Uid](#TeacherUid-Field) **(PK)** | *guid* | YES | no |
| FirstName | *string* | YES | no |
| LastName | *string* | YES | no |
##### Unique Keys
* Uid **(primary)**
##### Teacher.**Uid** (Field)
* this field represents the identity (PK) of the record


### Relations

| Navigation-Name | Role | Target-Type | Target-Multiplicity |
| --------------- | ----------- | ------------------- |
| [PrimaryClasses](#PrimaryClasses-refering-to-this-Teacher) | Referers | [Class](#Class) | * (multiple) |
| [Teachings](#Teachings-childs-of-this-Teacher) | Childs | [SubjectTeaching](#SubjectTeaching) | * (multiple) |
| [OwnedEducationItems](#OwnedEducationItems-refering-to-this-Teacher) | Referers | [TeacherRelatedEducationItem](#TeacherRelatedEducationItem) | * (multiple) |

##### **PrimaryClasses** (refering to this Teacher)
Target: [Class](#Class)
##### **Teachings** (childs of this Teacher)
Target: [SubjectTeaching](#SubjectTeaching)
##### **OwnedEducationItems** (refering to this Teacher)
Target: [TeacherRelatedEducationItem](#TeacherRelatedEducationItem)




## SubjectTeaching


### Fields

| Name | Type | Required | Fix |
| ---- | ---- | -------- | --- |
| [TeacherUid](#SubjectTeachingTeacherUid-Field) **(PK)** (FK) | *guid* | YES | no |
| [SubjectOfficialName](#SubjectTeachingSubjectOfficialName-Field) **(PK)** (FK) | *string* | YES | no |
##### Unique Keys
* TeacherUid + SubjectOfficialName **(primary)**
##### SubjectTeaching.**TeacherUid** (Field)
* this field represents the identity (PK) of the record
* this field is used as foreign key to address the related 'Teacher'
##### SubjectTeaching.**SubjectOfficialName** (Field)
* this field represents the identity (PK) of the record
* this field is used as foreign key to address the related 'Subject'


### Relations

| Navigation-Name | Role | Target-Type | Target-Multiplicity |
| --------------- | ----------- | ------------------- |
| [ScheduledLessons](#ScheduledLessons-refering-to-this-SubjectTeaching) | Referers | [Lesson](#Lesson) | * (multiple) |
| [Subject](#Subject-lookup-from-this-SubjectTeaching) | Lookup | [Subject](#Subject) | 0/1 (optional) |
| [Teacher](#Teacher-parent-of-this-SubjectTeaching) | Parent | [Teacher](#Teacher) | 0/1 (optional) |
| [RequiredItems](#RequiredItems-childs-of-this-SubjectTeaching) | Childs | [TeachingRequiredItem](#TeachingRequiredItem) | * (multiple) |

##### **ScheduledLessons** (refering to this SubjectTeaching)
Target: [Lesson](#Lesson)
##### **Subject** (lookup from this SubjectTeaching)
Target Type: [Subject](#Subject)
Addressed by: [SubjectOfficialName](#SubjectTeachingSubjectOfficialName-Field).
##### **Teacher** (parent of this SubjectTeaching)
Target Type: [Teacher](#Teacher)
Addressed by: [TeacherUid](#SubjectTeachingTeacherUid-Field).
##### **RequiredItems** (childs of this SubjectTeaching)
Target: [TeachingRequiredItem](#TeachingRequiredItem)




## TeachingRequiredItem


### Fields

| Name | Type | Required | Fix |
| ---- | ---- | -------- | --- |
| [Uid](#TeachingRequiredItemUid-Field) **(PK)** | *int32* | YES | no |
| [TeacherUid](#TeachingRequiredItemTeacherUid-Field) (FK) | *guid* | YES | no |
| [SubjectOfficialName](#TeachingRequiredItemSubjectOfficialName-Field) (FK) | *string* | YES | no |
| [RequiredEducationItemUid](#TeachingRequiredItemRequiredEducationItemUid-Field) (FK) | *guid* | YES | no |
##### Unique Keys
* Uid **(primary)**
##### TeachingRequiredItem.**Uid** (Field)
* this field represents the identity (PK) of the record
* this identity is a internal record id, so that it must not be exposed to other systems or displayed to end-users!
##### TeachingRequiredItem.**TeacherUid** (Field)
* this field is used as foreign key to address the related 'Teaching'
##### TeachingRequiredItem.**SubjectOfficialName** (Field)
* this field is used as foreign key to address the related 'Teaching'
##### TeachingRequiredItem.**RequiredEducationItemUid** (Field)
* this field is used as foreign key to address the related 'RequiredItem'


### Relations

| Navigation-Name | Role | Target-Type | Target-Multiplicity |
| --------------- | ----------- | ------------------- |
| [Teaching](#Teaching-parent-of-this-TeachingRequiredItem) | Parent | [SubjectTeaching](#SubjectTeaching) | 0/1 (optional) |
| [RequiredItem](#RequiredItem-lookup-from-this-TeachingRequiredItem) | Lookup | [TeacherRelatedEducationItem](#TeacherRelatedEducationItem) | 0/1 (optional) |

##### **Teaching** (parent of this TeachingRequiredItem)
Target Type: [SubjectTeaching](#SubjectTeaching)
Addressed by: [TeacherUid](#TeachingRequiredItemTeacherUid-Field), [SubjectOfficialName](#TeachingRequiredItemSubjectOfficialName-Field).
##### **RequiredItem** (lookup from this TeachingRequiredItem)
Target Type: [TeacherRelatedEducationItem](#TeacherRelatedEducationItem)
Addressed by: [RequiredEducationItemUid](#TeachingRequiredItemRequiredEducationItemUid-Field).


