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

create table [dbo].[defender]
(
[id_defend] int not null identity(1,1),
[DefenderKey] nvarchar(16) not null,
constraint [id_defend] primary key clustered
	([id_defend] ASC) on [primary] 
)

create table [dbo].[Teachers]
(
[id_teacher] int not null identity(1,1),
[Unique_Naim] nvarchar(max) not null,
[KolvoNeRegStudents] int not null
constraint [id_teacher] primary key clustered
	([id_teacher] ASC) on [Primary]
)

CREATE TABLE [DBO].[users]
(
[id_user] INT NOT NULL IDENTITY (1,1),
[User_Login] nvarchar(max) NOT NULL,
[User_Password] nvarchar(max) NOT NULL,
[User_Mail] nvarchar(max) not null,
[Teacher_id] int not null
constraint [id_user] PRIMARY KEY CLUSTERED
	([id_user] ASC) on [PRIMARY],
CONSTRAINT [FK_teacher_id] FOREIGN KEY ([teacher_id])
	REFERENCES [DBO].[teachers]([id_teacher])
)

create table [dbo].[Dostup]
(
[Id_Dostup] int not null identity (1,1),
[UpdateZadach] int not null,
[UpdateUsers] int not null,
[WorkZadach] int not null
constraint [id_Dostup] primary key clustered
    ([id_Dostup] ASC) on [primary]
)

create table [dbo].[Role]
(
[id_role] int not null identity(1,1),
[Naim] nvarchar(max) not null,
[users_id] int not null,
[Dostup_id] int not null
constraint [id_role] primary key clustered
	([id_role] asc) on [primary],
CONSTRAINT [FK_userssss_id] FOREIGN KEY ([users_id])
	REFERENCES [DBO].[users]([id_user]),
CONSTRAINT [FK_Dostup_id] FOREIGN KEY ([Dostup_id])
	REFERENCES [DBO].[Dostup]([id_Dostup])
)

create table [dbo].[InfoUser]
(
[id_Info] int not null identity (1,1),
[FIO] nvarchar(max) null,
[Study] nvarchar(max) null,
[Work] nvarchar(max) null,
[YearUser] nvarchar(max) null,
[Old] nvarchar(max) null,
[users_id] int not null
constraint [id_info] primary key clustered
	([id_info]ASC) on [Primary],
CONSTRAINT [FK_userr_id] FOREIGN KEY ([users_id])
	REFERENCES [DBO].[users]([id_user])
)

create table [dbo].[Lastotv]
(
[id_Last_otv] int not null identity (1,1),
[name_otv] nvarchar(max) null,
[Form_otv] nvarchar(max) null,
[users_id] int not null
constraint [id_Last_otv] primary key clustered
	([id_Last_otv] ASC) on [Primary],
CONSTRAINT [FK_userrr_id] FOREIGN KEY ([users_id])
	REFERENCES [DBO].[users]([id_user])
)

create table [dbo].[OtvSelected]
(
[id_otvselected] int not null identity(1,1),
[InfoSelected] nvarchar(max) not null,
[FormOtvSelected] nvarchar(max) not null,
[users_id] int not null
constraint [id_otvselected] primary key clustered
	([id_otvselected]ASC) on [Primary],
CONSTRAINT [FK_userrrs_id] FOREIGN KEY ([users_id])
	REFERENCES [DBO].[users]([id_user])
)

create table [dbo].[Zadacha]
(
[id_zadacha] int not null identity(1,1),
[Zapros] nvarchar(max) not null,
[sved] nvarchar(max) not null
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
CONSTRAINT [FK_Zadachaaaaa11_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)

create table [dbo].[Fenom1]
(
[id_Fenom1] int not null identity (1,1),
[RB] nvarchar(max) not null,
[RBText] nvarchar(max) not null,
[zadacha_id] int not null
constraint [id_Fenom1] primary key clustered
	([id_fenom1]ASC) on [primary],
CONSTRAINT [FK_Zadacha_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)

create table [dbo].[CBFormFill]
(
[id_CBFormFill] int not null identity (1,1),
[CB] nvarchar(max) not null,
[FormCB] nvarchar(max) not null,
[zadacha_id] int not null
constraint [id_CBFormFill] primary key clustered
	([id_CBFormFill]ASC) on [primary],
CONSTRAINT [FK_Zadachaa_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)

create table [dbo].[dpo]
(
[id_dpo] int not null identity (1,1),
[lb_small] nvarchar(max) null,	
[lb] nvarchar(max) not null,
[lbtext] nvarchar(max) not null,
[lb_image] nvarchar(max) null,
[lb_image2] nvarchar(max) null,
[zadacha_id] int not null
constraint [id_dpo] primary key clustered
	([id_dpo]ASC) on [primary],
CONSTRAINT [FK_Zadacha2_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)

create table [dbo].[vernotv]
(
[id_vernotv] int not null identity (1,1),
[otv] nvarchar(max) not null,
[FormVernOtv] nvarchar(max) not null,
[zadacha_id] int not null
constraint [id_vernotv] primary key clustered
	([id_vernotv]ASC) on [primary],
CONSTRAINT [FK_Zadacha6_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)

create table [dbo].[meropr]
(
[id_meropr] int not null identity (1,1),
[meroprtext] nvarchar(max) not null,
[zadacha_id] int not null
constraint [id_meropr] primary key clustered
	([id_meropr]ASC) on [primary],
CONSTRAINT [FK_Zadacha4_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)

create table [dbo].[katamnez]
(
[id_katamnez] int not null identity (1,1),
[katamneztext] nvarchar(max) not null,
[zadacha_id] int not null
constraint [id_katamnez] primary key clustered
	([id_katamnez]ASC) on [primary],
CONSTRAINT [FK_Zadacha5_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)
go

create view CreateTableForUpdateUsers
as select id_user as 'Номер пользователя', User_Login as 'Логин', User_Password as 'Пароль', User_Mail as 'Почта', Teacher_id as 'Номер преподавателя',
id_teacher as 'Код преподавателя', Unique_Naim as 'Уникальное имя', KolvoNeRegStudents as 'Количество оставшихся регистраций студентов', id_role as 'Номер роли',
Naim as 'Статус', users_id as 'Код пользователя', Dostup_id as 'Номер доступа'
from
users INNER JOIN
role on users.id_user = Role.users_id INNER JOIN
Teachers on users.Teacher_id = Teachers.id_teacher
go

create view CreateTableForDeleteUsers
as select id_user as '№   пользователя', User_Login as 'Логин', Naim as 'Статус'
from
users INNER JOIN
Role on users.id_user = Role.users_id
go