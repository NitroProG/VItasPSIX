USE [psico]
GO

create trigger DeleteRole on dbo.[Role]
for delete
as
declare @id_role int
select @id_role = deleted.id_role from dbo.[Role]
deleted 
if (select count (id_role) from dbo.[Role])<1
begin  
rollback tran
raiserror('Нельзя удалить единственного пользователя из Базы данных',16,1) 
end
go

create trigger DeleteUsers on Users
for delete
as
declare @id_user int
select @id_user = deleted.id_user from users
deleted 
if (select count (id_user) from users)<1
begin  
rollback tran
raiserror('Нельзя удалить единственного пользователя из Базы данных',16,1) 
end
go

create trigger DeleteDefender on defender
for delete
as
declare @id_defend int
select @id_defend = deleted.id_defend from defender
deleted 
if (select count (id_defend) from defender)<1
begin  
rollback tran
raiserror('Нельзя удалить ключ активации из Базы данных',16,1) 
end
go