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
@lb_small nvarchar(max),
@lb nvarchar(max),
@lbtext nvarchar(max),
@lb_image nvarchar(max),
@lb_image2 nvarchar(max)
)
AS
	insert into [dbo].[dpo]([lb_small],[lb],[lbtext],[lb_image],[lb_image2]) 
		   values((@lb_small),(@lb),(@lbtext),(@lb_image),(@lb_image2));
go

CREATE PROCEDURE [DBO].[dpo_update]
(
@dpo_id int,
@lb_small nvarchar(max),
@lb nvarchar(max),
@lbtext nvarchar(max),
@lb_image nvarchar(max),
@lb_image2 nvarchar(max)
)
AS
	update [dbo].dpo
	set
	lb_small=@lb_small,
	lb=@lb,
	lbtext=@lbtext,
	lb_image=@lb_image,
	lb_image2=@lb_image2
	where id_dpo=@dpo_id
go

CREATE PROCEDURE [DBO].[DpoSelected_delete]
(
@id_dposelected int
)
AS
	DELETE from [dbo].[DpoSelected]
	where id_dposelected=@id_dposelected;
go

CREATE PROCEDURE [DBO].[DpoSelected_add]
(
@InfoSelected nvarchar(max),
@users_id int
)
AS
	insert into [dbo].[DpoSelected]([InfoSelected],[users_id]) 
		   values((@InfoSelected),(@users_id));
go

CREATE PROCEDURE [DBO].[DpoSelected_update]
(
@id_dposelected int,
@InfoSelected nvarchar(max),
@users_id int
)
AS
	update [dbo].DpoSelected
	set
	InfoSelected=@InfoSelected,
	users_id=@users_id
	where id_dposelected=@id_dposelected
go

CREATE PROCEDURE [DBO].[dz_delete]
(
@id_dz int
)
AS
	DELETE from [dbo].[dz]
	where id_dz=@id_dz;
go

CREATE PROCEDURE [DBO].[dz_add]
(
@cb nvarchar(max),
@zadacha_id int
)
AS
	insert into [dbo].[dz]([CB],[zadacha_id]) values((@cb),(@zadacha_id));
go

CREATE PROCEDURE [DBO].[dz_update]
(
@id_dz int,
@cb nvarchar(max),
@zadacha_id int
)
AS
	update [dbo].dz
	set
	CB=@cb,
	zadacha_id=@zadacha_id
	where id_dz=@id_dz
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
@rb nvarchar(max),
@rbtext nvarchar(max),
@zadacha_id int
)
AS
	insert into [dbo].[Fenom1]([RB],[RBText],[zadacha_id]) values((@rb),(@rbtext),(@zadacha_id));
go

CREATE PROCEDURE [DBO].[Fenom1_update]
(
@id_fenom1 int,
@rb nvarchar(max),
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

CREATE PROCEDURE [DBO].[Fenom2_delete]
(
@id_fenom2 int
)
AS
	DELETE from [dbo].[Fenom2]
	where id_Fenom2=@id_fenom2;
go

CREATE PROCEDURE [DBO].[Fenom2_add]
(
@cb nvarchar(max),
@zadacha_id int
)
AS
	insert into [dbo].[Fenom2]([CB],[zadacha_id]) values((@cb),(@zadacha_id));
go

CREATE PROCEDURE [DBO].[Fenom2_update]
(
@id_fenom2 int,
@cb nvarchar(max),
@zadacha_id int
)
AS
	update [dbo].Fenom2
	set
	CB=@cb,
	zadacha_id=@zadacha_id
	where id_Fenom2=@id_fenom2
go

CREATE PROCEDURE [DBO].[FenomSelected_delete]
(
@id_fenomselected int
)
AS
	DELETE from [dbo].[FenomSelected]
	where id_fenomselected=@id_fenomselected;
go

CREATE PROCEDURE [DBO].[fenomSelected_add]
(
@InfoSelected nvarchar(max),
@users_id int
)
AS
	insert into [dbo].[FenomSelected]([InfoSelected],[users_id]) 
		   values((@InfoSelected),(@users_id));
go

CREATE PROCEDURE [DBO].[fenomSelected_update]
(
@id_fenomselected int,
@InfoSelected nvarchar(max),
@users_id int
)
AS
	update [dbo].FenomSelected
	set
	InfoSelected=@InfoSelected,
	users_id=@users_id
	where id_fenomselected=@id_fenomselected
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
@FIO nvarchar(max),
@Study nvarchar(max),
@Work nvarchar(max),
@Year nvarchar(max),
@Old nvarchar(max),
@User_id int
)
AS
	insert into [dbo].[InfoUser]([FIO],[Study],[Work],[YearUser],[Old],[users_id])
		   values((@FIO),(@Study),(@Work),(@Year),(@Old),(@User_id));
go

CREATE PROCEDURE [DBO].[InfoUser_update]
(
@id_info int,
@FIO nvarchar(max),
@Study nvarchar(max),
@Work nvarchar(max),
@Year nvarchar(max),
@Old nvarchar(max),
@User_id int
)
AS
	update [dbo].InfoUser
	set
	FIO=@FIO,
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

CREATE PROCEDURE [DBO].[otvDiag_delete]
(
@id_otvDiag int
)
AS
	DELETE from [dbo].[otvDiag]
	where id_otvDiag=@id_otvDiag;
go

CREATE PROCEDURE [DBO].[otvDiag_add]
(
@name_otv nvarchar(max),
@User_id int
)
AS
	insert into [dbo].[otvDiag]([name_otv],[users_id]) values((@name_otv),(@User_id));
go

CREATE PROCEDURE [DBO].[otvDiag_update]
(
@id_otvDiag int,
@name_otv nvarchar(max),
@User_id int
)
AS
	update [dbo].otvDiag
	set
	name_otv=@name_otv,
	users_id=@User_id
	where id_otvDiag=@id_otvDiag
go

CREATE PROCEDURE [DBO].[otvFenom_delete]
(
@id_otvFenom int
)
AS
	DELETE from [dbo].[otvFenom]
	where id_otvFenom=@id_otvFenom;
go

CREATE PROCEDURE [DBO].[otvFenom_add]
(
@name_otv nvarchar(max),
@User_id int
)
AS
	insert into [dbo].[otvFenom]([name_otv],[users_id]) values((@name_otv),(@User_id));
go

CREATE PROCEDURE [DBO].[otvFenom_update]
(
@id_otvFenom int,
@name_otv nvarchar(max),
@User_id int
)
AS
	update [dbo].otvFenom
	set
	name_otv=@name_otv,
	users_id=@User_id
	where id_otvFenom=@id_otvFenom
go

CREATE PROCEDURE [DBO].[otvGip_delete]
(
@id_otvGip int
)
AS
	DELETE from [dbo].[otvGip]
	where id_otvGip=@id_otvGip;
go

CREATE PROCEDURE [DBO].[otvGip_add]
(
@name_otv nvarchar(max),
@User_id int
)
AS
	insert into [dbo].[otvGip]([name_otv],[users_id]) values((@name_otv),(@User_id));
go

CREATE PROCEDURE [DBO].[otvGip_update]
(
@id_otvGip int,
@name_otv nvarchar(max),
@User_id int
)
AS
	update [dbo].otvGip
	set
	name_otv=@name_otv,
	users_id=@User_id
	where id_otvGip=@id_otvGip
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

CREATE PROCEDURE [DBO].[Students_delete]
(
@id_students int
)
AS
	DELETE from [dbo].[Students]
	where id_students=@id_students;
go

CREATE PROCEDURE [DBO].[Students_add]
(
@Student_Login nvarchar(max),
@Student_Password nvarchar(max),
@Student_Mail nvarchar(max),
@users_id int
)
AS
	insert into [dbo].[Students]([Student_login],[Student_Password],[Student_Mail],[users_id]) 
		   values((@Student_Login),(@Student_Password),(@Student_Mail),(@users_id));
go

CREATE PROCEDURE [DBO].[Students_update]
(
@id_students int,
@Student_Login nvarchar(max),
@Student_Password nvarchar(max),
@Student_Mail nvarchar(max),
@users_id int
)
AS
	update [dbo].Students
	set
	Student_login=@Student_Login,
	Student_Password=@Student_Password,
	Student_Mail=@Student_Mail,
	users_id=@users_id
	where id_students=@id_students
go

CREATE PROCEDURE [DBO].[teor_delete]
(
@id_teor int
)
AS
	DELETE from [dbo].[teor]
	where id_teor=@id_teor;
go

CREATE PROCEDURE [DBO].[teor_add]
(
@cb nvarchar(max),
@Zadacha_id int
)
AS
	insert into [dbo].[teor]([CB],[zadacha_id]) values((@cb),(@Zadacha_id));
go

CREATE PROCEDURE [DBO].[teor_update]
(
@id_teor int,
@cb nvarchar(max),
@Zadacha_id int
)
AS
	update [dbo].teor
	set
	CB=@cb,
	zadacha_id=@Zadacha_id
	where id_teor=@id_teor
go

CREATE PROCEDURE [DBO].[TeorSelected_delete]
(
@id_teorselected int
)
AS
	DELETE from [dbo].[TeorSelected]
	where id_teorselected=@id_teorselected;
go

CREATE PROCEDURE [DBO].[teorSelected_add]
(
@InfoSelected nvarchar(max),
@users_id int
)
AS
	insert into [dbo].[TeorSelected]([InfoSelected],[users_id]) 
		   values((@InfoSelected),(@users_id));
go

CREATE PROCEDURE [DBO].[teorSelected_update]
(
@id_teorselected int,
@InfoSelected nvarchar(max),
@users_id int
)
AS
	update [dbo].TeorSelected
	set
	InfoSelected=@InfoSelected,
	users_id=@users_id
	where id_teorselected=@id_teorselected
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
@Kolvo_students int,
@isadmin int
)
AS
	insert into [dbo].[users]([User_Login],[User_Password],[User_Mail],[Kolvo_students],[isadmin])
		   values((@User_Login),(@User_Password),(@User_Mail),(@Kolvo_students),(@isadmin));
go

CREATE PROCEDURE [DBO].[users_update]
(
@id_user int,
@User_Login nvarchar(max),
@User_Password nvarchar(max),
@User_Mail nvarchar(max),
@Kolvo_students int,
@isadmin int
)
AS
	update [dbo].users
	set
	User_Login=@User_Login,
	User_Password=@User_Password,
	User_Mail=@User_Mail,
	Kolvo_students=@Kolvo_students,
	isAdmin=@isadmin
	where id_user=@id_user
go

CREATE PROCEDURE [DBO].[vernotvDiag_delete]
(
@id_vernotv int
)
AS
	DELETE from [dbo].[vernotv_Diag]
	where id_vernotv=@id_vernotv;
go

CREATE PROCEDURE [DBO].[vernotvDiag_add]
(
@otv nvarchar(max),
@zadacha_id int
)
AS
	insert into [dbo].[vernotv_Diag]([otv],[zadacha_id]) values((@otv),(@Zadacha_id));
go

CREATE PROCEDURE [DBO].[vernotvDiag_update]
(
@id_vernotv int,
@otv nvarchar(max),
@zadacha_id int
)
AS
	update [dbo].vernotv_Diag
	set
	otv=@otv,
	zadacha_id=@zadacha_id
	where id_vernotv=@id_vernotv
go

CREATE PROCEDURE [DBO].[vernotvFenom_delete]
(
@id_vernotv int
)
AS
	DELETE from [dbo].[vernotv_Fenom]
	where id_vernotv=@id_vernotv;
go

CREATE PROCEDURE [DBO].[vernotvFenom_add]
(
@otv nvarchar(max),
@zadacha_id int
)
AS
	insert into [dbo].[vernotv_Fenom]([otv],[zadacha_id]) values((@otv),(@Zadacha_id));
go

CREATE PROCEDURE [DBO].[vernotvFenom_update]
(
@id_vernotv int,
@otv nvarchar(max),
@zadacha_id int
)
AS
	update [dbo].vernotv_Fenom
	set
	otv=@otv,
	zadacha_id=@zadacha_id
	where id_vernotv=@id_vernotv
go

CREATE PROCEDURE [DBO].[vernotvGip_delete]
(
@id_vernotv int
)
AS
	DELETE from [dbo].[vernotv_Gip]
	where id_vernotv=@id_vernotv;
go

CREATE PROCEDURE [DBO].[vernotvGip_add]
(
@otv nvarchar(max),
@zadacha_id int
)
AS
	insert into [dbo].[vernotv_Gip]([otv],[zadacha_id]) values((@otv),(@Zadacha_id));
go

CREATE PROCEDURE [DBO].[vernotvGip_update]
(
@id_vernotv int,
@otv nvarchar(max),
@zadacha_id int
)
AS
	update [dbo].vernotv_Gip
	set
	otv=@otv,
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