/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2017-12-15 10:19:13                          */
/*==============================================================*/


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
/* Table: OutInOrder                                            */
/*==============================================================*/
create table OutInOrder
(
   Id                   int not null auto_increment comment '���',
   Code                 varchar(20) not null comment '����',
   StoreId              int not null comment '�ŵ�id',
   SupplierId           int comment '��Ӧ��Id',
   Status               int not null comment '״̬',
   OutInOrderTypeId     int not null comment '��������',
   CreatedOn            datetime not null comment '����ʱ��',
   CreatedBy            int not null comment '������',
   CreatedByName        varchar(20) comment '��������',
   UpdatedOn            datetime not null comment '�޸�ʱ��',
   UpdatedBy            int not null comment '�޸���',
   UpdatedByName        varchar(20) comment '�޸�����',
   Remark               varchar(1000) comment '��ע',
   primary key (Id)
);

alter table OutInOrder comment '��������ⵥ';

/*==============================================================*/
/* Index: idx_OutInOrder_Code                                   */
/*==============================================================*/
create unique index idx_OutInOrder_Code on OutInOrder
(
   Code
);

/*==============================================================*/
/* Index: idx_OutInOrder_UpdateStoreIdStats                     */
/*==============================================================*/
create index idx_OutInOrder_UpdateStoreIdStats on OutInOrder
(
   StoreId,
   Status,
   UpdatedOn
);

/*==============================================================*/
/* Table: OutInOrderItem                                        */
/*==============================================================*/
create table OutInOrderItem
(
   Id                   int not null auto_increment comment '���',
   OutInOrderId         int not null,
   ProductId            int not null comment 'SKU����',
   Quantity             int not null comment '����',
   CostPrice            decimal(12,4) not null comment 'ʵ�ʳɱ���',
   LastCostPrice        decimal(12,4) not null comment '�������',
   PlusMinus            smallint not null comment '������������⣬�������⣩',
   primary key (Id)
);

alter table OutInOrderItem comment '��������ⵥ��ϸ';

/*==============================================================*/
/* Table: OutInOrderType                                        */
/*==============================================================*/
create table OutInOrderType
(
   Id                   int not null auto_increment,
   TypeName             varchar(32) not null,
   OutInInventory       smallint not null comment '����/������',
   primary key (Id)
);

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
   AppId                varchar(64),
   Status               int,
   CreateTime           datetime,
   primary key (SysNo)
);

alter table Pay_Request comment '֧������';

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
   SalePrice            decimal(12,2),
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
   PreInventoryQuantity int comment '�ڳ����',
   PreInventoryAmount   decimal(12,4) comment '�ڳ������',
   PurchaseQuantity     int comment '���������',
   PurchaseAmount       decimal(12,4) comment '���������',
   SaleQuantity         int comment '����������',
   SaleCostAmount       decimal(12,4) comment '�������۳ɱ����',
   SaleAmount           decimal(12,2) comment '�������۽��',
   EndInventoryQuantity int comment '��ĩ�����',
   EndInventoryAmount   decimal(12,4) comment '��ĩ�����',
   AvgCostPrice         decimal(12,4) comment '�ɱ�����',
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
   Code                 nvarchar(64) comment '����',
   StoreId              int comment '�ŵ�',
   PosId                int comment 'Pos��Id',
   OrderType            int comment '��������',
   RefundAccount        varchar(60) comment '�˿��˺�',
   Status               int comment '״̬',
   OrderAmount          decimal(12,2) comment '�������',
   PayAmount            decimal(12,2) comment '�ֽ�֧�����',
   OnlinePayAmount      decimal(12,2) comment '����֧�����',
   PaymentWay           int comment '֧����ʽ',
   PaidDate             datetime comment '֧��ʱ��',
   Hour                 int comment 'ʱ��',
   CreatedOn            datetime comment '����ʱ��',
   CreatedBy            int comment '������',
   UpdatedOn            datetime comment '�޸�ʱ��',
   UpdatedBy            int comment '�޸���',
   WorkScheduleCode     varchar(32) comment '��δ���',
   OrderLevel           int comment '��������1 ��ͨ������2 Vip����',
   SourceSaleOrderCode  varchar(64)
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
   AvgCostPrice         decimal(12,2) comment 'ƽ���ɱ���',
   SalePrice            decimal(12,2) comment '���ۼ�',
   RealPrice            decimal(12,2) comment 'ʵ���ۼ�',
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
/* Table: SaleReport                                            */
/*==============================================================*/
create table SaleReport
(
   StoreInventoryHistoryId int not null comment '�����ˮId',
   SaleOrderId          int comment '���۱���',
   ProductId            int comment '��ƷId',
   OrderType            int comment '��������',
   PaymentWay           int comment '֧����ʽ',
   OrderLevel           int comment '��������1 ��ͨ������2 Vip����',
   StoreId              int comment '�ŵ�',
   SupplierId           int comment '��Ӧ��Id',
   CostPrice            decimal(8,4) comment '�ɱ���',
   SalePrice            decimal(8,2) comment '���ۼ�',
   RealPrice            decimal(8,2) comment 'ʵ���ۼ�',
   Quantity             int comment '����',
   CreatedOn            datetime comment '����ʱ��',
   CreatedBy            int comment '������',
   UpdatedOn            datetime comment '�޸�ʱ��',
   primary key (StoreInventoryHistoryId)
);

alter table SaleReport comment '�� storeinventoryHistory  ����ȡ���������ݣ�������';

/*==============================================================*/
/* Index: idx_saleReport_CreatedOnPIdSId                        */
/*==============================================================*/
create index idx_saleReport_CreatedOnPIdSId on SaleReport
(
   CreatedOn
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
create unique index idx_SaleSync on SaleSync
(
   SaleDate,
   StoreId,
   PosId
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
   RowVersion           timestamp(0) NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP(0) COMMENT '�а汾',
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
   RowVersion           timestamp(0) NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP(0) COMMENT '�а汾',
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
   Price                decimal(14,4) comment '����',
   SupplierId           int comment '��Ӧ��Id',
   SalePrice            decimal(10,2) comment '�ۼ�',
   primary key (Id)
);

alter table StoreInventoryHistory comment '�ŵ�����ʷ��¼';

/*==============================================================*/
/* Index: idx_storeinventoryhistory_billCode                    */
/*==============================================================*/
create index idx_storeinventoryhistory_billCode on StoreInventoryHistory
(
   BillCode
);

/*==============================================================*/
/* Index: idx_SIHistory_CreatedOnStoreIdProductid               */
/*==============================================================*/
create index idx_SIHistory_CreatedOnStoreIdProductid on StoreInventoryHistory
(
   ProductId,
   StoreId,
   CreatedOn
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

-- ----------------------------
-- Table structure for payment_history
-- ----------------------------
DROP TABLE IF EXISTS `payment_history`;
CREATE TABLE `payment_history`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT COMMENT 'Id',
  `OrderCode` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '������',
  `OrderType` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '��������',
  `PaymentType` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '֧�����ͣ�΢�ţ�֧����������',
  `Amount` varchar(255) NOT NULL COMMENT '���(��λ��)',
  `RefundCode` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '�˿��',
  `TradeNo` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '֧����ҵ���׺�',
  `TradeAction` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '���׶��� request.pay  ;response.pay.notify;request.refund;response.refund.notify ',
  `RequestUrl` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '����Url',
  `RawData` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT 'ԭʼ��������',
  `CreatedOn` datetime(6) NOT NULL COMMENT '����ʱ��',
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `idx_payhistory_ordercode`(`OrderCode`, `RefundCode`, `TradeAction`, `OrderType`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;


-- ----------------------------
-- Table structure for setting
-- ----------------------------
DROP TABLE IF EXISTS `setting`;
CREATE TABLE `setting`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `KeyTitle` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT 'key���⣬��ʾkey����',
  `KeyName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT 'key���֣�����ʹ��',
  `ValueTitle` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT 'ֵ����',
  `Value` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT 'ֵ',
  `StoreId` int(11) NULL DEFAULT NULL COMMENT '�ŵ�Id,���ŵ�Ϊ 0',
  `DisplayOrder` int(11) NULL DEFAULT NULL COMMENT '��ʾ˳��',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 10 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of setting
-- ----------------------------
INSERT INTO `setting` VALUES (1, '��������', 'system.domain', '', 'http://localhost', 0, 0);
INSERT INTO `setting` VALUES (2, '֧���ص�url', 'pay.notify.url', '', '/Pay/Notify', 0, 0);
INSERT INTO `setting` VALUES (3, '֧����תurl', 'pay.return.url', '', '/Pay/Return', 0, 0);
INSERT INTO `setting` VALUES (4, '֧����appid', 'pay.alipay.appid', '', 'alipay1', 0, 0);
INSERT INTO `setting` VALUES (5, '֧��������', 'pay.alipay.public.key', '', '', 0, 0);
INSERT INTO `setting` VALUES (6, '֧����˽��', 'pay.alipay.private.key', '', '', 0, 0);
INSERT INTO `setting` VALUES (7, '΢��appid', 'pay.wechat.appid', '', 'wx2428e34e0e7dc6ef', 0, 0);
INSERT INTO `setting` VALUES (8, '΢���ܳ�', 'pay.wechat.appsecret', '', '51c56b886b5be869567dd389b3e5d1d6', 0, 0);
INSERT INTO `setting` VALUES (9, '΢���̻���', 'pay.wechat.mchid', '', '1233410002', 0, 0);
INSERT INTO `setting` VALUES (10, '΢���̻��ܳ�', 'pay.wechat.mchkey', '', 'e10adc3849ba56abbe56e056f20f883e', 0, 0);

-- ----------------------------
-- Records of outinordertype
-- ----------------------------

INSERT INTO `outinordertype` VALUES (1, '�ڳ�ת��', 1);
INSERT INTO `outinordertype` VALUES (2, '��ĩת��', -1);
INSERT INTO `outinordertype` VALUES (3, '��ӯ����', 1);
INSERT INTO `outinordertype` VALUES (4, '�̿�����', -1);
INSERT INTO `outinordertype` VALUES (5, '��Ʒ����', 1);
INSERT INTO `outinordertype` VALUES (6, '��Ʒ����', -1);

