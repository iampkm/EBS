/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2017-06-16 15:19:27                          */
/*==============================================================*/


drop table if exists Pay_App;

drop table if exists Pay_NotifyBack;

drop index IX_Pay_Request_OrderId_PayType on Pay_Request;

drop table if exists Pay_Request;

drop table if exists Pay_Result;

/*==============================================================*/
/* Table: Pay_App                                               */
/*==============================================================*/
create table Pay_App
(
   SysNo                int not null auto_increment,
   AppId                varchar(64),
   AppSecret            varchar(64),
   AppName              varchar(1024),
   Status               int,
   CreateTime           datetime,
   primary key (SysNo)
);

/*==============================================================*/
/* Table: Pay_NotifyBack                                        */
/*==============================================================*/
create table Pay_NotifyBack
(
   SysNo                int not null auto_increment,
   ResultSysNo          int,
   Msg                  varchar(1024),
   Status               int,
   CreateTime           datetime,
   RequestData          varchar(2048),
   primary key (SysNo)
);

/*==============================================================*/
/* Table: Pay_Request                                           */
/*==============================================================*/
create table Pay_Request
(
   SysNo                int not null auto_increment,
   OrderId              varchar(256),
   PaymentAmt           decimal(19,2),
   PayType              int,
   NotifyUrl            varchar(1024),
   ReturnUrl            varchar(1024),
   RequestData          varchar(2048),
   ExecuteResult        int,
   ResultDesc           varchar(1024),
   AppId                int,
   Status               int,
   CreateTime           datetime,
   primary key (SysNo)
);

alter table Pay_Request comment '÷ß∏∂«Î«Û';

/*==============================================================*/
/* Index: IX_Pay_Request_OrderId_PayType                        */
/*==============================================================*/
create index IX_Pay_Request_OrderId_PayType on Pay_Request
(
   OrderId,
   PayType
);

/*==============================================================*/
/* Table: Pay_Result                                            */
/*==============================================================*/
create table Pay_Result
(
   SysNo                int not null auto_increment,
   RequestSysNo         int,
   OrderId              varchar(256),
   TradeNo              varchar(256),
   PaymentAmt           decimal(19,2),
   PayType              int,
   RequestData          varchar(2048),
   ExecuteResult        int,
   ResultDesc           varchar(1024),
   NotifyStatus         int,
   CreateTime           datetime,
   ExtTradeNo           varchar(256),
   primary key (SysNo)
);

