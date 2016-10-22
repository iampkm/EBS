/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2016/10/22 8:41:29                           */
/*==============================================================*/


drop index idx_account_username on Account;

drop table if exists Account;

drop table if exists AccountLoginHistory;

drop table if exists Brand;

drop table if exists Category;

drop table if exists Inventory;

drop table if exists InventoryHistory;

drop table if exists Menu;

drop table if exists Product;

drop index idx_productSKU_code on ProductSKU;

drop table if exists ProductSKU;

drop table if exists ProductSpecification;

drop table if exists ProductSpecificationMapping;

drop table if exists ProductSpecificationOption;

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
   Id                   int not null comment '���',
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
   SaleQuantity         int comment '���ۿ��',
   OrderQuantity        int comment '�������',
   ActualQuantity       int comment 'ʵ�ʿ����',
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
   ActualQuantity       int comment 'ʵ�ʿ����',
   ChangeQuantity       int comment '�䶯��',
   CreatedOn            datetime comment '����ʱ��',
   CreatedBy            int comment '������',
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
/* Table: Product                                               */
/*==============================================================*/
create table Product
(
   Id                   int not null auto_increment comment '���',
   Name                 nvarchar(50) comment '��Ʒ��',
   ShowName             nvarchar(500) comment '��ʾ����',
   SellingPoint         nvarchar(100) comment '����',
   CategoryId           int comment '����Id',
   BrandId              int comment 'Ʒ��Id',
   InputRate            decimal comment '����˰��',
   OutRate              decimal comment '����˰��',
   IsGift               bool comment '�Ƿ���Ʒ',
   Length               decimal comment '��',
   Width                decimal comment '��',
   Height               decimal comment '��',
   Weight               decimal comment '����',
   Description          text comment '��������',
   Keywords             nvarchar(1000) comment '�ؼ���',
   IsPublish            bool comment '�Ƿ��ϼ�',
   CreatedOn            datetime comment '����ʱ��',
   primary key (Id)
);

alter table Product comment '��Ʒ��';

/*==============================================================*/
/* Table: ProductSKU                                            */
/*==============================================================*/
create table ProductSKU
(
   Id                   int not null auto_increment comment '���',
   ProductId            int comment '��Ʒ����',
   Code                 nvarchar(20) comment '����',
   BarCode              nvarchar(50) comment '����',
   SpecificationList    nvarchar(3000) comment '����б�',
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
/* Table: ProductSpecification                                  */
/*==============================================================*/
create table ProductSpecification
(
   Id                   int not null auto_increment comment '����',
   Name                 nvarchar(512) comment '������',
   CategoryId           varchar(18) comment '����',
   primary key (Id)
);

alter table ProductSpecification comment '��Ʒ���';

/*==============================================================*/
/* Table: ProductSpecificationMapping                           */
/*==============================================================*/
create table ProductSpecificationMapping
(
   Id                   int not null auto_increment comment '���',
   ProductId            int comment '��ƷId',
   ProductSpecificationId int comment '��Ʒ���Id',
   ProductSpecificationOptionId int comment '���ѡ��Id',
   primary key (Id)
);

alter table ProductSpecificationMapping comment '��Ʒ���ӳ��';

/*==============================================================*/
/* Table: ProductSpecificationOption                            */
/*==============================================================*/
create table ProductSpecificationOption
(
   Id                   int not null auto_increment comment '���',
   ProductSpecificationId int comment '��������',
   Value                nvarchar(100) comment 'ֵ',
   primary key (Id)
);

alter table ProductSpecificationOption comment '��Ʒ���ѡ��';

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
   ActualQuantity       int comment 'ʵ�ʿ����',
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
   ActualQuantity       int comment 'ʵ�ʿ����',
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

