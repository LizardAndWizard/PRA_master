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

create table Vehicle (
    IDVehicle int primary key identity (1, 1),
    ColourID int foreign key references Colour(IDColour) not null,
	CategoryID int foreign key references Category(IDCategory) not null,
	ModelID int foreign key references Model(IDModel) not null,
	Picture varbinary(max)
);
go

create table Instructor (
    IDInstructor int primary key identity (1, 1),
    PersonID int foreign key references Person(IDPerson) not null,
	VehicleID int foreign key references Vehicle(IDVehicle) not null
);
go

create table Student (
    OIB char(11) primary key,
    PersonID int foreign key references Person(IDPerson) unique not null,
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
('Volkswagen');

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
('Beetle', 10);

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
	@PicPath nvarchar(500)
as
	declare @tsql nvarchar(max)
	SET @tsql = 'insert into Vehicle(ColourID, CategoryID, ModelID, Picture) ' +
               ' SELECT ' + ''''
			   + cast(@Colour as varchar(10)) + '''' + ',' + ''''
			   + cast(@Category as varchar(10)) + '''' + ',' + ''''
			   + cast(@Model as varchar(10))  + '''' + ', * ' + 
               'FROM Openrowset( Bulk ' + '''' + @PicPath + '''' + ', Single_Blob) as img'
    EXEC (@tsql)
go

exec CreateVehicle 1, 6, 3, '...\Citroen_C3.jpg'
exec CreateVehicle 4, 6, 4, '...\Fiat_500.jpg'
exec CreateVehicle 6, 6, 6, '...\Honda_Civic.jpg'
exec CreateVehicle 1, 4, 8, '...\Honda_CB1000R.jpg'
exec CreateVehicle 5, 6, 9, '...\Tesla_Model_2.jpg'
exec CreateVehicle 2, 6, 10, '...\VW_Beetle.jpg'
go

insert into Person(FirstName, Lastname, Email, PswdHash, PswSalt)
values 
('Pero', 'Perić', 'pp@gmail.com', 'P3roM4j5t0r', 'salt'),
('Iva', 'Ivić', 'iiviich@gmail.com', 'BejbiLazanja', 'salt'),
('Marin', 'Marić', 'mar.mar@gmail.com', 'Pa$$w0rd', 'salt'),
('Bogo', 'Moljak', 'bogo.moljka@hotmail.com', 'NAJboljiDoktor', 'salt'),
('Florijan', 'Gavran', 'gavranov.let@gmail.com', 'KadCeRucakNecuJogurt', 'salt'),
('Veljko', 'Kunić', 'v.kunich@gmail.com', 'zapravoNajboljiDoktor', 'salt'),
('Miško', 'Krstić', 'misho.kr@outlook.com', 'TkoToTamoPeva', 'salt'),
('Laki', 'Topalović', 'laki.luk@gmail.com', 'MaratonciTrcePocasnikrug', 'salt'),
('Petar', 'Cvetković', 'petar.cvijet@gmail.com', 'Ljeto68', 'salt'),
('Dušan', 'Kovačević', 'dus.kov@gmail.com', 'Podzemlje', 'salt'),
('Srđan', 'Dragojević', 'srdjo@gmail.com', 'LepaSelaLepoGore', 'salt');
go

insert into Student(OIB, PersonID, HoursDriven)
values 
('12345678901', 5, 0),
('23469547885', 8, 12),
('34698765645', 4, 8),
('23433904849', 9, 2),
('93796783748', 1, 24),
('95738396078', 2, 27),
('78484735583', 6, 7),
('28466969468', 10, 13);
go

insert into Instructor(PersonID, VehicleID)
values 
(3, 1),
(7, 4),
(7, 3),
(11, 6);
go

insert into Review(StudentID, InstructorID, Grade, Comment)
values
('12345678901', 1, 1, 'Nije se pojavio na dogovoreno vrijeme i ne mogu se s njim dogovoriti za sljedeci termin.'),
('95738396078', 3, 5, null),
('12345678901',2, 5, 'Dobar momak. Smiren i korektan. Uvijek ce ti napomenuti ako napravis neku gresku pa dosta naucis.');
go

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
select * from Vehicle
select * from Colour
select * from Model
select * from Review
select * from Rezervation
select * from TimeSlot
select * from [State]
select * from Brand
select * from Category
