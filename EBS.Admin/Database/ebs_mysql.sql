/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2016/10/30 22:15:11                          */
/*==============================================================*/


drop index idx_account_username on Account;

drop table if exists Account;

drop table if exists AccountLoginHistory;

drop table if exists Area;

drop table if exists Brand;

drop table if exists Category;

drop table if exists Inventory;

drop table if exists InventoryHistory;

drop table if exists Menu;

drop index idx_pcodeseq_guidcode on ProductCodeSequence;

drop table if exists ProductCodeSequence;

drop index idx_productSKU_code on ProductSKU;

drop table if exists ProductSKU;

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

drop table if exists StoreInventoryHistory;

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
   ProductSKUId         int comment 'SKU编码',
   WarehouseId          int comment '仓库编码',
   Quantity             int comment '实际库存数',
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
   ProductSKUId         int comment 'SKU编码',
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
/* Table: ProductSKU                                            */
/*==============================================================*/
create table ProductSKU
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
   MarketPrice          decimal(8,2) comment '市场价',
   SalePrice            decimal(8,2) comment '销售价',
   WholeSalePrice       decimal(8,2) comment '批发价',
   CostPrice            decimal(8,2) comment '平均成本价',
   SubSkuCode           varchar(20) comment '子SKU代码',
   SubSkuQuantity       int comment '子SKU数量',
   CreatedOn            datetime comment '创建时间',
   primary key (Id)
);

alter table ProductSKU comment '商品SKU';

/*==============================================================*/
/* Index: idx_productSKU_code                                   */
/*==============================================================*/
create unique index idx_productSKU_code on ProductSKU
(
   Code
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
   ProductSKUId         int comment '商品skuid',
   CostPrice            decimal(8,2) comment '成本价',
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
   StoreId              int comment '门店Id',
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
   ProductSKUId         int comment '商品skuid',
   CostPrice            decimal(8,2) comment '成本价',
   Quantity             int comment '数量',
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
   Name                 nvarchar(128) comment '门店名',
   SourceKey            nvarchar(32) comment '门店唯一码',
   CreatedOn            datetime comment '创建时间',
   CreatedBy            int comment '创建人',
   Address              nvarchar(512) comment '地址',
   Contact              nvarchar(32) comment '联系人',
   Phone                nvarchar(32) comment '联系电话',
   Setting              text comment '设置',
   primary key (Id)
);

alter table Store comment '门店';

/*==============================================================*/
/* Table: StoreInventory                                        */
/*==============================================================*/
create table StoreInventory
(
   Id                   int not null comment '编号',
   ProductSKUId         int comment 'SKU编码',
   StoreId              int comment '门店编码',
   SaleQuantity         int comment '销售库存',
   OrderQuantity        int comment '订购库存',
   Quantity             int comment '实际库存数',
   WarnQuantity         int comment '警告库存',
   IsQuit               bool comment '是否退出',
   primary key (Id)
);

alter table StoreInventory comment '门店库存';

/*==============================================================*/
/* Table: StoreInventoryHistory                                 */
/*==============================================================*/
create table StoreInventoryHistory
(
   Id                   int not null comment '编号',
   ProductSKUId         int comment 'SKU编码',
   StoreId              int comment '仓库编码',
   Quantity             int comment '实际库存数',
   ChangeQuantity       int comment '变动数',
   CreatedOn            datetime comment '创建时间',
   CreatedBy            int comment '创建人',
   BillId               int comment '单据系统码',
   BillCode             varchar(20) comment '单据编码'
);

alter table StoreInventoryHistory comment '门店库存历史记录';

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
   Region               nvarchar(50) comment '区域',
   primary key (Id)
);

alter table Warehouse comment '仓库';

