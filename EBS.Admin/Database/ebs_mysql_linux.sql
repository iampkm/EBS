/*==============================================================*/
/* DBMS name:      MySQL 5.0                                    */
/* Created on:     2017-12-20 16:00:21                          */
/*==============================================================*/


drop index idx_accesstoken_cdkey on accesstoken;

drop table if exists accesstoken;

drop index idx_account_username on account;

drop table if exists account;

drop table if exists accountloginhistory;

drop index idx_adjustcontractprice_code on adjustcontractprice;

drop table if exists adjustcontractprice;

drop table if exists adjustcontractpriceitem;

drop table if exists adjustsaleprice;

drop table if exists adjustsalepriceitem;

drop index idx_adjuststoreprice_code on adjuststoreprice;

drop table if exists adjuststoreprice;

drop table if exists adjuststorepriceitem;

drop table if exists area;

drop index idx_billsequence_guidcode on billsequence;

drop table if exists billsequence;

drop table if exists brand;

drop table if exists category;

drop table if exists inventory;

drop table if exists inventoryhistory;

drop table if exists menu;

drop index idx_outinorder_updatestoreidstats on outinorder;

drop index idx_outinorder_code on outinorder;

drop table if exists outinorder;

drop table if exists outinorderitem;

drop table if exists outinordertype;

drop table if exists pay_app;

drop table if exists pay_notifyback;

drop index ix_pay_request_orderid_paytype on pay_request;

drop table if exists pay_request;

drop table if exists pay_result;

drop table if exists picture;

drop index idx_processhistory_fromid on processhistory;

drop table if exists processhistory;

drop index idx_product_barcode on product;

drop index idx_product_code on product;

drop table if exists product;

drop table if exists productareaprice;

drop index idx_pcodeseq_guidcode on productcodesequence;

drop table if exists productcodesequence;

drop table if exists productdetails;

drop table if exists productpicture;

drop index idx_purcontract_code on purchasecontract;

drop table if exists purchasecontract;

drop table if exists purchasecontractitem;

drop index idx_purchaseorder_code on purchaseorder;

drop table if exists purchaseorder;

drop table if exists purchaseorderitem;

drop table if exists purchasesaleinventory;

drop table if exists purchasesaleinventorydetail;

drop table if exists role;

drop table if exists rolemenu;

drop index idx_saleorder_storeidandupdatedon on saleorder;

drop index idx_saleorder_code on saleorder;

drop table if exists saleorder;

drop index idx_saleorderitem_productid on saleorderitem;

drop index idx_saleorderitem_saleorderid on saleorderitem;

drop table if exists saleorderitem;

drop index idx_salereport_createdonpidsid on salereport;

drop table if exists salereport;

drop index idx_salesync on salesync;

drop table if exists salesync;

drop table if exists shelf;

drop table if exists shelflayer;

drop table if exists shelflayerproduct;

drop table if exists stocktaking;

drop table if exists stocktakingitem;

drop table if exists stocktakingplan;

drop table if exists stocktakingplanitem;

drop index idx_store_code on store;

drop table if exists store;

drop index idx_storeinventory_pidandstoreid on storeinventory;

drop index idx_storeinventory_pid on storeinventory;

drop table if exists storeinventory;

drop index idx_storeinventorybath_pid on storeinventorybatch;

drop table if exists storeinventorybatch;

drop index idx_sihistory_createdonstoreidproductid on storeinventoryhistory;

drop index idx_storeinventoryhistory_billcode on storeinventoryhistory;

drop table if exists storeinventoryhistory;

drop index idx_storeinventorymonthly_pid on storeinventorymonthly;

drop table if exists storeinventorymonthly;

drop index idx_storepurchaseorder_code on storepurchaseorder;

drop table if exists storepurchaseorder;

drop table if exists storepurchaseorderitem;

drop index idx_supplier_code on supplier;

drop table if exists supplier;

drop table if exists supplierproduct;

drop index idx_transaferorder_code on transferorder;

drop table if exists transferorder;

drop table if exists transferorderitem;

drop table if exists vipcard;

drop index idx_vipproduct_productid on vipproduct;

drop table if exists vipproduct;

drop table if exists warehouse;

drop index idx_workschedule_code on workschedule;

drop table if exists workschedule;

/*==============================================================*/
/* Table: accesstoken                                           */
/*==============================================================*/
create table accesstoken
(
   id                   int not null auto_increment,
   storeid              int comment '门店ID',
   posid                int comment 'pos机Id',
   cdkey                varchar(50) comment '序列号',
   primary key (id)
);

/*==============================================================*/
/* Index: idx_accesstoken_cdkey                                 */
/*==============================================================*/
create unique index idx_accesstoken_cdkey on accesstoken
(
   cdkey
);

/*==============================================================*/
/* Table: account                                               */
/*==============================================================*/
create table account
(
   id                   int not null auto_increment comment '编号',
   username             nvarchar(64) comment '账户名',
   password             nvarchar(64) comment '密码',
   nickname             nvarchar(64) comment '昵称',
   roleid               int comment '角色ID',
   createdon            datetime comment '创建时间',
   status               int comment '状态',
   loginerrorcount      int comment '登录错误次数',
   lastupdatedate       datetime comment '最后修改时间',
   storeid              int comment '门店',
   canviewstores        varchar(200) comment '可以查询门店',
   primary key (id)
);

alter table account comment '后台管理账户表';

/*==============================================================*/
/* Index: idx_account_username                                  */
/*==============================================================*/
create unique index idx_account_username on account
(
   username
);

/*==============================================================*/
/* Table: accountloginhistory                                   */
/*==============================================================*/
create table accountloginhistory
(
   id                   int not null auto_increment comment '编号',
   accountid            int comment '账号id',
   username             nvarchar(64) comment '登录账号',
   createdon            datetime comment '登录时间',
   ipaddress            nvarchar(64) comment 'IP地址',
   loginstatus          int comment '登录状态',
   primary key (id)
);

alter table accountloginhistory comment '账号登录历史';

/*==============================================================*/
/* Table: adjustcontractprice                                   */
/*==============================================================*/
create table adjustcontractprice
(
   id                   int not null auto_increment comment '编号',
   code                 nvarchar(50) comment '调价单号',
   storeid              int comment '门店Id',
   supplierid           int comment '供应商Id',
   createdon            datetime comment '创建时间',
   createdby            int comment '创建人',
   updatedon            datetime comment '修改时间',
   updatedby            int comment '修改人',
   status               int comment '状态',
   remark               nvarchar(200) comment '备注',
   primary key (id)
);

/*==============================================================*/
/* Index: idx_adjustcontractprice_code                          */
/*==============================================================*/
create unique index idx_adjustcontractprice_code on adjustcontractprice
(
   code
);

/*==============================================================*/
/* Table: adjustcontractpriceitem                               */
/*==============================================================*/
create table adjustcontractpriceitem
(
   id                   int not null auto_increment comment '编号',
   adjustcontractpriceid int comment '调价单编码',
   productid            int comment '商品编号',
   contractprice        decimal(8,4) comment '合同价',
   adjustprice          decimal(8,4) comment '调整价',
   primary key (id)
);

alter table adjustcontractpriceitem comment '调整合明细';

/*==============================================================*/
/* Table: adjustsaleprice                                       */
/*==============================================================*/
create table adjustsaleprice
(
   id                   int not null auto_increment comment '编号',
   code                 nvarchar(50) comment '调价单号',
   createdon            datetime comment '创建时间',
   createdby            int comment '创建人',
   updatedon            datetime comment '修改时间',
   updatedby            int comment '修改人',
   status               int comment '状态',
   primary key (id)
);

alter table adjustsaleprice comment '调整售价';

/*==============================================================*/
/* Table: adjustsalepriceitem                                   */
/*==============================================================*/
create table adjustsalepriceitem
(
   id                   int not null auto_increment comment '编号',
   adjustsalepriceid    int comment '调价单编码',
   saleprice            decimal(8,2) comment '先售价',
   adjustprice          decimal(8,2) comment '原售价',
   productid            int comment '商品编号',
   primary key (id)
);

alter table adjustsalepriceitem comment '调整售价明细';

/*==============================================================*/
/* Table: adjuststoreprice                                      */
/*==============================================================*/
create table adjuststoreprice
(
   id                   int not null auto_increment comment '编号',
   code                 nvarchar(50) comment '调价单号',
   storeid              int comment '门店',
   createdon            datetime comment '创建时间',
   createdby            int comment '创建人',
   updatedon            datetime comment '修改时间',
   updatedby            int comment '修改人',
   status               int comment '状态',
   remark               varchar(500) comment '备注',
   primary key (id)
);

alter table adjuststoreprice comment '调整门店售价';

/*==============================================================*/
/* Index: idx_adjuststoreprice_code                             */
/*==============================================================*/
create unique index idx_adjuststoreprice_code on adjuststoreprice
(
   code
);

/*==============================================================*/
/* Table: adjuststorepriceitem                                  */
/*==============================================================*/
create table adjuststorepriceitem
(
   id                   int not null auto_increment comment '编号',
   adjuststorepriceid   int comment '调价单编码',
   storesaleprice       decimal(8,2) comment '原售价',
   adjustprice          decimal(8,2) comment '调整售价',
   productid            int comment '商品编号',
   primary key (id)
);

alter table adjuststorepriceitem comment '调整售价明细';

/*==============================================================*/
/* Table: area                                                  */
/*==============================================================*/
create table area
(
   id                   char(6) not null comment '编号',
   name                 nvarchar(64) comment '区域名',
   showname             nvarchar(64) comment '显示名称',
   fullname             nvarchar(256) comment '区域全民',
   level                int comment '层级',
   primary key (id)
);

alter table area comment '区域表';

/*==============================================================*/
/* Table: billsequence                                          */
/*==============================================================*/
create table billsequence
(
   id                   int not null auto_increment,
   guidcode             nvarchar(32) comment 'guid代码',
   primary key (id)
);

alter table billsequence comment '单据序列号';

/*==============================================================*/
/* Index: idx_billsequence_guidcode                             */
/*==============================================================*/
create unique index idx_billsequence_guidcode on billsequence
(
   guidcode
);

/*==============================================================*/
/* Table: brand                                                 */
/*==============================================================*/
create table brand
(
   id                   int not null auto_increment comment '编号',
   name                 nvarchar(128) comment '名称',
   primary key (id)
);

alter table brand comment '品牌';

/*==============================================================*/
/* Table: category                                              */
/*==============================================================*/
create table category
(
   id                   nvarchar(18) not null comment '编号',
   name                 nvarchar(64) comment '分类名',
   fullname             nvarchar(256) comment '全名',
   level                int comment '层级',
   primary key (id)
);

alter table category comment '商品分类';

/*==============================================================*/
/* Table: inventory                                             */
/*==============================================================*/
create table inventory
(
   id                   int not null auto_increment comment '编号',
   productid            int comment '编码',
   warehouseid          int comment '仓库编码',
   quantity             int comment '实际库存数',
   avgcostprice         decimal(8,2) comment '平均成本价',
   warnquantity         int comment '警告库存',
   isquit               bool comment '是否退出',
   primary key (id)
);

/*==============================================================*/
/* Table: inventoryhistory                                      */
/*==============================================================*/
create table inventoryhistory
(
   id                   int not null auto_increment comment '编号',
   productid            int comment '商品Id',
   warehouseid          int comment '仓库编码',
   quantity             int comment '实际库存数',
   changequantity       int comment '变动数',
   createdon            datetime comment '创建时间',
   billid               int comment '单据系统码',
   billcode             varchar(20) comment '单据编码',
   primary key (id)
);

alter table inventoryhistory comment '库存历史记录';

/*==============================================================*/
/* Table: menu                                                  */
/*==============================================================*/
create table menu
(
   id                   int not null auto_increment comment '编号',
   parentid             int comment '父编号',
   name                 nvarchar(64) comment '名称',
   url                  nvarchar(256) comment '连接',
   icon                 nvarchar(64) comment '图标',
   displayorder         int comment '显示顺序',
   urltype              int comment '连接类型',
   primary key (id)
);

alter table menu comment '系统菜单';

/*==============================================================*/
/* Table: outinorder                                            */
/*==============================================================*/
create table outinorder
(
   id                   int not null auto_increment comment '编号',
   code                 varchar(20) not null comment '单号',
   storeid              int not null comment '门店id',
   supplierid           int comment '供应商Id',
   status               int not null comment '状态',
   outinordertypeid     int not null comment '出入库类别',
   createdon            datetime not null comment '创建时间',
   createdby            int not null comment '创建人',
   createdbyname        varchar(20) comment '创建人名',
   updatedon            datetime not null comment '修改时间',
   updatedby            int not null comment '修改人',
   updatedbyname        varchar(20) comment '修改人名',
   remark               varchar(1000) comment '备注',
   primary key (id)
);

alter table outinorder comment '其他出入库单';

/*==============================================================*/
/* Index: idx_outinorder_code                                   */
/*==============================================================*/
create unique index idx_outinorder_code on outinorder
(
   code
);

/*==============================================================*/
/* Index: idx_outinorder_updatestoreidstats                     */
/*==============================================================*/
create index idx_outinorder_updatestoreidstats on outinorder
(
   storeid,
   status,
   updatedon
);

/*==============================================================*/
/* Table: outinorderitem                                        */
/*==============================================================*/
create table outinorderitem
(
   id                   int not null auto_increment comment '编号',
   outinorderid         int not null,
   productid            int not null comment 'SKU编码',
   quantity             int not null comment '数量',
   costprice            decimal(12,4) not null comment '实际成本价',
   lastcostprice        decimal(12,4) not null comment '最近进价',
   plusminus            smallint not null comment '正负（正，入库，负，出库）',
   primary key (id)
);

alter table outinorderitem comment '其他出入库单明细';

/*==============================================================*/
/* Table: outinordertype                                        */
/*==============================================================*/
create table outinordertype
(
   id                   int not null auto_increment,
   typename             varchar(32) not null,
   outininventory       smallint not null comment '（入/出）库',
   primary key (id)
);

/*==============================================================*/
/* Table: pay_app                                               */
/*==============================================================*/
create table pay_app
(
   sysno                int not null auto_increment,
   appid                varchar(64),
   appsecret            varchar(64),
   appname              varchar(1024),
   status               int,
   createtime           datetime,
   primary key (sysno)
);

/*==============================================================*/
/* Table: pay_notifyback                                        */
/*==============================================================*/
create table pay_notifyback
(
   sysno                int not null auto_increment,
   resultsysno          int,
   msg                  varchar(1024),
   status               int,
   createtime           datetime,
   requestdata          varchar(2048),
   primary key (sysno)
);

/*==============================================================*/
/* Table: pay_request                                           */
/*==============================================================*/
create table pay_request
(
   sysno                int not null auto_increment,
   orderid              varchar(256),
   paymentamt           decimal(19,2),
   paytype              int,
   notifyurl            varchar(1024),
   returnurl            varchar(1024),
   requestdata          varchar(2048),
   executeresult        int,
   resultdesc           varchar(1024),
   appid                varchar(64),
   status               int,
   createtime           datetime,
   primary key (sysno)
);

alter table pay_request comment '支付请求';

/*==============================================================*/
/* Index: ix_pay_request_orderid_paytype                        */
/*==============================================================*/
create index ix_pay_request_orderid_paytype on pay_request
(
   orderid,
   paytype
);

/*==============================================================*/
/* Table: pay_result                                            */
/*==============================================================*/
create table pay_result
(
   sysno                int not null auto_increment,
   requestsysno         int,
   orderid              varchar(256),
   tradeno              varchar(256),
   paymentamt           decimal(19,2),
   paytype              int,
   requestdata          varchar(2048),
   executeresult        int,
   resultdesc           varchar(1024),
   notifystatus         int,
   createtime           datetime,
   exttradeno           varchar(256),
   primary key (sysno)
);

/*==============================================================*/
/* Table: picture                                               */
/*==============================================================*/
create table picture
(
   id                   int not null auto_increment comment '图片ID',
   url                  varchar(500) comment '链接地址',
   title                varchar(50) comment '图片标题',
   primary key (id)
);

alter table picture comment '图片';

/*==============================================================*/
/* Table: processhistory                                        */
/*==============================================================*/
create table processhistory
(
   id                   int not null auto_increment comment '编号',
   createdby            int comment '创建人',
   createdbyname        varchar(50) comment '创建人名',
   createdon            datetime comment '创建时间',
   status               int comment '状态',
   formid               int comment '表单Id',
   formtype             nvarchar(64) comment '表单类型',
   remark               nvarchar(1000) comment '备注',
   primary key (id)
);

alter table processhistory comment '表单处理历史记录';

/*==============================================================*/
/* Index: idx_processhistory_fromid                             */
/*==============================================================*/
create index idx_processhistory_fromid on processhistory
(
   formid,
   formtype
);

/*==============================================================*/
/* Table: product                                               */
/*==============================================================*/
create table product
(
   id                   int not null auto_increment comment '编号',
   code                 nvarchar(20) comment '编码',
   name                 nvarchar(50) comment '商品名',
   showname             nvarchar(500) comment '显示名称',
   sellingpoint         nvarchar(100) comment '卖点',
   categoryid           nvarchar(18) comment '分类Id',
   brandid              int comment '品牌Id',
   inputrate            decimal comment '进项税率',
   outrate              decimal comment '销项税率',
   isgift               bool comment '是否赠品',
   length               decimal comment '长',
   width                decimal comment '宽',
   height               decimal comment '高',
   weight               decimal comment '重量',
   unit                 nvarchar(10) comment '单位',
   keywords             nvarchar(200) comment '关键字',
   barcode              nvarchar(50) comment '条码',
   specification        nvarchar(200) comment '规格名',
   oldprice             decimal(8,2) comment '原价',
   saleprice            decimal(8,2) comment '销售价',
   wholesaleprice       decimal(8,2) comment '批发价',
   subskucode           varchar(20) comment '子SKU代码',
   subskuquantity       int comment '子SKU数量',
   specificationquantity nvarchar(100) comment '件规, 多个逗号分隔',
   createdon            datetime comment '创建时间',
   createdby            int comment '创建人',
   updatedon            datetime comment '修改时间',
   updatedby            int comment '修改人',
   madein               varchar(200) comment '产地',
   grade                varchar(50) comment '等级',
   primary key (id)
);

alter table product comment '商品';

/*==============================================================*/
/* Index: idx_product_code                                      */
/*==============================================================*/
create unique index idx_product_code on product
(
   code
);

/*==============================================================*/
/* Index: idx_product_barcode                                   */
/*==============================================================*/
create unique index idx_product_barcode on product
(
   barcode
);

/*==============================================================*/
/* Table: productareaprice                                      */
/*==============================================================*/
create table productareaprice
(
   id                   int not null auto_increment,
   productid            int,
   areaid               char(6),
   saleprice            decimal(12,2),
   primary key (id)
);

alter table productareaprice comment '商品区域价，该表不再使用';

/*==============================================================*/
/* Table: productcodesequence                                   */
/*==============================================================*/
create table productcodesequence
(
   id                   int not null auto_increment,
   guidcode             nvarchar(32) comment 'guid代码',
   primary key (id)
);

alter table productcodesequence comment '产品编码序列号，此表用来生成商品SKU 表中的 Code 字段';

/*==============================================================*/
/* Index: idx_pcodeseq_guidcode                                 */
/*==============================================================*/
create unique index idx_pcodeseq_guidcode on productcodesequence
(
   guidcode
);

/*==============================================================*/
/* Table: productdetails                                        */
/*==============================================================*/
create table productdetails
(
   productid            int  not null,
   description          text comment '详情描述',
   primary key (productid)
);

alter table productdetails comment '商品详情';

/*==============================================================*/
/* Table: productpicture                                        */
/*==============================================================*/
create table productpicture
(
   id                   int not null comment '编号',
   productid            int comment '商品编号',
   pictureid            int comment '图片Id',
   primary key (id)
);

/*==============================================================*/
/* Table: purchasecontract                                      */
/*==============================================================*/
create table purchasecontract
(
   id                   int not null auto_increment comment '编号',
   code                 nvarchar(50) comment '合同号',
   name                 nvarchar(50) comment '合同名称',
   storeids             varchar(300) comment '供应门店，多个都好分割',
   supplierid           int comment '供应商Id',
   contact              nvarchar(32) comment '联系人',
   startdate            datetime comment '开始日期',
   enddate              datetime comment '结束日期',
   createdon            datetime comment '创建时间',
   createdby            int comment '创建人',
   updatedon            datetime comment '修改时间',
   updatedby            int comment '修改人',
   status               int comment '状态',
   remark               varchar(1000) comment '备注',
   primary key (id)
);

alter table purchasecontract comment '采购合同';

/*==============================================================*/
/* Index: idx_purcontract_code                                  */
/*==============================================================*/
create unique index idx_purcontract_code on purchasecontract
(
   code
);

/*==============================================================*/
/* Table: purchasecontractitem                                  */
/*==============================================================*/
create table purchasecontractitem
(
   id                   int not null auto_increment comment '编号',
   purchasecontractid   int comment '采购合同编号',
   productid            int comment '商品skuid',
   contractprice        decimal(8,4) comment '合同价',
   status               int comment '供货状态',
   primary key (id)
);

alter table purchasecontractitem comment '采购合同明细';

/*==============================================================*/
/* Table: purchaseorder                                         */
/*==============================================================*/
create table purchaseorder
(
   id                   int not null auto_increment comment '编号',
   purchasecontractid   int comment '采购合同编号',
   code                 nvarchar(20) comment '订单号',
   type                 int comment '单据类型:  进货 1，退货 2',
   warehouseid          int comment '仓库Id',
   supplierid           int comment '供应商Id',
   createdon            datetime comment '创建时间',
   createdby            int comment '创建人',
   updatedon            datetime comment '修改时间',
   updatedby            int comment '修改人',
   status               int comment '状态',
   total                decimal(8,2) comment '金额',
   primary key (id)
);

alter table purchaseorder comment '采购订单';

/*==============================================================*/
/* Index: idx_purchaseorder_code                                */
/*==============================================================*/
create unique index idx_purchaseorder_code on purchaseorder
(
   code
);

/*==============================================================*/
/* Table: purchaseorderitem                                     */
/*==============================================================*/
create table purchaseorderitem
(
   id                   int not null auto_increment comment '编号',
   purchaseorderid      int comment '采购订单编号',
   productid            int comment '商品skuid',
   contractprice        decimal(8,2) comment '合同价',
   price                decimal(8,2) comment '进价',
   quantity             int comment '数量',
   actualquantity       int comment '实际数量',
   isgift               bool comment '赠品是否赠品',
   primary key (id)
);

alter table purchaseorderitem comment '采购订单明细';

/*==============================================================*/
/* Table: purchasesaleinventory                                 */
/*==============================================================*/
create table purchasesaleinventory
(
   yearmonth            int not null comment '年',
   storeid              int not null comment '门店',
   storename            varchar(100) comment '门店名',
   preinventoryquantity int comment '期初库存',
   preinventoryamount   decimal(12,4) comment '期初库存金额',
   purchasequantity     int comment '本期入库数',
   purchaseamount       decimal(12,4) comment '本期入库金额',
   salequantity         int comment '本期销售数',
   salecostamount       decimal(12,4) comment '本期销售成本金额',
   saleamount           decimal(12,2) comment '本期销售金额',
   endinventoryquantity int comment '期末库存数',
   endinventoryamount   decimal(12,4) comment '期末库存金额',
   updatedon            datetime comment '更新时间',
   primary key (yearmonth, storeid)
);

alter table purchasesaleinventory comment '进销存报表';

/*==============================================================*/
/* Table: purchasesaleinventorydetail                           */
/*==============================================================*/
create table purchasesaleinventorydetail
(
   yearmonth            int not null comment '年月',
   storeid              int not null comment '门店id',
   productid            int not null comment '商品编码',
   preinventoryquantity int comment '期初库存',
   preinventoryamount   decimal(12,4) comment '期初库存金额',
   purchasequantity     int comment '本期入库数',
   purchaseamount       decimal(12,4) comment '本期入库金额',
   salequantity         int comment '本期销售数',
   salecostamount       decimal(12,4) comment '本期销售成本金额',
   saleamount           decimal(12,2) comment '本期销售金额',
   endinventoryquantity int comment '期末库存数',
   endinventoryamount   decimal(12,4) comment '期末库存金额',
   avgcostprice         decimal(12,4) comment '成本均价',
   updatedon            datetime comment '更新时间',
   primary key (yearmonth, storeid, productid)
);

alter table purchasesaleinventorydetail comment '进销存明细报表';

/*==============================================================*/
/* Table: role                                                  */
/*==============================================================*/
create table role
(
   id                   int not null auto_increment comment '编号',
   name                 nvarchar(64) comment '角色名称',
   description          nvarchar(1024) comment '描述',
   primary key (id)
);

alter table role comment '账户角色表';

/*==============================================================*/
/* Table: rolemenu                                              */
/*==============================================================*/
create table rolemenu
(
   id                   int not null auto_increment comment '编号',
   roleid               int comment '角色编号',
   menuid               int comment '菜单编号',
   primary key (id)
);

alter table rolemenu comment '角色菜单对应表';

/*==============================================================*/
/* Table: saleorder                                             */
/*==============================================================*/
create table saleorder
(
   id                   int not null auto_increment,
   code                 nvarchar(20) comment '编码',
   storeid              int comment '门店',
   posid                int comment 'Pos机Id',
   ordertype            int comment '订单类型',
   refundaccount        varchar(60) comment '退款账号',
   status               int comment '状态',
   orderamount          decimal(12,2) comment '订单金额',
   payamount            decimal(12,2) comment '现金支付金额',
   onlinepayamount      decimal(12,2) comment '在线支付金额',
   paymentway           int comment '支付方式',
   paiddate             datetime comment '支付时间',
   hour                 int comment '时段',
   createdon            datetime comment '创建时间',
   createdby            int comment '创建人',
   updatedon            datetime comment '修改时间',
   updatedby            int comment '修改人',
   workschedulecode     varchar(32) comment '班次代码',
   orderlevel           int comment '订单级别：1 普通订单，2 Vip订单',
   primary key (id)
);

alter table saleorder comment '销售订单';

/*==============================================================*/
/* Index: idx_saleorder_code                                    */
/*==============================================================*/
create unique index idx_saleorder_code on saleorder
(
   code
);

/*==============================================================*/
/* Index: idx_saleorder_storeidandupdatedon                     */
/*==============================================================*/
create index idx_saleorder_storeidandupdatedon on saleorder
(
   storeid,
   status,
   updatedon
);

/*==============================================================*/
/* Table: saleorderitem                                         */
/*==============================================================*/
create table saleorderitem
(
   id                   int not null auto_increment,
   saleorderid          int comment '销售编码',
   productid            int comment '商品Id',
   productcode          nvarchar(20) comment '商品编码',
   productname          nvarchar(50) comment '商品名',
   avgcostprice         decimal(12,2) comment '平均成本价',
   saleprice            decimal(12,2) comment '销售价',
   realprice            decimal(12,2) comment '实际售价',
   quantity             int comment '数量',
   primary key (id)
);

/*==============================================================*/
/* Index: idx_saleorderitem_saleorderid                         */
/*==============================================================*/
create index idx_saleorderitem_saleorderid on saleorderitem
(
   saleorderid
);

/*==============================================================*/
/* Index: idx_saleorderitem_productid                           */
/*==============================================================*/
create index idx_saleorderitem_productid on saleorderitem
(
   productid
);

/*==============================================================*/
/* Table: salereport                                            */
/*==============================================================*/
create table salereport
(
   storeinventoryhistoryid int not null comment '库存流水Id',
   saleorderid          int comment '销售编码',
   productid            int comment '商品Id',
   ordertype            int comment '订单类型',
   paymentway           int comment '支付方式',
   orderlevel           int comment '订单级别：1 普通订单，2 Vip订单',
   storeid              int comment '门店',
   supplierid           int comment '供应商Id',
   costprice            decimal(8,4) comment '成本价',
   saleprice            decimal(8,2) comment '销售价',
   realprice            decimal(8,2) comment '实际售价',
   quantity             int comment '数量',
   createdon            datetime comment '创建时间',
   createdby            int comment '创建人',
   updatedon            datetime comment '修改时间',
   primary key (storeinventoryhistoryid)
);

alter table salereport comment '从 storeinventoryHistory  中提取的销售数据，报表用';

/*==============================================================*/
/* Index: idx_salereport_createdonpidsid                        */
/*==============================================================*/
create index idx_salereport_createdonpidsid on salereport
(
   createdon
);

/*==============================================================*/
/* Table: salesync                                              */
/*==============================================================*/
create table salesync
(
   id                   int not null auto_increment,
   saledate             varchar(20) comment '销售日',
   storeid              int comment '门店ID',
   posid                int comment '收银机',
   ordercount           int comment '订单数',
   ordertotalamount     decimal(8,2) comment '订单总金额',
   clientupdatedon      datetime comment '上传时间',
   primary key (id)
);

/*==============================================================*/
/* Index: idx_salesync                                          */
/*==============================================================*/
create unique index idx_salesync on salesync
(
   saledate,
   storeid,
   posid
);

/*==============================================================*/
/* Table: shelf                                                 */
/*==============================================================*/
create table shelf
(
   id                   int not null auto_increment comment '编号',
   storeid              int,
   code                 varchar(20) comment '货架码',
   number               int comment '货架码',
   name                 varchar(50) comment '货架名',
   primary key (id)
);

alter table shelf comment '货架';

/*==============================================================*/
/* Table: shelflayer                                            */
/*==============================================================*/
create table shelflayer
(
   id                   int not null auto_increment comment '编号',
   code                 varchar(20) comment '货架码',
   number               int comment '货架码',
   shelfid              int comment '货架名',
   primary key (id)
);

alter table shelflayer comment '货架层';

/*==============================================================*/
/* Table: shelflayerproduct                                     */
/*==============================================================*/
create table shelflayerproduct
(
   id                   int not null auto_increment comment '编号',
   storeid              int,
   code                 varchar(20) comment '货架码',
   number               int comment '货架码',
   productid            int comment '商品ID',
   quantity             int,
   shelflayerid         int,
   primary key (id)
);

/*==============================================================*/
/* Table: stocktaking                                           */
/*==============================================================*/
create table stocktaking
(
   id                   int not null auto_increment comment '编号',
   stocktakingplanid    int comment '盘点计划编号',
   code                 nvarchar(20) comment '盘点单号',
   stocktakingtype      int comment '盘点表类型1 盘点空表，2 盘点修正表',
   shelfcode            nvarchar(20) comment '货架码',
   createdby            int comment '创建人',
   createdbyname        nvarchar(50) comment '创建人名',
   createdon            datetime comment '创建时间',
   status               int comment '状态（待审，已审）',
   updatedon            datetime comment '修改时间',
   updatedby            int comment '修改人',
   updatedbyname        varchar(50) comment '修改人名',
   storeid              int comment '门店',
   note                 nvarchar(1000) comment '备注',
   primary key (id)
);

alter table stocktaking comment '盘点表';

/*==============================================================*/
/* Table: stocktakingitem                                       */
/*==============================================================*/
create table stocktakingitem
(
   id                   int not null auto_increment comment '编号',
   stocktakingid        int,
   productid            nvarchar(50) comment '商品编码',
   costprice            decimal(8,4) comment '调拨成本价',
   saleprice            decimal(8,2) comment '销售价',
   quantity             int comment '盘点锁定库存数',
   countquantity        int comment '盘点数量',
   corectquantity       int comment '修正数',
   corectreason         nvarchar(500) comment '修正原因',
   note                 nvarchar(500) comment '备注',
   primary key (id)
);

alter table stocktakingitem comment '盘点明细';

/*==============================================================*/
/* Table: stocktakingplan                                       */
/*==============================================================*/
create table stocktakingplan
(
   id                   int not null auto_increment comment '编号',
   code                 nvarchar(20) comment '盘点代码',
   createdby            int comment '创建人',
   createdbyname        nvarchar(50) comment '创建人名',
   createdon            datetime comment '创建时间',
   updatedby            int comment '更新人',
   updatedbyname        nvarchar(50) comment '更新人名',
   updatedon            datetime comment '更新时间',
   method               int comment '盘点方式（大盘：小盘）',
   status               int comment '盘点状态（待盘，初盘，复盘，终盘）',
   storeid              int comment '门店编号',
   note                 nvarchar(1000) comment '备注',
   stocktakingdate      datetime comment '盘点日期',
   primary key (id)
);

alter table stocktakingplan comment '盘点计划';

/*==============================================================*/
/* Table: stocktakingplanitem                                   */
/*==============================================================*/
create table stocktakingplanitem
(
   id                   int not null auto_increment comment '编号',
   stocktakingplanid    int comment '盘点计划编号',
   productid            int comment '系统编码',
   costprice            decimal(8,4) comment '调拨成本价',
   saleprice            decimal(8,2) comment '销售价',
   quantity             int comment '库存数量',
   countquantity        int comment '盘点数量',
   primary key (id)
);

alter table stocktakingplanitem comment '盘点计划明细';

/*==============================================================*/
/* Table: store                                                 */
/*==============================================================*/
create table store
(
   id                   int not null auto_increment comment '编号',
   code                 nvarchar(20) comment '代码',
   number               int comment '编号',
   name                 nvarchar(128) comment '门店名',
   sourcekey            nvarchar(32) comment '门店唯一码',
   createdon            datetime comment '创建时间',
   createdby            int comment '创建人',
   areaid               char(6) comment '区域ID',
   address              nvarchar(512) comment '地址',
   contact              nvarchar(32) comment '联系人',
   phone                nvarchar(32) comment '联系电话',
   setting              text comment '设置',
   licensecode          varchar(50) comment '门店授权码',
   primary key (id)
);

alter table store comment '门店';

/*==============================================================*/
/* Index: idx_store_code                                        */
/*==============================================================*/
create unique index idx_store_code on store
(
   code
);

/*==============================================================*/
/* Table: storeinventory                                        */
/*==============================================================*/
create table storeinventory
(
   id                   int not null auto_increment comment '编号',
   storeid              int comment '门店编码',
   productid            int comment '商品Id',
   salequantity         int comment '销售库存',
   orderquantity        int comment '订购库存',
   quantity             int comment '实际库存数',
   avgcostprice         decimal(8,4) comment '平均成本价',
   warnquantity         int comment '警告库存',
   isquit               bool comment '是否退出',
   lastcostprice        decimal(8,2) comment '最新进价',
   storesaleprice       decimal(8,2) comment '门店售价',
   status               int comment '状态',
   rowversion           timestamp comment '行版本',
   primary key (id)
);

alter table storeinventory comment '门店库存';

/*==============================================================*/
/* Index: idx_storeinventory_pid                                */
/*==============================================================*/
create index idx_storeinventory_pid on storeinventory
(
   productid
);

/*==============================================================*/
/* Index: idx_storeinventory_pidandstoreid                      */
/*==============================================================*/
create index idx_storeinventory_pidandstoreid on storeinventory
(
   storeid,
   productid
);

/*==============================================================*/
/* Table: storeinventorybatch                                   */
/*==============================================================*/
create table storeinventorybatch
(
   id                   int not null auto_increment comment '编号',
   productid            int comment 'SKU编码',
   storeid              int comment '仓库编码',
   supplierid           int comment '供应商Id',
   quantity             int comment '实际库存数',
   productiondate       datetime comment '生产日期',
   shelflife            int comment '保质期',
   contractprice        decimal(8,4) comment '合同价',
   price                decimal(8,4) comment '实际入库进价',
   createdon            datetime comment '创建时间',
   createdby            int comment '创建人',
   batchno              bigint comment '批次号',
   rowversion           timestamp comment '行版本',
   primary key (id)
);

alter table storeinventorybatch comment '门店商品批次';

/*==============================================================*/
/* Index: idx_storeinventorybath_pid                            */
/*==============================================================*/
create index idx_storeinventorybath_pid on storeinventorybatch
(
   productid
);

/*==============================================================*/
/* Table: storeinventoryhistory                                 */
/*==============================================================*/
create table storeinventoryhistory
(
   id                   int not null auto_increment comment '编号',
   productid            int comment 'SKU编码',
   storeid              int comment '仓库编码',
   quantity             int comment '实际库存数',
   changequantity       int comment '变动数',
   createdon            datetime comment '创建时间',
   createdby            int comment '创建人',
   billid               int comment '单据系统码',
   billcode             varchar(20) comment '单据编码',
   billtype             int comment '单据类型',
   batchno              bigint comment '批次号',
   price                decimal(14,4) comment '进价',
   supplierid           int comment '供应商Id',
   saleprice            decimal(10,2) comment '售价',
   primary key (id)
);

alter table storeinventoryhistory comment '门店库存历史记录';

/*==============================================================*/
/* Index: idx_storeinventoryhistory_billcode                    */
/*==============================================================*/
create index idx_storeinventoryhistory_billcode on storeinventoryhistory
(
   billcode
);

/*==============================================================*/
/* Index: idx_sihistory_createdonstoreidproductid               */
/*==============================================================*/
create index idx_sihistory_createdonstoreidproductid on storeinventoryhistory
(
   productid,
   storeid,
   createdon
);

/*==============================================================*/
/* Table: storeinventorymonthly                                 */
/*==============================================================*/
create table storeinventorymonthly
(
   id                   int not null auto_increment comment '编号',
   monthly              varchar(10) comment '会计期间 2017-01 按月存储',
   storeid              int comment '门店编码',
   productid            int comment '商品Id',
   quantity             int comment '实际库存数',
   avgcostprice         decimal(8,4) comment '平均成本价',
   primary key (id)
);

alter table storeinventorymonthly comment '门店库存月报';

/*==============================================================*/
/* Index: idx_storeinventorymonthly_pid                         */
/*==============================================================*/
create index idx_storeinventorymonthly_pid on storeinventorymonthly
(
   productid
);

/*==============================================================*/
/* Table: storepurchaseorder                                    */
/*==============================================================*/
create table storepurchaseorder
(
   id                   int not null auto_increment comment '编号',
   code                 nvarchar(20) comment '订单号',
   ordertype            int comment '单据类型: 进 1 退 2',
   storeid              int comment '门店Id',
   supplierbill         nvarchar(200) comment '供应商单据号',
   supplierid           int comment '供应商Id',
   createdon            datetime comment '创建时间',
   createdby            int comment '创建人',
   createdbyname        nvarchar(50) comment '创建人名',
   receivedby           int comment '收货人',
   receivedon           datetime comment '收货日期',
   receivedbyname       varchar(50) comment '收货人名',
   storagedby           int comment '入库人',
   storagedbyname       nvarchar(50) comment '入库人名',
   storagedon           datetime comment '入库日期',
   status               int comment '状态',
   isgift               bool comment '是否赠品',
   primary key (id)
);

alter table storepurchaseorder comment '门店采购订单';

/*==============================================================*/
/* Index: idx_storepurchaseorder_code                           */
/*==============================================================*/
create unique index idx_storepurchaseorder_code on storepurchaseorder
(
   code
);

/*==============================================================*/
/* Table: storepurchaseorderitem                                */
/*==============================================================*/
create table storepurchaseorderitem
(
   id                   int not null auto_increment comment '编号',
   storepurchaseorderid int comment '门店采购订单编号',
   productid            int comment '商品skuid',
   contractprice        decimal(8,4) comment '合同价',
   price                decimal(8,4) comment '进价',
   specificationquantity int comment '件规',
   quantity             int comment '数量',
   actualquantity       int comment '实际数量',
   productiondate       datetime comment '生产日期',
   shelflife            int comment '保质期',
   batchno              bigint comment '批次号',
   primary key (id)
);

alter table storepurchaseorderitem comment '门店采购订单明细';

/*==============================================================*/
/* Table: supplier                                              */
/*==============================================================*/
create table supplier
(
   id                   int not null auto_increment comment '编号',
   code                 nvarchar(20) comment '供应商编码',
   name                 nvarchar(100) comment '供应商名',
   type                 int comment '供应商类别',
   shortname            nvarchar(50) comment '简称',
   contact              nvarchar(300) comment '联系人',
   phone                nvarchar(300) comment '联系电话',
   qq                   nvarchar(300),
   address              nvarchar(100),
   bank                 nvarchar(50) comment '开户行',
   bankaccount          nvarchar(50) comment '开户行账号',
   bankaccountname      varchar(50) comment '开户名',
   taxno                nvarchar(50) comment '税号',
   licenseno            nvarchar(50) comment '执照号',
   createdon            datetime comment '创建时间',
   createdby            int comment '创建人',
   updatedon            datetime comment '修改时间',
   updatedby            int comment '修改人',
   primary key (id)
);

alter table supplier comment '供应商';

/*==============================================================*/
/* Index: idx_supplier_code                                     */
/*==============================================================*/
create unique index idx_supplier_code on supplier
(
   code
);

/*==============================================================*/
/* Table: supplierproduct                                       */
/*==============================================================*/
create table supplierproduct
(
   id                   int not null auto_increment comment '编号',
   supplierid           int comment '供应商Id',
   productid            int comment '商品',
   price                decimal(8,4) comment '价格',
   status               int comment '供货状态',
   comparestatus        int comment '比价状态',
   updatedon            datetime comment '修改时间',
   updatedby            int comment '修改人',
   primary key (id)
);

alter table supplierproduct comment '供应商商品';

/*==============================================================*/
/* Table: transferorder                                         */
/*==============================================================*/
create table transferorder
(
   id                   int not null auto_increment comment '编号',
   code                 varchar(20) comment '调拨单号',
   fromstoreid          int comment '从门店',
   fromstorename        char(50) comment '从门店名',
   tostorename          char(50) comment '到门店名',
   tostoreid            int comment '到门店',
   status               int comment '状态',
   createdon            datetime comment '创建时间',
   createdby            int comment '创建人',
   createdbyname        varchar(30) comment '创建人名',
   updatedon            datetime comment '修改时间',
   updatedby            int comment '修改人',
   updatedbyname        varchar(30) comment '修改人名',
   primary key (id)
);

alter table transferorder comment '调拨单';

/*==============================================================*/
/* Index: idx_transaferorder_code                               */
/*==============================================================*/
create unique index idx_transaferorder_code on transferorder
(
   code
);

/*==============================================================*/
/* Table: transferorderitem                                     */
/*==============================================================*/
create table transferorderitem
(
   id                   int not null auto_increment comment '编号',
   transferorderid      int comment '调拨单ID',
   supplierid           int comment '供应商Id',
   productid            int comment 'SKU编码',
   quantity             int comment '数量',
   contractprice        decimal(8,4) comment '合同价',
   price                decimal(8,4) comment '成本价',
   batchno              bigint comment '批次',
   productiondate       datetime comment '生产日期',
   shelflife            int comment '保质期',
   primary key (id)
);

alter table transferorderitem comment '调拨明细';

/*==============================================================*/
/* Table: vipcard                                               */
/*==============================================================*/
create table vipcard
(
   id                   int not null auto_increment,
   code                 varchar(50) comment '会员卡号',
   discount             decimal(8,2) comment '折扣',
   primary key (id)
);

alter table vipcard comment '会员卡';

/*==============================================================*/
/* Table: vipproduct                                            */
/*==============================================================*/
create table vipproduct
(
   id                   int not null auto_increment,
   productid            int,
   saleprice            decimal(8,2),
   primary key (id)
);

/*==============================================================*/
/* Index: idx_vipproduct_productid                              */
/*==============================================================*/
create unique index idx_vipproduct_productid on vipproduct
(
   productid
);

/*==============================================================*/
/* Table: warehouse                                             */
/*==============================================================*/
create table warehouse
(
   id                   int not null auto_increment,
   code                 nvarchar(20) comment '代码',
   name                 nvarchar(50) comment '仓库名',
   areaid               char(6) comment '区域',
   address              nvarchar(100) comment '地址',
   primary key (id)
);

alter table warehouse comment '仓库';

/*==============================================================*/
/* Table: workschedule                                          */
/*==============================================================*/
create table workschedule
(
   id                   int not null auto_increment,
   code                 nvarchar(50) comment '代码',
   storeid              int comment '门店',
   posid                int comment 'Pos机Id',
   cashamount           decimal(8,2) comment '收现金额',
   startdate            datetime comment '开始时间',
   enddate              datetime comment '结束时间',
   createdby            int comment '创建人',
   createdbyname        varchar(50) comment '创建人名',
   endby                int comment '交班人',
   endbyname            varchar(50) comment '交班人名',
   primary key (id)
);

alter table workschedule comment '班次记录表';

/*==============================================================*/
/* Index: idx_workschedule_code                                 */
/*==============================================================*/
create unique index idx_workschedule_code on workschedule
(
   code
);

