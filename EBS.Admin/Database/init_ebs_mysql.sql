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
   StoreId              int comment '门店ID',
   PosId                int comment 'pos机Id',
   CDKey                varchar(50) comment '序列号',
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
   CanViewStores        varchar(200) comment '可以查询门店',
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
   StoreId              int comment '门店Id',
   SupplierId           int comment '供应商Id',
   CreatedOn            datetime comment '创建时间',
   CreatedBy            int comment '创建人',
   UpdatedOn            datetime comment '修改时间',
   UpdatedBy            int comment '修改人',
   Status               int comment '状态',
   Remark               nvarchar(200) comment '备注',
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
   ContractPrice        decimal(8,4) comment '合同价',
   AdjustPrice          decimal(8,4) comment '调整价',
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
   AdjustPrice          decimal(8,2) comment '原售价',
   ProductId            int comment '商品编号',
   primary key (Id)
);

alter table AdjustSalePriceItem comment '调整售价明细';

/*==============================================================*/
/* Table: AdjustStorePrice                                      */
/*==============================================================*/
create table AdjustStorePrice
(
   Id                   int not null auto_increment comment '编号',
   Code                 nvarchar(50) comment '调价单号',
   StoreId              int comment '门店',
   CreatedOn            datetime comment '创建时间',
   CreatedBy            int comment '创建人',
   UpdatedOn            datetime comment '修改时间',
   UpdatedBy            int comment '修改人',
   Status               int comment '状态',
   Remark               varchar(500) comment '备注',
   primary key (Id)
);

alter table AdjustStorePrice comment '调整门店售价';

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
   Id                   int not null auto_increment comment '编号',
   AdjustStorePriceId   int comment '调价单编码',
   StoreSalePrice       decimal(8,2) comment '原售价',
   AdjustPrice          decimal(8,2) comment '调整售价',
   ProductId            int comment '商品编号',
   primary key (Id)
);

alter table AdjustStorePriceItem comment '调整售价明细';

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
/* Table: OutInOrder                                            */
/*==============================================================*/
create table OutInOrder
(
   Id                   int not null auto_increment comment '编号',
   Code                 varchar(20) not null comment '单号',
   StoreId              int not null comment '门店id',
   SupplierId           int comment '供应商Id',
   Status               int not null comment '状态',
   OutInOrderTypeId     int not null comment '出入库类别',
   CreatedOn            datetime not null comment '创建时间',
   CreatedBy            int not null comment '创建人',
   CreatedByName        varchar(20) comment '创建人名',
   UpdatedOn            datetime not null comment '修改时间',
   UpdatedBy            int not null comment '修改人',
   UpdatedByName        varchar(20) comment '修改人名',
   Remark               varchar(1000) comment '备注',
   primary key (Id)
);

alter table OutInOrder comment '其他出入库单';

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
   Id                   int not null auto_increment comment '编号',
   OutInOrderId         int not null,
   ProductId            int not null comment 'SKU编码',
   Quantity             int not null comment '数量',
   CostPrice            decimal(12,4) not null comment '实际成本价',
   LastCostPrice        decimal(12,4) not null comment '最近进价',
   PlusMinus            smallint not null comment '正负（正，入库，负，出库）',
   primary key (Id)
);

alter table OutInOrderItem comment '其他出入库单明细';

/*==============================================================*/
/* Table: OutInOrderType                                        */
/*==============================================================*/
create table OutInOrderType
(
   Id                   int not null auto_increment,
   TypeName             varchar(32) not null,
   OutInInventory       smallint not null comment '（入/出）库',
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

alter table Pay_Request comment '支付请求';

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
   Id                   int not null auto_increment comment '图片ID',
   Url                  varchar(500) comment '链接地址',
   Title                varchar(50) comment '图片标题',
   primary key (Id)
);

alter table Picture comment '图片';

/*==============================================================*/
/* Table: ProcessHistory                                        */
/*==============================================================*/
create table ProcessHistory
(
   Id                   int not null auto_increment comment '编号',
   CreatedBy            int comment '创建人',
   CreatedByName        varchar(50) comment '创建人名',
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
   Keywords             nvarchar(200) comment '关键字',
   BarCode              nvarchar(50) comment '条码',
   Specification        nvarchar(200) comment '规格名',
   OldPrice             decimal(8,2) comment '原价',
   SalePrice            decimal(8,2) comment '销售价',
   WholeSalePrice       decimal(8,2) comment '批发价',
   SubSkuCode           varchar(20) comment '子SKU代码',
   SubSkuQuantity       int comment '子SKU数量',
   SpecificationQuantity nvarchar(100) comment '件规, 多个逗号分隔',
   CreatedOn            datetime comment '创建时间',
   CreatedBy            int comment '创建人',
   UpdatedOn            datetime comment '修改时间',
   UpdatedBy            int comment '修改人',
   MadeIn               varchar(200) comment '产地',
   Grade                varchar(50) comment '等级',
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

alter table ProductAreaPrice comment '商品区域价，该表不再使用';

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
/* Table: ProductDetails                                        */
/*==============================================================*/
create table ProductDetails
(
   ProductId            int  not null,
   Description          text comment '详情描述',
   primary key (ProductId)
);

alter table ProductDetails comment '商品详情';

/*==============================================================*/
/* Table: ProductPicture                                        */
/*==============================================================*/
create table ProductPicture
(
   Id                   int not null comment '编号',
   ProductId            int comment '商品编号',
   PictureId            int comment '图片Id',
   primary key (Id)
);

/*==============================================================*/
/* Table: PurchaseContract                                      */
/*==============================================================*/
create table PurchaseContract
(
   Id                   int not null auto_increment comment '编号',
   Code                 nvarchar(50) comment '合同号',
   Name                 nvarchar(50) comment '合同名称',
   StoreIds             varchar(300) comment '供应门店，多个都好分割',
   SupplierId           int comment '供应商Id',
   Contact              nvarchar(32) comment '联系人',
   StartDate            datetime comment '开始日期',
   EndDate              datetime comment '结束日期',
   CreatedOn            datetime comment '创建时间',
   CreatedBy            int comment '创建人',
   UpdatedOn            datetime comment '修改时间',
   UpdatedBy            int comment '修改人',
   Status               int comment '状态',
   Remark               varchar(1000) comment '备注',
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
   ContractPrice        decimal(8,4) comment '合同价',
   Status               int comment '供货状态',
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
/* Table: PurchaseSaleInventory                                 */
/*==============================================================*/
create table PurchaseSaleInventory
(
   YearMonth            int not null comment '年',
   StoreId              int not null comment '门店',
   StoreName            varchar(100) comment '门店名',
   PreInventoryQuantity int comment '期初库存',
   PreInventoryAmount   decimal(12,4) comment '期初库存金额',
   PurchaseQuantity     int comment '本期入库数',
   PurchaseAmount       decimal(12,4) comment '本期入库金额',
   SaleQuantity         int comment '本期销售数',
   SaleCostAmount       decimal(12,4) comment '本期销售成本金额',
   SaleAmount           decimal(12,2) comment '本期销售金额',
   EndInventoryQuantity int comment '期末库存数',
   EndInventoryAmount   decimal(12,4) comment '期末库存金额',
   UpdatedOn            datetime comment '更新时间',
   primary key (YearMonth, StoreId)
);

alter table PurchaseSaleInventory comment '进销存报表';

/*==============================================================*/
/* Table: PurchaseSaleInventoryDetail                           */
/*==============================================================*/
create table PurchaseSaleInventoryDetail
(
   YearMonth            int not null comment '年月',
   StoreId              int not null comment '门店id',
   ProductId            int not null comment '商品编码',
   PreInventoryQuantity int comment '期初库存',
   PreInventoryAmount   decimal(12,4) comment '期初库存金额',
   PurchaseQuantity     int comment '本期入库数',
   PurchaseAmount       decimal(12,4) comment '本期入库金额',
   SaleQuantity         int comment '本期销售数',
   SaleCostAmount       decimal(12,4) comment '本期销售成本金额',
   SaleAmount           decimal(12,2) comment '本期销售金额',
   EndInventoryQuantity int comment '期末库存数',
   EndInventoryAmount   decimal(12,4) comment '期末库存金额',
   AvgCostPrice         decimal(12,4) comment '成本均价',
   UpdatedOn            datetime comment '更新时间',
   primary key (YearMonth, StoreId, ProductId)
);

alter table PurchaseSaleInventoryDetail comment '进销存明细报表';

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
/* Table: SaleOrder                                             */
/*==============================================================*/
create table SaleOrder
(
   Id                   int not null auto_increment,
   Code                 nvarchar(64) comment '编码',
   StoreId              int comment '门店',
   PosId                int comment 'Pos机Id',
   OrderType            int comment '订单类型',
   RefundAccount        varchar(60) comment '退款账号',
   Status               int comment '状态',
   OrderAmount          decimal(12,2) comment '订单金额',
   PayAmount            decimal(12,2) comment '现金支付金额',
   OnlinePayAmount      decimal(12,2) comment '在线支付金额',
   PaymentWay           int comment '支付方式',
   PaidDate             datetime comment '支付时间',
   Hour                 int comment '时段',
   CreatedOn            datetime comment '创建时间',
   CreatedBy            int comment '创建人',
   UpdatedOn            datetime comment '修改时间',
   UpdatedBy            int comment '修改人',
   WorkScheduleCode     varchar(32) comment '班次代码',
   OrderLevel           int comment '订单级别：1 普通订单，2 Vip订单',
   SourceSaleOrderCode  varchar(64)
   primary key (Id)
);

alter table SaleOrder comment '销售订单';

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
   SaleOrderId          int comment '销售编码',
   ProductId            int comment '商品Id',
   ProductCode          nvarchar(20) comment '商品编码',
   ProductName          nvarchar(50) comment '商品名',
   AvgCostPrice         decimal(12,2) comment '平均成本价',
   SalePrice            decimal(12,2) comment '销售价',
   RealPrice            decimal(12,2) comment '实际售价',
   Quantity             int comment '数量',
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
   StoreInventoryHistoryId int not null comment '库存流水Id',
   SaleOrderId          int comment '销售编码',
   ProductId            int comment '商品Id',
   OrderType            int comment '订单类型',
   PaymentWay           int comment '支付方式',
   OrderLevel           int comment '订单级别：1 普通订单，2 Vip订单',
   StoreId              int comment '门店',
   SupplierId           int comment '供应商Id',
   CostPrice            decimal(8,4) comment '成本价',
   SalePrice            decimal(8,2) comment '销售价',
   RealPrice            decimal(8,2) comment '实际售价',
   Quantity             int comment '数量',
   CreatedOn            datetime comment '创建时间',
   CreatedBy            int comment '创建人',
   UpdatedOn            datetime comment '修改时间',
   primary key (StoreInventoryHistoryId)
);

alter table SaleReport comment '从 storeinventoryHistory  中提取的销售数据，报表用';

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
   SaleDate             varchar(20) comment '销售日',
   StoreId              int comment '门店ID',
   PosId                int comment '收银机',
   OrderCount           int comment '订单数',
   OrderTotalAmount     decimal(8,2) comment '订单总金额',
   ClientUpdatedOn      datetime comment '上传时间',
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
   Id                   int not null auto_increment comment '编号',
   StoreId              int,
   Code                 varchar(20) comment '货架码',
   Number               int comment '货架码',
   Name                 varchar(50) comment '货架名',
   primary key (Id)
);

alter table Shelf comment '货架';

/*==============================================================*/
/* Table: ShelfLayer                                            */
/*==============================================================*/
create table ShelfLayer
(
   Id                   int not null auto_increment comment '编号',
   Code                 varchar(20) comment '货架码',
   Number               int comment '货架码',
   ShelfId              int comment '货架名',
   primary key (Id)
);

alter table ShelfLayer comment '货架层';

/*==============================================================*/
/* Table: ShelfLayerProduct                                     */
/*==============================================================*/
create table ShelfLayerProduct
(
   Id                   int not null auto_increment comment '编号',
   StoreId              int,
   Code                 varchar(20) comment '货架码',
   Number               int comment '货架码',
   ProductId            int comment '商品ID',
   Quantity             int,
   ShelfLayerId         int,
   primary key (Id)
);

/*==============================================================*/
/* Table: Stocktaking                                           */
/*==============================================================*/
create table Stocktaking
(
   Id                   int not null auto_increment comment '编号',
   StocktakingPlanId    int comment '盘点计划编号',
   Code                 nvarchar(20) comment '盘点单号',
   StocktakingType      int comment '盘点表类型1 盘点空表，2 盘点修正表',
   ShelfCode            nvarchar(20) comment '货架码',
   CreatedBy            int comment '创建人',
   CreatedByName        nvarchar(50) comment '创建人名',
   CreatedOn            datetime comment '创建时间',
   Status               int comment '状态（待审，已审）',
   UpdatedOn            datetime comment '修改时间',
   UpdatedBy            int comment '修改人',
   UpdatedByName        varchar(50) comment '修改人名',
   StoreId              int comment '门店',
   Note                 nvarchar(1000) comment '备注',
   primary key (Id)
);

alter table Stocktaking comment '盘点表';

/*==============================================================*/
/* Table: StocktakingItem                                       */
/*==============================================================*/
create table StocktakingItem
(
   Id                   int not null auto_increment comment '编号',
   StocktakingId        int,
   ProductId            nvarchar(50) comment '商品编码',
   CostPrice            decimal(8,4) comment '调拨成本价',
   SalePrice            decimal(8,2) comment '销售价',
   Quantity             int comment '盘点锁定库存数',
   CountQuantity        int comment '盘点数量',
   CorectQuantity       int comment '修正数',
   CorectReason         nvarchar(500) comment '修正原因',
   Note                 nvarchar(500) comment '备注',
   primary key (Id)
);

alter table StocktakingItem comment '盘点明细';

/*==============================================================*/
/* Table: StocktakingPlan                                       */
/*==============================================================*/
create table StocktakingPlan
(
   Id                   int not null auto_increment comment '编号',
   Code                 nvarchar(20) comment '盘点代码',
   CreatedBy            int comment '创建人',
   CreatedByName        nvarchar(50) comment '创建人名',
   CreatedOn            datetime comment '创建时间',
   UpdatedBy            int comment '更新人',
   UpdatedByName        nvarchar(50) comment '更新人名',
   UpdatedOn            datetime comment '更新时间',
   Method               int comment '盘点方式（大盘：小盘）',
   Status               int comment '盘点状态（待盘，初盘，复盘，终盘）',
   StoreId              int comment '门店编号',
   Note                 nvarchar(1000) comment '备注',
   StocktakingDate      datetime comment '盘点日期',
   primary key (Id)
);

alter table StocktakingPlan comment '盘点计划';

/*==============================================================*/
/* Table: StocktakingPlanItem                                   */
/*==============================================================*/
create table StocktakingPlanItem
(
   Id                   int not null auto_increment comment '编号',
   StocktakingPlanId    int comment '盘点计划编号',
   ProductId            int comment '系统编码',
   CostPrice            decimal(8,4) comment '调拨成本价',
   SalePrice            decimal(8,2) comment '销售价',
   Quantity             int comment '库存数量',
   CountQuantity        int comment '盘点数量',
   primary key (Id)
);

alter table StocktakingPlanItem comment '盘点计划明细';

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
   LicenseCode          varchar(50) comment '门店授权码',
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
   AvgCostPrice         decimal(8,4) comment '平均成本价',
   WarnQuantity         int comment '警告库存',
   IsQuit               bool comment '是否退出',
   LastCostPrice        decimal(8,2) comment '最新进价',
   StoreSalePrice       decimal(8,2) comment '门店售价',
   Status               int comment '状态',
   RowVersion           timestamp(0) NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP(0) COMMENT '行版本',
   primary key (Id)
);

alter table StoreInventory comment '门店库存';

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
   Id                   int not null auto_increment comment '编号',
   ProductId            int comment 'SKU编码',
   StoreId              int comment '仓库编码',
   SupplierId           int comment '供应商Id',
   Quantity             int comment '实际库存数',
   ProductionDate       datetime comment '生产日期',
   ShelfLife            int comment '保质期',
   ContractPrice        decimal(8,4) comment '合同价',
   Price                decimal(8,4) comment '实际入库进价',
   CreatedOn            datetime comment '创建时间',
   CreatedBy            int comment '创建人',
   BatchNo              bigint comment '批次号',
   RowVersion           timestamp(0) NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP(0) COMMENT '行版本',
   primary key (Id)
);

alter table StoreInventoryBatch comment '门店商品批次';

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
   BatchNo              bigint comment '批次号',
   Price                decimal(14,4) comment '进价',
   SupplierId           int comment '供应商Id',
   SalePrice            decimal(10,2) comment '售价',
   primary key (Id)
);

alter table StoreInventoryHistory comment '门店库存历史记录';

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
   Id                   int not null auto_increment comment '编号',
   Monthly              varchar(10) comment '会计期间 2017-01 按月存储',
   StoreId              int comment '门店编码',
   ProductId            int comment '商品Id',
   Quantity             int comment '实际库存数',
   AvgCostPrice         decimal(8,4) comment '平均成本价',
   primary key (Id)
);

alter table StoreInventoryMonthly comment '门店库存月报';

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
   Id                   int not null auto_increment comment '编号',
   Code                 nvarchar(20) comment '订单号',
   OrderType            int comment '单据类型: 进 1 退 2',
   StoreId              int comment '门店Id',
   SupplierBill         nvarchar(200) comment '供应商单据号',
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
   ContractPrice        decimal(8,4) comment '合同价',
   Price                decimal(8,4) comment '进价',
   SpecificationQuantity int comment '件规',
   Quantity             int comment '数量',
   ActualQuantity       int comment '实际数量',
   ProductionDate       datetime comment '生产日期',
   ShelfLife            int comment '保质期',
   BatchNo              bigint comment '批次号',
   primary key (Id)
);

alter table StorePurchaseOrderItem comment '门店采购订单明细';

/*==============================================================*/
/* Table: Supplier                                              */
/*==============================================================*/
create table Supplier
(
   Id                   int not null auto_increment comment '编号',
   Code                 nvarchar(20) comment '供应商编码',
   Name                 nvarchar(100) comment '供应商名',
   Type                 int comment '供应商类别',
   ShortName            nvarchar(50) comment '简称',
   Contact              nvarchar(300) comment '联系人',
   Phone                nvarchar(300) comment '联系电话',
   QQ                   nvarchar(300),
   Address              nvarchar(100),
   Bank                 nvarchar(50) comment '开户行',
   BankAccount          nvarchar(50) comment '开户行账号',
   BankAccountName      varchar(50) comment '开户名',
   TaxNo                nvarchar(50) comment '税号',
   LicenseNo            nvarchar(50) comment '执照号',
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
/* Table: SupplierProduct                                       */
/*==============================================================*/
create table SupplierProduct
(
   Id                   int not null auto_increment comment '编号',
   SupplierId           int comment '供应商Id',
   ProductId            int comment '商品',
   Price                decimal(8,4) comment '价格',
   Status               int comment '供货状态',
   CompareStatus        int comment '比价状态',
   UpdatedOn            datetime comment '修改时间',
   UpdatedBy            int comment '修改人',
   primary key (Id)
);

alter table SupplierProduct comment '供应商商品';

/*==============================================================*/
/* Table: TransferOrder                                         */
/*==============================================================*/
create table TransferOrder
(
   Id                   int not null auto_increment comment '编号',
   Code                 varchar(20) comment '调拨单号',
   FromStoreId          int comment '从门店',
   FromStoreName        char(50) comment '从门店名',
   ToStoreName          char(50) comment '到门店名',
   ToStoreId            int comment '到门店',
   Status               int comment '状态',
   CreatedOn            datetime comment '创建时间',
   CreatedBy            int comment '创建人',
   CreatedByName        varchar(30) comment '创建人名',
   UpdatedOn            datetime comment '修改时间',
   UpdatedBy            int comment '修改人',
   UpdatedByName        varchar(30) comment '修改人名',
   primary key (Id)
);

alter table TransferOrder comment '调拨单';

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
   Id                   int not null auto_increment comment '编号',
   TransferOrderId      int comment '调拨单ID',
   SupplierId           int comment '供应商Id',
   ProductId            int comment 'SKU编码',
   Quantity             int comment '数量',
   ContractPrice        decimal(8,4) comment '合同价',
   Price                decimal(8,4) comment '成本价',
   BatchNo              bigint comment '批次',
   ProductionDate       datetime comment '生产日期',
   ShelfLife            int comment '保质期',
   primary key (Id)
);

alter table TransferOrderItem comment '调拨明细';

/*==============================================================*/
/* Table: VipCard                                               */
/*==============================================================*/
create table VipCard
(
   Id                   int not null auto_increment,
   Code                 varchar(50) comment '会员卡号',
   Discount             decimal(8,2) comment '折扣',
   primary key (Id)
);

alter table VipCard comment '会员卡';

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
   Code                 nvarchar(20) comment '代码',
   Name                 nvarchar(50) comment '仓库名',
   AreaId               char(6) comment '区域',
   Address              nvarchar(100) comment '地址',
   primary key (Id)
);

alter table Warehouse comment '仓库';

/*==============================================================*/
/* Table: WorkSchedule                                          */
/*==============================================================*/
create table WorkSchedule
(
   Id                   int not null auto_increment,
   Code                 nvarchar(50) comment '代码',
   StoreId              int comment '门店',
   PosId                int comment 'Pos机Id',
   CashAmount           decimal(8,2) comment '收现金额',
   StartDate            datetime comment '开始时间',
   EndDate              datetime comment '结束时间',
   CreatedBy            int comment '创建人',
   CreatedByName        varchar(50) comment '创建人名',
   EndBy                int comment '交班人',
   EndByName            varchar(50) comment '交班人名',
   primary key (Id)
);

alter table WorkSchedule comment '班次记录表';

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
  `OrderCode` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '订单号',
  `OrderType` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '订单类型',
  `PaymentType` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '支付类型：微信，支付宝，银联',
  `Amount` varchar(255) NOT NULL COMMENT '金额(单位分)',
  `RefundCode` varchar(64) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '退款单号',
  `TradeNo` varchar(128) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '支付企业交易号',
  `TradeAction` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '交易动作 request.pay  ;response.pay.notify;request.refund;response.refund.notify ',
  `RequestUrl` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '请求Url',
  `RawData` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '原始报文数据',
  `CreatedOn` datetime(6) NOT NULL COMMENT '创建时间',
  PRIMARY KEY (`Id`) USING BTREE,
  INDEX `idx_payhistory_ordercode`(`OrderCode`, `RefundCode`, `TradeAction`, `OrderType`) USING BTREE
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;


-- ----------------------------
-- Table structure for setting
-- ----------------------------
DROP TABLE IF EXISTS `setting`;
CREATE TABLE `setting`  (
  `Id` int(11) NOT NULL AUTO_INCREMENT,
  `KeyTitle` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT 'key标题，提示key作用',
  `KeyName` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT 'key名字，程序使用',
  `ValueTitle` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NULL DEFAULT NULL COMMENT '值描述',
  `Value` varchar(2000) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci NOT NULL COMMENT '值',
  `StoreId` int(11) NULL DEFAULT NULL COMMENT '门店Id,无门店为 0',
  `DisplayOrder` int(11) NULL DEFAULT NULL COMMENT '显示顺序',
  PRIMARY KEY (`Id`) USING BTREE
) ENGINE = InnoDB AUTO_INCREMENT = 10 CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of setting
-- ----------------------------
INSERT INTO `setting` VALUES (1, '本机域名', 'system.domain', '', 'http://localhost', 0, 0);
INSERT INTO `setting` VALUES (2, '支付回调url', 'pay.notify.url', '', '/Pay/Notify', 0, 0);
INSERT INTO `setting` VALUES (3, '支付跳转url', 'pay.return.url', '', '/Pay/Return', 0, 0);
INSERT INTO `setting` VALUES (4, '支付宝appid', 'pay.alipay.appid', '', 'alipay1', 0, 0);
INSERT INTO `setting` VALUES (5, '支付宝公密', 'pay.alipay.public.key', '', '', 0, 0);
INSERT INTO `setting` VALUES (6, '支付宝私密', 'pay.alipay.private.key', '', '', 0, 0);
INSERT INTO `setting` VALUES (7, '微信appid', 'pay.wechat.appid', '', 'wx2428e34e0e7dc6ef', 0, 0);
INSERT INTO `setting` VALUES (8, '微信密匙', 'pay.wechat.appsecret', '', '51c56b886b5be869567dd389b3e5d1d6', 0, 0);
INSERT INTO `setting` VALUES (9, '微信商户号', 'pay.wechat.mchid', '', '1233410002', 0, 0);
INSERT INTO `setting` VALUES (10, '微信商户密匙', 'pay.wechat.mchkey', '', 'e10adc3849ba56abbe56e056f20f883e', 0, 0);

-- ----------------------------
-- Records of outinordertype
-- ----------------------------

INSERT INTO `outinordertype` VALUES (1, '期初转入', 1);
INSERT INTO `outinordertype` VALUES (2, '期末转出', -1);
INSERT INTO `outinordertype` VALUES (3, '盘盈调账', 1);
INSERT INTO `outinordertype` VALUES (4, '盘亏调账', -1);
INSERT INTO `outinordertype` VALUES (5, '商品益余', 1);
INSERT INTO `outinordertype` VALUES (6, '商品残损', -1);

