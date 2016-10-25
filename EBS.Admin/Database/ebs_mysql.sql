/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2016-10-25 14:12:34                          */
/*==============================================================*/


drop index idx_account_username on Account;

drop table if exists Account;

drop table if exists AccountLoginHistory;

drop table if exists Brand;

drop table if exists Category;

drop table if exists Inventory;

drop table if exists InventoryHistory;

drop table if exists Menu;

drop index idx_pcodeseq_guidcode on ProductCodeSequence;

drop table if exists ProductCodeSequence;

drop index idx_productSKU_code on ProductSKU;

drop table if exists ProductSKU;

drop table if exists Role;

drop table if exists RoleMenu;

drop table if exists Store;

drop table if exists StoreInventory;

drop table if exists StoreInventoryHistory;

drop table if exists Warehouse;

/*==============================================================*/
/* Table: Account                                               */
/*==============================================================*/
create table Account
(
   Id                   int not null auto_increment comment '���',
   UserName             nvarchar(64) comment '�˻���',
   Password             nvarchar(64) comment '����',
   NickName             nvarchar(64) comment '�ǳ�',
   RoleId               int comment '��ɫID',
   CreatedOn            datetime comment '����ʱ��',
   Status               int comment '״̬',
   LoginErrorCount      int comment '��¼�������',
   LastUpdateDate       datetime comment '����޸�ʱ��',
   primary key (Id)
);

alter table Account comment '��̨�����˻���';

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
   Id                   int not null auto_increment comment '���',
   AccountId            int comment '�˺�id',
   UserName             nvarchar(64) comment '��¼�˺�',
   CreatedOn            datetime comment '��¼ʱ��',
   IPAddress            nvarchar(64) comment 'IP��ַ',
   LoginStatus          int comment '��¼״̬',
   primary key (Id)
);

alter table AccountLoginHistory comment '�˺ŵ�¼��ʷ';

/*==============================================================*/
/* Table: Brand                                                 */
/*==============================================================*/
create table Brand
(
   Id                   int not null auto_increment comment '���',
   Name                 nvarchar(128) comment '����',
   primary key (Id)
);

alter table Brand comment 'Ʒ��';

/*==============================================================*/
/* Table: Category                                              */
/*==============================================================*/
create table Category
(
   Id                   nvarchar(18) not null comment '���',
   Name                 nvarchar(64) comment '������',
   FullName             nvarchar(256) comment 'ȫ��',
   Level                int comment '�㼶',
   primary key (Id)
);

alter table Category comment '��Ʒ����';

/*==============================================================*/
/* Table: Inventory                                             */
/*==============================================================*/
create table Inventory
(
   Id                   int not null auto_increment comment '���',
   ProductSKUId         int comment 'SKU����',
   WarehouseId          int comment '�ֿ����',
   Quantity             int comment 'ʵ�ʿ����',
   WarnQuantity         int comment '������',
   IsQuit               bool comment '�Ƿ��˳�',
   primary key (Id)
);

/*==============================================================*/
/* Table: InventoryHistory                                      */
/*==============================================================*/
create table InventoryHistory
(
   Id                   int not null auto_increment comment '���',
   ProductSKUId         int comment 'SKU����',
   WarehouseId          int comment '�ֿ����',
   Quantity             int comment 'ʵ�ʿ����',
   ChangeQuantity       int comment '�䶯��',
   CreatedOn            datetime comment '����ʱ��',
   BillId               int comment '����ϵͳ��',
   BillCode             varchar(20) comment '���ݱ���',
   primary key (Id)
);

alter table InventoryHistory comment '�����ʷ��¼';

/*==============================================================*/
/* Table: Menu                                                  */
/*==============================================================*/
create table Menu
(
   Id                   int not null auto_increment comment '���',
   ParentId             int comment '�����',
   Name                 nvarchar(64) comment '����',
   Url                  nvarchar(256) comment '����',
   Icon                 nvarchar(64) comment 'ͼ��',
   DisplayOrder         int comment '��ʾ˳��',
   UrlType              int comment '��������',
   primary key (Id)
);

alter table Menu comment 'ϵͳ�˵�';

/*==============================================================*/
/* Table: ProductCodeSequence                                   */
/*==============================================================*/
create table ProductCodeSequence
(
   Id                   int not null auto_increment,
   GuidCode             nvarchar(32) comment 'guid����',
   primary key (Id)
);

alter table ProductCodeSequence comment '��Ʒ�������кţ��˱�����������ƷSKU ���е� Code �ֶ�';

/*==============================================================*/
/* Index: idx_pcodeseq_guidcode                                 */
/*==============================================================*/
create unique index idx_pcodeseq_guidcode on ProductCodeSequence
(
   GuidCode
);

/*==============================================================*/
/* Table: ProductSKU                                            */
/*==============================================================*/
create table ProductSKU
(
   Id                   int not null auto_increment comment '���',
   Code                 nvarchar(20) comment '����',
   Name                 nvarchar(50) comment '��Ʒ��',
   ShowName             nvarchar(500) comment '��ʾ����',
   SellingPoint         nvarchar(100) comment '����',
   CategoryId           nvarchar(18) comment '����Id',
   BrandId              int comment 'Ʒ��Id',
   InputRate            decimal comment '����˰��',
   OutRate              decimal comment '����˰��',
   IsGift               bool comment '�Ƿ���Ʒ',
   Length               decimal comment '��',
   Width                decimal comment '��',
   Height               decimal comment '��',
   Weight               decimal comment '����',
   Unit                 nvarchar(10) comment '��λ',
   Description          text comment '��������',
   Keywords             nvarchar(200) comment '�ؼ���',
   IsPublish            bool comment '�Ƿ��ϼ�',
   BarCode              nvarchar(50) comment '����',
   Specification        nvarchar(200) comment '�����',
   MarketPrice          decimal(8,2) comment '�г���',
   SalePrice            decimal(8,2) comment '���ۼ�',
   WholeSalePrice       decimal(8,2) comment '������',
   CostPrice            decimal(8,2) comment 'ƽ���ɱ���',
   SubSkuCode           varchar(20) comment '��SKU����',
   SubSkuQuantity       int comment '��SKU����',
   CreatedOn            datetime comment '����ʱ��',
   primary key (Id)
);

alter table ProductSKU comment '��ƷSKU';

/*==============================================================*/
/* Index: idx_productSKU_code                                   */
/*==============================================================*/
create unique index idx_productSKU_code on ProductSKU
(
   Code
);

/*==============================================================*/
/* Table: Role                                                  */
/*==============================================================*/
create table Role
(
   Id                   int not null auto_increment comment '���',
   Name                 nvarchar(64) comment '��ɫ����',
   Description          nvarchar(1024) comment '����',
   primary key (Id)
);

alter table Role comment '�˻���ɫ��';

/*==============================================================*/
/* Table: RoleMenu                                              */
/*==============================================================*/
create table RoleMenu
(
   Id                   int not null auto_increment comment '���',
   RoleId               int comment '��ɫ���',
   MenuId               int comment '�˵����',
   primary key (Id)
);

alter table RoleMenu comment '��ɫ�˵���Ӧ��';

/*==============================================================*/
/* Table: Store                                                 */
/*==============================================================*/
create table Store
(
   Id                   int not null auto_increment comment '���',
   Name                 nvarchar(128) comment '�ŵ���',
   SourceKey            nvarchar(32) comment '�ŵ�Ψһ��',
   CreatedOn            datetime comment '����ʱ��',
   CreatedBy            int comment '������',
   Address              nvarchar(512) comment '��ַ',
   Contact              nvarchar(32) comment '��ϵ��',
   Phone                nvarchar(32) comment '��ϵ�绰',
   Setting              text comment '����',
   primary key (Id)
);

alter table Store comment '�ŵ�';

/*==============================================================*/
/* Table: StoreInventory                                        */
/*==============================================================*/
create table StoreInventory
(
   Id                   int not null comment '���',
   ProductSKUId         int comment 'SKU����',
   StoreId              int comment '�ŵ����',
   SaleQuantity         int comment '���ۿ��',
   OrderQuantity        int comment '�������',
   Quantity             int comment 'ʵ�ʿ����',
   WarnQuantity         int comment '������',
   IsQuit               bool comment '�Ƿ��˳�',
   primary key (Id)
);

alter table StoreInventory comment '�ŵ���';

/*==============================================================*/
/* Table: StoreInventoryHistory                                 */
/*==============================================================*/
create table StoreInventoryHistory
(
   Id                   int not null comment '���',
   ProductSKUId         int comment 'SKU����',
   StoreId              int comment '�ֿ����',
   Quantity             int comment 'ʵ�ʿ����',
   ChangeQuantity       int comment '�䶯��',
   CreatedOn            datetime comment '����ʱ��',
   CreatedBy            int comment '������',
   BillId               int comment '����ϵͳ��',
   BillCode             varchar(20) comment '���ݱ���'
);

alter table StoreInventoryHistory comment '�ŵ�����ʷ��¼';

/*==============================================================*/
/* Table: Warehouse                                             */
/*==============================================================*/
create table Warehouse
(
   Id                   int not null auto_increment,
   Code                 nvarchar(20) comment '����',
   Name                 nvarchar(50) comment '�ֿ���',
   Region               nvarchar(50) comment '����',
   primary key (Id)
);

alter table Warehouse comment '�ֿ�';

