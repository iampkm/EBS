/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2016-11-10 09:47:18                          */
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

drop table if exists Warehouse;

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
   StoreId              int comment '门店',
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
/* Table: AdjustContractPrice                                   */
/*==============================================================*/
create table AdjustContractPrice
(
   Id                   int not null auto_increment comment '编号',
   Code                 nvarchar(50) comment '调价单号',
   Name                 nvarchar(50) comment '调价名',
   StoreId              int comment '门店Id',
   SupplierId           int comment '供应商Id',
   StartDate            datetime comment '开始日期',
   EndDate              datetime comment '结束日期',
   CreatedOn            datetime comment '创建时间',
   CreatedBy            int comment '创建人',
   UpdatedOn            datetime comment '修改时间',
   UpdatedBy            int comment '修改人',
   Status               int comment '状态',
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
   Id                   int not null auto_increment comment '编号',
   AdjustContractPriceId int comment '调价单编码',
   ProductId            int comment '商品编号',
   OldContractPrice     decimal(8,2) comment '原合同价',
   ContractPrice        decimal(8,2) comment '先合同价',
   primary key (Id)
);

alter table AdjustContractPriceItem comment '调整合明细';

/*==============================================================*/
/* Table: AdjustSalePrice                                       */
/*==============================================================*/
create table AdjustSalePrice
(
   Id                   int not null auto_increment comment '编号',
   Code                 nvarchar(50) comment '调价单号',
   Name                 nvarchar(50) comment '调价名称',
   StoreId              int comment '门店Id',
   SupplierId           int comment '供应商Id',
   StartDate            datetime comment '开始日期',
   EndDate              datetime comment '结束日期',
   CreatedOn            datetime comment '创建时间',
   CreatedBy            int comment '创建人',
   UpdatedOn            datetime comment '修改时间',
   UpdatedBy            int comment '修改人',
   Status               int comment '状态',
   primary key (Id)
);

alter table AdjustSalePrice comment '调整售价';

/*==============================================================*/
/* Table: AdjustSalePriceItem                                   */
/*==============================================================*/
create table AdjustSalePriceItem
(
   Id                   int not null auto_increment comment '编号',
   AdjustSalePriceId    int comment '调价单编码',
   SalePrice            decimal(8,2) comment '先售价',
   OldSalePrice         decimal(8,2) comment '原售价',
   ProductId            int comment '商品编号',
   primary key (Id)
);

alter table AdjustSalePriceItem comment '调整售价明细';

/*==============================================================*/
/* Table: Area                                                  */
/*==============================================================*/
create table Area
(
   Id                   char(6) not null comment '编号',
   Name                 nvarchar(64) comment '区域名',
   ShowName             nvarchar(64) comment '显示名称',
   FullName             nvarchar(256) comment '区域全民',
   Level                int comment '层级',
   primary key (Id)
);

alter table Area comment '区域表';

/*==============================================================*/
/* Table: BillSequence                                          */
/*==============================================================*/
create table BillSequence
(
   Id                   int not null auto_increment,
   GuidCode             nvarchar(32) comment 'guid代码',
   primary key (Id)
);

alter table BillSequence comment '单据序列号';

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
   Id                   int not null auto_increment comment '编号',
   Name                 nvarchar(128) comment '名称',
   primary key (Id)
);

alter table Brand comment '品牌';

/*==============================================================*/
/* Table: Category                                              */
/*==============================================================*/
create table Category
(
   Id                   nvarchar(18) not null comment '编号',
   Name                 nvarchar(64) comment '分类名',
   FullName             nvarchar(256) comment '全名',
   Level                int comment '层级',
   primary key (Id)
);

alter table Category comment '商品分类';

/*==============================================================*/
/* Table: Inventory                                             */
/*==============================================================*/
create table Inventory
(
   Id                   int not null auto_increment comment '编号',
   ProductId            int comment '编码',
   WarehouseId          int comment '仓库编码',
   Quantity             int comment '实际库存数',
   AvgCostPrice         decimal(8,2) comment '平均成本价',
   WarnQuantity         int comment '警告库存',
   IsQuit               bool comment '是否退出',
   primary key (Id)
);

/*==============================================================*/
/* Table: InventoryHistory                                      */
/*==============================================================*/
create table InventoryHistory
(
   Id                   int not null auto_increment comment '编号',
   ProductId            int comment '商品Id',
   WarehouseId          int comment '仓库编码',
   Quantity             int comment '实际库存数',
   ChangeQuantity       int comment '变动数',
   CreatedOn            datetime comment '创建时间',
   BillId               int comment '单据系统码',
   BillCode             varchar(20) comment '单据编码',
   primary key (Id)
);

alter table InventoryHistory comment '库存历史记录';

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
/* Table: ProcessHistory                                        */
/*==============================================================*/
create table ProcessHistory
(
   Id                   int not null auto_increment comment '编号',
   CreatedBy            int comment '创建人',
   CreatedByName        nvarchar(64) comment '创建人名',
   CreatedOn            datetime comment '创建时间',
   Status               int comment '状态',
   FormId               int comment '表单Id',
   FormType             nvarchar(64) comment '表单类型',
   Remark               nvarchar(1000) comment '备注',
   primary key (Id)
);

alter table ProcessHistory comment '表单处理历史记录';

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
   Id                   int not null auto_increment comment '编号',
   Code                 nvarchar(20) comment '编码',
   Name                 nvarchar(50) comment '商品名',
   ShowName             nvarchar(500) comment '显示名称',
   SellingPoint         nvarchar(100) comment '卖点',
   CategoryId           nvarchar(18) comment '分类Id',
   BrandId              int comment '品牌Id',
   InputRate            decimal comment '进项税率',
   OutRate              decimal comment '销项税率',
   IsGift               bool comment '是否赠品',
   Length               decimal comment '长',
   Width                decimal comment '宽',
   Height               decimal comment '高',
   Weight               decimal comment '重量',
   Unit                 nvarchar(10) comment '单位',
   Description          text comment '详情描述',
   Keywords             nvarchar(200) comment '关键字',
   IsPublish            bool comment '是否上架',
   BarCode              nvarchar(50) comment '条码',
   Specification        nvarchar(200) comment '规格名',
   OldPrice             decimal(8,2) comment '原价',
   SalePrice            decimal(8,2) comment '销售价',
   WholeSalePrice       decimal(8,2) comment '批发价',
   SubSkuCode           varchar(20) comment '子SKU代码',
   SubSkuQuantity       int comment '子SKU数量',
   SpecificationQuantity nvarchar(100) comment '件规, 多个逗号分隔',
   CreatedOn            datetime comment '创建时间',
   primary key (Id)
);

alter table Product comment '商品';

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
   GuidCode             nvarchar(32) comment 'guid代码',
   primary key (Id)
);

alter table ProductCodeSequence comment '产品编码序列号，此表用来生成商品SKU 表中的 Code 字段';

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
   Id                   int not null auto_increment comment '编号',
   Code                 nvarchar(50) comment '合同号',
   Name                 nvarchar(50) comment '合同名称',
   StoreId              int comment '门店Id',
   SupplierId           int comment '供应商Id',
   Contact              nvarchar(32) comment '联系人',
   StartDate            datetime comment '开始日期',
   EndDate              datetime comment '结束日期',
   CreatedOn            datetime comment '创建时间',
   CreatedBy            int comment '创建人',
   UpdatedOn            datetime comment '修改时间',
   UpdatedBy            int comment '修改人',
   Status               int comment '状态',
   primary key (Id)
);

alter table PurchaseContract comment '采购合同';

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
   Id                   int not null auto_increment comment '编号',
   PurchaseContractId   int comment '采购合同编号',
   ProductId            int comment '商品skuid',
   ContractPrice        decimal(8,2) comment '合同价',
   primary key (Id)
);

alter table PurchaseContractItem comment '采购合同明细';

/*==============================================================*/
/* Table: PurchaseOrder                                         */
/*==============================================================*/
create table PurchaseOrder
(
   Id                   int not null auto_increment comment '编号',
   PurchaseContractId   int comment '采购合同编号',
   Code                 nvarchar(20) comment '订单号',
   Type                 int comment '单据类型:  进货 1，退货 2',
   WarehouseId          int comment '仓库Id',
   SupplierId           int comment '供应商Id',
   CreatedOn            datetime comment '创建时间',
   CreatedBy            int comment '创建人',
   UpdatedOn            datetime comment '修改时间',
   UpdatedBy            int comment '修改人',
   Status               int comment '状态',
   Total                decimal(8,2) comment '金额',
   primary key (Id)
);

alter table PurchaseOrder comment '采购订单';

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
   Id                   int not null auto_increment comment '编号',
   PurchaseOrderId      int comment '采购订单编号',
   ProductId            int comment '商品skuid',
   ContractPrice        decimal(8,2) comment '合同价',
   Price                decimal(8,2) comment '进价',
   Quantity             int comment '数量',
   ActualQuantity       int comment '实际数量',
   IsGift               bool comment '赠品是否赠品',
   primary key (Id)
);

alter table PurchaseOrderItem comment '采购订单明细';

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

/*==============================================================*/
/* Table: Store                                                 */
/*==============================================================*/
create table Store
(
   Id                   int not null auto_increment comment '编号',
   Code                 nvarchar(20) comment '代码',
   Number               int comment '编号',
   Name                 nvarchar(128) comment '门店名',
   SourceKey            nvarchar(32) comment '门店唯一码',
   CreatedOn            datetime comment '创建时间',
   CreatedBy            int comment '创建人',
   AreaId               char(6) comment '区域ID',
   Address              nvarchar(512) comment '地址',
   Contact              nvarchar(32) comment '联系人',
   Phone                nvarchar(32) comment '联系电话',
   Setting              text comment '设置',
   primary key (Id)
);

alter table Store comment '门店';

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
   Id                   int not null auto_increment comment '编号',
   StoreId              int comment '门店编码',
   ProductId            int comment '商品Id',
   SaleQuantity         int comment '销售库存',
   OrderQuantity        int comment '订购库存',
   Quantity             int comment '实际库存数',
   AvgCostPrice         decimal(8,2) comment '平均成本价',
   WarnQuantity         int comment '警告库存',
   IsQuit               bool comment '是否退出',
   primary key (Id)
);

alter table StoreInventory comment '门店库存';

/*==============================================================*/
/* Table: StoreInventoryBatch                                   */
/*==============================================================*/
create table StoreInventoryBatch
(
   Id                   int not null auto_increment comment '编号',
   ProductId            int comment 'SKU编码',
   StoreId              int comment '仓库编码',
   SupplierId           int comment '供应商Id',
   Quantity             int comment '实际库存数',
   ProductionDate       datetime comment '生产日期',
   ShelfLife            int comment '保质期',
   Price                decimal(8,2) comment '进价',
   CreatedOn            datetime comment '创建时间',
   CreatedBy            int comment '创建人',
   BatchNo              nvarchar(20) comment '批次号',
   primary key (Id)
);

alter table StoreInventoryBatch comment '门店商品批次';

/*==============================================================*/
/* Table: StoreInventoryHistory                                 */
/*==============================================================*/
create table StoreInventoryHistory
(
   Id                   int not null auto_increment comment '编号',
   ProductId            int comment 'SKU编码',
   StoreId              int comment '仓库编码',
   Quantity             int comment '实际库存数',
   ChangeQuantity       int comment '变动数',
   CreatedOn            datetime comment '创建时间',
   CreatedBy            int comment '创建人',
   BillId               int comment '单据系统码',
   BillCode             varchar(20) comment '单据编码',
   BillType             int comment '单据类型',
   BatchNo              nvarchar(20) comment '批次号',
   Price                decimal(8,2) comment '进价',
   primary key (Id)
);

alter table StoreInventoryHistory comment '门店库存历史记录';

/*==============================================================*/
/* Table: StorePurchaseOrder                                    */
/*==============================================================*/
create table StorePurchaseOrder
(
   Id                   int not null auto_increment comment '编号',
   Code                 nvarchar(20) comment '订单号',
   StoreId              int comment '门店Id',
   SupplierBill         nvarchar(20) comment '供应商单据号',
   SupplierId           int comment '供应商Id',
   CreatedOn            datetime comment '创建时间',
   CreatedBy            int comment '创建人',
   CreatedByName        nvarchar(50) comment '创建人名',
   ReceivedBy           int comment '收货人',
   ReceivedOn           datetime comment '收货日期',
   ReceivedByName       varchar(50) comment '收货人名',
   StoragedBy           int comment '入库人',
   StoragedByName       nvarchar(50) comment '入库人名',
   StoragedOn           datetime comment '入库日期',
   Status               int comment '状态',
   IsGift               bool comment '是否赠品',
   BatchNo              nvarchar(20) comment '批次号',
   primary key (Id)
);

alter table StorePurchaseOrder comment '门店采购订单';

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
   Id                   int not null auto_increment comment '编号',
   StorePurchaseOrderId int comment '门店采购订单编号',
   ProductId            int comment '商品skuid',
   ContractPrice        decimal(8,2) comment '合同价',
   Price                decimal(8,2) comment '进价',
   SpecificationQuantity int comment '件规',
   Quantity             int comment '数量',
   ActualQuantity       int comment '实际数量',
   ProductionDate       datetime comment '生产日期',
   ShelfLife            int comment '保质期',
   primary key (Id)
);

alter table StorePurchaseOrderItem comment '门店采购订单明细';

/*==============================================================*/
/* Table: Supplier                                              */
/*==============================================================*/
create table Supplier
(
   Id                   int not null auto_increment comment '编号',
   Code                 nvarchar(20) comment '合同号',
   Name                 nvarchar(100) comment '供应商名',
   Type                 int comment '供应商类别',
   ShortName            nvarchar(50) comment '简称',
   Contact              nvarchar(50) comment '联系人',
   Phone                nvarchar(50) comment '联系电话',
   QQ                   nvarchar(30),
   Address              nvarchar(100),
   Bank                 nvarchar(50) comment '开户行',
   BankAccount          nvarchar(50) comment '开户行账号',
   TaxNo                nvarchar(50) comment '税号',
   LicenseNo            nvarchar(50) comment '执照号',
   AreaId               char(6) comment '供货区域',
   CreatedOn            datetime comment '创建时间',
   CreatedBy            int comment '创建人',
   UpdatedOn            datetime comment '修改时间',
   UpdatedBy            int comment '修改人',
   primary key (Id)
);

alter table Supplier comment '供应商';

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
   Code                 nvarchar(20) comment '代码',
   Name                 nvarchar(50) comment '仓库名',
   AreaId               char(6) comment '区域',
   Address              nvarchar(100) comment '地址',
   primary key (Id)
);

alter table Warehouse comment '仓库';

