INSERT INTO `role` VALUES ('1', 'ϵͳ����Ա', '��������Ա');

-- userName = admin,password = admin
INSERT INTO `account` VALUES ('1', 'admin', '21232f297a57a5a743894a0e4a801fc3', 'admin', '1', '2017-12-15 10:23:33', '1', '0', '2017-12-15 10:23:59', '0', null);
-- ----------------------------
-- Records of menu
-- ----------------------------
INSERT INTO `menu` VALUES ('1', '0', '����', '#', 'fa-gears', '0', '1');
INSERT INTO `menu` VALUES ('2', '1', '�˻�����', '/Account/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('3', '1', '��ɫ����', '/Role/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('4', '1', '�˵�����', '/Menu/Index', 'fa-folder', '0', '1');

INSERT INTO `menu` VALUES ('5', '0', '���Ͻ���', '#', 'fa-table', '0', '1');
INSERT INTO `menu` VALUES ('6', '5', '��Ʒ���Ͻ���', '/Product/index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('7', '5', 'Ʒ�����Ͻ���', '/Category/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('8', '5', 'Ʒ�����Ͻ���', '/Brand/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('9', '5', '��Ӧ�̽���', '/Supplier/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('10', '5', '�ŵ����Ͻ���', '/Store/Index', 'fa-folder', '0', '1');

INSERT INTO `menu` VALUES ('11', '0', '�ɹ�', '#', 'fa-truck', '0', '1');
INSERT INTO `menu` VALUES ('12', '11', '�ɹ�����-�Ƶ�', '/StorePurchaseOrder/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('13', '11', '�ɹ�����-�ջ�|���', '/StorePurchaseOrder/ReceiveIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('14', '11', '�ɹ�����-��ѯ', '/StorePurchaseOrder/Query', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('15', '11', '�ɹ�����-�������', '/StorePurchaseOrder/FinanceIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('16', '11', '�ɹ��˵�-�Ƶ�', '/StorePurchaseOrder/RefundIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('17', '11', '�ɹ��˵�-�˻�|����', '/StorePurchaseOrder/WaitRefundIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('18', '11', '�ɹ��˵�-��ѯ', '/StorePurchaseOrder/QueryRefund', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('19', '11', '�ɹ��˵�-�������', '/StorePurchaseOrder/FinanceRefundIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('20', '11', '�ɹ���-���ܱ���', '/StorePurchaseOrder/Summary', 'fa-folder', '0', '1');

INSERT INTO `menu` VALUES ('21', '0', '���', '#', 'fa-database', '0', '1');
INSERT INTO `menu` VALUES ('22', '21', '����ѯ', '/StoreInventory/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('23', '21', '�����ˮ', '/StoreInventory/History', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('24', '21', '�������', '/StoreInventory/Batch', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('25', '21', '��Ʒ���ϲ�ѯ', '/StoreInventory/Product', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('26', '21', '�����汨��', '/Report/PurchaseSaleInventory', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('27', '21', '��������ϸ����', '/Report/PurchaseSaleInventoryDetail', 'fa-folder', '0', '1');

INSERT INTO `menu` VALUES ('28', '0', '�̵�', '#', 'fa-calculator', '0', '1');
INSERT INTO `menu` VALUES ('29', '28', '���ܹ���', '/Shelf/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('30', '28', '�̵�ƻ�', '/StocktakingPlan/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('31', '28', '�̵㵥-�Ƶ�', '/Stocktaking/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('32', '28', '�̵�����ܱ�', '/StocktakingPlan/Summary', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('33', '28', '�̵�-�����', '/StocktakingPlan/Finish', 'fa-folder', '0', '1');

INSERT INTO `menu` VALUES ('34', '0', '�۸����', '#', 'fa-rmb', '0', '1');
INSERT INTO `menu` VALUES ('35', '34', '�ۼ۵���', '/AdjustSalePrice/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('36', '34', '�ŵ����-�Ƶ�', '/AdjustStorePrice/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('37', '34', '�ŵ����-����', '/AdjustStorePrice/AuditIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('38', '34', '�ŵ����-����', '/AdjustStorePrice/Finish', 'fa-folder', '0', '1');

INSERT INTO `menu` VALUES ('39', '0', '����', '#', 'fa-line-chart', '0', '1');
INSERT INTO `menu` VALUES ('40', '39', 'ǰ̨������ˮ��ѯ', '/SaleOrder/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('41', '39', 'ǰ̨Ӫҵ��˶�', '/SaleOrder/SaleSummary', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('42', '39', 'ǰ̨��������˶�', '/SaleOrder/SaleCheck', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('43', '39', 'ǰ̨���۶���', '/SaleOrder/SaleSync', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('44', '39', '��Ʒ���ۻ���', '/SaleOrder/SingleProductSale', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('45', '39', '������ʷ����', '/SaleOrder/SaleReport', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('46', '39', '����ʵʱ����', '/SaleOrder/RealTimeSaleReport', 'fa-folder', '0', '1');

INSERT INTO `menu` VALUES ('47', '0', '����', '#', 'fa-exchange', '0', '1');
INSERT INTO `menu` VALUES ('48', '47', '������-�Ƶ�', '/TransferOrder/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('49', '47', '������-���', '/TransferOrder/AuditIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('50', '47', '������-�����', '/TransferOrder/Finish', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('51', '47', '������-���ܱ���', '/TransferOrder/Summary', 'fa-folder', '0', '1');

INSERT INTO `menu` VALUES ('52', '0', '��ͬ', '#', 'fa-book', '0', '1');
INSERT INTO `menu` VALUES ('53', '52', '��Ӧ����Ʒ�ȼ�', '/Supplier/Product', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('54', '52', '�ɹ���ͬ-����', '/PurchaseContract/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('55', '52', '�ɹ���ͬ-���', '/PurchaseContract/AuditIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('56', '52', '��ͬ����-�Ƶ�', '/AdjustContractPrice/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('57', '52', '��ͬ����-����', '/AdjustContractPrice/AuditIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('58', '52', '��ͬ����-����', '/AdjustContractPrice/Audited', 'fa-folder', '0', '1');

