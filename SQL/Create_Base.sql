create database PRA_Autoskola COLLATE Croatian_CI_AS
go

use PRA_Autoskola
go

create table Osoba (
    IDOsoba int primary key identity (1, 1),
    Ime nvarchar(50) not null,
    Prezime varchar(50) not null,
    Email nvarchar(50) not null,
    Lozinka nvarchar(255) not null
);
go

create table Marka (
    IDMarka int primary key identity (1, 1),
    Naziv nvarchar(50) not null
);
go

create table Model (
    IDModel int primary key identity (1, 1),
    Naziv nvarchar(50) not null,
	MarkaID int foreign key references Marka(IDMarka) not null
);
go

create table Kategorija (
    IDKategorije int primary key identity (1, 1),
    Naziv nvarchar(50) not null
);
go

create table Boja (
    IDBoje int primary key identity (1, 1),
    Naziv nvarchar(50) not null
);
go

create table Vozilo (
    IDVozila int primary key identity (1, 1),
    BojaID int foreign key references Boja(IDBoje) not null,
	KategorijaID int foreign key references Kategorija(IDKategorije) not null,
	ModelID int foreign key references Model(IDModel) not null,
	Slika varbinary(max)
);
go

create table Instruktor (
    IDInstruktora int primary key identity (1, 1),
    OsobaID int foreign key references Osoba(IDOsoba) not null,
	VoziloID int foreign key references Vozilo(IDVozila) not null
);
go

create table Polaznik (
    OIB char(11) primary key,
    OsobaID int foreign key references Osoba(IDOsoba) unique not null,
	OdvozenoSati int default 0
);
go

create table Recenzija (
    IDRecenzije int primary key identity (1, 1),
    PolaznikID char(11) foreign key references Polaznik(OIB) not null,
	InstruktorID int foreign key references Instruktor(IDInstruktora) not null,
	Ocjena int not null,
	Komentar nvarchar(max)
);
go

create table Stanje (
    IDStanja int primary key identity (1, 1),
    Naziv nvarchar(50) not null
);
go

create table Rezervacija (
    IDRezervacije int primary key identity (1, 1),
    PolaznikID char(11) foreign key references Polaznik(OIB) not null,
	InstruktorID int foreign key references Instruktor(IDInstruktora) not null,
	StanjeID int foreign key references Stanje(IDStanja) default 3 not null,
	Pocetak datetime not null,
	Zavrsetak datetime not null
);
go

create table Termin (
    IDTermina int primary key identity (1, 1),
    RezrvacijaID int foreign key references Rezervacija(IDRezervacije) not null,
	Obradeno bit default 0
);
go

insert into Boja(Naziv)
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

insert into Kategorija(Naziv)
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

insert into Marka(Naziv)
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

insert into Model(Naziv, MarkaID)
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

insert into Stanje(Naziv)
values 
('APPROVED'),
('CANCELLED'),
('PENDING');

select * from model

go
create or alter proc CreateVozilo
	@Boja int,
	@Kategorija int,
	@Model int,
	@PutanjaSlike nvarchar(500)
as
	declare @tsql nvarchar(max)
	SET @tsql = 'insert into Vozilo(BojaID, KategorijaID, ModelID, Slika) ' +
               ' SELECT ' + '''' + cast(@Boja as varchar(10)) + '''' + ',' + '''' + cast(@Kategorija as varchar(10)) + '''' + ',' + '''' + cast(@Model as varchar(10))  + '''' + ', * ' + 
               'FROM Openrowset( Bulk ' + '''' + @PutanjaSlike + '''' + ', Single_Blob) as img'
    EXEC (@tsql)
go

exec CreateVozilo 1, 6, 3, 'C:\Temp\PRA_slike\Citroen_C3.jpg'
exec CreateVozilo 4, 6, 4, 'C:\Temp\PRA_slike\Fiat_500.jpg'
exec CreateVozilo 6, 6, 6, 'C:\Temp\PRA_slike\Honda_Civic.jpg'
exec CreateVozilo 1, 4, 8, 'C:\Temp\PRA_slike\Honda_CB1000R.jpg'
exec CreateVozilo 5, 4, 9, 'C:\Temp\PRA_slike\Tesla_Model_2.jpg'
exec CreateVozilo 2, 4, 10, 'C:\Temp\PRA_slike\VW_Beetle.jpg'
go

insert into Osoba(Ime, Prezime, Email, Lozinka)
values 
('Pero', 'Perić', 'pp@gmail.com', 'P3roM4j5t0r'),
('Iva', 'Ivić', 'iiviich@gmail.com', 'BejbiLazanja'),
('Marin', 'marić', 'mar.mar@gmail.com', 'Pa$$w0rd'),
('Bogo', 'Moljak', 'bogo.moljka@hotmail.com', 'NAJboljiDoktor'),
('Florijan', 'Gavran', 'gavranov.let@gmail.com', 'KadCeRucakNecuJogurt'),
('Veljko', 'Kunić', 'v.kunich@gmail.com', 'zapravoNajboljiDoktor'),
('Miško', 'Krstić', 'misho.kr@outlook.com', 'TkoToTamoPeva'),
('Laki', 'Topalović', 'laki.luk@gmail.com', 'MaratonciTrcePocasnikrug'),
('Petar', 'Cvetković', 'petar.cvijet@gmail.com', 'Ljeto68'),
('Dušan', 'Kovačević', 'dus.kov@gmail.com', 'Podzemlje'),
('Srđan', 'Dragojević', 'srdjo@gmail.com', 'LepaSelaLepoGore');
go

insert into Polaznik(OIB, OsobaID, OdvozenoSati)
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

insert into Instruktor(OsobaID, VoziloID)
values 
(3, 1),
(7, 4),
(11, 6);
go

insert into Recenzija(PolaznikID, InstruktorID, Ocjena, Komentar)
values
('12345678901', 1, 1, 'Nije se pojavio na dogovoreno vrijeme i ne mogu se s njim dogovoriti za sljedeci termin.'),
('95738396078', 3, 5, null),
('12345678901',2, 5, 'Dobar momak. Smiren i korektan. Uvijek ce ti napomenuti ako napravis neku gresku pa dosta naucis.');
go

insert into Rezervacija(PolaznikID, InstruktorID, StanjeID, Pocetak, Zavrsetak)
values
('12345678901', 2, 1, '2024-6-1 12:30', '2024-6-1 13:45'),
('28466969468', 1, 2, '2024-6-7 10:00', '2024-6-7 11:30'),
('34698765645', 2, 1, '2024-6-3 17:00', '2024-6-3 18:30'),
('78484735583', 3, 1, '2024-6-23 9:30', '2024-6-1 11:00'),
('28466969468', 1, 3, '2024-6-1 13:30', '2024-6-1 15:00'),
('23469547885', 3, 3, '2024-6-7 18:30', '2024-6-1 19:15');
go

