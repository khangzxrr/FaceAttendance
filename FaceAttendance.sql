create table Keys(
	id int identity(1,1) primary key not null,
	keyText varchar(30) not null,
	isUsed bit default 0,
	key_type varchar(30),
	create_date date default GETDATE(),
	expiry_date date
)


create table SchoolAccounts(
	id int identity(1,1) primary key not null,
	name nvarchar(100),
	username nvarchar(30),
	password nvarchar(30),
	external_id varchar(100), /* Google login id */

	key_id int foreign key references Keys(id)
)

create table Major(
	id int identity(1,1) primary key not null,
	school_id int foreign key references SchoolAccounts(id),
	name nvarchar(100)
)

create table TeacherAccounts(
	id int identity(1,1) primary key not null,
	email nvarchar(30),
	password nvarchar(30),
	external_id varchar(100),

	teach_in_school int foreign key references SchoolAccounts(id) not null
)


create table Subjects(
	id int identity(1,1) primary key not null,
	start_date datetime not null,
	name nvarchar(100),
	room nvarchar(100) not null,
	major_id int foreign key references Major(id) not null,
	teacher_observer int foreign key references TeacherAccounts(id)
)

create table Students(
	id int identity(1,1) primary key not null,
	name nvarchar(100),
	date_of_birth date 
)


create table Attendances(
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









