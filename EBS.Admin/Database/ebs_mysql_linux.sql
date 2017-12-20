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
   storeid              int comment '�ŵ�ID',
   posid                int comment 'pos��Id',
   cdkey                varchar(50) comment '���к�',
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
   id                   int not null auto_increment comment '���',
   username             nvarchar(64) comment '�˻���',
   password             nvarchar(64) comment '����',
   nickname             nvarchar(64) comment '�ǳ�',
   roleid               int comment '��ɫID',
   createdon            datetime comment '����ʱ��',
   status               int comment '״̬',
   loginerrorcount      int comment '��¼�������',
   lastupdatedate       datetime comment '����޸�ʱ��',
   storeid              int comment '�ŵ�',
   canviewstores        varchar(200) comment '���Բ�ѯ�ŵ�',
   primary key (id)
);

alter table account comment '��̨�����˻���';

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
   id                   int not null auto_increment comment '���',
   accountid            int comment '�˺�id',
   username             nvarchar(64) comment '��¼�˺�',
   createdon            datetime comment '��¼ʱ��',
   ipaddress            nvarchar(64) comment 'IP��ַ',
   loginstatus          int comment '��¼״̬',
   primary key (id)
);

alter table accountloginhistory comment '�˺ŵ�¼��ʷ';

/*==============================================================*/
/* Table: adjustcontractprice                                   */
/*==============================================================*/
create table adjustcontractprice
(
   id                   int not null auto_increment comment '���',
   code                 nvarchar(50) comment '���۵���',
   storeid              int comment '�ŵ�Id',
   supplierid           int comment '��Ӧ��Id',
   createdon            datetime comment '����ʱ��',
   createdby            int comment '������',
   updatedon            datetime comment '�޸�ʱ��',
   updatedby            int comment '�޸���',
   status               int comment '״̬',
   remark               nvarchar(200) comment '��ע',
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
   id                   int not null auto_increment comment '���',
   adjustcontractpriceid int comment '���۵�����',
   productid            int comment '��Ʒ���',
   contractprice        decimal(8,4) comment '��ͬ��',
   adjustprice          decimal(8,4) comment '������',
   primary key (id)
);

alter table adjustcontractpriceitem comment '��������ϸ';

/*==============================================================*/
/* Table: adjustsaleprice                                       */
/*==============================================================*/
create table adjustsaleprice
(
   id                   int not null auto_increment comment '���',
   code                 nvarchar(50) comment '���۵���',
   createdon            datetime comment '����ʱ��',
   createdby            int comment '������',
   updatedon            datetime comment '�޸�ʱ��',
   updatedby            int comment '�޸���',
   status               int comment '״̬',
   primary key (id)
);

alter table adjustsaleprice comment '�����ۼ�';

/*==============================================================*/
/* Table: adjustsalepriceitem                                   */
/*==============================================================*/
create table adjustsalepriceitem
(
   id                   int not null auto_increment comment '���',
   adjustsalepriceid    int comment '���۵�����',
   saleprice            decimal(8,2) comment '���ۼ�',
   adjustprice          decimal(8,2) comment 'ԭ�ۼ�',
   productid            int comment '��Ʒ���',
   primary key (id)
);

alter table adjustsalepriceitem comment '�����ۼ���ϸ';

/*==============================================================*/
/* Table: adjuststoreprice                                      */
/*==============================================================*/
create table adjuststoreprice
(
   id                   int not null auto_increment comment '���',
   code                 nvarchar(50) comment '���۵���',
   storeid              int comment '�ŵ�',
   createdon            datetime comment '����ʱ��',
   createdby            int comment '������',
   updatedon            datetime comment '�޸�ʱ��',
   updatedby            int comment '�޸���',
   status               int comment '״̬',
   remark               varchar(500) comment '��ע',
   primary key (id)
);

alter table adjuststoreprice comment '�����ŵ��ۼ�';

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
   id                   int not null auto_increment comment '���',
   adjuststorepriceid   int comment '���۵�����',
   storesaleprice       decimal(8,2) comment 'ԭ�ۼ�',
   adjustprice          decimal(8,2) comment '�����ۼ�',
   productid            int comment '��Ʒ���',
   primary key (id)
);

alter table adjuststorepriceitem comment '�����ۼ���ϸ';

/*==============================================================*/
/* Table: area                                                  */
/*==============================================================*/
create table area
(
   id                   char(6) not null comment '���',
   name                 nvarchar(64) comment '������',
   showname             nvarchar(64) comment '��ʾ����',
   fullname             nvarchar(256) comment '����ȫ��',
   level                int comment '�㼶',
   primary key (id)
);

alter table area comment '�����';

/*==============================================================*/
/* Table: billsequence                                          */
/*==============================================================*/
create table billsequence
(
   id                   int not null auto_increment,
   guidcode             nvarchar(32) comment 'guid����',
   primary key (id)
);

alter table billsequence comment '�������к�';

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
   id                   int not null auto_increment comment '���',
   name                 nvarchar(128) comment '����',
   primary key (id)
);

alter table brand comment 'Ʒ��';

/*==============================================================*/
/* Table: category                                              */
/*==============================================================*/
create table category
(
   id                   nvarchar(18) not null comment '���',
   name                 nvarchar(64) comment '������',
   fullname             nvarchar(256) comment 'ȫ��',
   level                int comment '�㼶',
   primary key (id)
);

alter table category comment '��Ʒ����';

/*==============================================================*/
/* Table: inventory                                             */
/*==============================================================*/
create table inventory
(
   id                   int not null auto_increment comment '���',
   productid            int comment '����',
   warehouseid          int comment '�ֿ����',
   quantity             int comment 'ʵ�ʿ����',
   avgcostprice         decimal(8,2) comment 'ƽ���ɱ���',
   warnquantity         int comment '������',
   isquit               bool comment '�Ƿ��˳�',
   primary key (id)
);

/*==============================================================*/
/* Table: inventoryhistory                                      */
/*==============================================================*/
create table inventoryhistory
(
   id                   int not null auto_increment comment '���',
   productid            int comment '��ƷId',
   warehouseid          int comment '�ֿ����',
   quantity             int comment 'ʵ�ʿ����',
   changequantity       int comment '�䶯��',
   createdon            datetime comment '����ʱ��',
   billid               int comment '����ϵͳ��',
   billcode             varchar(20) comment '���ݱ���',
   primary key (id)
);

alter table inventoryhistory comment '�����ʷ��¼';

/*==============================================================*/
/* Table: menu                                                  */
/*==============================================================*/
create table menu
(
   id                   int not null auto_increment comment '���',
   parentid             int comment '�����',
   name                 nvarchar(64) comment '����',
   url                  nvarchar(256) comment '����',
   icon                 nvarchar(64) comment 'ͼ��',
   displayorder         int comment '��ʾ˳��',
   urltype              int comment '��������',
   primary key (id)
);

alter table menu comment 'ϵͳ�˵�';

/*==============================================================*/
/* Table: outinorder                                            */
/*==============================================================*/
create table outinorder
(
   id                   int not null auto_increment comment '���',
   code                 varchar(20) not null comment '����',
   storeid              int not null comment '�ŵ�id',
   supplierid           int comment '��Ӧ��Id',
   status               int not null comment '״̬',
   outinordertypeid     int not null comment '��������',
   createdon            datetime not null comment '����ʱ��',
   createdby            int not null comment '������',
   createdbyname        varchar(20) comment '��������',
   updatedon            datetime not null comment '�޸�ʱ��',
   updatedby            int not null comment '�޸���',
   updatedbyname        varchar(20) comment '�޸�����',
   remark               varchar(1000) comment '��ע',
   primary key (id)
);

alter table outinorder comment '��������ⵥ';

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
   id                   int not null auto_increment comment '���',
   outinorderid         int not null,
   productid            int not null comment 'SKU����',
   quantity             int not null comment '����',
   costprice            decimal(12,4) not null comment 'ʵ�ʳɱ���',
   lastcostprice        decimal(12,4) not null comment '�������',
   plusminus            smallint not null comment '������������⣬�������⣩',
   primary key (id)
);

alter table outinorderitem comment '��������ⵥ��ϸ';

/*==============================================================*/
/* Table: outinordertype                                        */
/*==============================================================*/
create table outinordertype
(
   id                   int not null auto_increment,
   typename             varchar(32) not null,
   outininventory       smallint not null comment '����/������',
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

alter table pay_request comment '֧������';

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
   id                   int not null auto_increment comment 'ͼƬID',
   url                  varchar(500) comment '���ӵ�ַ',
   title                varchar(50) comment 'ͼƬ����',
   primary key (id)
);

alter table picture comment 'ͼƬ';

/*==============================================================*/
/* Table: processhistory                                        */
/*==============================================================*/
create table processhistory
(
   id                   int not null auto_increment comment '���',
   createdby            int comment '������',
   createdbyname        varchar(50) comment '��������',
   createdon            datetime comment '����ʱ��',
   status               int comment '״̬',
   formid               int comment '��Id',
   formtype             nvarchar(64) comment '������',
   remark               nvarchar(1000) comment '��ע',
   primary key (id)
);

alter table processhistory comment '��������ʷ��¼';

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
   id                   int not null auto_increment comment '���',
   code                 nvarchar(20) comment '����',
   name                 nvarchar(50) comment '��Ʒ��',
   showname             nvarchar(500) comment '��ʾ����',
   sellingpoint         nvarchar(100) comment '����',
   categoryid           nvarchar(18) comment '����Id',
   brandid              int comment 'Ʒ��Id',
   inputrate            decimal comment '����˰��',
   outrate              decimal comment '����˰��',
   isgift               bool comment '�Ƿ���Ʒ',
   length               decimal comment '��',
   width                decimal comment '��',
   height               decimal comment '��',
   weight               decimal comment '����',
   unit                 nvarchar(10) comment '��λ',
   keywords             nvarchar(200) comment '�ؼ���',
   barcode              nvarchar(50) comment '����',
   specification        nvarchar(200) comment '�����',
   oldprice             decimal(8,2) comment 'ԭ��',
   saleprice            decimal(8,2) comment '���ۼ�',
   wholesaleprice       decimal(8,2) comment '������',
   subskucode           varchar(20) comment '��SKU����',
   subskuquantity       int comment '��SKU����',
   specificationquantity nvarchar(100) comment '����, ������ŷָ�',
   createdon            datetime comment '����ʱ��',
   createdby            int comment '������',
   updatedon            datetime comment '�޸�ʱ��',
   updatedby            int comment '�޸���',
   madein               varchar(200) comment '����',
   grade                varchar(50) comment '�ȼ�',
   primary key (id)
);

alter table product comment '��Ʒ';

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

alter table productareaprice comment '��Ʒ����ۣ��ñ���ʹ��';

/*==============================================================*/
/* Table: productcodesequence                                   */
/*==============================================================*/
create table productcodesequence
(
   id                   int not null auto_increment,
   guidcode             nvarchar(32) comment 'guid����',
   primary key (id)
);

alter table productcodesequence comment '��Ʒ�������кţ��˱�����������ƷSKU ���е� Code �ֶ�';

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
   description          text comment '��������',
   primary key (productid)
);

alter table productdetails comment '��Ʒ����';

/*==============================================================*/
/* Table: productpicture                                        */
/*==============================================================*/
create table productpicture
(
   id                   int not null comment '���',
   productid            int comment '��Ʒ���',
   pictureid            int comment 'ͼƬId',
   primary key (id)
);

/*==============================================================*/
/* Table: purchasecontract                                      */
/*==============================================================*/
create table purchasecontract
(
   id                   int not null auto_increment comment '���',
   code                 nvarchar(50) comment '��ͬ��',
   name                 nvarchar(50) comment '��ͬ����',
   storeids             varchar(300) comment '��Ӧ�ŵ꣬������÷ָ�',
   supplierid           int comment '��Ӧ��Id',
   contact              nvarchar(32) comment '��ϵ��',
   startdate            datetime comment '��ʼ����',
   enddate              datetime comment '��������',
   createdon            datetime comment '����ʱ��',
   createdby            int comment '������',
   updatedon            datetime comment '�޸�ʱ��',
   updatedby            int comment '�޸���',
   status               int comment '״̬',
   remark               varchar(1000) comment '��ע',
   primary key (id)
);

alter table purchasecontract comment '�ɹ���ͬ';

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
   id                   int not null auto_increment comment '���',
   purchasecontractid   int comment '�ɹ���ͬ���',
   productid            int comment '��Ʒskuid',
   contractprice        decimal(8,4) comment '��ͬ��',
   status               int comment '����״̬',
   primary key (id)
);

alter table purchasecontractitem comment '�ɹ���ͬ��ϸ';

/*==============================================================*/
/* Table: purchaseorder                                         */
/*==============================================================*/
create table purchaseorder
(
   id                   int not null auto_increment comment '���',
   purchasecontractid   int comment '�ɹ���ͬ���',
   code                 nvarchar(20) comment '������',
   type                 int comment '��������:  ���� 1���˻� 2',
   warehouseid          int comment '�ֿ�Id',
   supplierid           int comment '��Ӧ��Id',
   createdon            datetime comment '����ʱ��',
   createdby            int comment '������',
   updatedon            datetime comment '�޸�ʱ��',
   updatedby            int comment '�޸���',
   status               int comment '״̬',
   total                decimal(8,2) comment '���',
   primary key (id)
);

alter table purchaseorder comment '�ɹ�����';

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
   id                   int not null auto_increment comment '���',
   purchaseorderid      int comment '�ɹ��������',
   productid            int comment '��Ʒskuid',
   contractprice        decimal(8,2) comment '��ͬ��',
   price                decimal(8,2) comment '����',
   quantity             int comment '����',
   actualquantity       int comment 'ʵ������',
   isgift               bool comment '��Ʒ�Ƿ���Ʒ',
   primary key (id)
);

alter table purchaseorderitem comment '�ɹ�������ϸ';

/*==============================================================*/
/* Table: purchasesaleinventory                                 */
/*==============================================================*/
create table purchasesaleinventory
(
   yearmonth            int not null comment '��',
   storeid              int not null comment '�ŵ�',
   storename            varchar(100) comment '�ŵ���',
   preinventoryquantity int comment '�ڳ����',
   preinventoryamount   decimal(12,4) comment '�ڳ������',
   purchasequantity     int comment '���������',
   purchaseamount       decimal(12,4) comment '���������',
   salequantity         int comment '����������',
   salecostamount       decimal(12,4) comment '�������۳ɱ����',
   saleamount           decimal(12,2) comment '�������۽��',
   endinventoryquantity int comment '��ĩ�����',
   endinventoryamount   decimal(12,4) comment '��ĩ�����',
   updatedon            datetime comment '����ʱ��',
   primary key (yearmonth, storeid)
);

alter table purchasesaleinventory comment '�����汨��';

/*==============================================================*/
/* Table: purchasesaleinventorydetail                           */
/*==============================================================*/
create table purchasesaleinventorydetail
(
   yearmonth            int not null comment '����',
   storeid              int not null comment '�ŵ�id',
   productid            int not null comment '��Ʒ����',
   preinventoryquantity int comment '�ڳ����',
   preinventoryamount   decimal(12,4) comment '�ڳ������',
   purchasequantity     int comment '���������',
   purchaseamount       decimal(12,4) comment '���������',
   salequantity         int comment '����������',
   salecostamount       decimal(12,4) comment '�������۳ɱ����',
   saleamount           decimal(12,2) comment '�������۽��',
   endinventoryquantity int comment '��ĩ�����',
   endinventoryamount   decimal(12,4) comment '��ĩ�����',
   avgcostprice         decimal(12,4) comment '�ɱ�����',
   updatedon            datetime comment '����ʱ��',
   primary key (yearmonth, storeid, productid)
);

alter table purchasesaleinventorydetail comment '��������ϸ����';

/*==============================================================*/
/* Table: role                                                  */
/*==============================================================*/
create table role
(
   id                   int not null auto_increment comment '���',
   name                 nvarchar(64) comment '��ɫ����',
   description          nvarchar(1024) comment '����',
   primary key (id)
);

alter table role comment '�˻���ɫ��';

/*==============================================================*/
/* Table: rolemenu                                              */
/*==============================================================*/
create table rolemenu
(
   id                   int not null auto_increment comment '���',
   roleid               int comment '��ɫ���',
   menuid               int comment '�˵����',
   primary key (id)
);

alter table rolemenu comment '��ɫ�˵���Ӧ��';

/*==============================================================*/
/* Table: saleorder                                             */
/*==============================================================*/
create table saleorder
(
   id                   int not null auto_increment,
   code                 nvarchar(20) comment '����',
   storeid              int comment '�ŵ�',
   posid                int comment 'Pos��Id',
   ordertype            int comment '��������',
   refundaccount        varchar(60) comment '�˿��˺�',
   status               int comment '״̬',
   orderamount          decimal(12,2) comment '�������',
   payamount            decimal(12,2) comment '�ֽ�֧�����',
   onlinepayamount      decimal(12,2) comment '����֧�����',
   paymentway           int comment '֧����ʽ',
   paiddate             datetime comment '֧��ʱ��',
   hour                 int comment 'ʱ��',
   createdon            datetime comment '����ʱ��',
   createdby            int comment '������',
   updatedon            datetime comment '�޸�ʱ��',
   updatedby            int comment '�޸���',
   workschedulecode     varchar(32) comment '��δ���',
   orderlevel           int comment '��������1 ��ͨ������2 Vip����',
   primary key (id)
);

alter table saleorder comment '���۶���';

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
   saleorderid          int comment '���۱���',
   productid            int comment '��ƷId',
   productcode          nvarchar(20) comment '��Ʒ����',
   productname          nvarchar(50) comment '��Ʒ��',
   avgcostprice         decimal(12,2) comment 'ƽ���ɱ���',
   saleprice            decimal(12,2) comment '���ۼ�',
   realprice            decimal(12,2) comment 'ʵ���ۼ�',
   quantity             int comment '����',
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
   storeinventoryhistoryid int not null comment '�����ˮId',
   saleorderid          int comment '���۱���',
   productid            int comment '��ƷId',
   ordertype            int comment '��������',
   paymentway           int comment '֧����ʽ',
   orderlevel           int comment '��������1 ��ͨ������2 Vip����',
   storeid              int comment '�ŵ�',
   supplierid           int comment '��Ӧ��Id',
   costprice            decimal(8,4) comment '�ɱ���',
   saleprice            decimal(8,2) comment '���ۼ�',
   realprice            decimal(8,2) comment 'ʵ���ۼ�',
   quantity             int comment '����',
   createdon            datetime comment '����ʱ��',
   createdby            int comment '������',
   updatedon            datetime comment '�޸�ʱ��',
   primary key (storeinventoryhistoryid)
);

alter table salereport comment '�� storeinventoryHistory  ����ȡ���������ݣ�������';

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
   saledate             varchar(20) comment '������',
   storeid              int comment '�ŵ�ID',
   posid                int comment '������',
   ordercount           int comment '������',
   ordertotalamount     decimal(8,2) comment '�����ܽ��',
   clientupdatedon      datetime comment '�ϴ�ʱ��',
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
   id                   int not null auto_increment comment '���',
   storeid              int,
   code                 varchar(20) comment '������',
   number               int comment '������',
   name                 varchar(50) comment '������',
   primary key (id)
);

alter table shelf comment '����';

/*==============================================================*/
/* Table: shelflayer                                            */
/*==============================================================*/
create table shelflayer
(
   id                   int not null auto_increment comment '���',
   code                 varchar(20) comment '������',
   number               int comment '������',
   shelfid              int comment '������',
   primary key (id)
);

alter table shelflayer comment '���ܲ�';

/*==============================================================*/
/* Table: shelflayerproduct                                     */
/*==============================================================*/
create table shelflayerproduct
(
   id                   int not null auto_increment comment '���',
   storeid              int,
   code                 varchar(20) comment '������',
   number               int comment '������',
   productid            int comment '��ƷID',
   quantity             int,
   shelflayerid         int,
   primary key (id)
);

/*==============================================================*/
/* Table: stocktaking                                           */
/*==============================================================*/
create table stocktaking
(
   id                   int not null auto_increment comment '���',
   stocktakingplanid    int comment '�̵�ƻ����',
   code                 nvarchar(20) comment '�̵㵥��',
   stocktakingtype      int comment '�̵������1 �̵�ձ�2 �̵�������',
   shelfcode            nvarchar(20) comment '������',
   createdby            int comment '������',
   createdbyname        nvarchar(50) comment '��������',
   createdon            datetime comment '����ʱ��',
   status               int comment '״̬����������',
   updatedon            datetime comment '�޸�ʱ��',
   updatedby            int comment '�޸���',
   updatedbyname        varchar(50) comment '�޸�����',
   storeid              int comment '�ŵ�',
   note                 nvarchar(1000) comment '��ע',
   primary key (id)
);

alter table stocktaking comment '�̵��';

/*==============================================================*/
/* Table: stocktakingitem                                       */
/*==============================================================*/
create table stocktakingitem
(
   id                   int not null auto_increment comment '���',
   stocktakingid        int,
   productid            nvarchar(50) comment '��Ʒ����',
   costprice            decimal(8,4) comment '�����ɱ���',
   saleprice            decimal(8,2) comment '���ۼ�',
   quantity             int comment '�̵����������',
   countquantity        int comment '�̵�����',
   corectquantity       int comment '������',
   corectreason         nvarchar(500) comment '����ԭ��',
   note                 nvarchar(500) comment '��ע',
   primary key (id)
);

alter table stocktakingitem comment '�̵���ϸ';

/*==============================================================*/
/* Table: stocktakingplan                                       */
/*==============================================================*/
create table stocktakingplan
(
   id                   int not null auto_increment comment '���',
   code                 nvarchar(20) comment '�̵����',
   createdby            int comment '������',
   createdbyname        nvarchar(50) comment '��������',
   createdon            datetime comment '����ʱ��',
   updatedby            int comment '������',
   updatedbyname        nvarchar(50) comment '��������',
   updatedon            datetime comment '����ʱ��',
   method               int comment '�̵㷽ʽ�����̣�С�̣�',
   status               int comment '�̵�״̬�����̣����̣����̣����̣�',
   storeid              int comment '�ŵ���',
   note                 nvarchar(1000) comment '��ע',
   stocktakingdate      datetime comment '�̵�����',
   primary key (id)
);

alter table stocktakingplan comment '�̵�ƻ�';

/*==============================================================*/
/* Table: stocktakingplanitem                                   */
/*==============================================================*/
create table stocktakingplanitem
(
   id                   int not null auto_increment comment '���',
   stocktakingplanid    int comment '�̵�ƻ����',
   productid            int comment 'ϵͳ����',
   costprice            decimal(8,4) comment '�����ɱ���',
   saleprice            decimal(8,2) comment '���ۼ�',
   quantity             int comment '�������',
   countquantity        int comment '�̵�����',
   primary key (id)
);

alter table stocktakingplanitem comment '�̵�ƻ���ϸ';

/*==============================================================*/
/* Table: store                                                 */
/*==============================================================*/
create table store
(
   id                   int not null auto_increment comment '���',
   code                 nvarchar(20) comment '����',
   number               int comment '���',
   name                 nvarchar(128) comment '�ŵ���',
   sourcekey            nvarchar(32) comment '�ŵ�Ψһ��',
   createdon            datetime comment '����ʱ��',
   createdby            int comment '������',
   areaid               char(6) comment '����ID',
   address              nvarchar(512) comment '��ַ',
   contact              nvarchar(32) comment '��ϵ��',
   phone                nvarchar(32) comment '��ϵ�绰',
   setting              text comment '����',
   licensecode          varchar(50) comment '�ŵ���Ȩ��',
   primary key (id)
);

alter table store comment '�ŵ�';

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
   id                   int not null auto_increment comment '���',
   storeid              int comment '�ŵ����',
   productid            int comment '��ƷId',
   salequantity         int comment '���ۿ��',
   orderquantity        int comment '�������',
   quantity             int comment 'ʵ�ʿ����',
   avgcostprice         decimal(8,4) comment 'ƽ���ɱ���',
   warnquantity         int comment '������',
   isquit               bool comment '�Ƿ��˳�',
   lastcostprice        decimal(8,2) comment '���½���',
   storesaleprice       decimal(8,2) comment '�ŵ��ۼ�',
   status               int comment '״̬',
   rowversion           timestamp comment '�а汾',
   primary key (id)
);

alter table storeinventory comment '�ŵ���';

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
   id                   int not null auto_increment comment '���',
   productid            int comment 'SKU����',
   storeid              int comment '�ֿ����',
   supplierid           int comment '��Ӧ��Id',
   quantity             int comment 'ʵ�ʿ����',
   productiondate       datetime comment '��������',
   shelflife            int comment '������',
   contractprice        decimal(8,4) comment '��ͬ��',
   price                decimal(8,4) comment 'ʵ��������',
   createdon            datetime comment '����ʱ��',
   createdby            int comment '������',
   batchno              bigint comment '���κ�',
   rowversion           timestamp comment '�а汾',
   primary key (id)
);

alter table storeinventorybatch comment '�ŵ���Ʒ����';

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
   id                   int not null auto_increment comment '���',
   productid            int comment 'SKU����',
   storeid              int comment '�ֿ����',
   quantity             int comment 'ʵ�ʿ����',
   changequantity       int comment '�䶯��',
   createdon            datetime comment '����ʱ��',
   createdby            int comment '������',
   billid               int comment '����ϵͳ��',
   billcode             varchar(20) comment '���ݱ���',
   billtype             int comment '��������',
   batchno              bigint comment '���κ�',
   price                decimal(14,4) comment '����',
   supplierid           int comment '��Ӧ��Id',
   saleprice            decimal(10,2) comment '�ۼ�',
   primary key (id)
);

alter table storeinventoryhistory comment '�ŵ�����ʷ��¼';

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
   id                   int not null auto_increment comment '���',
   monthly              varchar(10) comment '����ڼ� 2017-01 ���´洢',
   storeid              int comment '�ŵ����',
   productid            int comment '��ƷId',
   quantity             int comment 'ʵ�ʿ����',
   avgcostprice         decimal(8,4) comment 'ƽ���ɱ���',
   primary key (id)
);

alter table storeinventorymonthly comment '�ŵ����±�';

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
   id                   int not null auto_increment comment '���',
   code                 nvarchar(20) comment '������',
   ordertype            int comment '��������: �� 1 �� 2',
   storeid              int comment '�ŵ�Id',
   supplierbill         nvarchar(200) comment '��Ӧ�̵��ݺ�',
   supplierid           int comment '��Ӧ��Id',
   createdon            datetime comment '����ʱ��',
   createdby            int comment '������',
   createdbyname        nvarchar(50) comment '��������',
   receivedby           int comment '�ջ���',
   receivedon           datetime comment '�ջ�����',
   receivedbyname       varchar(50) comment '�ջ�����',
   storagedby           int comment '�����',
   storagedbyname       nvarchar(50) comment '�������',
   storagedon           datetime comment '�������',
   status               int comment '״̬',
   isgift               bool comment '�Ƿ���Ʒ',
   primary key (id)
);

alter table storepurchaseorder comment '�ŵ�ɹ�����';

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
   id                   int not null auto_increment comment '���',
   storepurchaseorderid int comment '�ŵ�ɹ��������',
   productid            int comment '��Ʒskuid',
   contractprice        decimal(8,4) comment '��ͬ��',
   price                decimal(8,4) comment '����',
   specificationquantity int comment '����',
   quantity             int comment '����',
   actualquantity       int comment 'ʵ������',
   productiondate       datetime comment '��������',
   shelflife            int comment '������',
   batchno              bigint comment '���κ�',
   primary key (id)
);

alter table storepurchaseorderitem comment '�ŵ�ɹ�������ϸ';

/*==============================================================*/
/* Table: supplier                                              */
/*==============================================================*/
create table supplier
(
   id                   int not null auto_increment comment '���',
   code                 nvarchar(20) comment '��Ӧ�̱���',
   name                 nvarchar(100) comment '��Ӧ����',
   type                 int comment '��Ӧ�����',
   shortname            nvarchar(50) comment '���',
   contact              nvarchar(300) comment '��ϵ��',
   phone                nvarchar(300) comment '��ϵ�绰',
   qq                   nvarchar(300),
   address              nvarchar(100),
   bank                 nvarchar(50) comment '������',
   bankaccount          nvarchar(50) comment '�������˺�',
   bankaccountname      varchar(50) comment '������',
   taxno                nvarchar(50) comment '˰��',
   licenseno            nvarchar(50) comment 'ִ�պ�',
   createdon            datetime comment '����ʱ��',
   createdby            int comment '������',
   updatedon            datetime comment '�޸�ʱ��',
   updatedby            int comment '�޸���',
   primary key (id)
);

alter table supplier comment '��Ӧ��';

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
   id                   int not null auto_increment comment '���',
   supplierid           int comment '��Ӧ��Id',
   productid            int comment '��Ʒ',
   price                decimal(8,4) comment '�۸�',
   status               int comment '����״̬',
   comparestatus        int comment '�ȼ�״̬',
   updatedon            datetime comment '�޸�ʱ��',
   updatedby            int comment '�޸���',
   primary key (id)
);

alter table supplierproduct comment '��Ӧ����Ʒ';

/*==============================================================*/
/* Table: transferorder                                         */
/*==============================================================*/
create table transferorder
(
   id                   int not null auto_increment comment '���',
   code                 varchar(20) comment '��������',
   fromstoreid          int comment '���ŵ�',
   fromstorename        char(50) comment '���ŵ���',
   tostorename          char(50) comment '���ŵ���',
   tostoreid            int comment '���ŵ�',
   status               int comment '״̬',
   createdon            datetime comment '����ʱ��',
   createdby            int comment '������',
   createdbyname        varchar(30) comment '��������',
   updatedon            datetime comment '�޸�ʱ��',
   updatedby            int comment '�޸���',
   updatedbyname        varchar(30) comment '�޸�����',
   primary key (id)
);

alter table transferorder comment '������';

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
   id                   int not null auto_increment comment '���',
   transferorderid      int comment '������ID',
   supplierid           int comment '��Ӧ��Id',
   productid            int comment 'SKU����',
   quantity             int comment '����',
   contractprice        decimal(8,4) comment '��ͬ��',
   price                decimal(8,4) comment '�ɱ���',
   batchno              bigint comment '����',
   productiondate       datetime comment '��������',
   shelflife            int comment '������',
   primary key (id)
);

alter table transferorderitem comment '������ϸ';

/*==============================================================*/
/* Table: vipcard                                               */
/*==============================================================*/
create table vipcard
(
   id                   int not null auto_increment,
   code                 varchar(50) comment '��Ա����',
   discount             decimal(8,2) comment '�ۿ�',
   primary key (id)
);

alter table vipcard comment '��Ա��';

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
   code                 nvarchar(20) comment '����',
   name                 nvarchar(50) comment '�ֿ���',
   areaid               char(6) comment '����',
   address              nvarchar(100) comment '��ַ',
   primary key (id)
);

alter table warehouse comment '�ֿ�';

/*==============================================================*/
/* Table: workschedule                                          */
/*==============================================================*/
create table workschedule
(
   id                   int not null auto_increment,
   code                 nvarchar(50) comment '����',
   storeid              int comment '�ŵ�',
   posid                int comment 'Pos��Id',
   cashamount           decimal(8,2) comment '���ֽ��',
   startdate            datetime comment '��ʼʱ��',
   enddate              datetime comment '����ʱ��',
   createdby            int comment '������',
   createdbyname        varchar(50) comment '��������',
   endby                int comment '������',
   endbyname            varchar(50) comment '��������',
   primary key (id)
);

alter table workschedule comment '��μ�¼��';

/*==============================================================*/
/* Index: idx_workschedule_code                                 */
/*==============================================================*/
create unique index idx_workschedule_code on workschedule
(
   code
);

