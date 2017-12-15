INSERT INTO `role` VALUES ('1', '系统管理员', '超级管理员');

-- userName = admin,password = admin
INSERT INTO `account` VALUES ('1', 'admin', '21232f297a57a5a743894a0e4a801fc3', 'admin', '1', '2017-12-15 10:23:33', '1', '0', '2017-12-15 10:23:59', '0', null);
-- ----------------------------
-- Records of menu
-- ----------------------------
INSERT INTO `menu` VALUES ('1', '0', '设置', '#', 'fa-gears', '0', '1');
INSERT INTO `menu` VALUES ('2', '1', '账户管理', '/Account/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('3', '1', '角色管理', '/Role/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('4', '1', '菜单管理', '/Menu/Index', 'fa-folder', '0', '1');

INSERT INTO `menu` VALUES ('5', '0', '资料建档', '#', 'fa-table', '0', '1');
INSERT INTO `menu` VALUES ('6', '5', '商品资料建档', '/Product/index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('7', '5', '品类资料建档', '/Category/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('8', '5', '品牌资料建档', '/Brand/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('9', '5', '供应商建档', '/Supplier/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('10', '5', '门店资料建档', '/Store/Index', 'fa-folder', '0', '1');

INSERT INTO `menu` VALUES ('11', '0', '采购', '#', 'fa-truck', '0', '1');
INSERT INTO `menu` VALUES ('12', '11', '采购订单-制单', '/StorePurchaseOrder/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('13', '11', '采购订单-收货|入库', '/StorePurchaseOrder/ReceiveIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('14', '11', '采购订单-查询', '/StorePurchaseOrder/Query', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('15', '11', '采购订单-财务审核', '/StorePurchaseOrder/FinanceIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('16', '11', '采购退单-制单', '/StorePurchaseOrder/RefundIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('17', '11', '采购退单-退货|出库', '/StorePurchaseOrder/WaitRefundIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('18', '11', '采购退单-查询', '/StorePurchaseOrder/QueryRefund', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('19', '11', '采购退单-财务审核', '/StorePurchaseOrder/FinanceRefundIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('20', '11', '采购单-汇总报表', '/StorePurchaseOrder/Summary', 'fa-folder', '0', '1');

INSERT INTO `menu` VALUES ('21', '0', '库存', '#', 'fa-database', '0', '1');
INSERT INTO `menu` VALUES ('22', '21', '库存查询', '/StoreInventory/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('23', '21', '库存流水', '/StoreInventory/History', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('24', '21', '库存批次', '/StoreInventory/Batch', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('25', '21', '商品资料查询', '/StoreInventory/Product', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('26', '21', '进销存报表', '/Report/PurchaseSaleInventory', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('27', '21', '进销存明细报表', '/Report/PurchaseSaleInventoryDetail', 'fa-folder', '0', '1');

INSERT INTO `menu` VALUES ('28', '0', '盘点', '#', 'fa-calculator', '0', '1');
INSERT INTO `menu` VALUES ('29', '28', '货架管理', '/Shelf/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('30', '28', '盘点计划', '/StocktakingPlan/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('31', '28', '盘点单-制单', '/Stocktaking/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('32', '28', '盘点差错汇总表', '/StocktakingPlan/Summary', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('33', '28', '盘点-已完成', '/StocktakingPlan/Finish', 'fa-folder', '0', '1');

INSERT INTO `menu` VALUES ('34', '0', '价格管理', '#', 'fa-rmb', '0', '1');
INSERT INTO `menu` VALUES ('35', '34', '售价调整', '/AdjustSalePrice/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('36', '34', '门店调价-制单', '/AdjustStorePrice/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('37', '34', '门店调价-待审', '/AdjustStorePrice/AuditIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('38', '34', '门店调价-已完', '/AdjustStorePrice/Finish', 'fa-folder', '0', '1');

INSERT INTO `menu` VALUES ('39', '0', '销售', '#', 'fa-line-chart', '0', '1');
INSERT INTO `menu` VALUES ('40', '39', '前台收银流水查询', '/SaleOrder/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('41', '39', '前台营业额核对', '/SaleOrder/SaleSummary', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('42', '39', '前台收银防损核对', '/SaleOrder/SaleCheck', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('43', '39', '前台销售对账', '/SaleOrder/SaleSync', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('44', '39', '单品销售汇总', '/SaleOrder/SingleProductSale', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('45', '39', '销售历史报表', '/SaleOrder/SaleReport', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('46', '39', '销售实时报表', '/SaleOrder/RealTimeSaleReport', 'fa-folder', '0', '1');

INSERT INTO `menu` VALUES ('47', '0', '调拨', '#', 'fa-exchange', '0', '1');
INSERT INTO `menu` VALUES ('48', '47', '调拨单-制单', '/TransferOrder/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('49', '47', '调拨单-审核', '/TransferOrder/AuditIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('50', '47', '调拨单-已完成', '/TransferOrder/Finish', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('51', '47', '调拨单-汇总报表', '/TransferOrder/Summary', 'fa-folder', '0', '1');

INSERT INTO `menu` VALUES ('52', '0', '合同', '#', 'fa-book', '0', '1');
INSERT INTO `menu` VALUES ('53', '52', '供应商商品比价', '/Supplier/Product', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('54', '52', '采购合同-建档', '/PurchaseContract/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('55', '52', '采购合同-审核', '/PurchaseContract/AuditIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('56', '52', '合同调价-制单', '/AdjustContractPrice/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('57', '52', '合同调价-待审', '/AdjustContractPrice/AuditIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('58', '52', '合同调价-已审', '/AdjustContractPrice/Audited', 'fa-folder', '0', '1');

