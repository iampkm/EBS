/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2016-11-04 17:34:49                          */
/*==============================================================*/


drop index idx_account_username on Account;

drop table if exists Account;

drop table if exists AccountLoginHistory;

drop table if exists Area;

drop index idx_BillSequence_guidcode on BillSequence;

drop table if exists BillSequence;

drop table if exists Brand;

drop table if exists Category;

drop table if exists Inventory;

drop table if exists InventoryHistory;

drop table if exists Menu;

drop index idx_ProcessHistory_fromId on ProcessHistory;

drop table if exists ProcessHistory;

drop index idx_product_code on Product;

drop table if exists Product;

drop index idx_pcodeseq_guidcode on ProductCodeSequence;

drop table if exists ProductCodeSequence;

drop index idx_purcontract_code on PurchaseContract;

drop table if exists PurchaseContract;

drop table if exists PurchaseContractItem;

drop index idx_PurchaseOrder_code on PurchaseOrder;

drop table if exists PurchaseOrder;

drop table if exists PurchaseOrderItem;

drop table if exists Role;

drop table if exists RoleMenu;

drop table if exists Store;

drop table if exists StoreInventory;

drop table if exists StoreInventoryBatch;

drop table if exists StoreInventoryHistory;

drop index idx_StorePurchaseOrder_code on StorePurchaseOrder;

drop table if exists StorePurchaseOrder;

drop table if exists StorePurchaseOrderItem;

drop index idx_supplier_code on Supplier;

drop table if exists Supplier;

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
   StoreId              int comment '�ŵ�',
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
/* Table: Area                                                  */
/*==============================================================*/
create table Area
(
   Id                   char(6) not null comment '���',
   Name                 nvarchar(64) comment '������',
   ShowName             nvarchar(64) comment '��ʾ����',
   FullName             nvarchar(256) comment '����ȫ��',
   Level                int comment '�㼶',
   primary key (Id)
);

alter table Area comment '�����';

/*==============================================================*/
/* Table: BillSequence                                          */
/*==============================================================*/
create table BillSequence
(
   Id                   int not null auto_increment,
   GuidCode             nvarchar(32) comment 'guid����',
   primary key (Id)
);

alter table BillSequence comment '�������к�';

/*==============================================================*/
/* Index: idx_BillSequence_guidcode                             */
/*==============================================================*/
create unique index idx_BillSequence_guidcode on BillSequence
(
   GuidCode
);

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
   ProductId            int comment '����',
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
   ProductId            int comment '��ƷId',
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
/* Table: ProcessHistory                                        */
/*==============================================================*/
create table ProcessHistory
(
   Id                   int not null auto_increment comment '���',
   CreatedBy            int comment '������',
   CreatedByName        nvarchar(64) comment '��������',
   CreatedOn            datetime comment '����ʱ��',
   Status               int comment '״̬',
   FormId               int comment '��Id',
   FormType             nvarchar(64) comment '������',
   Remark               nvarchar(1000) comment '��ע',
   primary key (Id)
);

alter table ProcessHistory comment '��������ʷ��¼';

/*==============================================================*/
/* Index: idx_ProcessHistory_fromId                             */
/*==============================================================*/
create index idx_ProcessHistory_fromId on ProcessHistory
(
   FormId,
   FormType
);

/*==============================================================*/
/* Table: Product                                               */
/*==============================================================*/
create table Product
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
   OldPrice             decimal(8,2) comment 'ԭ��',
   SalePrice            decimal(8,2) comment '���ۼ�',
   WholeSalePrice       decimal(8,2) comment '������',
   CostPrice            decimal(8,2) comment 'ƽ���ɱ���',
   SubSkuCode           varchar(20) comment '��SKU����',
   SubSkuQuantity       int comment '��SKU����',
   SpecificationQuantity nvarchar(100) comment '����, ������ŷָ�',
   CreatedOn            datetime comment '����ʱ��',
   primary key (Id)
);

alter table Product comment '��Ʒ';

/*==============================================================*/
/* Index: idx_product_code                                      */
/*==============================================================*/
create unique index idx_product_code on Product
(
   Code
);

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
/* Table: PurchaseContract                                      */
/*==============================================================*/
create table PurchaseContract
(
   Id                   int not null auto_increment comment '���',
   Code                 nvarchar(50) comment '��ͬ��',
   Name                 nvarchar(50) comment '��ͬ����',
   StoreId              int comment '�ŵ�Id',
   SupplierId           int comment '��Ӧ��Id',
   Contact              nvarchar(32) comment '��ϵ��',
   StartDate            datetime comment '��ʼ����',
   EndDate              datetime comment '��������',
   CreatedOn            datetime comment '����ʱ��',
   CreatedBy            int comment '������',
   UpdatedOn            datetime comment '�޸�ʱ��',
   UpdatedBy            int comment '�޸���',
   Status               int comment '״̬',
   primary key (Id)
);

alter table PurchaseContract comment '�ɹ���ͬ';

/*==============================================================*/
/* Index: idx_purcontract_code                                  */
/*==============================================================*/
create unique index idx_purcontract_code on PurchaseContract
(
   Code
);

/*==============================================================*/
/* Table: PurchaseContractItem                                  */
/*==============================================================*/
create table PurchaseContractItem
(
   Id                   int not null auto_increment comment '���',
   PurchaseContractId   int comment '�ɹ���ͬ���',
   ProductId            int comment '��Ʒskuid',
   ContractPrice        decimal(8,2) comment '��ͬ��',
   primary key (Id)
);

alter table PurchaseContractItem comment '�ɹ���ͬ��ϸ';

/*==============================================================*/
/* Table: PurchaseOrder                                         */
/*==============================================================*/
create table PurchaseOrder
(
   Id                   int not null auto_increment comment '���',
   PurchaseContractId   int comment '�ɹ���ͬ���',
   Code                 nvarchar(20) comment '������',
   Type                 int comment '��������:  ���� 1���˻� 2',
   WarehouseId          int comment '�ֿ�Id',
   SupplierId           int comment '��Ӧ��Id',
   CreatedOn            datetime comment '����ʱ��',
   CreatedBy            int comment '������',
   UpdatedOn            datetime comment '�޸�ʱ��',
   UpdatedBy            int comment '�޸���',
   Status               int comment '״̬',
   Total                decimal(8,2) comment '���',
   primary key (Id)
);

alter table PurchaseOrder comment '�ɹ�����';

/*==============================================================*/
/* Index: idx_PurchaseOrder_code                                */
/*==============================================================*/
create unique index idx_PurchaseOrder_code on PurchaseOrder
(
   Code
);

/*==============================================================*/
/* Table: PurchaseOrderItem                                     */
/*==============================================================*/
create table PurchaseOrderItem
(
   Id                   int not null auto_increment comment '���',
   PurchaseOrderId      int comment '�ɹ��������',
   ProductId            int comment '��Ʒskuid',
   ContractPrice        decimal(8,2) comment '��ͬ��',
   Price                decimal(8,2) comment '����',
   Quantity             int comment '����',
   ActualQuantity       int comment 'ʵ������',
   IsGift               bool comment '��Ʒ�Ƿ���Ʒ',
   primary key (Id)
);

alter table PurchaseOrderItem comment '�ɹ�������ϸ';

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
   Id                   int not null auto_increment comment '���',
   StoreId              int comment '�ŵ����',
   ProductId            int comment '��ƷId',
   SaleQuantity         int comment '���ۿ��',
   OrderQuantity        int comment '�������',
   Quantity             int comment 'ʵ�ʿ����',
   WarnQuantity         int comment '������',
   IsQuit               bool comment '�Ƿ��˳�',
   primary key (Id)
);

alter table StoreInventory comment '�ŵ���';

/*==============================================================*/
/* Table: StoreInventoryBatch                                   */
/*==============================================================*/
create table StoreInventoryBatch
(
   Id                   int not null auto_increment comment '���',
   ProductId            int comment 'SKU����',
   StoreId              int comment '�ֿ����',
   SupplierId           int comment '��Ӧ��Id',
   Quantity             int comment 'ʵ�ʿ����',
   ProductionDate       datetime comment '��������',
   ShelfLife            int comment '������',
   Price                decimal(8,2) comment '����',
   CreatedOn            datetime comment '����ʱ��',
   CreatedBy            int comment '������',
   BatchNo              nvarchar(20) comment '���κ�',
   primary key (Id)
);

alter table StoreInventoryBatch comment '�ŵ���Ʒ����';

/*==============================================================*/
/* Table: StoreInventoryHistory                                 */
/*==============================================================*/
create table StoreInventoryHistory
(
   Id                   int not null auto_increment comment '���',
   ProductId            int comment 'SKU����',
   StoreId              int comment '�ֿ����',
   Quantity             int comment 'ʵ�ʿ����',
   ChangeQuantity       int comment '�䶯��',
   CreatedOn            datetime comment '����ʱ��',
   CreatedBy            int comment '������',
   BillId               int comment '����ϵͳ��',
   BillCode             varchar(20) comment '���ݱ���',
   BillType             int comment '��������',
   BatchNo              nvarchar(20) comment '���κ�',
   Price                decimal(8,2) comment '����',
   primary key (Id)
);

alter table StoreInventoryHistory comment '�ŵ�����ʷ��¼';

/*==============================================================*/
/* Table: StorePurchaseOrder                                    */
/*==============================================================*/
create table StorePurchaseOrder
(
   Id                   int not null auto_increment comment '���',
   Code                 nvarchar(20) comment '������',
   StoreId              int comment '�ŵ�Id',
   SupplierBill         nvarchar(20) comment '��Ӧ�̵��ݺ�',
   SupplierId           int comment '��Ӧ��Id',
   CreatedOn            datetime comment '����ʱ��',
   CreatedBy            int comment '������',
   CreatedByName        nvarchar(50) comment '��������',
   ReceivedBy           int comment '�ջ���',
   ReceivedOn           datetime comment '�ջ�����',
   ReceivedByName       varchar(50) comment '�ջ�����',
   StoragedBy           int comment '�����',
   StoragedByName       nvarchar(50) comment '�������',
   StoragedOn           datetime comment '�������',
   Status               int comment '״̬',
   IsGift               bool comment '�Ƿ���Ʒ',
   BatchNo              nvarchar(20) comment '���κ�',
   primary key (Id)
);

alter table StorePurchaseOrder comment '�ŵ�ɹ�����';

/*==============================================================*/
/* Index: idx_StorePurchaseOrder_code                           */
/*==============================================================*/
create unique index idx_StorePurchaseOrder_code on StorePurchaseOrder
(
   Code
);

/*==============================================================*/
/* Table: StorePurchaseOrderItem                                */
/*==============================================================*/
create table StorePurchaseOrderItem
(
   Id                   int not null auto_increment comment '���',
   StorePurchaseOrderId int comment '�ŵ�ɹ��������',
   ProductId            int comment '��Ʒskuid',
   ContractPrice        decimal(8,2) comment '��ͬ��',
   Price                decimal(8,2) comment '����',
   Quantity             int comment '����',
   ActualQuantity       int comment 'ʵ������',
   ProductionDate       datetime comment '��������',
   ShelfLife            int comment '������',
   primary key (Id)
);

alter table StorePurchaseOrderItem comment '�ŵ�ɹ�������ϸ';

/*==============================================================*/
/* Table: Supplier                                              */
/*==============================================================*/
create table Supplier
(
   Id                   int not null auto_increment comment '���',
   Code                 nvarchar(20) comment '��ͬ��',
   Name                 nvarchar(100) comment '��Ӧ����',
   Type                 int comment '��Ӧ�����',
   ShortName            nvarchar(50) comment '���',
   Contact              nvarchar(50) comment '��ϵ��',
   Phone                nvarchar(50) comment '��ϵ�绰',
   QQ                   nvarchar(30),
   Address              nvarchar(100),
   Bank                 nvarchar(50) comment '������',
   BankAccount          nvarchar(50) comment '�������˺�',
   TaxNo                nvarchar(50) comment '˰��',
   LicenseNo            nvarchar(50) comment 'ִ�պ�',
   AreaId               char(6) comment '��������',
   CreatedOn            datetime comment '����ʱ��',
   CreatedBy            int comment '������',
   UpdatedOn            datetime comment '�޸�ʱ��',
   UpdatedBy            int comment '�޸���',
   primary key (Id)
);

alter table Supplier comment '��Ӧ��';

/*==============================================================*/
/* Index: idx_supplier_code                                     */
/*==============================================================*/
create unique index idx_supplier_code on Supplier
(
   Code
);

/*==============================================================*/
/* Table: Warehouse                                             */
/*==============================================================*/
create table Warehouse
(
   Id                   int not null auto_increment,
   Code                 nvarchar(20) comment '����',
   Name                 nvarchar(50) comment '�ֿ���',
   AreaId               char(6) comment '����',
   Address              nvarchar(100) comment '��ַ',
   primary key (Id)
);

alter table Warehouse comment '�ֿ�';

