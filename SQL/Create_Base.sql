create database PRA_Driving_School COLLATE Croatian_CI_AS
go

use PRA_Driving_School
go

create table Person (
    IDPerson int primary key identity (1, 1),
    FirstName nvarchar(50) not null,
    Lastname varchar(50) not null,
    Email nvarchar(50) not null,
    PswdHash nvarchar(255) not null,
	PswSalt nvarchar(255) not null
);
go

create table Brand (
    IDBrand int primary key identity (1, 1),
    [Name] nvarchar(50) not null
);
go

create table Model (
    IDModel int primary key identity (1, 1),
    [Name] nvarchar(50) not null,
	BrandID int foreign key references Brand(IDBrand) not null
);
go

create table Category (
    IDCategory int primary key identity (1, 1),
    [Name] nvarchar(50) not null
);
go

create table Colour (
    IDColour int primary key identity (1, 1),
    [Name] nvarchar(50) not null
);
go

create table Instructor (
    IDInstructor int primary key identity (1, 1),
    PersonID int foreign key references Person(IDPerson) not null,
);
go

create table Vehicle (
    IDVehicle int primary key identity (1, 1),
    ColourID int foreign key references Colour(IDColour) not null,
	CategoryID int foreign key references Category(IDCategory) not null,
	ModelID int foreign key references Model(IDModel) not null,
	InstructorID int foreign key references Instructor(IDInstructor) not null,
	Picture varbinary(max)
);
go

create table Student (
    OIB char(11) primary key,
    PersonID int foreign key references Person(IDPerson) unique not null,
	VehicleID int foreign key references Vehicle(IDVehicle) null,
	HoursDriven int default 0
);
go

create table Review (
    IDReview int primary key identity (1, 1),
    StudentID char(11) foreign key references Student(OIB) not null,
	InstructorID int foreign key references Instructor(IDInstructor) not null,
	Grade int not null,
	Comment nvarchar(max)
);
go

create table [State] (
    IDState int primary key identity (1, 1),
    [Name] nvarchar(50) not null
);
go

create table Request (
	IDRequest int primary key identity (1, 1),
	StudentID char(11) foreign key references Student(OIB) not null,
	InstructorID int foreign key references Instructor(IDInstructor) not null,
	StateID int foreign key references [State](IDState) not null
);
go

create table Rezervation (
    IDRezervation int primary key identity (1, 1),
    StudentID char(11) foreign key references Student(OIB) not null,
	InstructorID int foreign key references Instructor(IDInstructor) not null,
	StateID int foreign key references [State](IDState) default 3 not null,
	StartDate datetime not null,
	EndDate datetime not null
);
go

create table TimeSlot (
    IDTimeSlot int primary key identity (1, 1),
    RezervationID int foreign key references Rezervation(IDRezervation) not null,
	Done bit default 0
);
go

insert into Colour([Name])
values 
('Crna'),
('Bijela'),
('Siva'),
('Crvena'),
('Plava'),
('Žuta'),
('Zelena'),
('Narančasta'),
('Smeđa'),
('Ljubičasta');

insert into Category([Name])
values 
('AM'),
('A1'),
('A2'),
('A'),
('B1'),
('B'),
('C1'),
('C'),
('D1'),
('D'),
('BE'),
('C1E'),
('CE'),
('D1E'),
('DE'),
('F'),
('G'),
('H');

insert into Brand([Name])
values 
('Audi'),
('BMW'),
('Citroën'),
('Fiat'),
('Ford'),
('Honda'),
('Mercedes-Benz'),
('Nissan'),
('Tesla'),
('Volkswagen'),
('Chevrolet'),
('Renault');

insert into Model([Name], BrandID)
values 
('A4', 1),
('M4', 2),
('C3', 3),
('500', 4),
('Fiesta', 5),
('Civic', 6),
('A220', 7),
('CB1000R', 6),
('Model 2', 9),
('Beetle', 10),
('Malibu', 11),
('Alpine', 12);

insert into State([Name])
values 
('APPROVED'),
('CANCELLED'),
('PENDING');

go
create or alter proc CreateVehicle
	@Colour int,
	@Category int,
	@Model int,
	@Instructor int,
	@PicPath nvarchar(500)
as
	declare @tsql nvarchar(max)
	SET @tsql = 'insert into Vehicle(ColourID, CategoryID, ModelID, InstructorID, Picture) ' +
               ' SELECT ' + ''''
			   + cast(@Colour as varchar(10)) + '''' + ',' + ''''
			   + cast(@Category as varchar(10)) + '''' + ',' + ''''
			   + cast(@Model as varchar(10))  + '''' + ',' + ''''
			   + cast(@Instructor as varchar(10))  + '''' + ', * ' + 
               'FROM Openrowset( Bulk ' + '''' + @PicPath + '''' + ', Single_Blob) as img'
    EXEC (@tsql)
go

insert into Person(FirstName, Lastname, Email, PswdHash, PswSalt)
values 
('Pero', 'Perić', 'pp@gmail.com', '32xeU2ICKqbGEK5EJuU0s0f7cFaJBnsRV5hu6pCutUU=', 'yTu9ryRQ0ydkG1YZAhhNYQ=='),
('Iva', 'Ivić', 'iiviich@gmail.com', 'EeoJbpi8QzTQSYEmfsr4OlG5PuAURS/LqrFnJTC1wmg=', 'a1naoPX1qqlTuWU8KopZHQ=='),
('Marin', 'Marić', 'mar.mar@gmail.com', 'U67MZpoOLP0zQ6KFnSLAisruBXhF4RaQYc7CYP8B0eM=', 'gP0gh/H+NdDCjXNB3djp5w=='),
('Bogo', 'Moljak', 'bogo.moljka@hotmail.com', 'KeYsVeKHcQyk//YF8QCJZ2YxDGktWEYUVKGgfg9pjnU=', 'zGw2IE4cQ7oNKOBW4p9xuQ=='),
('Florijan', 'Gavran', 'gavranov.let@gmail.com', '3O4nZDb3sXuMxum0PnVhExQIF0Q2iXjsS8FVw805424=', 'EFxTyCTLxeafMQgpjxUJIg=='),
('Veljko', 'Kunić', 'v.kunich@gmail.com', 'Gxrtl7P5lkE1c5CuFCSHhA0+NryW8g8JQOYyxG8JIlI=', '38XbCZ4P9yT7YWdIjEkl+g=='),
('Miško', 'Krstić', 'misho.kr@outlook.com', 'LTc3WOipbkL8wfmkJAIpj6xF0k2p+ix9U+6g3kHiHDU=', 'evXico2kvhoieII9TeDASw=='),
('Laki', 'Topalović', 'laki.luk@gmail.com', 'adJjXolh6ZVeta1DsJqks0UEauaKExHKXjGd+8S9PDs=', 'R2x7tdoAhEYwMjvmAM6OkA=='),
('Petar', 'Cvetković', 'petar.cvijet@gmail.com', 'jLSaRRXCfgxIE52lKdVRiTcO2y9SHD/TriqvKjaS8sA=', 'elblK1iu04LppxHUU3Dd8A=='),
('Dušan', 'Kovačević', 'dus.kov@gmail.com', 'RBgPcvt5wGF49tgcPhKdUbjZpSBXr0qynVqrhYtjnZk=+8S9PDs=', 'EEsmnA9CRp0PK11lslhx8w=='),
('Srđan', 'Dragojević', 'srdjo@gmail.com', '8SHQbgk4OIWtZMkxgJflvl/3iJsK/5m1NxtxE4zt70A=', 'kuvK6Z9wNsm66Vf8q7rImg=='),
('Ryan', 'Gosling', 'literally.me@gmail.com', 'LsAjamilmwxtTTwjSHhYtD9t2mzjQLZxi6g9u8docx8=', '38uaEl7J7nModeNNQCEeMg=='),
('Misato', 'Katsuragi', 'mis.ato@nerv.com', '8ATeNb16/77BZqGYsEQQzzPM9BaHx7/uNmUd56Chkp8=', 'SY+h4g9bJzyaHTf1VIsCMA==');
go

insert into Instructor(PersonID)
values 
(3),
(7),
(11),
(12),
(13);
go

exec CreateVehicle 1, 6, 3, 1, 'C:\Users\Paulo\Desktop\PRASS\PRA_master\SQL\PRA_slike\Citroen_C3.jpg'
exec CreateVehicle 4, 6, 4, 3, 'C:\Users\Paulo\Desktop\PRASS\PRA_master\SQL\PRA_slike\Fiat_500.jpg'
exec CreateVehicle 6, 6, 6, 2, 'C:\Users\Paulo\Desktop\PRASS\PRA_master\SQL\PRA_slike\Honda_Civic.jpg'
exec CreateVehicle 1, 4, 8, 2, 'C:\Users\Paulo\Desktop\PRASS\PRA_master\SQL\PRA_slike\Honda_CB1000R.jpg'
exec CreateVehicle 5, 6, 9, 3, 'C:\Users\Paulo\Desktop\PRASS\PRA_master\SQL\PRA_slike\Tesla_Model_2.jpg'
exec CreateVehicle 2, 6, 10, 3, 'C:\Users\Paulo\Desktop\PRASS\PRA_master\SQL\PRA_slike\VW_Beetle.jpg'
exec CreateVehicle 3, 6, 11, 4, 'C:\Users\Paulo\Desktop\PRASS\PRA_master\SQL\PRA_slike\Chevy_Malibu.jpg'
exec CreateVehicle 5, 6, 12, 5, 'C:\Users\Paulo\Desktop\PRASS\PRA_master\SQL\PRA_slike\Renault_Alpine.jpg'
go

insert into Student(OIB, PersonID, VehicleID, HoursDriven)
values 
('12345678901', 5, 3, 0),
('23469547885', 8, 2, 12),
('34698765645', 4, 4, 8),
('23433904849', 9, null, 2),
('93796783748', 1, null, 24),
('95738396078', 2, null, 27),
('78484735583', 6, 5, 7),
('28466969468', 10, 1, 13);
go

insert into Review(StudentID, InstructorID, Grade, Comment)
values
('12345678901', 1, 1, 'Nije se pojavio na dogovoreno vrijeme i ne mogu se s njim dogovoriti za sljedeci termin.'),
('95738396078', 3, 5, null),
('12345678901',2, 5, 'Dobar momak. Smiren i korektan. Uvijek ce ti napomenuti ako napravis neku gresku pa dosta naucis.');
go

insert into Request(StudentID, InstructorID, StateID)
values
('12345678901', 5, 2),
('12345678901', 2, 1),
('28466969468', 1, 1),
('34698765645', 2, 1),
('78484735583', 3, 1),
('23469547885', 3, 1)

insert into Rezervation(StudentID, InstructorID, StateID, StartDate, EndDate)
values
('12345678901', 2, 1, '2024-6-1 12:30', '2024-6-1 13:45'),
('28466969468', 1, 2, '2024-6-7 10:00', '2024-6-7 11:30'),
('34698765645', 2, 1, '2024-6-3 17:00', '2024-6-3 18:30'),
('78484735583', 3, 1, '2024-6-23 9:30', '2024-6-1 11:00'),
('28466969468', 1, 3, '2024-6-1 13:30', '2024-6-1 15:00'),
('23469547885', 3, 3, '2024-6-7 18:30', '2024-6-1 19:15');
go

select * from Person
select * from Student
select * from Instructor
select * from Request
select * from Vehicle
select * from Colour
select * from Model
select * from Review
select * from Rezervation
select * from TimeSlot
select * from [State]
select * from Brand
select * from Category
