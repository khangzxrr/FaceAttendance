create table Keys(
	id int identity(1,1) primary key not null,
	keyText varchar(30) not null unique,
	isUsed bit default 0,
	key_type varchar(30),
	create_date datetime default GETDATE(),
	expiry_date datetime
)



create table SchoolAccounts(
	id int identity(1,1) primary key not null,
	name nvarchar(100),
	username nvarchar(30) unique,
	password nvarchar(30),
	external_id varchar(100), /* Google login id */

	key_id int foreign key references Keys(id)
)

create table Majors(
	id int identity(1,1) primary key not null,
	school_id int foreign key references SchoolAccounts(id),
	startDate datetime,
	name nvarchar(100)
)

create table TeacherAccounts(
	id int identity(1,1) primary key not null,
	email nvarchar(30) unique,
	password nvarchar(30),
	external_id varchar(100),

	teach_in_school int foreign key references SchoolAccounts(id) not null
)


create table Subjects(
	id int identity(1,1) primary key not null,
	name nvarchar(100),
	room nvarchar(100) not null,
	major_id int foreign key references Majors(id) not null,
	teacher_observer int foreign key references TeacherAccounts(id)
)

create table Students(
	id int identity(1,1) primary key not null,
	name nvarchar(100),
	date_of_birth date,
	school_id int foreign key references SchoolAccounts(id)
)


create table Attendances(
	id int identity(1,1) primary key not null,
	id_subject int foreign key references Subjects(id),
	id_student int foreign key references Students(id),
	checkin datetime,
	checkout datetime
)

create table StudentImages(
	id int identity(1,1) primary key not null,
	url nvarchar(255) not null,
	student_id int foreign key references Students(id)
)









