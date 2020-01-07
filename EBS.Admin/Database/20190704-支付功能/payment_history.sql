/*
 Navicat Premium Data Transfer

 Source Server         : 本机mysql
 Source Server Type    : MySQL
 Source Server Version : 80011
 Source Host           : localhost:3306
 Source Schema         : ebsdb

 Target Server Type    : MySQL
 Target Server Version : 80011
 File Encoding         : 65001

 Date: 11/07/2019 17:37:35
*/

SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

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
) ENGINE = InnoDB CHARACTER SET = utf8mb4 COLLATE = utf8mb4_general_ci ROW_FORMAT = Dynamic;

-- ----------------------------
-- Records of setting
-- ----------------------------
INSERT INTO `setting` VALUES (1, '本机域名', 'system.domain', '', 'http://localhost', 0, 0);
INSERT INTO `setting` VALUES (2, '支付回调url', 'pay.notify.url', '', '/Pay/Notify', 0, 0);
INSERT INTO `setting` VALUES (3, '支付跳转url', 'pay.return.url', '', '/Pay/Return', 0, 0);
INSERT INTO `setting` VALUES (4, '支付宝appid', 'pay.alipay.appid', '', 'alipay1', 0, 0);
INSERT INTO `setting` VALUES (5, '支付宝公密', 'pay.alipay.public.key', '', '', 0, 0);
INSERT INTO `setting` VALUES (6, '支付宝私密', 'pay.alipay.private.key', '', '', 0, 0);
INSERT INTO `setting` VALUES (7, '微信appid', 'pay.wechat.appid', '', 'wechat1', 0, 0);
INSERT INTO `setting` VALUES (8, '微信密匙', 'pay.wechat.appsecret', '', '', 0, 0);
INSERT INTO `setting` VALUES (9, '微信商户号', 'pay.wechat.mchid', '', '', 0, 0);
INSERT INTO `setting` VALUES (10, '微信商户密匙', 'pay.wechat.mchkey', '', '', 0, 0);

SET FOREIGN_KEY_CHECKS = 1;


alter table saleorder
add SourceSaleOrderCode varchar(32);
