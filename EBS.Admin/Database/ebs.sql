/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2012                    */
/* Created on:     2016-09-02 09:39:01                          */
/*==============================================================*/


if exists (select 1
            from  sysobjects
           where  id = object_id('Account')
            and   type = 'U')
   drop table Account
go

if exists (select 1
            from  sysobjects
           where  id = object_id('AccountLoginHistory')
            and   type = 'U')
   drop table AccountLoginHistory
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Menu')
            and   type = 'U')
   drop table Menu
go

if exists (select 1
            from  sysobjects
           where  id = object_id('MenuOfRole')
            and   type = 'U')
   drop table MenuOfRole
go

if exists (select 1
            from  sysobjects
           where  id = object_id('Role')
            and   type = 'U')
   drop table Role
go

/*==============================================================*/
/* Table: Account                                               */
/*==============================================================*/
create table Account (
   Id                   int                  not null,
   UserName             nvarchar(64)         null,
   Password             nvarchar(64)         null,
   NickName             nvarchar(64)         null,
   CreatedOn            datetime             null,
   Status               int                  null,
   RoleId               int                  null,
   constraint PK_ACCOUNT primary key (Id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('Account') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty 'MS_Description',  
   'user', @CurrentUser, 'table', 'Account' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty 'MS_Description',  
   'ºóÌ¨¹ÜÀíÕË»§±í', 
   'user', @CurrentUser, 'table', 'Account'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Account')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Id')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Account', 'column', 'Id'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '±àºÅ',
   'user', @CurrentUser, 'table', 'Account', 'column', 'Id'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Account')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'UserName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Account', 'column', 'UserName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'ÕË»§Ãû',
   'user', @CurrentUser, 'table', 'Account', 'column', 'UserName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Account')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Password')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Account', 'column', 'Password'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'ÃÜÂë',
   'user', @CurrentUser, 'table', 'Account', 'column', 'Password'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Account')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'NickName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Account', 'column', 'NickName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'êÇ³Æ',
   'user', @CurrentUser, 'table', 'Account', 'column', 'NickName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Account')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreatedOn')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Account', 'column', 'CreatedOn'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '´´½¨Ê±¼ä',
   'user', @CurrentUser, 'table', 'Account', 'column', 'CreatedOn'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Account')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Status')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Account', 'column', 'Status'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '×´Ì¬',
   'user', @CurrentUser, 'table', 'Account', 'column', 'Status'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Account')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'RoleId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Account', 'column', 'RoleId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '½ÇÉ«±àºÅ',
   'user', @CurrentUser, 'table', 'Account', 'column', 'RoleId'
go

/*==============================================================*/
/* Table: AccountLoginHistory                                   */
/*==============================================================*/
create table AccountLoginHistory (
   Id                   int                  not null,
   AccountId            int                  null,
   UserName             nvarchar(64)         null,
   CreatedOn            datetime             null,
   IPAddress            nvarchar(64)         null,
   LoginStatus          nvarchar(32)         null,
   constraint PK_ACCOUNTLOGINHISTORY primary key (Id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('AccountLoginHistory') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty 'MS_Description',  
   'user', @CurrentUser, 'table', 'AccountLoginHistory' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty 'MS_Description',  
   'ÕËºÅµÇÂ¼ÀúÊ·', 
   'user', @CurrentUser, 'table', 'AccountLoginHistory'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('AccountLoginHistory')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Id')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'AccountLoginHistory', 'column', 'Id'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '±àºÅ',
   'user', @CurrentUser, 'table', 'AccountLoginHistory', 'column', 'Id'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('AccountLoginHistory')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'AccountId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'AccountLoginHistory', 'column', 'AccountId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'ÕËºÅid',
   'user', @CurrentUser, 'table', 'AccountLoginHistory', 'column', 'AccountId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('AccountLoginHistory')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'UserName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'AccountLoginHistory', 'column', 'UserName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'µÇÂ¼ÕËºÅ',
   'user', @CurrentUser, 'table', 'AccountLoginHistory', 'column', 'UserName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('AccountLoginHistory')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'CreatedOn')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'AccountLoginHistory', 'column', 'CreatedOn'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'µÇÂ¼Ê±¼ä',
   'user', @CurrentUser, 'table', 'AccountLoginHistory', 'column', 'CreatedOn'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('AccountLoginHistory')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'IPAddress')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'AccountLoginHistory', 'column', 'IPAddress'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'IPµØÖ·',
   'user', @CurrentUser, 'table', 'AccountLoginHistory', 'column', 'IPAddress'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('AccountLoginHistory')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'LoginStatus')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'AccountLoginHistory', 'column', 'LoginStatus'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'µÇÂ¼×´Ì¬',
   'user', @CurrentUser, 'table', 'AccountLoginHistory', 'column', 'LoginStatus'
go

/*==============================================================*/
/* Table: Menu                                                  */
/*==============================================================*/
create table Menu (
   Id                   int                  not null,
   ParentId             int                  null,
   Name                 nvarchar(64)         null,
   Url                  nvarchar(256)        null,
   Icon                 nvarchar(64)         null,
   DisplayOrder         int                  null,
   constraint PK_MENU primary key (Id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('Menu') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty 'MS_Description',  
   'user', @CurrentUser, 'table', 'Menu' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty 'MS_Description',  
   'ÏµÍ³²Ëµ¥', 
   'user', @CurrentUser, 'table', 'Menu'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menu')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Id')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menu', 'column', 'Id'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '±àºÅ',
   'user', @CurrentUser, 'table', 'Menu', 'column', 'Id'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menu')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ParentId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menu', 'column', 'ParentId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '¸¸±àºÅ',
   'user', @CurrentUser, 'table', 'Menu', 'column', 'ParentId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menu')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Name')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menu', 'column', 'Name'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Ãû³Æ',
   'user', @CurrentUser, 'table', 'Menu', 'column', 'Name'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menu')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Url')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menu', 'column', 'Url'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Á¬½Ó',
   'user', @CurrentUser, 'table', 'Menu', 'column', 'Url'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menu')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Icon')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menu', 'column', 'Icon'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'Í¼±ê',
   'user', @CurrentUser, 'table', 'Menu', 'column', 'Icon'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Menu')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'DisplayOrder')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Menu', 'column', 'DisplayOrder'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'ÏÔÊ¾Ë³Ðò',
   'user', @CurrentUser, 'table', 'Menu', 'column', 'DisplayOrder'
go

/*==============================================================*/
/* Table: MenuOfRole                                            */
/*==============================================================*/
create table MenuOfRole (
   Id                   int                  not null,
   RoleId               int                  null,
   MenuId               int                  null,
   constraint PK_MENUOFROLE primary key (Id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('MenuOfRole') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty 'MS_Description',  
   'user', @CurrentUser, 'table', 'MenuOfRole' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty 'MS_Description',  
   '½ÇÉ«²Ëµ¥¶ÔÓ¦±í', 
   'user', @CurrentUser, 'table', 'MenuOfRole'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('MenuOfRole')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Id')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'MenuOfRole', 'column', 'Id'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '±àºÅ',
   'user', @CurrentUser, 'table', 'MenuOfRole', 'column', 'Id'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('MenuOfRole')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'RoleId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'MenuOfRole', 'column', 'RoleId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '½ÇÉ«±àºÅ',
   'user', @CurrentUser, 'table', 'MenuOfRole', 'column', 'RoleId'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('MenuOfRole')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'MenuId')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'MenuOfRole', 'column', 'MenuId'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '²Ëµ¥±àºÅ',
   'user', @CurrentUser, 'table', 'MenuOfRole', 'column', 'MenuId'
go

/*==============================================================*/
/* Table: Role                                                  */
/*==============================================================*/
create table Role (
   Id                   int                  not null,
   Name                 nvarchar(64)         null,
   Description          nvarchar(1024)       null,
   constraint PK_ROLE primary key (Id)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('Role') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty 'MS_Description',  
   'user', @CurrentUser, 'table', 'Role' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty 'MS_Description',  
   'ÕË»§½ÇÉ«±í', 
   'user', @CurrentUser, 'table', 'Role'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Role')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Id')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Role', 'column', 'Id'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '±àºÅ',
   'user', @CurrentUser, 'table', 'Role', 'column', 'Id'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Role')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Name')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Role', 'column', 'Name'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '½ÇÉ«Ãû³Æ',
   'user', @CurrentUser, 'table', 'Role', 'column', 'Name'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('Role')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'Description')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'Role', 'column', 'Description'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   'ÃèÊö',
   'user', @CurrentUser, 'table', 'Role', 'column', 'Description'
go

