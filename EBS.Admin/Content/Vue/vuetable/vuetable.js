Vue.component('vue-table', {
    template: '#grid-template',
    props: {
        data: { type: Array, default: [] },
        columns: Array,
        url: { type: String, default: "" },
        args: Object,
        showSearch: { type: Boolean, default: true },
        showCheckbox: { type: Boolean, default: true },
        showallcheckbox: { type: Boolean, default: true },
        showPagination: { type: Boolean, default: true },
        showToolbar: { type: Boolean, default: true },
        pageSizes: { type: Array, default: [20, 50, 100, 200, 500] },
        id: { type: String, default: "Id" },
        height: '500px',
        showSum: { type: Boolean, default: false },
        sum: { type: Array, default: [] },
        singleselectionmodel: { type: Boolean, default: false },
        buttons: { type: Array, default: [] },
        showtoexcel: { type: Boolean, default: false },
        showtoexcel: { type: Boolean, default: false },
        firstLoadShowData: { type: Boolean, default: true }, // first-Load-Show-Data
        autoQuery: { type: Boolean, default: true }, // 查询args 条件变化，自动查询数据
        rowClassName: { type: Function, default: function (row, index) { } }
    },
    data: function () {
        var sortOrders = {};
        this.columns.forEach(function (key) {
            sortOrders[key.name] = 1
        });
        return {
            sortKey: '',
            sortOrders: sortOrders,
            filterKey: '',
            pageIndex: 1,
            pageSize: this.pageSizes[1],
            pageNumber: 10,
            pages: 0,
            total: 0, //total records
            maxPageNumber: 0,
            showLoading: false,
            columnWidth: '150',
            indexWidth: '40',
        }
    },
    methods: {
        sortBy: function (key) {
            this.sortKey = key
            this.sortOrders[key] = this.sortOrders[key] * -1
        },
        initColumns: function () {
            var self = this;
            var obj;
            this.columns.forEach(function (column, i) {
                if (typeof (column) === 'string') {
                    obj = {
                        name: column,
                        localName: column,
                        visible: true,
                        width: self.columnWidth + 'px',
                        style: '',
                        sum: '',
                    }
                } else {
                    obj = {
                        name: column.name,
                        localName: column.localName,
                        visible: (column.visible === undefined) ? true : column.visible,
                        width: (column.width === undefined) ? self.columnWidth + 'px' : column.width + 'px',
                        style: (column.style === undefined) ? '' : column.style,
                        sum: (column.sum === undefined) ? '' : column.sum,
                    }
                }
                self.columns.$set(i, obj)
            })
        },
        setColumnStyle: function (columnStyle, column, key, item) {
            if (columnStyle == "") {
                return column;
            }
            columnStyle = columnStyle.replace('{column}', column).replace("{id}", key);
            // 替换 自定义列
            //for (var i = 0; i < data.length; i++) {
            //    var item = data[i];
            //    for (var col in item) {
            //        columnStyle = columnStyle.replace("{" + col + "}", item[col]);
            //    }
            //}
            for (var col in item) {
                columnStyle = columnStyle.replace("{" + col + "}", item[col]);
            }
            return columnStyle;
        },
        changePageSize: function (page) {
            this.pageSize = page;
            this.loadData();
        },
        loadData: function () {
            var _url = this.url;
            if (_url == undefined || _url == "") { return; }
            var _pageArgs = {
                pageIndex: this.pageIndex,
                pageSize: this.pageSize,
                isPaging: this.showPagination
            };
            // var _args = $.extend({}, _pageArgs, this.args);
            var _args = Object.assign({}, _pageArgs, this.args);
            var self = this;
            $.ajax({
                url: _url, type: "get", cache: false, data: _args, dataType: "json",
                success: function (result) {
                    if (result.success) {
                        self.data = result.data;
                        self.data.forEach(function (item, index) {
                            item.checked = false;
                            self.data.$set(index, item);
                        });
                        self.indexWidth = self.data.length < 99 ? 38 : 52;  //设置索引列宽度
                        self.total = result.total;
                        self.setPages();
                        // 设置列合计值
                        if (result.sum != undefined && self.showSum) {
                            result.sum.forEach(function (line) {
                                self.columns.forEach(function (column, index) {
                                    if (index == 0) {
                                        column.sum = "合计："
                                    }
                                    if (line.Column == column.name) {
                                        column.sum = line.Value;
                                        return;
                                    }
                                })
                            })
                        }
                    }
                },
                beforeSend: function () {
                    self.showLoading = true;
                },
                complete: function () {
                    self.showLoading = false;
                }
            });
        },
        showPage: function () {
            this.showPagination = this.showPagination ? false : true;
            this.loadData();
        },
        goFirst: function () {
            this.pageIndex = 1;
            this.loadData();
        },
        goNext: function () {
            if (this.pageIndex < this.pages) {
                this.pageIndex++
                this.loadData()
            }
        },
        goPrevious: function () {
            if (this.pageIndex > 1) {
                this.pageIndex--;
                this.loadData();
            }
        },
        goLast: function () {
            this.pageIndex = this.pages;
            this.loadData();
        },
        gotoPage: function (page) {
            if (page != this.pageIndex && (page > 0 && page <= this.pages)) {
                this.pageIndex = page
                this.loadData();
            }
        },
        setPages: function () {
            // var _total = this.$data.total;
            var _total = this.total;
            var _maxPage = parseInt(_total / this.pageSize);
            this.pages = _total % this.pageSize == 0 ? _maxPage : _maxPage + 1;
            var number = this.pageNumber;
            var maxPage = this.pages;
            this.maxPageNumber = maxPage < number ? maxPage : number;
        },
        checkAll: function (isChecked) {
            var self = this;
            this.data.forEach(function (item, index) {
                item.checked = isChecked;
                self.data.$set(index, item);
            });
        },
        getPageNumber: function (page) {
            var times = parseInt((this.pageIndex - 1) / this.pageNumber);
            return page + times * this.pageNumber + 1;
        },
        getNextPitchNumber: function () {
            var times = parseInt((this.pageIndex - 1) / this.pageNumber);
            return (times + 1) * this.pageNumber + 1;
        },
        toExcel: function () {
            // $('#btnToExcel').bootstrapExcelExport({ tableSelector: '#tableData' });
            //附加参数
            var href = this.url + "?toExcel=true";
            var parameters = this.args;
            for (var name in parameters) {
                href += "&" + name + "=" + encodeURIComponent(parameters[name]);
            }

            window.location.href = href;
        },
        selectRow: function (index) {
            var self = this;
            var item = this.data[index];
            item.checked = !item.checked;
            this.data.$set(index, item);
            if (this.singleselectionmodel && item.checked) {
                this.data.forEach(function (item, index1) {
                    if (index1 != index && item.checked) {
                        item.checked = false;
                        self.data.$set(index1, item);
                    }
                });
            }
            //触发行点击事件
            this.$emit('row-click', item);
        },
        clickButton: function (row, btn) {
            this.$emit('btn-click', row, btn);
        }
    },
    watch: {
        args: {
            handler: function () {
                this.pageIndex = 1;
                if (this.autoQuery) {
                    this.loadData();
                }
            },
            deep: true
        }
    },
    created: function () {
        //实例初始化时调用
        this.initColumns();
        if (this.firstLoadShowData) {
            this.loadData();
        }
    }
})