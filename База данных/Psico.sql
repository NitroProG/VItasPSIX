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
[DefenderKey] nvarchar(44) not null
constraint [id_defend] primary key clustered
	([id_defend] ASC) on [primary] 
)

create table [dbo].[Teachers]
(
[id_teacher] int not null identity(1,1),
[Unique_Naim] nvarchar(30) not null,
[User_End_Data] date not null,
[KolvoNeRegStudents] int not null
constraint [id_teacher] primary key clustered
	([id_teacher] ASC) on [Primary]
)

CREATE TABLE [DBO].[users]
(
[id_user] INT NOT NULL IDENTITY (1,1),
[User_Login] nvarchar(50) NOT NULL,
[User_Password] nvarchar(88) NOT NULL,
[User_Mail] nvarchar(216) not null,
[Fam] nvarchar(150) null,
[Imya] nvarchar(150) null,
[Otch] nvarchar(150) null,
[Study] nvarchar(50) null,
[Work] nvarchar(300) null,
[YearUser] int null,
[Old] int null,
[UserStatus] bit not null,
[Teacher_id] int not null
constraint [id_user] PRIMARY KEY CLUSTERED
	([id_user] ASC) on [PRIMARY],
CONSTRAINT [FK_teacher_id] FOREIGN KEY ([teacher_id])
	REFERENCES [DBO].[teachers]([id_teacher])
)

create table [dbo].[Dostup]
(
[Id_Dostup] int not null identity (1,1),
[UpdateZadach] bit not null,
[UpdateUsers] bit not null,
[WorkZadach] bit not null
constraint [id_Dostup] primary key clustered
    ([id_Dostup] ASC) on [primary]
)

create table [dbo].[Role]
(
[id_role] int not null identity(1,1),
[Naim] nvarchar(10) not null,
[users_id] int not null,
[Dostup_id] int not null
constraint [id_role] primary key clustered
	([id_role] asc) on [primary],
CONSTRAINT [FK_userssss_id] FOREIGN KEY ([users_id])
	REFERENCES [DBO].[users]([id_user]),
CONSTRAINT [FK_Dostup_id] FOREIGN KEY ([Dostup_id])
	REFERENCES [DBO].[Dostup]([id_Dostup])
)

create table [dbo].[Lastotv]
(
[id_Last_otv] int not null identity (1,1),
[name_otv] nvarchar(max) not null,
[Form_otv] nvarchar(10) not null,
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
[FormOtvSelected] nvarchar(10) not null,
[users_id] int not null
constraint [id_otvselected] primary key clustered
	([id_otvselected]ASC) on [Primary],
CONSTRAINT [FK_userrrs_id] FOREIGN KEY ([users_id])
	REFERENCES [DBO].[users]([id_user])
)

create table [dbo].[Zadacha]
(
[id_zadacha] int not null identity(1,1),
[Zapros] nvarchar(300) not null,
[sved] nvarchar(100) not null,
[meroprtext] nvarchar(max) null,
[katamneztext] nvarchar(max) null
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
[RB] nvarchar(100) not null,
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
[FormCB] nvarchar(10) not null,
[zadacha_id] int not null
constraint [id_CBFormFill] primary key clustered
	([id_CBFormFill]ASC) on [primary],
CONSTRAINT [FK_Zadachaa_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)

create table [dbo].[dpo]
(
[id_dpo] int not null identity (1,1),
[lb_small] nvarchar(100) null,	
[lb] nvarchar(300) not null,
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
[FormVernOtv] nvarchar(10) not null,
[zadacha_id] int not null
constraint [id_vernotv] primary key clustered
	([id_vernotv]ASC) on [primary],
CONSTRAINT [FK_Zadacha6_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)
go

create view CreateTableForUpdateUsers
as select id_user as N'Номер пользователя', User_Login as N'Логин', User_Password as N'Пароль', User_Mail as N'Почта', Teacher_id as N'Номер преподавателя',
id_teacher as N'Код преподавателя', Unique_Naim as N'Уникальное имя',User_End_Data as 'Дата окончания использования', KolvoNeRegStudents as N'Количество оставшихся регистраций студентов',
id_role as N'Номер роли', Naim, users_id as N'Код пользователя', Dostup_id as N'Номер доступа'
from
users INNER JOIN
role on users.id_user = Role.users_id INNER JOIN
Teachers on users.Teacher_id = Teachers.id_teacher
go

create view CreateTableForDeleteUsers
as select id_user as N'№   пользователя', User_Login as N'Логин', Naim as N'Статус'
from
users INNER JOIN
Role on users.id_user = Role.users_id
go