INSERT INTO `role` VALUES ('1', '系统管理员', '超级管理员');

-- userName = admin,password = admin
INSERT INTO `account` VALUES ('1', 'admin', '21232f297a57a5a743894a0e4a801fc3', 'admin', '1', '2017-12-15 10:23:33', '1', '0', '2017-12-15 10:23:59', '0', null);

-- ----------------------------
-- Records of role
-- ----------------------------
INSERT INTO `role` VALUES (1, '系统管理员', '超级管理员');
INSERT INTO `role` VALUES (2, '门店管理员', NULL);
INSERT INTO `role` VALUES (3, '店长', NULL);
INSERT INTO `role` VALUES (4, '店员', NULL);
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
INSERT INTO `menu` VALUES ('57', '52', '合同调价-审核', '/AdjustContractPrice/AuditIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('58', '52', '合同调价-已审', '/AdjustContractPrice/Audited', 'fa-folder', '0', '1');

INSERT INTO `menu` VALUES ('59', '0', '其他出入库', '#', 'fa-bus', '0', '1');
INSERT INTO `menu` VALUES ('60', '59', '其他入库-制单', '/OutInOrder/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('61', '59', '其他入库单-审核', '/OutInOrder/AuditIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('62', '59', '其他入库单-财务复审', '/OutInOrder/FinanceIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('63', '59', '其他入库-查询', '/OutInOrder/Finish', 'fa-folder', '0', '1');

INSERT INTO `menu` VALUES ('64', '59', '其他出库-制单', '/OutInOrder/RefundIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('65', '59', '其他出库单-审核', '/OutInOrder/RefundAuditIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('66', '59', '其他出库单-财务复审', '/OutInOrder/RefundFinanceIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('67', '59', '其他出库-查询', '/OutInOrder/RefundFinish', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('68', '59', '其他出入库-汇总', '/OutInOrder/Summary', 'fa-folder', '0', '1');

INSERT INTO `menu` VALUES ('69', '5', '门店授权码', '/Store/EditLicense', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('70', '34', '会员价管理', '/VipProduct/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('71', '34', '商品价格校对', '/Product/PriceCheck', 'fa-folder', '0', '1');


-- ----------------------------
-- Records of rolemenu
-- ----------------------------
INSERT INTO `rolemenu` VALUES (1, 2, 1);
INSERT INTO `rolemenu` VALUES (2, 2, 2);
INSERT INTO `rolemenu` VALUES (3, 2, 3);
INSERT INTO `rolemenu` VALUES (4, 2, 4);
INSERT INTO `rolemenu` VALUES (5, 2, 5);
INSERT INTO `rolemenu` VALUES (6, 2, 6);
INSERT INTO `rolemenu` VALUES (7, 2, 7);
INSERT INTO `rolemenu` VALUES (8, 2, 8);
INSERT INTO `rolemenu` VALUES (9, 2, 9);
INSERT INTO `rolemenu` VALUES (10, 2, 10);
INSERT INTO `rolemenu` VALUES (11, 2, 69);
INSERT INTO `rolemenu` VALUES (12, 2, 11);
INSERT INTO `rolemenu` VALUES (13, 2, 12);
INSERT INTO `rolemenu` VALUES (14, 2, 13);
INSERT INTO `rolemenu` VALUES (15, 2, 14);
INSERT INTO `rolemenu` VALUES (16, 2, 15);
INSERT INTO `rolemenu` VALUES (17, 2, 16);
INSERT INTO `rolemenu` VALUES (18, 2, 17);
INSERT INTO `rolemenu` VALUES (19, 2, 18);
INSERT INTO `rolemenu` VALUES (20, 2, 19);
INSERT INTO `rolemenu` VALUES (21, 2, 20);
INSERT INTO `rolemenu` VALUES (22, 2, 21);
INSERT INTO `rolemenu` VALUES (23, 2, 22);
INSERT INTO `rolemenu` VALUES (24, 2, 23);
INSERT INTO `rolemenu` VALUES (25, 2, 24);
INSERT INTO `rolemenu` VALUES (26, 2, 25);
INSERT INTO `rolemenu` VALUES (27, 2, 26);
INSERT INTO `rolemenu` VALUES (28, 2, 27);
INSERT INTO `rolemenu` VALUES (29, 2, 28);
INSERT INTO `rolemenu` VALUES (30, 2, 29);
INSERT INTO `rolemenu` VALUES (31, 2, 30);
INSERT INTO `rolemenu` VALUES (32, 2, 31);
INSERT INTO `rolemenu` VALUES (33, 2, 32);
INSERT INTO `rolemenu` VALUES (34, 2, 33);
INSERT INTO `rolemenu` VALUES (35, 2, 34);
INSERT INTO `rolemenu` VALUES (36, 2, 35);
INSERT INTO `rolemenu` VALUES (37, 2, 36);
INSERT INTO `rolemenu` VALUES (38, 2, 37);
INSERT INTO `rolemenu` VALUES (39, 2, 38);
INSERT INTO `rolemenu` VALUES (40, 2, 70);
INSERT INTO `rolemenu` VALUES (41, 2, 71);
INSERT INTO `rolemenu` VALUES (42, 2, 39);
INSERT INTO `rolemenu` VALUES (43, 2, 40);
INSERT INTO `rolemenu` VALUES (44, 2, 41);
INSERT INTO `rolemenu` VALUES (45, 2, 42);
INSERT INTO `rolemenu` VALUES (46, 2, 43);
INSERT INTO `rolemenu` VALUES (47, 2, 44);
INSERT INTO `rolemenu` VALUES (48, 2, 45);
INSERT INTO `rolemenu` VALUES (49, 2, 46);
INSERT INTO `rolemenu` VALUES (50, 2, 47);
INSERT INTO `rolemenu` VALUES (51, 2, 48);
INSERT INTO `rolemenu` VALUES (52, 2, 49);
INSERT INTO `rolemenu` VALUES (53, 2, 50);
INSERT INTO `rolemenu` VALUES (54, 2, 51);
INSERT INTO `rolemenu` VALUES (55, 2, 52);
INSERT INTO `rolemenu` VALUES (56, 2, 53);
INSERT INTO `rolemenu` VALUES (57, 2, 54);
INSERT INTO `rolemenu` VALUES (58, 2, 55);
INSERT INTO `rolemenu` VALUES (59, 2, 56);
INSERT INTO `rolemenu` VALUES (60, 2, 57);
INSERT INTO `rolemenu` VALUES (61, 2, 58);
INSERT INTO `rolemenu` VALUES (62, 2, 59);
INSERT INTO `rolemenu` VALUES (63, 2, 60);
INSERT INTO `rolemenu` VALUES (64, 2, 61);
INSERT INTO `rolemenu` VALUES (65, 2, 62);
INSERT INTO `rolemenu` VALUES (66, 2, 63);
INSERT INTO `rolemenu` VALUES (67, 2, 64);
INSERT INTO `rolemenu` VALUES (68, 2, 65);
INSERT INTO `rolemenu` VALUES (69, 2, 66);
INSERT INTO `rolemenu` VALUES (70, 2, 67);
INSERT INTO `rolemenu` VALUES (71, 2, 68);
INSERT INTO `rolemenu` VALUES (72, 3, 5);
INSERT INTO `rolemenu` VALUES (73, 3, 69);
INSERT INTO `rolemenu` VALUES (74, 3, 11);
INSERT INTO `rolemenu` VALUES (75, 3, 12);
INSERT INTO `rolemenu` VALUES (76, 3, 13);
INSERT INTO `rolemenu` VALUES (77, 3, 14);
INSERT INTO `rolemenu` VALUES (78, 3, 15);
INSERT INTO `rolemenu` VALUES (79, 3, 16);
INSERT INTO `rolemenu` VALUES (80, 3, 17);
INSERT INTO `rolemenu` VALUES (81, 3, 18);
INSERT INTO `rolemenu` VALUES (82, 3, 19);
INSERT INTO `rolemenu` VALUES (83, 3, 20);
INSERT INTO `rolemenu` VALUES (84, 3, 21);
INSERT INTO `rolemenu` VALUES (85, 3, 22);
INSERT INTO `rolemenu` VALUES (86, 3, 23);
INSERT INTO `rolemenu` VALUES (87, 3, 24);
INSERT INTO `rolemenu` VALUES (88, 3, 25);
INSERT INTO `rolemenu` VALUES (89, 3, 26);
INSERT INTO `rolemenu` VALUES (90, 3, 27);
INSERT INTO `rolemenu` VALUES (91, 3, 28);
INSERT INTO `rolemenu` VALUES (92, 3, 29);
INSERT INTO `rolemenu` VALUES (93, 3, 30);
INSERT INTO `rolemenu` VALUES (94, 3, 31);
INSERT INTO `rolemenu` VALUES (95, 3, 32);
INSERT INTO `rolemenu` VALUES (96, 3, 33);
INSERT INTO `rolemenu` VALUES (97, 3, 34);
INSERT INTO `rolemenu` VALUES (98, 3, 36);
INSERT INTO `rolemenu` VALUES (99, 3, 37);
INSERT INTO `rolemenu` VALUES (100, 3, 38);
INSERT INTO `rolemenu` VALUES (101, 3, 70);
INSERT INTO `rolemenu` VALUES (102, 3, 71);
INSERT INTO `rolemenu` VALUES (103, 3, 39);
INSERT INTO `rolemenu` VALUES (104, 3, 40);
INSERT INTO `rolemenu` VALUES (105, 3, 41);
INSERT INTO `rolemenu` VALUES (106, 3, 42);
INSERT INTO `rolemenu` VALUES (107, 3, 43);
INSERT INTO `rolemenu` VALUES (108, 3, 44);
INSERT INTO `rolemenu` VALUES (109, 3, 45);
INSERT INTO `rolemenu` VALUES (110, 3, 46);
INSERT INTO `rolemenu` VALUES (111, 3, 47);
INSERT INTO `rolemenu` VALUES (112, 3, 48);
INSERT INTO `rolemenu` VALUES (113, 3, 49);
INSERT INTO `rolemenu` VALUES (114, 3, 50);
INSERT INTO `rolemenu` VALUES (115, 3, 51);
INSERT INTO `rolemenu` VALUES (116, 3, 59);
INSERT INTO `rolemenu` VALUES (117, 3, 60);
INSERT INTO `rolemenu` VALUES (118, 3, 61);
INSERT INTO `rolemenu` VALUES (119, 3, 62);
INSERT INTO `rolemenu` VALUES (120, 3, 63);
INSERT INTO `rolemenu` VALUES (121, 3, 64);
INSERT INTO `rolemenu` VALUES (122, 3, 65);
INSERT INTO `rolemenu` VALUES (123, 3, 66);
INSERT INTO `rolemenu` VALUES (124, 3, 67);
INSERT INTO `rolemenu` VALUES (125, 3, 68);
INSERT INTO `rolemenu` VALUES (126, 4, 11);
INSERT INTO `rolemenu` VALUES (127, 4, 12);
INSERT INTO `rolemenu` VALUES (128, 4, 13);
INSERT INTO `rolemenu` VALUES (129, 4, 14);
INSERT INTO `rolemenu` VALUES (130, 4, 16);
INSERT INTO `rolemenu` VALUES (131, 4, 17);
INSERT INTO `rolemenu` VALUES (132, 4, 18);
INSERT INTO `rolemenu` VALUES (133, 4, 21);
INSERT INTO `rolemenu` VALUES (134, 4, 25);
INSERT INTO `rolemenu` VALUES (135, 4, 28);
INSERT INTO `rolemenu` VALUES (136, 4, 29);
INSERT INTO `rolemenu` VALUES (137, 4, 31);
INSERT INTO `rolemenu` VALUES (138, 4, 34);
INSERT INTO `rolemenu` VALUES (139, 4, 71);
INSERT INTO `rolemenu` VALUES (140, 4, 39);
INSERT INTO `rolemenu` VALUES (141, 4, 43);
INSERT INTO `rolemenu` VALUES (142, 4, 47);
INSERT INTO `rolemenu` VALUES (143, 4, 48);
INSERT INTO `rolemenu` VALUES (144, 4, 50);
INSERT INTO `rolemenu` VALUES (145, 4, 59);
INSERT INTO `rolemenu` VALUES (146, 4, 60);
INSERT INTO `rolemenu` VALUES (147, 4, 63);
INSERT INTO `rolemenu` VALUES (148, 4, 64);
INSERT INTO `rolemenu` VALUES (149, 4, 67);