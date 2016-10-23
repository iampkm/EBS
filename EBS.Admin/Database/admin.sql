INSERT INTO `account` VALUES ('1', 'admin', '21232f297a57a5a743894a0e4a801fc3', 'admin', '1', '2016-10-22 08:49:26', '1', '0', '2016-10-22 08:49:35');

INSERT INTO `role` VALUES ('1', '系统管理员', '超管');

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
