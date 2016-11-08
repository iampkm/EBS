/*
Navicat MySQL Data Transfer

Source Server         : 测试库
Source Server Version : 50710
Source Host           : localhost:3306
Source Database       : ebs

Target Server Type    : MYSQL
Target Server Version : 50710
File Encoding         : 65001

Date: 2016-11-08 23:36:35
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `menu`
-- ----------------------------
DROP TABLE IF EXISTS `menu`;
CREATE TABLE `menu` (
  `Id` int(11) NOT NULL AUTO_INCREMENT COMMENT '编号',
  `ParentId` int(11) DEFAULT NULL COMMENT '父编号',
  `Name` varchar(64) DEFAULT NULL COMMENT '名称',
  `Url` varchar(256) DEFAULT NULL COMMENT '连接',
  `Icon` varchar(64) DEFAULT NULL COMMENT '图标',
  `DisplayOrder` int(11) DEFAULT NULL COMMENT '显示顺序',
  `UrlType` int(11) DEFAULT NULL COMMENT '连接类型',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8 COMMENT='系统菜单';

-- ----------------------------
-- Records of menu
-- ----------------------------
INSERT INTO `menu` VALUES ('1', '0', '设置', '#', 'fa-gears', '0', '1');
INSERT INTO `menu` VALUES ('2', '1', '账户管理', '/Account/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('3', '1', '角色管理', '/Role/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('4', '1', '菜单管理', '/Menu/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('5', '0', '数据维护', '#', 'fa-table', '0', '1');
INSERT INTO `menu` VALUES ('6', '5', '商品管理', '/Product/index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('7', '5', '品类管理', '/Category/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('8', '5', '品牌管理', '/Brand/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('9', '0', '采购', '#', 'fa-truck', '0', '1');
INSERT INTO `menu` VALUES ('10', '9', '采购单', '/StorePurchaseOrder/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('11', '9', '供应商', '/Supplier/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('12', '9', '采购合同', '/PurchaseContract/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('13', '9', '采购合同-审核', '/PurchaseContract/AuditIndex', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('14', '0', '库存', '#', 'fa-database', '0', '1');
INSERT INTO `menu` VALUES ('15', '14', '库存查询', '/StoreInventory/Index', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('16', '14', '库存流水', '/StoreInventory/History', 'fa-folder', '0', '1');
INSERT INTO `menu` VALUES ('17', '14', '库存批次', '/StoreInventory/Batch', 'fa-folder', '0', '1');
