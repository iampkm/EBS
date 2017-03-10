/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2017-03-08 15:59:09                          */
/*==============================================================*/


drop index idx_accesstoken_cdkey on AccessToken;

drop table if exists AccessToken;

drop index idx_account_username on Account;

drop table if exists Account;

drop table if exists AccountLoginHistory;

drop index idx_adjustcontractprice_code on AdjustContractPrice;

drop table if exists AdjustContractPrice;

drop table if exists AdjustContractPriceItem;

drop table if exists AdjustSalePrice;

drop table if exists AdjustSalePriceItem;

drop index idx_AdjustStorePrice_code on AdjustStorePrice;

drop table if exists AdjustStorePrice;

drop table if exists AdjustStorePriceItem;

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

drop index idx_product_barcode on Product;

drop index idx_product_code on Product;

drop table if exists Product;

drop table if exists ProductAreaPrice;

drop index idx_pcodeseq_guidcode on ProductCodeSequence;

drop table if exists ProductCodeSequence;

drop table if exists ProductDetails;

drop table if exists ProductPicture;

drop index idx_ProductStorePrice_pid on ProductStorePrice;

drop table if exists ProductStorePrice;

drop index idx_purcontract_code on PurchaseContract;

drop table if exists PurchaseContract;

drop table if exists PurchaseContractItem;

drop index idx_PurchaseOrder_code on PurchaseOrder;

drop table if exists PurchaseOrder;

drop table if exists PurchaseOrderItem;

drop table if exists PurchaseSaleInventory;

drop table if exists PurchaseSaleInventoryDetail;

drop table if exists Role;

drop table if exists RoleMenu;

drop index idx_saleorder_StoreIdAndupdatedOn on SaleOrder;

drop index idx_saleorder_updatedOn on SaleOrder;

drop index idx_saleorder_code on SaleOrder;

drop table if exists SaleOrder;

drop index idx_saleorderitem_productid on SaleOrderItem;

drop index idx_saleorderitem_saleorderid on SaleOrderItem;

drop table if exists SaleOrderItem;

drop index idx_SaleSync on SaleSync;

drop table if exists SaleSync;

drop table if exists Shelf;

drop table if exists ShelfLayer;

drop table if exists ShelfLayerProduct;

drop table if exists Stocktaking;

drop table if exists StocktakingItem;

drop table if exists StocktakingPlan;

drop table if exists StocktakingPlanItem;

drop index idx_store_Code on Store;

drop table if exists Store;

drop index idx_storeInventory_pidAndStoreId on StoreInventory;

drop index idx_storeInventory_pid on StoreInventory;

drop table if exists StoreInventory;

drop index idx_storeInventoryBath_pid on StoreInventoryBatch;

drop table if exists StoreInventoryBatch;

drop index idx_storeinventoryhistory_productid on StoreInventoryHistory;

drop index idx_storeinventoryhistory_billCode on StoreInventoryHistory;

drop index idx_storeinventoryhistory_CreatedOn on StoreInventoryHistory;

drop table if exists StoreInventoryHistory;

drop index idx_storeInventoryMonthly_pid on StoreInventoryMonthly;

drop table if exists StoreInventoryMonthly;

drop index idx_StorePurchaseOrder_code on StorePurchaseOrder;

drop table if exists StorePurchaseOrder;

drop table if exists StorePurchaseOrderItem;

drop index idx_supplier_code on Supplier;

drop table if exists Supplier;

drop table if exists SupplierProduct;

drop index idx_transaferOrder_code on TransferOrder;

drop table if exists TransferOrder;

drop table if exists TransferOrderItem;

drop table if exists VipCard;

drop index idx_vipProduct_productid on VipProduct;

drop table if exists VipProduct;

drop table if exists Warehouse;

drop index idx_workschedule_code on WorkSchedule;

drop table if exists WorkSchedule;

/*==============================================================*/
/* Table: AccessToken                                           */
/*==============================================================*/
create table AccessToken
(
   Id                   int not null auto_increment,
   StoreId              int comment '�ŵ�ID',
   PosId                int comment 'pos��Id',
   CDKey                varchar(50) comment '���к�',
   primary key (Id)
);

/*==============================================================*/
/* Index: idx_accesstoken_cdkey                                 */
/*==============================================================*/
create unique index idx_accesstoken_cdkey on AccessToken
(
   CDKey
);

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
   StoreId              int comment '�ŵ�Id',
   SupplierId           int comment '��Ӧ��Id',
   CreatedOn            datetime comment '����ʱ��',
   CreatedBy            int comment '������',
   UpdatedOn            datetime comment '�޸�ʱ��',
   UpdatedBy            int comment '�޸���',
   Status               int comment '״̬',
   Remark               nvarchar(200) comment '��ע',
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
   ContractPrice        decimal(8,4) comment '��ͬ��',
   AdjustPrice          decimal(8,4) comment '������',
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
/* Table: AdjustStorePrice                                      */
/*==============================================================*/
create table AdjustStorePrice
(
   Id                   int not null auto_increment comment '���',
   Code                 nvarchar(50) comment '���۵���',
   StoreId              int comment '�ŵ�',
   CreatedOn            datetime comment '����ʱ��',
   CreatedBy            int comment '������',
   UpdatedOn            datetime comment '�޸�ʱ��',
   UpdatedBy            int comment '�޸���',
   Status               int comment '״̬',
   Remark               varchar(500) comment '��ע',
   primary key (Id)
);

alter table AdjustStorePrice comment '�����ŵ��ۼ�';

/*==============================================================*/
/* Index: idx_AdjustStorePrice_code                             */
/*==============================================================*/
create unique index idx_AdjustStorePrice_code on AdjustStorePrice
(
   Code
);

/*==============================================================*/
/* Table: AdjustStorePriceItem                                  */
/*==============================================================*/
create table AdjustStorePriceItem
(
   Id                   int not null auto_increment comment '���',
   AdjustStorePriceId   int comment '���۵�����',
   StoreSalePrice       decimal(8,2) comment 'ԭ�ۼ�',
   AdjustPrice          decimal(8,2) comment '�����ۼ�',
   ProductId            int comment '��Ʒ���',
   primary key (Id)
);

alter table AdjustStorePriceItem comment '�����ۼ���ϸ';

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
   CreatedByName        varchar(50) comment '��������',
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
   MadeIn               varchar(200) comment '����',
   Grade                varchar(50) comment '�ȼ�',
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
/* Index: idx_product_barcode                                   */
/*==============================================================*/
create unique index idx_product_barcode on Product
(
   BarCode
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

alter table ProductAreaPrice comment '��Ʒ����ۣ��ñ���ʹ��';

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
   Status               int comment '״̬',
   primary key (Id)
);

alter table ProductStorePrice comment '��Ʒ�ŵ�ۣ� �˱���ʹ�ã�����������غϣ��ÿ������';

/*==============================================================*/
/* Index: idx_ProductStorePrice_pid                             */
/*==============================================================*/
create unique index idx_ProductStorePrice_pid on ProductStorePrice
(
   ProductId,
   StoreId
);

/*==============================================================*/
/* Table: PurchaseContract                                      */
/*==============================================================*/
create table PurchaseContract
(
   Id                   int not null auto_increment comment '���',
   Code                 nvarchar(50) comment '��ͬ��',
   Name                 nvarchar(50) comment '��ͬ����',
   StoreIds             varchar(300) comment '��Ӧ�ŵ꣬������÷ָ�',
   SupplierId           int comment '��Ӧ��Id',
   Contact              nvarchar(32) comment '��ϵ��',
   StartDate            datetime comment '��ʼ����',
   EndDate              datetime comment '��������',
   CreatedOn            datetime comment '����ʱ��',
   CreatedBy            int comment '������',
   UpdatedOn            datetime comment '�޸�ʱ��',
   UpdatedBy            int comment '�޸���',
   Status               int comment '״̬',
   Remark               varchar(1000) comment '��ע',
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
   ContractPrice        decimal(8,4) comment '��ͬ��',
   Status               int comment '����״̬',
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
/* Table: PurchaseSaleInventory                                 */
/*==============================================================*/
create table PurchaseSaleInventory
(
   YearMonth            int not null comment '��',
   StoreId              int not null comment '�ŵ�',
   StoreName            varchar(100) comment '�ŵ���',
   PreInventoryQuantity int comment '�ڳ����',
   PreInventoryAmount   decimal(12,4) comment '�ڳ������',
   PurchaseQuantity     int comment '���������',
   PurchaseAmount       decimal(12,4) comment '���������',
   SaleQuantity         int comment '����������',
   SaleCostAmount       decimal(12,4) comment '�������۳ɱ����',
   SaleAmount           decimal(12,2) comment '�������۽��',
   EndInventoryQuantity int comment '��ĩ�����',
   EndInventoryAmount   decimal(12,4) comment '��ĩ�����',
   UpdatedOn            datetime comment '����ʱ��',
   primary key (YearMonth, StoreId)
);

alter table PurchaseSaleInventory comment '�����汨��';

/*==============================================================*/
/* Table: PurchaseSaleInventoryDetail                           */
/*==============================================================*/
create table PurchaseSaleInventoryDetail
(
   YearMonth            int not null comment '����',
   StoreId              int not null comment '�ŵ�id',
   ProductId            int not null comment '��Ʒ����',
   ProductCode          varchar(20) comment '��Ʒ����',
   BarCode              varchar(20) comment '����',
   ProductName          varchar(50) comment 'Ʒ��',
   PreInventoryQuantity int comment '�ڳ����',
   PreInventoryAmount   decimal(12,4) comment '�ڳ������',
   PurchaseQuantity     int comment '���������',
   PurchaseAmount       decimal(12,4) comment '���������',
   SaleQuantity         int comment '����������',
   SaleCostAmount       decimal(12,4) comment '�������۳ɱ����',
   SaleAmount           decimal(12,2) comment '�������۽��',
   EndInventoryQuantity int comment '��ĩ�����',
   EndInventoryAmount   decimal(12,4) comment '��ĩ�����',
   UpdatedOn            datetime comment '����ʱ��',
   primary key (YearMonth, StoreId, ProductId)
);

alter table PurchaseSaleInventoryDetail comment '��������ϸ����';

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
   WorkScheduleCode     varchar(32) comment '��δ���',
   primary key (Id)
);

alter table SaleOrder comment '���۶���';

/*==============================================================*/
/* Index: idx_saleorder_code                                    */
/*==============================================================*/
create unique index idx_saleorder_code on SaleOrder
(
   Code
);

/*==============================================================*/
/* Index: idx_saleorder_updatedOn                               */
/*==============================================================*/
create index idx_saleorder_updatedOn on SaleOrder
(
   UpdatedOn
);

/*==============================================================*/
/* Index: idx_saleorder_StoreIdAndupdatedOn                     */
/*==============================================================*/
create index idx_saleorder_StoreIdAndupdatedOn on SaleOrder
(
   StoreId,
   Status,
   UpdatedOn
);

/*==============================================================*/
/* Table: SaleOrderItem                                         */
/*==============================================================*/
create table SaleOrderItem
(
   Id                   int not null auto_increment,
   SaleOrderId          int comment '���۱���',
   ProductId            int comment '��ƷId',
   ProductCode          nvarchar(20) comment '��Ʒ����',
   ProductName          nvarchar(50) comment '��Ʒ��',
   AvgCostPrice         decimal(8,2) comment 'ƽ���ɱ���',
   SalePrice            decimal(8,2) comment '���ۼ�',
   RealPrice            decimal(8,2) comment 'ʵ���ۼ�',
   Quantity             int comment '����',
   primary key (Id)
);

/*==============================================================*/
/* Index: idx_saleorderitem_saleorderid                         */
/*==============================================================*/
create index idx_saleorderitem_saleorderid on SaleOrderItem
(
   SaleOrderId
);

/*==============================================================*/
/* Index: idx_saleorderitem_productid                           */
/*==============================================================*/
create index idx_saleorderitem_productid on SaleOrderItem
(
   ProductId
);

/*==============================================================*/
/* Table: SaleSync                                              */
/*==============================================================*/
create table SaleSync
(
   Id                   int not null auto_increment,
   SaleDate             varchar(20) comment '������',
   StoreId              int comment '�ŵ�ID',
   PosId                int comment '������',
   OrderCount           int comment '������',
   OrderTotalAmount     decimal(8,2) comment '�����ܽ��',
   ClientUpdatedOn      datetime comment '�ϴ�ʱ��',
   primary key (Id)
);

/*==============================================================*/
/* Index: idx_SaleSync                                          */
/*==============================================================*/
create index idx_SaleSync on SaleSync
(
   SaleDate
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
   Id                   int not null auto_increment comment '���',
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
   Id                   int not null auto_increment comment '���',
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
   CreatedBy            int comment '������',
   CreatedByName        nvarchar(50) comment '��������',
   CreatedOn            datetime comment '����ʱ��',
   Status               int comment '״̬����������',
   UpdatedOn            datetime comment '�޸�ʱ��',
   UpdatedBy            int comment '�޸���',
   UpdatedByName        varchar(50) comment '�޸�����',
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
   Id                   int not null auto_increment comment '���',
   StocktakingId        int,
   ProductId            nvarchar(50) comment '��Ʒ����',
   CostPrice            decimal(8,4) comment '�����ɱ���',
   SalePrice            decimal(8,2) comment '���ۼ�',
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
   CreatedBy            int comment '������',
   CreatedByName        nvarchar(50) comment '��������',
   CreatedOn            datetime comment '����ʱ��',
   UpdatedBy            int comment '������',
   UpdatedByName        nvarchar(50) comment '��������',
   UpdatedOn            datetime comment '����ʱ��',
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
   CostPrice            decimal(8,4) comment '�����ɱ���',
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
   AvgCostPrice         decimal(8,4) comment 'ƽ���ɱ���',
   WarnQuantity         int comment '������',
   IsQuit               bool comment '�Ƿ��˳�',
   LastCostPrice        decimal(8,2) comment '���½���',
   StoreSalePrice       decimal(8,2) comment '�ŵ��ۼ�',
   Status               int comment '״̬',
   primary key (Id)
);

alter table StoreInventory comment '�ŵ���';

/*==============================================================*/
/* Index: idx_storeInventory_pid                                */
/*==============================================================*/
create index idx_storeInventory_pid on StoreInventory
(
   ProductId
);

/*==============================================================*/
/* Index: idx_storeInventory_pidAndStoreId                      */
/*==============================================================*/
create index idx_storeInventory_pidAndStoreId on StoreInventory
(
   StoreId,
   ProductId
);

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
   ContractPrice        decimal(8,4) comment '��ͬ��',
   Price                decimal(8,4) comment 'ʵ��������',
   CreatedOn            datetime comment '����ʱ��',
   CreatedBy            int comment '������',
   BatchNo              bigint comment '���κ�',
   primary key (Id)
);

alter table StoreInventoryBatch comment '�ŵ���Ʒ����';

/*==============================================================*/
/* Index: idx_storeInventoryBath_pid                            */
/*==============================================================*/
create index idx_storeInventoryBath_pid on StoreInventoryBatch
(
   ProductId
);

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
   BatchNo              bigint comment '���κ�',
   Price                decimal(8,4) comment '����',
   primary key (Id)
);

alter table StoreInventoryHistory comment '�ŵ�����ʷ��¼';

/*==============================================================*/
/* Index: idx_storeinventoryhistory_CreatedOn                   */
/*==============================================================*/
create index idx_storeinventoryhistory_CreatedOn on StoreInventoryHistory
(
   CreatedOn
);

/*==============================================================*/
/* Index: idx_storeinventoryhistory_billCode                    */
/*==============================================================*/
create index idx_storeinventoryhistory_billCode on StoreInventoryHistory
(
   BillCode
);

/*==============================================================*/
/* Index: idx_storeinventoryhistory_productid                   */
/*==============================================================*/
create index idx_storeinventoryhistory_productid on StoreInventoryHistory
(
   ProductId
);

/*==============================================================*/
/* Table: StoreInventoryMonthly                                 */
/*==============================================================*/
create table StoreInventoryMonthly
(
   Id                   int not null auto_increment comment '���',
   Monthly              varchar(10) comment '����ڼ� 2017-01 ���´洢',
   StoreId              int comment '�ŵ����',
   ProductId            int comment '��ƷId',
   Quantity             int comment 'ʵ�ʿ����',
   AvgCostPrice         decimal(8,4) comment 'ƽ���ɱ���',
   primary key (Id)
);

alter table StoreInventoryMonthly comment '�ŵ����±�';

/*==============================================================*/
/* Index: idx_storeInventoryMonthly_pid                         */
/*==============================================================*/
create index idx_storeInventoryMonthly_pid on StoreInventoryMonthly
(
   ProductId
);

/*==============================================================*/
/* Table: StorePurchaseOrder                                    */
/*==============================================================*/
create table StorePurchaseOrder
(
   Id                   int not null auto_increment comment '���',
   Code                 nvarchar(20) comment '������',
   OrderType            int comment '��������: �� 1 �� 2',
   StoreId              int comment '�ŵ�Id',
   SupplierBill         nvarchar(200) comment '��Ӧ�̵��ݺ�',
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
   ContractPrice        decimal(8,4) comment '��ͬ��',
   Price                decimal(8,4) comment '����',
   SpecificationQuantity int comment '����',
   Quantity             int comment '����',
   ActualQuantity       int comment 'ʵ������',
   ProductionDate       datetime comment '��������',
   ShelfLife            int comment '������',
   BatchNo              bigint comment '���κ�',
   primary key (Id)
);

alter table StorePurchaseOrderItem comment '�ŵ�ɹ�������ϸ';

/*==============================================================*/
/* Table: Supplier                                              */
/*==============================================================*/
create table Supplier
(
   Id                   int not null auto_increment comment '���',
   Code                 nvarchar(20) comment '��Ӧ�̱���',
   Name                 nvarchar(100) comment '��Ӧ����',
   Type                 int comment '��Ӧ�����',
   ShortName            nvarchar(50) comment '���',
   Contact              nvarchar(300) comment '��ϵ��',
   Phone                nvarchar(300) comment '��ϵ�绰',
   QQ                   nvarchar(300),
   Address              nvarchar(100),
   Bank                 nvarchar(50) comment '������',
   BankAccount          nvarchar(50) comment '�������˺�',
   BankAccountName      varchar(50) comment '������',
   TaxNo                nvarchar(50) comment '˰��',
   LicenseNo            nvarchar(50) comment 'ִ�պ�',
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
   Price                decimal(8,4) comment '�۸�',
   Status               int comment '����״̬',
   CompareStatus        int comment '�ȼ�״̬',
   UpdatedOn            datetime comment '�޸�ʱ��',
   UpdatedBy            int comment '�޸���',
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
/* Index: idx_transaferOrder_code                               */
/*==============================================================*/
create unique index idx_transaferOrder_code on TransferOrder
(
   Code
);

/*==============================================================*/
/* Table: TransferOrderItem                                     */
/*==============================================================*/
create table TransferOrderItem
(
   Id                   int not null auto_increment comment '���',
   TransferOrderId      int comment '������ID',
   SupplierId           int comment '��Ӧ��Id',
   ProductId            int comment 'SKU����',
   Quantity             int comment '����',
   ContractPrice        decimal(8,4) comment '��ͬ��',
   Price                decimal(8,4) comment '�ɱ���',
   BatchNo              bigint comment '����',
   ProductionDate       datetime comment '��������',
   ShelfLife            int comment '������',
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
/* Index: idx_vipProduct_productid                              */
/*==============================================================*/
create unique index idx_vipProduct_productid on VipProduct
(
   ProductId
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

/*==============================================================*/
/* Table: WorkSchedule                                          */
/*==============================================================*/
create table WorkSchedule
(
   Id                   int not null auto_increment,
   Code                 nvarchar(50) comment '����',
   StoreId              int comment '�ŵ�',
   PosId                int comment 'Pos��Id',
   CashAmount           decimal(8,2) comment '���ֽ��',
   StartDate            datetime comment '��ʼʱ��',
   EndDate              datetime comment '����ʱ��',
   CreatedBy            int comment '������',
   CreatedByName        varchar(50) comment '��������',
   EndBy                int comment '������',
   EndByName            varchar(50) comment '��������',
   primary key (Id)
);

alter table WorkSchedule comment '��μ�¼��';

/*==============================================================*/
/* Index: idx_workschedule_code                                 */
/*==============================================================*/
create unique index idx_workschedule_code on WorkSchedule
(
   Code
);

