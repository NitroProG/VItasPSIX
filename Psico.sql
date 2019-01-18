SET ANSI_PADDING ON
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO 

CREATE DATABASE [psico]
GO

USE [psico]
GO

CREATE TABLE [DBO].[users]
(
[id_user] INT NOT NULL IDENTITY (1,1),
[User_Login] varchar(max) NOT NULL,
[User_Password] varchar(max) NOT NULL,
[isAdmin] int NOT NULL
constraint [id_user] PRIMARY KEY CLUSTERED
	([id_user] ASC) on [PRIMARY]
)

create table [dbo].[Zadacha]
(
[id_zadacha] int not null identity(1,1),
[Zapros] varchar(max) not null,
[sved] varchar(max) not null
constraint [id_zadacha] primary key clustered
	([id_zadacha]ASC) on [Primary]
)

create table [dbo].[resh]
(
[id_resh] int not null identity(1,1),
[users_id] int not null,
[zadacha_id] int not null
constraint [id_resh] primary key clustered
	([id_resh]ASC) on [Primary],
CONSTRAINT [FK_users_id] FOREIGN KEY ([users_id])
	REFERENCES [DBO].[users]([id_user]),
CONSTRAINT [FK_Zadacha7_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)

create table [dbo].[Fenom1]
(
[id_Fenom1] int not null identity (1,1),
[RB] varchar(max) not null,
[RBText] varchar(max) not null,
[zadacha_id] int not null
constraint [id_Fenom1] primary key clustered
	([id_fenom1]ASC) on [primary],
CONSTRAINT [FK_Zadacha_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)

create table [dbo].[Fenom2]
(
[id_Fenom2] int not null identity (1,1),
[CB] varchar(max) not null,
[zadacha_id] int not null
constraint [id_Fenom2] primary key clustered
	([id_fenom2]ASC) on [primary],
CONSTRAINT [FK_Zadachaa_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)

create table [dbo].[teor]
(
[id_teor] int not null identity (1,1),
[CB] varchar(max) not null,
[zadacha_id] int not null
constraint [id_teor] primary key clustered
	([id_teor]ASC) on [primary],
CONSTRAINT [FK_Zadacha1_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)

create table [dbo].[dpo]
(
[id_dpo] int not null identity (1,1),
[lb_small] varchar(max) null,	
[lb] varchar(max) not null,
[lbtext] varchar(max) not null,
[lb_image] varchar(max) null,
[lb_image2] varchar(max) null,
[zadacha_id] int not null
constraint [id_dpo] primary key clustered
	([id_dpo]ASC) on [primary],
CONSTRAINT [FK_Zadacha2_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)

create table [dbo].[dz]
(
[id_dz] int not null identity (1,1),
[CB] varchar(max) not null,
[zadacha_id] int not null
constraint [id_dz] primary key clustered
	([id_dz]ASC) on [primary],
CONSTRAINT [FK_Zadacha3_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)

create table [dbo].[vernotv]
(
[id_vernotv] int not null identity (1,1),
[otv] varchar(max) not null,
[zadacha_id] int not null
constraint [id_vernotv] primary key clustered
	([id_vernotv]ASC) on [primary],
CONSTRAINT [FK_Zadacha6_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)

create table [dbo].[meropr]
(
[id_meropr] int not null identity (1,1),
[meroprtext] varchar(max) not null,
[zadacha_id] int not null
constraint [id_meropr] primary key clustered
	([id_meropr]ASC) on [primary],
CONSTRAINT [FK_Zadacha4_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)

create table [dbo].[katamnez]
(
[id_katamnez] int not null identity (1,1),
[katamneztext] varchar(max) not null,
[zadacha_id] int not null
constraint [id_katamnez] primary key clustered
	([id_katamnez]ASC) on [primary],
CONSTRAINT [FK_Zadacha5_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)

INSERT into dbo.users(User_Login,User_Password,isAdmin)
	Values ('admin','admin','1');
INSERT into dbo.users(User_Login,User_Password,isAdmin)
	Values ('test','test','0');
go

CREATE PROCEDURE [DBO].[users_add]
(
@User_Login varchar(max),
@User_Password varchar(max),
@isadmin int
)
AS
	insert into [dbo].[users]([User_Login],[User_Password],[isadmin]) values((@User_Login),(@User_Password),(@isadmin));
go

CREATE PROCEDURE [DBO].[users_update]
(
@id_user int,
@User_Login varchar(max),
@User_Password varchar(max),
@isadmin int
)
AS
	update [dbo].users
	set
	User_Login=@User_Login,
	User_Password=@User_Password,
	isAdmin=@isadmin
	where id_user=@id_user
go

CREATE PROCEDURE [DBO].[resh_add]
(
@Users_id int,
@Zadacha_id int
)
AS
	insert into [dbo].[resh]([users_id],[zadacha_id]) values((@Users_id),(@Zadacha_id));
go