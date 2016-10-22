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
   Id                   int not null comment '编号',
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
   SaleQuantity         int comment '销售库存',
   OrderQuantity        int comment '订购库存',
   ActualQuantity       int comment '实际库存数',
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
   ActualQuantity       int comment '实际库存数',
   ChangeQuantity       int comment '变动数',
   CreatedOn            datetime comment '创建时间',
   CreatedBy            int comment '创建人',
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
/* Table: Product                                               */
/*==============================================================*/
create table Product
(
   Id                   int not null auto_increment comment '编号',
   Name                 nvarchar(50) comment '商品名',
   ShowName             nvarchar(500) comment '显示名称',
   SellingPoint         nvarchar(100) comment '卖点',
   CategoryId           int comment '分类Id',
   BrandId              int comment '品牌Id',
   InputRate            decimal comment '进项税率',
   OutRate              decimal comment '销项税率',
   IsGift               bool comment '是否赠品',
   Length               decimal comment '长',
   Width                decimal comment '宽',
   Height               decimal comment '高',
   Weight               decimal comment '重量',
   Description          text comment '详情描述',
   Keywords             nvarchar(1000) comment '关键字',
   IsPublish            bool comment '是否上架',
   CreatedOn            datetime comment '创建时间',
   primary key (Id)
);

alter table Product comment '商品表';

/*==============================================================*/
/* Table: ProductSKU                                            */
/*==============================================================*/
create table ProductSKU
(
   Id                   int not null auto_increment comment '编号',
   ProductId            int comment '商品编码',
   Code                 nvarchar(20) comment '编码',
   BarCode              nvarchar(50) comment '条码',
   SpecificationList    nvarchar(3000) comment '规格列表',
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
/* Table: ProductSpecification                                  */
/*==============================================================*/
create table ProductSpecification
(
   Id                   int not null auto_increment comment '编码',
   Name                 nvarchar(512) comment '属性名',
   CategoryId           varchar(18) comment '分类',
   primary key (Id)
);

alter table ProductSpecification comment '商品规格';

/*==============================================================*/
/* Table: ProductSpecificationMapping                           */
/*==============================================================*/
create table ProductSpecificationMapping
(
   Id                   int not null auto_increment comment '编号',
   ProductId            int comment '商品Id',
   ProductSpecificationId int comment '商品规格Id',
   ProductSpecificationOptionId int comment '规格选项Id',
   primary key (Id)
);

alter table ProductSpecificationMapping comment '商品规格映射';

/*==============================================================*/
/* Table: ProductSpecificationOption                            */
/*==============================================================*/
create table ProductSpecificationOption
(
   Id                   int not null auto_increment comment '编号',
   ProductSpecificationId int comment '规格项编码',
   Value                nvarchar(100) comment '值',
   primary key (Id)
);

alter table ProductSpecificationOption comment '商品规格选项';

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
   ActualQuantity       int comment '实际库存数',
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
   ActualQuantity       int comment '实际库存数',
   ChangeQuantity       int comment '变动数',
   CreatedOn            datetime comment '创建时间',
   CreatedBy            int comment '创建人',
   BillId               int comment '单据系统码',
   BillCode             varchar(20) comment '单据编码'
);

alter table StoreInventoryHistory comment '门店库存历史记录';

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

