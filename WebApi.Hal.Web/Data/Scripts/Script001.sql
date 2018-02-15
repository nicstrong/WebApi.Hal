﻿create table BeerStyles
(
	Id INTEGER PRIMARY KEY,
	Name nvarchar(100) NOT NULL,
	[Description] ntext NULL
)
;


create table Breweries
(
	Id INTEGER PRIMARY KEY,
	Name nvarchar(100) NOT NULL,
	[Address] nvarchar(255) NULL,
	City nvarchar(100) NULL,
	Country nvarchar(100) NULL,
	Phone nvarchar(100) NULL,
	Website nvarchar(100) NULL,
	Twitter nvarchar(100) NULL,
	Notes ntext NULL
)
;

create table Beers
(
	Id INTEGER PRIMARY KEY,
	Name nvarchar(100) NOT NULL,
	Style_Id INT NULL,
	Brewery_Id int NULL,
	Abv decimal(3,2) NULL,
	FOREIGN KEY(Style_Id) REFERENCES BeerStyles(Id),
	FOREIGN KEY(Brewery_Id) REFERENCES Breweries(Id)
)
;
