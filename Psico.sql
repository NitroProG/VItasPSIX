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

CREATE TABLE [DBO].[users]
(
[id_user] INT NOT NULL IDENTITY (1,1),
[User_Login] nvarchar(max) NOT NULL,
[User_Password] nvarchar(max) NOT NULL,
[User_Mail] nvarchar(max) not null,
[Kolvo_students] int not null,
[isAdmin] int NOT NULL
constraint [id_user] PRIMARY KEY CLUSTERED
	([id_user] ASC) on [PRIMARY]
)

create table [dbo].[students]
(
[id_students] int not null identity(1,1),
[Student_login] nvarchar(max) not null,
[Student_Password] nvarchar(max) not null,
[Student_Mail] nvarchar(max) not null,
[users_id] int not null
constraint [id_students] PRIMARY KEY CLUSTERED
	([id_students] ASC) on [PRIMARY],
CONSTRAINT [FK_userstudents_id] FOREIGN KEY ([users_id])
	REFERENCES [DBO].[users]([id_user])
)

create table [dbo].[otvFenom]
(
[id_otvFenom] int not null identity (1,1),
[name_otv] nvarchar(max) null,
[users_id] int not null
constraint [id_otvFenom] primary key clustered
	([id_otvFenom] ASC) on [Primary],
CONSTRAINT [FK_userrr_id] FOREIGN KEY ([users_id])
	REFERENCES [DBO].[users]([id_user])
)

create table [dbo].[otvGip]
(
[id_otvGip] int not null identity (1,1),
[name_otv] nvarchar(max) null,
[users_id] int not null
constraint [id_otvGip] primary key clustered
	([id_otvGip] ASC) on [Primary],
CONSTRAINT [FK_userrrr_id] FOREIGN KEY ([users_id])
	REFERENCES [DBO].[users]([id_user])
)

create table [dbo].[otvDiag]
(
[id_otvDiag] int not null identity (1,1),
[name_otv] nvarchar(max) null,
[users_id] int not null
constraint [id_otvDiag] primary key clustered
	([id_otvDiag] ASC) on [Primary],
CONSTRAINT [FK_userrrrr_id] FOREIGN KEY ([users_id])
	REFERENCES [DBO].[users]([id_user])
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
CONSTRAINT [FK_Zadacha7_id] FOREIGN KEY ([Zadacha_id])
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

create table [dbo].[Fenom2]
(
[id_Fenom2] int not null identity (1,1),
[CB] nvarchar(max) not null,
[zadacha_id] int not null
constraint [id_Fenom2] primary key clustered
	([id_fenom2]ASC) on [primary],
CONSTRAINT [FK_Zadachaa_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)

create table [dbo].[teor]
(
[id_teor] int not null identity (1,1),
[CB] nvarchar(max) not null,
[zadacha_id] int not null
constraint [id_teor] primary key clustered
	([id_teor]ASC) on [primary],
CONSTRAINT [FK_Zadacha1_id] FOREIGN KEY ([Zadacha_id])
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

create table [dbo].[dz]
(
[id_dz] int not null identity (1,1),
[CB] nvarchar(max) not null,
[zadacha_id] int not null
constraint [id_dz] primary key clustered
	([id_dz]ASC) on [primary],
CONSTRAINT [FK_Zadacha3_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)

create table [dbo].[vernotv_Fenom]
(
[id_vernotv] int not null identity (1,1),
[otv] nvarchar(max) not null,
[zadacha_id] int not null
constraint [id_vernotv] primary key clustered
	([id_vernotv]ASC) on [primary],
CONSTRAINT [FK_Zadacha6_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)

create table [dbo].[vernotv_Gip]
(
[id_vernotv] int not null identity (1,1),
[otv] nvarchar(max) not null,
[zadacha_id] int not null
constraint [id_vernotvGip] primary key clustered
	([id_vernotv]ASC) on [primary],
CONSTRAINT [FK_Zadacha8_id] FOREIGN KEY ([Zadacha_id])
	REFERENCES [DBO].[Zadacha]([id_Zadacha])
)

create table [dbo].[vernotv_Diag]
(
[id_vernotv] int not null identity (1,1),
[otv] nvarchar(max) not null,
[zadacha_id] int not null
constraint [id_vernotvDiag] primary key clustered
	([id_vernotv]ASC) on [primary],
CONSTRAINT [FK_Zadacha9_id] FOREIGN KEY ([Zadacha_id])
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

create table [dbo].[DpoSelected]
(
[id_dposelected] int not null identity(1,1),
[InfoSelected] nvarchar(max) not null,
[users_id] int not null
constraint [id_dposelected] primary key clustered
	([id_dposelected]ASC) on [Primary],
CONSTRAINT [FK_userrrs_id] FOREIGN KEY ([users_id])
	REFERENCES [DBO].[users]([id_user])
)

create table [dbo].[FenomSelected]
(
[id_fenomselected] int not null identity(1,1),
[InfoSelected] nvarchar(max) not null,
[users_id] int not null
constraint [id_fenomselected] primary key clustered
	([id_fenomselected]ASC) on [Primary],
CONSTRAINT [FK_userrrrs_id] FOREIGN KEY ([users_id])
	REFERENCES [DBO].[users]([id_user])
)

create table [dbo].[TeorSelected]
(
[id_teorselected] int not null identity(1,1),
[InfoSelected] nvarchar(max) not null,
[users_id] int not null
constraint [id_teorselected] primary key clustered
	([id_teorselected]ASC) on [Primary],
CONSTRAINT [FK_userrrrrs_id] FOREIGN KEY ([users_id])
	REFERENCES [DBO].[users]([id_user])
)