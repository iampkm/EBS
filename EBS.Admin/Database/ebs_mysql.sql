/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2016-12-06 11:43:04                          */
/*==============================================================*/


drop index idx_account_username on Account;

drop table if exists Account;

drop table if exists AccountLoginHistory;

drop index idx_adjustcontractprice_code on AdjustContractPrice;

drop table if exists AdjustContractPrice;

drop table if exists AdjustContractPriceItem;

drop table if exists AdjustSalePrice;

drop table if exists AdjustSalePriceItem;

drop table if exists Area;

drop index idx_BillSequence_guidcode on BillSequence;

drop table if exists BillSequence;

drop table if exists Brand;

drop table if exists Category;

drop table if exists Inventory;

drop table if exists InventoryHistory;

drop table if exists Menu;

drop table if exists Picture;

drop index idx_ProcessHistory_fromId on ProcessHistory;

drop table if exists ProcessHistory;

drop index idx_product_code on Product;

drop table if exists Product;

drop table if exists ProductAreaPrice;

drop index idx_pcodeseq_guidcode on ProductCodeSequence;

drop table if exists ProductCodeSequence;

drop table if exists ProductDetails;

drop table if exists ProductPicture;

drop table if exists ProductStorePrice;

drop index idx_purcontract_code on PurchaseContract;

drop table if exists PurchaseContract;

drop table if exists PurchaseContractItem;

drop index idx_PurchaseOrder_code on PurchaseOrder;

drop table if exists PurchaseOrder;

drop table if exists PurchaseOrderItem;

drop table if exists Role;

drop table if exists RoleMenu;

drop table if exists SaleOrder;

drop table if exists SaleOrderItem;

drop table if exists Shelf;

drop table if exists ShelfLayer;

drop table if exists ShelfLayerProduct;

drop table if exists Stocktaking;

drop table if exists StocktakingItem;

drop table if exists StocktakingPlan;

drop table if exists StocktakingPlanItem;

drop index idx_store_Code on Store;

drop table if exists Store;

drop table if exists StoreInventory;

drop table if exists StoreInventoryBatch;

drop table if exists StoreInventoryHistory;

drop index idx_StorePurchaseOrder_code on StorePurchaseOrder;

drop table if exists StorePurchaseOrder;

drop table if exists StorePurchaseOrderItem;

drop index idx_supplier_code on Supplier;

drop table if exists Supplier;

drop table if exists SupplierProduct;

drop table if exists TransferOrder;

drop table if exists TransferOrderItem;

drop table if exists VipCard;

drop table if exists VipProduct;

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
   CanViewStores        varchar(200) comment '���Բ�ѯ�ŵ�',
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
/* Table: AdjustContractPrice                                   */
/*==============================================================*/
create table AdjustContractPrice
(
   Id                   int not null auto_increment comment '���',
   Code                 nvarchar(50) comment '���۵���',
   Name                 nvarchar(50) comment '������',
   StoreId              int comment '�ŵ�Id',
   SupplierId           int comment '��Ӧ��Id',
   StartDate            datetime comment '��ʼ����',
   EndDate              datetime comment '��������',
   CreatedOn            datetime comment '����ʱ��',
   CreatedBy            int comment '������',
   UpdatedOn            datetime comment '�޸�ʱ��',
   UpdatedBy            int comment '�޸���',
   Status               int comment '״̬',
   primary key (Id)
);

/*==============================================================*/
/* Index: idx_adjustcontractprice_code                          */
/*==============================================================*/
create unique index idx_adjustcontractprice_code on AdjustContractPrice
(
   Code
);

/*==============================================================*/
/* Table: AdjustContractPriceItem                               */
/*==============================================================*/
create table AdjustContractPriceItem
(
   Id                   int not null auto_increment comment '���',
   AdjustContractPriceId int comment '���۵�����',
   ProductId            int comment '��Ʒ���',
   ContractPrice        decimal(8,2) comment '��ͬ��',
   AdjustPrice          decimal(8,2) comment '������',
   primary key (Id)
);

alter table AdjustContractPriceItem comment '��������ϸ';

/*==============================================================*/
/* Table: AdjustSalePrice                                       */
/*==============================================================*/
create table AdjustSalePrice
(
   Id                   int not null auto_increment comment '���',
   Code                 nvarchar(50) comment '���۵���',
   CreatedOn            datetime comment '����ʱ��',
   CreatedBy            int comment '������',
   UpdatedOn            datetime comment '�޸�ʱ��',
   UpdatedBy            int comment '�޸���',
   Status               int comment '״̬',
   primary key (Id)
);

alter table AdjustSalePrice comment '�����ۼ�';

/*==============================================================*/
/* Table: AdjustSalePriceItem                                   */
/*==============================================================*/
create table AdjustSalePriceItem
(
   Id                   int not null auto_increment comment '���',
   AdjustSalePriceId    int comment '���۵�����',
   SalePrice            decimal(8,2) comment '���ۼ�',
   AdjustPrice          decimal(8,2) comment 'ԭ�ۼ�',
   ProductId            int comment '��Ʒ���',
   primary key (Id)
);

alter table AdjustSalePriceItem comment '�����ۼ���ϸ';

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
   AvgCostPrice         decimal(8,2) comment 'ƽ���ɱ���',
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
/* Table: Picture                                               */
/*==============================================================*/
create table Picture
(
   Id                   int not null auto_increment comment 'ͼƬID',
   Url                  varchar(500) comment '���ӵ�ַ',
   Title                varchar(50) comment 'ͼƬ����',
   primary key (Id)
);

alter table Picture comment 'ͼƬ';

/*==============================================================*/
/* Table: ProcessHistory                                        */
/*==============================================================*/
create table ProcessHistory
(
   Id                   int not null auto_increment comment '���',
   CreatedBy            int comment '������',
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
   Keywords             nvarchar(200) comment '�ؼ���',
   BarCode              nvarchar(50) comment '����',
   Specification        nvarchar(200) comment '�����',
   OldPrice             decimal(8,2) comment 'ԭ��',
   SalePrice            decimal(8,2) comment '���ۼ�',
   WholeSalePrice       decimal(8,2) comment '������',
   SubSkuCode           varchar(20) comment '��SKU����',
   SubSkuQuantity       int comment '��SKU����',
   SpecificationQuantity nvarchar(100) comment '����, ������ŷָ�',
   CreatedOn            datetime comment '����ʱ��',
   CreatedBy            int comment '������',
   UpdatedOn            datetime comment '�޸�ʱ��',
   UpdatedBy            int comment '�޸���',
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
/* Table: ProductAreaPrice                                      */
/*==============================================================*/
create table ProductAreaPrice
(
   Id                   int not null auto_increment,
   ProductId            int,
   AreaId               char(6),
   SalePrice            decimal(8,2),
   primary key (Id)
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
/* Table: ProductDetails                                        */
/*==============================================================*/
create table ProductDetails
(
   ProductId            int  not null,
   Description          text comment '��������',
   primary key (ProductId)
);

alter table ProductDetails comment '��Ʒ����';

/*==============================================================*/
/* Table: ProductPicture                                        */
/*==============================================================*/
create table ProductPicture
(
   Id                   int not null comment '���',
   ProductId            int comment '��Ʒ���',
   PictureId            int comment 'ͼƬId',
   primary key (Id)
);

/*==============================================================*/
/* Table: ProductStorePrice                                     */
/*==============================================================*/
create table ProductStorePrice
(
   Id                   int not null auto_increment,
   ProductId            int,
   StoreId              int,
   SalePrice            decimal(8,2),
   primary key (Id)
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
/* Table: SaleOrder                                             */
/*==============================================================*/
create table SaleOrder
(
   Id                   int not null auto_increment,
   Code                 nvarchar(20) comment '����',
   StoreId              int comment '�ŵ�',
   PosId                int comment 'Pos��Id',
   OrderType            int comment '��������',
   RefundAccount        varchar(60) comment '�˿��˺�',
   Status               int comment '״̬',
   OrderAmount          decimal(8,2) comment '�������',
   PayAmount            decimal(8,2) comment '�ֽ�֧�����',
   OnlinePayAmount      decimal(8,2) comment '����֧�����',
   PaymentWay           int comment '֧����ʽ',
   PaidDate             datetime comment '֧��ʱ��',
   Hour                 int comment 'ʱ��',
   CreatedOn            datetime comment '����ʱ��',
   CreatedBy            int comment '������',
   UpdatedOn            datetime comment '�޸�ʱ��',
   UpdatedBy            int comment '�޸���',
   primary key (Id)
);

alter table SaleOrder comment '���۶���';

/*==============================================================*/
/* Table: SaleOrderItem                                         */
/*==============================================================*/
create table SaleOrderItem
(
   Id                   int not null,
   SaleOrderId          int comment '���۱���',
   ProductId            int comment '��ƷId',
   ProductCode          nvarchar(20) comment '��Ʒ����',
   ProductName          nvarchar(50) comment '��Ʒ��',
   AvgCostPrice         decimal(8,2) comment 'ƽ���ɱ���',
   SalePrice            decimal(8,2) comment '���ۼ�',
   RealPrice            decimal(8,2) comment 'ʵ���ۼ�',
   Quanttiy             int comment '����',
   primary key (Id)
);

/*==============================================================*/
/* Table: Shelf                                                 */
/*==============================================================*/
create table Shelf
(
   Id                   int not null auto_increment comment '���',
   StoreId              int,
   Code                 varchar(20) comment '������',
   Number               int comment '������',
   Name                 varchar(50) comment '������',
   primary key (Id)
);

alter table Shelf comment '����';

/*==============================================================*/
/* Table: ShelfLayer                                            */
/*==============================================================*/
create table ShelfLayer
(
   Id                   int not null comment '���',
   Code                 varchar(20) comment '������',
   Number               int comment '������',
   ShelfId              int comment '������',
   primary key (Id)
);

alter table ShelfLayer comment '���ܲ�';

/*==============================================================*/
/* Table: ShelfLayerProduct                                     */
/*==============================================================*/
create table ShelfLayerProduct
(
   Id                   int not null comment '���',
   StoreId              int,
   Code                 varchar(20) comment '������',
   Number               int comment '������',
   ProductId            int comment '��ƷID',
   Quantity             int,
   ShelfLayerId         int,
   primary key (Id)
);

/*==============================================================*/
/* Table: Stocktaking                                           */
/*==============================================================*/
create table Stocktaking
(
   Id                   int not null auto_increment comment '���',
   StocktakingPlanId    int comment '�̵�ƻ����',
   Code                 nvarchar(20) comment '�̵㵥��',
   StocktakingType      int comment '�̵������1 �̵�ձ�2 �̵�������',
   ShelfCode            nvarchar(20) comment '������',
   CreateTime           datetime comment '����ʱ��',
   CreateBy             int comment '������',
   CreateByName         nvarchar(50) comment '��������',
   Status               int comment '״̬����������',
   StoreId              int comment '�ŵ�',
   Note                 nvarchar(1000) comment '��ע',
   primary key (Id)
);

alter table Stocktaking comment '�̵��';

/*==============================================================*/
/* Table: StocktakingItem                                       */
/*==============================================================*/
create table StocktakingItem
(
   Id                   int not null comment '���',
   StocktakingId        int,
   ProductId            nvarchar(50) comment '��Ʒ����',
   ProductName          nvarchar(300) comment '��Ʒ��',
   BarCode              nvarchar(50) comment '����',
   Specification        nvarchar(100) comment '���',
   CostPrice            decimal(8,2) comment '�����ɱ���',
   SalesPrice           decimal(8,2) comment '���ۼ�',
   Quantity             int comment '�̵����������',
   CountQuantity        int comment '�̵�����',
   CorectQuantity       int comment '������',
   CorectReason         nvarchar(500) comment '����ԭ��',
   Note                 nvarchar(500) comment '��ע',
   primary key (Id)
);

alter table StocktakingItem comment '�̵���ϸ';

/*==============================================================*/
/* Table: StocktakingPlan                                       */
/*==============================================================*/
create table StocktakingPlan
(
   Id                   int not null auto_increment comment '���',
   Code                 nvarchar(20) comment '�̵����',
   CreateBy             int comment '������',
   CreateByName         nvarchar(50) comment '��������',
   CreateTime           datetime comment '����ʱ�䣨�̵����ڣ�',
   UpdateBy             int comment '������',
   UpdateByName         nvarchar(50) comment '��������',
   UpdateTime           datetime comment '����ʱ��',
   Method               int comment '�̵㷽ʽ�����̣�С�̣�',
   Status               int comment '�̵�״̬�����̣����̣����̣����̣�',
   StoreId              int comment '�ŵ���',
   Note                 nvarchar(1000) comment '��ע',
   StocktakingDate      datetime comment '�̵�����',
   primary key (Id)
);

alter table StocktakingPlan comment '�̵�ƻ�';

/*==============================================================*/
/* Table: StocktakingPlanItem                                   */
/*==============================================================*/
create table StocktakingPlanItem
(
   Id                   int not null auto_increment comment '���',
   StocktakingPlanId    int comment '�̵�ƻ����',
   ProductId            int comment 'ϵͳ����',
   ProductName          nvarchar(300) comment '��Ʒ��',
   BarCode              nvarchar(50) comment '����',
   Specification        nvarchar(100) comment '���',
   CostPrice            decimal(8,2) comment '�����ɱ���',
   SalePrice            decimal(8,2) comment '���ۼ�',
   Quantity             int comment '�������',
   CountQuantity        int comment '�̵�����',
   primary key (Id)
);

alter table StocktakingPlanItem comment '�̵�ƻ���ϸ';

/*==============================================================*/
/* Table: Store                                                 */
/*==============================================================*/
create table Store
(
   Id                   int not null auto_increment comment '���',
   Code                 nvarchar(20) comment '����',
   Number               int comment '���',
   Name                 nvarchar(128) comment '�ŵ���',
   SourceKey            nvarchar(32) comment '�ŵ�Ψһ��',
   CreatedOn            datetime comment '����ʱ��',
   CreatedBy            int comment '������',
   AreaId               char(6) comment '����ID',
   Address              nvarchar(512) comment '��ַ',
   Contact              nvarchar(32) comment '��ϵ��',
   Phone                nvarchar(32) comment '��ϵ�绰',
   Setting              text comment '����',
   LicenseCode          varchar(50) comment '�ŵ���Ȩ��',
   primary key (Id)
);

alter table Store comment '�ŵ�';

/*==============================================================*/
/* Index: idx_store_Code                                        */
/*==============================================================*/
create unique index idx_store_Code on Store
(
   Code
);

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
   AvgCostPrice         decimal(8,2) comment 'ƽ���ɱ���',
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
   OrderType            int comment '��������: �� 1 �� 2',
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
   SpecificationQuantity int comment '����',
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
/* Table: SupplierProduct                                       */
/*==============================================================*/
create table SupplierProduct
(
   Id                   int not null auto_increment comment '���',
   SupplierId           int comment '��Ӧ��Id',
   ProductId            int comment '��Ʒ',
   Price                decimal(8,2) comment '�۸�',
   primary key (Id)
);

alter table SupplierProduct comment '��Ӧ����Ʒ';

/*==============================================================*/
/* Table: TransferOrder                                         */
/*==============================================================*/
create table TransferOrder
(
   Id                   int not null auto_increment comment '���',
   Code                 varchar(20) comment '��������',
   FromStoreId          int comment '���ŵ�',
   FromStoreName        char(50) comment '���ŵ���',
   ToStoreName          char(50) comment '���ŵ���',
   ToStoreId            int comment '���ŵ�',
   Status               int comment '״̬',
   CreatedOn            datetime comment '����ʱ��',
   CreatedBy            int comment '������',
   CreatedByName        varchar(30) comment '��������',
   UpdatedOn            datetime comment '�޸�ʱ��',
   UpdatedBy            int comment '�޸���',
   UpdatedByName        varchar(30) comment '�޸�����',
   primary key (Id)
);

alter table TransferOrder comment '������';

/*==============================================================*/
/* Table: TransferOrderItem                                     */
/*==============================================================*/
create table TransferOrderItem
(
   Id                   int not null comment '���',
   TransferOrderId      int comment '������ID',
   ProductId            int comment 'SKU����',
   Quantity             int comment '����',
   primary key (Id)
);

alter table TransferOrderItem comment '������ϸ';

/*==============================================================*/
/* Table: VipCard                                               */
/*==============================================================*/
create table VipCard
(
   Id                   int not null auto_increment,
   Code                 varchar(50) comment '��Ա����',
   Discount             decimal(8,2) comment '�ۿ�',
   primary key (Id)
);

alter table VipCard comment '��Ա��';

/*==============================================================*/
/* Table: VipProduct                                            */
/*==============================================================*/
create table VipProduct
(
   Id                   int not null auto_increment,
   ProductId            int,
   SalePrice            decimal(8,2),
   primary key (Id)
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

