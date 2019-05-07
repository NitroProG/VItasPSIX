USE [psico]
GO

CREATE PROCEDURE [DBO].[defend_delete]
(
@id_defend int
)
AS
	DELETE from [dbo].[defender]
	where id_defend=@id_defend;
go

CREATE PROCEDURE [DBO].[defend_add]
(
@DefenderKey nvarchar(16)
)
AS
	insert into [dbo].[defender]([DefenderKey]) 
		   values((@DefenderKey));
go

CREATE PROCEDURE [DBO].[defend_update]
(
@id_defend int,
@DefenderKey nvarchar(16)
)
AS
	update [dbo].defender
	set
	DefenderKey=@DefenderKey
	where id_defend=@id_defend
go

CREATE PROCEDURE [DBO].[dpo_delete]
(
@id_dpo int
)
AS
	DELETE from [dbo].[dpo]
	where id_dpo=@id_dpo;
go

CREATE PROCEDURE [DBO].[dpo_add]
(
@lb_small nvarchar(100),
@lb nvarchar(max),
@lbtext nvarchar(max),
@lb_image nvarchar(max),
@lb_image2 nvarchar(max),
@zadacha_id int
)
AS
	insert into [dbo].[dpo]([lb_small],[lb],[lbtext],[lb_image],[lb_image2],[zadacha_id]) 
		   values((@lb_small),(@lb),(@lbtext),(@lb_image),(@lb_image2),(@zadacha_id));
go

CREATE PROCEDURE [DBO].[dpo_update]
(
@dpo_id int,
@lb_small nvarchar(100),
@lb nvarchar(max),
@lbtext nvarchar(max),
@lb_image nvarchar(max),
@lb_image2 nvarchar(max),
@zadacha_id int
)
AS
	update [dbo].dpo
	set
	lb_small=@lb_small,
	lb=@lb,
	lbtext=@lbtext,
	lb_image=@lb_image,
	lb_image2=@lb_image2,
	zadacha_id=@zadacha_id
	where id_dpo=@dpo_id
go

CREATE PROCEDURE [DBO].[OtvSelected_delete]
(
@id_otvselected int
)
AS
	DELETE from [dbo].[OtvSelected]
	where id_otvselected=@id_otvselected;
go

CREATE PROCEDURE [DBO].[OtvSelected_add]
(
@InfoSelected nvarchar(max),
@FormOtvSelected nvarchar(10),
@users_id int
)
AS
	insert into [dbo].[OtvSelected]([InfoSelected],[FormOtvSelected],[users_id]) 
		   values((@InfoSelected),(@FormOtvSelected),(@users_id));
go

CREATE PROCEDURE [DBO].[OtvSelected_update]
(
@id_otvselected int,
@InfoSelected nvarchar(max),
@FormOtvSelected nvarchar(10),
@users_id int
)
AS
	update [dbo].OtvSelected
	set
	InfoSelected=@InfoSelected,
	FormOtvSelected=@FormOtvSelected,
	users_id=@users_id
	where id_otvselected=@id_otvselected
go

CREATE PROCEDURE [DBO].[CBFormFill_delete]
(
@id_CBFormFill int
)
AS
	DELETE from [dbo].[CBFormFill]
	where id_CBFormFill=@id_CBFormFill;
go

CREATE PROCEDURE [DBO].[CBFormFill_add]
(
@cb nvarchar(max),
@FormCB nvarchar(10),
@zadacha_id int
)
AS
	insert into [dbo].[CBFormFill]([CB],[FormCB],[zadacha_id]) values((@cb),(@FormCB),(@zadacha_id));
go

CREATE PROCEDURE [DBO].[CBFormFill_update]
(
@id_CBFormFill int,
@cb nvarchar(max),
@FormCB nvarchar(10),
@zadacha_id int
)
AS
	update [dbo].CBFormFill
	set
	CB=@cb,
	FormCB=@FormCB,
	zadacha_id=@zadacha_id
	where id_CBFormFill=@id_CBFormFill
go

CREATE PROCEDURE [DBO].[Fenom1_delete]
(
@id_fenom1 int
)
AS
	DELETE from [dbo].[Fenom1]
	where id_Fenom1=@id_fenom1;
go

CREATE PROCEDURE [DBO].[Fenom1_add]
(
@rb nvarchar(50),
@rbtext nvarchar(max),
@zadacha_id int
)
AS
	insert into [dbo].[Fenom1]([RB],[RBText],[zadacha_id]) values((@rb),(@rbtext),(@zadacha_id));
go

CREATE PROCEDURE [DBO].[Fenom1_update]
(
@id_fenom1 int,
@rb nvarchar(50),
@rbtext nvarchar(max),
@zadacha_id int
)
AS
	update [dbo].Fenom1
	set
	RB=@rb,
	RBText=@rbtext,
	zadacha_id=@zadacha_id
	where id_Fenom1=@id_fenom1
go

CREATE PROCEDURE [DBO].[Infouser_delete]
(
@id_info int
)
AS
	DELETE from [dbo].[InfoUser]
	where id_Info=@id_info;
go

CREATE PROCEDURE [DBO].[InfoUser_add]
(
@Fam nvarchar(max),
@Imya nvarchar(max),
@Otch nvarchar(max),
@Study nvarchar(30),
@Work nvarchar(max),
@Year nvarchar(1),
@Old int,
@User_id int
)
AS
	insert into [dbo].[InfoUser]([Fam],[Imya],[Otch],[Study],[Work],[YearUser],[Old],[users_id])
		   values((@Fam),(@Imya),(@Otch),(@Study),(@Work),(@Year),(@Old),(@User_id));
go

CREATE PROCEDURE [DBO].[InfoUser_update]
(
@id_info int,
@Fam nvarchar(max),
@Imya nvarchar(max),
@Otch nvarchar(max),
@Study nvarchar(30),
@Work nvarchar(max),
@Year nvarchar(1),
@Old int,
@User_id int
)
AS
	update [dbo].InfoUser
	set
	Fam=@Fam,
	Imya=@Imya,
	Otch=@Otch,
	Study=@Study,
	Work=@Work,
	YearUser=@Year,
	Old=@Old,
	users_id=@User_id
	where id_Info=@id_info
go

CREATE PROCEDURE [DBO].[katamnez_delete]
(
@id_katamnez int
)
AS
	DELETE from [dbo].[katamnez]
	where id_katamnez=@id_katamnez;
go

CREATE PROCEDURE [DBO].[katamnez_add]
(
@katamneztext nvarchar(max),
@zadacha_id int
)
AS
	insert into [dbo].[katamnez]([katamneztext],[zadacha_id]) values((@katamneztext),(@zadacha_id));
go

CREATE PROCEDURE [DBO].[katamnez_update]
(
@id_katamnez int,
@katamneztext nvarchar(max),
@zadacha_id int
)
AS
	update [dbo].katamnez
	set
	katamneztext=@katamneztext,
	zadacha_id=@zadacha_id
	where id_katamnez=@id_katamnez
go

CREATE PROCEDURE [DBO].[meropr_delete]
(
@id_meropr int
)
AS
	DELETE from [dbo].[meropr]
	where id_meropr=@id_meropr;
go

CREATE PROCEDURE [DBO].[meropr_add]
(
@meroprtext nvarchar(max),
@zadacha_id int
)
AS
	insert into [dbo].[meropr]([meroprtext],[zadacha_id]) values((@meroprtext),(@zadacha_id));
go

CREATE PROCEDURE [DBO].[meropr_update]
(
@id_meropr int,
@meroprtext nvarchar(max),
@zadacha_id int
)
AS
	update [dbo].meropr
	set
	meroprtext=@meroprtext,
	zadacha_id=@zadacha_id
	where id_meropr=@id_meropr
go

CREATE PROCEDURE [DBO].[Lastotv_delete]
(
@id_Last_otv int
)
AS
	DELETE from [dbo].[Lastotv]
	where id_Last_otv=@id_Last_otv;
go

CREATE PROCEDURE [DBO].[Lastotv_add]
(
@name_otv nvarchar(max),
@Form_otv nvarchar(10),
@User_id int
)
AS
	insert into [dbo].[Lastotv]([name_otv],[Form_otv],[users_id]) values((@name_otv),(@Form_otv),(@User_id));
go

CREATE PROCEDURE [DBO].[Lastotv_update]
(
@id_Last_otv int,
@name_otv nvarchar(max),
@Form_otv nvarchar(10),
@User_id int
)
AS
	update [dbo].Lastotv
	set
	name_otv=@name_otv,
	Form_otv=@Form_otv,
	users_id=@User_id
	where id_Last_otv=@id_Last_otv
go


CREATE PROCEDURE [DBO].[Resh_delete]
(
@id_resh int
)
AS
	DELETE from [dbo].[resh]
	where id_resh=@id_resh;
go

CREATE PROCEDURE [DBO].[resh_add]
(
@Users_id int,
@Zadacha_id int
)
AS
	insert into [dbo].[resh]([users_id],[zadacha_id]) values((@Users_id),(@Zadacha_id));
go

CREATE PROCEDURE [DBO].[Resh_update]
(
@id_resh int,
@Users_id int,
@Zadacha_id int
)
AS
	update [dbo].resh
	set
	users_id=@Users_id,
	zadacha_id=@Zadacha_id
	where id_resh=@id_resh
go

CREATE PROCEDURE [DBO].[Users_delete]
(
@id_user int
)
AS
	DELETE from [dbo].[users]
	where id_user=@id_user;
go

CREATE PROCEDURE [DBO].[users_add]
(
@User_Login nvarchar(max),
@User_Password nvarchar(max),
@User_Mail nvarchar(max),
@Teacher_id int
)
AS
	insert into [dbo].[users]([User_Login],[User_Password],[User_Mail],[Teacher_id])
		   values((@User_Login),(@User_Password),(@User_Mail),(@Teacher_id));
go

CREATE PROCEDURE [DBO].[users_update]
(
@id_user int,
@User_Login nvarchar(max),
@User_Password nvarchar(max),
@User_Mail nvarchar(max),
@Teacher_id int
)
AS
	update [dbo].users
	set
	User_Login=@User_Login,
	User_Password=@User_Password,
	User_Mail=@User_Mail,
	Teacher_id=@Teacher_id
	where id_user=@id_user
go

CREATE PROCEDURE [DBO].[vernotv_delete]
(
@id_vernotv int
)
AS
	DELETE from [dbo].[vernotv]
	where id_vernotv=@id_vernotv;
go

CREATE PROCEDURE [DBO].[vernotv_add]
(
@otv nvarchar(max),
@FormVernOtv nvarchar(10),
@zadacha_id int
)
AS
	insert into [dbo].[vernotv]([otv],[FormVernOtv],[zadacha_id]) values((@otv),(@FormVernOtv),(@Zadacha_id));
go

CREATE PROCEDURE [DBO].[vernotv_update]
(
@id_vernotv int,
@otv nvarchar(max),
@FormVernOtv nvarchar(10),
@zadacha_id int
)
AS
	update [dbo].vernotv
	set
	otv=@otv,
	FormVernOtv=@FormVernOtv,
	zadacha_id=@zadacha_id
	where id_vernotv=@id_vernotv
go

CREATE PROCEDURE [DBO].[Zadacha_delete]
(
@id_zadacha int
)
AS
	DELETE from [dbo].[Zadacha]
	where id_zadacha=@id_zadacha;
go

CREATE PROCEDURE [DBO].[Zadacha_add]
(
@Zapros nvarchar(max),
@sved nvarchar(max)
)
AS
	insert into [dbo].[Zadacha]([Zapros],[sved]) values((@Zapros),(@sved));
go

CREATE PROCEDURE [DBO].[Zadacha_update]
(
@id_zadacha int,
@Zapros nvarchar(max),
@sved nvarchar(max)
)
AS
	update [dbo].Zadacha
	set
	Zapros=@Zapros,
	sved=@sved
	where id_zadacha=@id_zadacha
go

CREATE PROCEDURE [DBO].[Dostup_delete]
(
@id_Dostup int
)
AS
	DELETE from [dbo].[Dostup]
	where id_Dostup=@id_Dostup;
go

CREATE PROCEDURE [DBO].[Dostup_add]
(
@UpdateZadach int,
@UpdateUsers int,
@WorkZadach int
)
AS
	insert into [dbo].[Dostup]([UpdateZadach],[UpdateUsers],[WorkZadach]) values((@UpdateZadach),(@UpdateUsers),(@WorkZadach));
go

CREATE PROCEDURE [DBO].[Dostup_update]
(
@id_Dostup int,
@UpdateZadach int,
@UpdateUsers int,
@WorkZadach int
)
AS
	update [dbo].Dostup
	set
	UpdateZadach=@UpdateZadach,
	UpdateUsers=@UpdateUsers,
	WorkZadach=@WorkZadach
	where id_Dostup=@id_Dostup
go

CREATE PROCEDURE [DBO].[Role_delete]
(
@id_role int
)
AS
	DELETE from [dbo].[Role]
	where id_role=@id_role;
go

CREATE PROCEDURE [DBO].[Role_add]
(
@Naim nvarchar(10),
@users_id int,
@Dostup_id int
)
AS
	insert into [dbo].[Role]([Naim],[users_id],[Dostup_id]) values((@Naim),(@users_id),(@Dostup_id));
go

CREATE PROCEDURE [DBO].[Role_update]
(
@id_role int,
@Naim nvarchar(10),
@users_id int,
@Dostup_id int
)
AS
	update [dbo].[Role]
	set
	naim=@Naim,
	users_id=@users_id,
	Dostup_id=@Dostup_id
	where id_role=@id_role
go

CREATE PROCEDURE [DBO].[Teachers_delete]
(
@id_teacher int
)
AS
	DELETE from [dbo].[Teachers]
	where id_teacher=@id_teacher;
go

CREATE PROCEDURE [DBO].[Teachers_add]
(
@Unique_Naim nvarchar(30),
@User_End_Data nvarchar(10),
@KolvoNeRegStudents int
)
AS
	insert into [dbo].[Teachers]([Unique_Naim],[User_End_Data],[KolvoNeRegStudents]) values((@Unique_Naim),(@User_End_Data),(@KolvoNeRegStudents));
go

CREATE PROCEDURE [DBO].[Teachers_update]
(
@id_teacher int,
@Unique_Naim nvarchar(30),
@User_End_Data nvarchar(10),
@KolvoNeRegStudents int
)
AS
	update [dbo].[Teachers]
	set
	Unique_Naim=@Unique_Naim,
	User_End_Data=@User_End_Data,
	KolvoNeRegStudents=@KolvoNeRegStudents
	where id_teacher=@id_teacher
go