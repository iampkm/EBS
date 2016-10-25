/*
Navicat MySQL Data Transfer

Source Server         : 本地mysql
Source Server Version : 50710
Source Host           : localhost:3306
Source Database       : ebs

Target Server Type    : MYSQL
Target Server Version : 50710
File Encoding         : 65001

Date: 2016-10-24 09:45:05
*/

SET FOREIGN_KEY_CHECKS=0;

-- ----------------------------
-- Table structure for `category`
-- ----------------------------
DROP TABLE IF EXISTS `category`;
CREATE TABLE `category` (
  `Id` varchar(16) NOT NULL COMMENT '编号',
  `Name` varchar(64) DEFAULT NULL COMMENT '分类名',
  `FullName` varchar(256) DEFAULT NULL COMMENT '全名',
  `Level` int(11) DEFAULT NULL COMMENT '层级',
  PRIMARY KEY (`Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='商品分类';

-- ----------------------------
-- Records of category
-- ----------------------------
INSERT INTO `category` VALUES ('01', '食品', '食品', '1');
INSERT INTO `category` VALUES ('0101', '烟类', '食品-烟类', '2');
INSERT INTO `category` VALUES ('010101', '国产烟', '食品-烟类-国产烟', '3');
INSERT INTO `category` VALUES ('0102', '酒类', '食品-酒类', '2');
INSERT INTO `category` VALUES ('010201', '白酒', '食品-酒类-白酒', '3');
INSERT INTO `category` VALUES ('010202', '黄酒米酒', '食品-酒类-黄酒米酒', '3');
INSERT INTO `category` VALUES ('010203', '啤酒', '食品-酒类-啤酒', '3');
INSERT INTO `category` VALUES ('010204', '葡萄酒', '食品-酒类-葡萄酒', '3');
INSERT INTO `category` VALUES ('010205', '洋酒', '食品-酒类-洋酒', '3');
INSERT INTO `category` VALUES ('010206', '预调酒', '食品-酒类-预调酒', '3');
INSERT INTO `category` VALUES ('0103', '饮料', '食品-饮料', '2');
INSERT INTO `category` VALUES ('010301', '碳酸饮料', '食品-饮料-碳酸饮料', '3');
INSERT INTO `category` VALUES ('010302', '饮用水', '食品-饮料-饮用水', '3');
INSERT INTO `category` VALUES ('010303', '苏打饮料', '食品-饮料-苏打饮料', '3');
INSERT INTO `category` VALUES ('010304', '茶饮料', '食品-饮料-茶饮料', '3');
INSERT INTO `category` VALUES ('010305', '果汁', '食品-饮料-果汁', '3');
INSERT INTO `category` VALUES ('010306', '机能饮料', '食品-饮料-机能饮料', '3');
INSERT INTO `category` VALUES ('010307', '豆乳饮料', '食品-饮料-豆乳饮料', '3');
INSERT INTO `category` VALUES ('010308', '咖啡直饮', '食品-饮料-咖啡直饮', '3');
INSERT INTO `category` VALUES ('0104', '饼干点心', '食品-饼干点心', '2');
INSERT INTO `category` VALUES ('010401', '饼干', '食品-饼干点心-饼干', '3');
INSERT INTO `category` VALUES ('010402', '点心', '食品-饼干点心-点心', '3');
INSERT INTO `category` VALUES ('010403', '土特产', '食品-饼干点心-土特产', '3');
INSERT INTO `category` VALUES ('0105', '糖果', '食品-糖果', '2');
INSERT INTO `category` VALUES ('010501', '硬糖', '食品-糖果-硬糖', '3');
INSERT INTO `category` VALUES ('010502', '软糖', '食品-糖果-软糖', '3');
INSERT INTO `category` VALUES ('010503', '口香/润喉糖', '食品-糖果-口香/润喉糖', '3');
INSERT INTO `category` VALUES ('010504', '巧克力', '食品-糖果-巧克力', '3');
INSERT INTO `category` VALUES ('0106', '休闲小食', '食品-休闲小食', '2');
INSERT INTO `category` VALUES ('010601', '膨化食品', '食品-休闲小食-膨化食品', '3');
INSERT INTO `category` VALUES ('010602', '蜜饯果干', '食品-休闲小食-蜜饯果干', '3');
INSERT INTO `category` VALUES ('010603', '坚果炒货', '食品-休闲小食-坚果炒货', '3');
INSERT INTO `category` VALUES ('010604', '肉干小吃', '食品-休闲小食-肉干小吃', '3');
INSERT INTO `category` VALUES ('010605', '豆干素食', '食品-休闲小食-豆干素食', '3');
INSERT INTO `category` VALUES ('010606', '果冻布丁', '食品-休闲小食-果冻布丁', '3');
INSERT INTO `category` VALUES ('0107', '季节性食品', '食品-季节性食品', '2');
INSERT INTO `category` VALUES ('010701', '月饼', '食品-季节性食品-月饼', '3');
INSERT INTO `category` VALUES ('010702', '粽子', '食品-季节性食品-粽子', '3');
INSERT INTO `category` VALUES ('0108', '婴幼儿食品', '食品-婴幼儿食品', '2');
INSERT INTO `category` VALUES ('010801', '婴幼儿奶粉', '食品-婴幼儿食品-婴幼儿奶粉', '3');
INSERT INTO `category` VALUES ('010802', '婴幼儿辅食', '食品-婴幼儿食品-婴幼儿辅食', '3');
INSERT INTO `category` VALUES ('0109', '茶叶', '食品-茶叶', '2');
INSERT INTO `category` VALUES ('010901', '袋泡茶', '食品-茶叶-袋泡茶', '3');
INSERT INTO `category` VALUES ('010902', '散装茶叶', '食品-茶叶-散装茶叶', '3');
INSERT INTO `category` VALUES ('0110', '冲调品', '食品-冲调品', '2');
INSERT INTO `category` VALUES ('011001', '咖啡', '食品-冲调品-咖啡', '3');
INSERT INTO `category` VALUES ('011002', '麦片', '食品-冲调品-麦片', '3');
INSERT INTO `category` VALUES ('011003', '中式冲调粉', '食品-冲调品-中式冲调粉', '3');
INSERT INTO `category` VALUES ('011004', '饮料冲调粉', '食品-冲调品-饮料冲调粉', '3');
INSERT INTO `category` VALUES ('0111', '保健食品滋补品', '食品-保健食品滋补品', '2');
INSERT INTO `category` VALUES ('011101', '蜂产品', '食品-保健食品滋补品-蜂产品', '3');
INSERT INTO `category` VALUES ('0112', '食品油', '食品-食品油', '2');
INSERT INTO `category` VALUES ('011201', '桶装油', '食品-食品油-桶装油', '3');
INSERT INTO `category` VALUES ('0113', '米面制品', '食品-米面制品', '2');
INSERT INTO `category` VALUES ('011301', '米类', '食品-米面制品-米类', '3');
INSERT INTO `category` VALUES ('011302', '面类', '食品-米面制品-面类', '3');
INSERT INTO `category` VALUES ('011303', '杂粮类', '食品-米面制品-杂粮类', '3');
INSERT INTO `category` VALUES ('0114', '方便食品', '食品-方便食品', '2');
INSERT INTO `category` VALUES ('011401', '方便面', '食品-方便食品-方便面', '3');
INSERT INTO `category` VALUES ('011402', '方便饭', '食品-方便食品-方便饭', '3');
INSERT INTO `category` VALUES ('011403', '早餐熟食', '食品-方便食品-早餐熟食', '3');
INSERT INTO `category` VALUES ('0115', '调味品', '食品-调味品', '2');
INSERT INTO `category` VALUES ('011501', '酱油', '食品-调味品-酱油', '3');
INSERT INTO `category` VALUES ('011502', '醋', '食品-调味品-醋', '3');
INSERT INTO `category` VALUES ('011503', '其他调味品', '食品-调味品-其他调味品', '3');
INSERT INTO `category` VALUES ('011504', '佐餐食品', '食品-调味品-佐餐食品', '3');
INSERT INTO `category` VALUES ('0116', '调味料', '食品-调味料', '2');
INSERT INTO `category` VALUES ('011601', '盐/糖', '食品-调味料-盐/糖', '3');
INSERT INTO `category` VALUES ('011602', '调鲜品', '食品-调味料-调鲜品', '3');
INSERT INTO `category` VALUES ('011603', '粉料', '食品-调味料-粉料', '3');
INSERT INTO `category` VALUES ('011604', '辛香料', '食品-调味料-辛香料', '3');
INSERT INTO `category` VALUES ('011605', '酱料汤料', '食品-调味料-酱料汤料', '3');
INSERT INTO `category` VALUES ('0117', '罐头食品', '食品-罐头食品', '2');
INSERT INTO `category` VALUES ('011701', '肉类罐头', '食品-罐头食品-肉类罐头', '3');
INSERT INTO `category` VALUES ('011702', '蔬菜水果罐头', '食品-罐头食品-蔬菜水果罐头', '3');
INSERT INTO `category` VALUES ('011703', '八宝饭/粥', '食品-罐头食品-八宝饭/粥', '3');
INSERT INTO `category` VALUES ('0118', '腌腊品', '食品-腌腊品', '2');
INSERT INTO `category` VALUES ('0119', '干副', '食品-干副', '2');
INSERT INTO `category` VALUES ('011901', '干果类', '食品-干副-干果类', '3');
INSERT INTO `category` VALUES ('011902', '干菜类', '食品-干副-干菜类', '3');
INSERT INTO `category` VALUES ('011903', '海产品类', '食品-干副-海产品类', '3');
INSERT INTO `category` VALUES ('0120', '乳制品', '食品-乳制品', '2');
INSERT INTO `category` VALUES ('012001', '常温奶制品', '食品-乳制品-常温奶制品', '3');
INSERT INTO `category` VALUES ('012002', '低温奶', '食品-乳制品-低温奶', '3');
INSERT INTO `category` VALUES ('012003', '常温酸牛奶', '食品-乳制品-常温酸牛奶', '3');
INSERT INTO `category` VALUES ('0121', '冷藏食品', '食品-冷藏食品', '2');
INSERT INTO `category` VALUES ('012101', '肉肠类', '食品-冷藏食品-肉肠类', '3');
INSERT INTO `category` VALUES ('0122', '冷冻食品', '食品-冷冻食品', '2');
INSERT INTO `category` VALUES ('012201', '冷冻生鲜', '食品-冷冻食品-冷冻生鲜', '3');
INSERT INTO `category` VALUES ('012202', '雪糕冷饮', '食品-冷冻食品-雪糕冷饮', '3');
INSERT INTO `category` VALUES ('02', '用品', '用品', '1');
INSERT INTO `category` VALUES ('0201', '护肤美妆用品', '用品-护肤美妆用品', '2');
INSERT INTO `category` VALUES ('020101', '护肤品', '用品-护肤美妆用品-护肤品', '3');
INSERT INTO `category` VALUES ('020102', '身体护理', '用品-护肤美妆用品-身体护理', '3');
INSERT INTO `category` VALUES ('020103', '男式', '用品-护肤美妆用品-男式', '3');
INSERT INTO `category` VALUES ('020104', '美妆用品', '用品-护肤美妆用品-美妆用品', '3');
INSERT INTO `category` VALUES ('020105', '唇部护理', '用品-护肤美妆用品-唇部护理', '3');
INSERT INTO `category` VALUES ('020106', '彩妆', '用品-护肤美妆用品-彩妆', '3');
INSERT INTO `category` VALUES ('020107', '防晒用品', '用品-护肤美妆用品-防晒用品', '3');
INSERT INTO `category` VALUES ('020108', '面膜', '用品-护肤美妆用品-面膜', '3');
INSERT INTO `category` VALUES ('020109', '洁面用品', '用品-护肤美妆用品-洁面用品', '3');
INSERT INTO `category` VALUES ('0202', '个洗用品', '用品-个洗用品', '2');
INSERT INTO `category` VALUES ('020201', '洗发护发', '用品-个洗用品-洗发护发', '3');
INSERT INTO `category` VALUES ('020202', '沐浴用品', '用品-个洗用品-沐浴用品', '3');
INSERT INTO `category` VALUES ('020203', '口腔清洁用品', '用品-个洗用品-口腔清洁用品', '3');
INSERT INTO `category` VALUES ('020204', '洗手液', '用品-个洗用品-洗手液', '3');
INSERT INTO `category` VALUES ('0203', '纸制品', '用品-纸制品', '2');
INSERT INTO `category` VALUES ('020301', '卫生纸', '用品-纸制品-卫生纸', '3');
INSERT INTO `category` VALUES ('020302', '面巾纸', '用品-纸制品-面巾纸', '3');
INSERT INTO `category` VALUES ('020303', '尿裤尿片', '用品-纸制品-尿裤尿片', '3');
INSERT INTO `category` VALUES ('020304', '妇女卫生用品', '用品-纸制品-妇女卫生用品', '3');
INSERT INTO `category` VALUES ('020305', '一次性内裤', '用品-纸制品-一次性内裤', '3');
INSERT INTO `category` VALUES ('020306', '湿纸巾', '用品-纸制品-湿纸巾', '3');
INSERT INTO `category` VALUES ('0204', '洗衣用品', '用品-洗衣用品', '2');
INSERT INTO `category` VALUES ('020401', '一般洗衣用品', '用品-洗衣用品-一般洗衣用品', '3');
INSERT INTO `category` VALUES ('020402', '特殊洗衣用品', '用品-洗衣用品-特殊洗衣用品', '3');
INSERT INTO `category` VALUES ('0205', '清洁用品', '用品-清洁用品', '2');
INSERT INTO `category` VALUES ('020501', '厨房浴厕清洁用品', '用品-清洁用品-厨房浴厕清洁用品', '3');
INSERT INTO `category` VALUES ('020502', '皮革保养品', '用品-清洁用品-皮革保养品', '3');
INSERT INTO `category` VALUES ('020503', '杀虫用品', '用品-清洁用品-杀虫用品', '3');
INSERT INTO `category` VALUES ('020504', '家居清洁剂', '用品-清洁用品-家居清洁剂', '3');
INSERT INTO `category` VALUES ('020505', '除湿、芳香剂', '用品-清洁用品-除湿、芳香剂', '3');
INSERT INTO `category` VALUES ('0206', '急救用品及其他', '用品-急救用品及其他', '2');
INSERT INTO `category` VALUES ('020601', '急救用品', '用品-急救用品及其他-急救用品', '3');
INSERT INTO `category` VALUES ('020602', '计生用品', '用品-急救用品及其他-计生用品', '3');
INSERT INTO `category` VALUES ('0207', '宠物用品', '用品-宠物用品', '2');
INSERT INTO `category` VALUES ('020701', '宠物食品', '用品-宠物用品-宠物食品', '3');
INSERT INTO `category` VALUES ('020702', '宠物用品', '用品-宠物用品-宠物用品', '3');
INSERT INTO `category` VALUES ('0208', '杯罐', '用品-杯罐', '2');
INSERT INTO `category` VALUES ('020801', '杯', '用品-杯罐-杯', '3');
INSERT INTO `category` VALUES ('020802', '壶', '用品-杯罐-壶', '3');
INSERT INTO `category` VALUES ('0209', '一次性用品', '用品-一次性用品', '2');
INSERT INTO `category` VALUES ('020901', '一次性餐具', '用品-一次性用品-一次性餐具', '3');
INSERT INTO `category` VALUES ('020902', '桌布、手套', '用品-一次性用品-桌布、手套', '3');
INSERT INTO `category` VALUES ('020903', '纸杯', '用品-一次性用品-纸杯', '3');
INSERT INTO `category` VALUES ('020904', '牙签', '用品-一次性用品-牙签', '3');
INSERT INTO `category` VALUES ('020905', '棉签', '用品-一次性用品-棉签', '3');
INSERT INTO `category` VALUES ('020906', '鞋套', '用品-一次性用品-鞋套', '3');
INSERT INTO `category` VALUES ('020907', '保鲜膜、袋', '用品-一次性用品-保鲜膜、袋', '3');
INSERT INTO `category` VALUES ('0210', '厨房用品', '用品-厨房用品', '2');
INSERT INTO `category` VALUES ('021001', '餐具', '用品-厨房用品-餐具', '3');
INSERT INTO `category` VALUES ('021002', '炊具', '用品-厨房用品-炊具', '3');
INSERT INTO `category` VALUES ('021003', '刀具', '用品-厨房用品-刀具', '3');
INSERT INTO `category` VALUES ('021004', '保鲜容器', '用品-厨房用品-保鲜容器', '3');
INSERT INTO `category` VALUES ('0211', '家庭整理用品', '用品-家庭整理用品', '2');
INSERT INTO `category` VALUES ('021101', '收纳箱、袋', '用品-家庭整理用品-收纳箱、袋', '3');
INSERT INTO `category` VALUES ('0212', '清洁工具', '用品-清洁工具', '2');
INSERT INTO `category` VALUES ('021201', '垃圾桶、袋', '用品-清洁工具-垃圾桶、袋', '3');
INSERT INTO `category` VALUES ('021202', '桶、盆', '用品-清洁工具-桶、盆', '3');
INSERT INTO `category` VALUES ('021203', '清洁巾', '用品-清洁工具-清洁巾', '3');
INSERT INTO `category` VALUES ('021204', '清洁刷', '用品-清洁工具-清洁刷', '3');
INSERT INTO `category` VALUES ('021205', '扫除用具', '用品-清洁工具-扫除用具', '3');
INSERT INTO `category` VALUES ('021206', '晒衣用具', '用品-清洁工具-晒衣用具', '3');
INSERT INTO `category` VALUES ('021207', '洗衣用具', '用品-清洁工具-洗衣用具', '3');
INSERT INTO `category` VALUES ('021208', '地垫', '用品-清洁工具-地垫', '3');
INSERT INTO `category` VALUES ('0213', '卫浴保护', '用品-卫浴保护', '2');
INSERT INTO `category` VALUES ('021301', '浴室配件', '用品-卫浴保护-浴室配件', '3');
INSERT INTO `category` VALUES ('021302', '卫生附件', '用品-卫浴保护-卫生附件', '3');
INSERT INTO `category` VALUES ('0214', '日用家居用品', '用品-日用家居用品', '2');
INSERT INTO `category` VALUES ('021401', '桌椅凳', '用品-日用家居用品-桌椅凳', '3');
INSERT INTO `category` VALUES ('021402', '家居装饰用品', '用品-日用家居用品-家居装饰用品', '3');
INSERT INTO `category` VALUES ('021403', '活性碳', '用品-日用家居用品-活性碳', '3');
INSERT INTO `category` VALUES ('021404', '其他家居用品', '用品-日用家居用品-其他家居用品', '3');
INSERT INTO `category` VALUES ('021405', '伞', '用品-日用家居用品-伞', '3');
INSERT INTO `category` VALUES ('0215', '节庆及车用', '用品-节庆及车用', '2');
INSERT INTO `category` VALUES ('021501', '汽车配件', '用品-节庆及车用-汽车配件', '3');
INSERT INTO `category` VALUES ('021502', '蜡烛', '用品-节庆及车用-蜡烛', '3');
INSERT INTO `category` VALUES ('021503', '季节性装饰', '用品-节庆及车用-季节性装饰', '3');
INSERT INTO `category` VALUES ('0216', '文体', '用品-文体', '2');
INSERT INTO `category` VALUES ('021601', '学习用品', '用品-文体-学习用品', '3');
INSERT INTO `category` VALUES ('021602', '体育用品', '用品-文体-体育用品', '3');
INSERT INTO `category` VALUES ('021603', '玩具', '用品-文体-玩具', '3');
INSERT INTO `category` VALUES ('0217', '婴幼儿童用品', '用品-婴幼儿童用品', '2');
INSERT INTO `category` VALUES ('021701', '婴幼儿个洗', '用品-婴幼儿童用品-婴幼儿个洗', '3');
INSERT INTO `category` VALUES ('021702', '婴幼儿湿巾', '用品-婴幼儿童用品-婴幼儿湿巾', '3');
INSERT INTO `category` VALUES ('021703', '婴幼儿身体护理', '用品-婴幼儿童用品-婴幼儿身体护理', '3');
INSERT INTO `category` VALUES ('021704', '婴幼儿用品', '用品-婴幼儿童用品-婴幼儿用品', '3');
INSERT INTO `category` VALUES ('021705', '婴幼儿餐具', '用品-婴幼儿童用品-婴幼儿餐具', '3');
INSERT INTO `category` VALUES ('021706', '婴幼儿洗衣用品', '用品-婴幼儿童用品-婴幼儿洗衣用品', '3');
INSERT INTO `category` VALUES ('021707', '婴幼儿服饰', '用品-婴幼儿童用品-婴幼儿服饰', '3');
INSERT INTO `category` VALUES ('0218', '日用小件', '用品-日用小件', '2');
INSERT INTO `category` VALUES ('021801', '插线板', '用品-日用小件-插线板', '3');
INSERT INTO `category` VALUES ('021802', '电池及冲电器材', '用品-日用小件-电池及冲电器材', '3');
INSERT INTO `category` VALUES ('021803', '打火机', '用品-日用小件-打火机', '3');
INSERT INTO `category` VALUES ('0219', '日用针纺', '用品-日用针纺', '2');
INSERT INTO `category` VALUES ('021901', '毛巾', '用品-日用针纺-毛巾', '3');
INSERT INTO `category` VALUES ('021902', '袜子', '用品-日用针纺-袜子', '3');
INSERT INTO `category` VALUES ('021903', '拖鞋', '用品-日用针纺-拖鞋', '3');
INSERT INTO `category` VALUES ('021904', '床用', '用品-日用针纺-床用', '3');
INSERT INTO `category` VALUES ('021905', '垫类', '用品-日用针纺-垫类', '3');
INSERT INTO `category` VALUES ('021907', '内裤', '用品-日用针纺-内裤', '3');
