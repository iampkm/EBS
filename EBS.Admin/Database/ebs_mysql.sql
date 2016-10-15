/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2016-10-11 13:53:53                          */
/*==============================================================*/


drop index idx_account_username on Account;

drop table if exists Account;

drop table if exists AccountLoginHistory;

drop table if exists Menu;

drop table if exists Role;

drop table if exists RoleMenu;

/*==============================================================*/
/* Table: Account                                               */
/*==============================================================*/
create table Account
(
   Id                   int not null auto_increment comment '编号',
   UserName             nvarchar(64) comment '账户名',
   Password             nvarchar(64) comment '密码',
   NickName             nvarchar(64) comment '昵称',
   RoleId               int comment '角色ID',
   CreatedOn            datetime comment '创建时间',
   Status               int comment '状态',
   LoginErrorCount      int comment '登录错误次数',
   LastUpdateDate       datetime comment '最后修改时间',
   primary key (Id)
);

alter table Account comment '后台管理账户表';

/*==============================================================*/
/* Index: idx_account_username                                  */
/*==============================================================*/
create unique index idx_account_username on Account
(
   UserName
);

/*==============================================================*/
/* Table: AccountLoginHistory                                   */
/*==============================================================*/
create table AccountLoginHistory
(
   Id                   int not null auto_increment comment '编号',
   AccountId            int comment '账号id',
   UserName             nvarchar(64) comment '登录账号',
   CreatedOn            datetime comment '登录时间',
   IPAddress            nvarchar(64) comment 'IP地址',
   LoginStatus          int comment '登录状态',
   primary key (Id)
);

alter table AccountLoginHistory comment '账号登录历史';

/*==============================================================*/
/* Table: Menu                                                  */
/*==============================================================*/
create table Menu
(
   Id                   int not null auto_increment comment '编号',
   ParentId             int comment '父编号',
   Name                 nvarchar(64) comment '名称',
   Url                  nvarchar(256) comment '连接',
   Icon                 nvarchar(64) comment '图标',
   DisplayOrder         int comment '显示顺序',
   UrlType              int comment '连接类型',
   primary key (Id)
);

alter table Menu comment '系统菜单';

/*==============================================================*/
/* Table: Role                                                  */
/*==============================================================*/
create table Role
(
   Id                   int not null auto_increment comment '编号',
   Name                 nvarchar(64) comment '角色名称',
   Description          nvarchar(1024) comment '描述',
   primary key (Id)
);

alter table Role comment '账户角色表';

/*==============================================================*/
/* Table: RoleMenu                                              */
/*==============================================================*/
create table RoleMenu
(
   Id                   int not null auto_increment comment '编号',
   RoleId               int comment '角色编号',
   MenuId               int comment '菜单编号',
   primary key (Id)
);

alter table RoleMenu comment '角色菜单对应表';

