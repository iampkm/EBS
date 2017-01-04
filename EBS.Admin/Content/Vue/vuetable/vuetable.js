Vue.component('vue-table', {
    template: '#grid-template',
    props: {
        data: { type: Array, default: [] },
        columns: Array,
        url: { type: String, default: "" },
        args: Object,
        showSearch: { type: Boolean, default: true },
        showCheckbox: { type: Boolean, default: true },
        showPagination: { type: Boolean, default: true },
        showToolbar: { type: Boolean, default: true },
        pageSizes: { type: Array, default: [20, 50, 100, 200, 500] },
        id: { type: String, default: "Id" },
        height: '500px'       
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
            pageSize: this.pageSizes[4],
            pageNumber: 10,
            pages: 0,
            total: 0, //total records
            showLoading: false,
            columnWidth: '150',
            indexWidth: '40'
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
                        style: ''                        
                    }
                } else {
                    obj = {
                        name: column.name,
                        localName: column.localName,
                        visible: (column.visible === undefined) ? true : column.visible,
                        width: (column.width === undefined) ? self.columnWidth + 'px' : column.width + 'px',
                        style: (column.style === undefined) ? '' : column.style,
                    }
                }
                self.columns.$set(i, obj)
            })
        },
        setColumnStyle: function (columnStyle, column, key) {
            return columnStyle == "" ? column : columnStyle.replace('{column}', column).replace("{id}", key);
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
            var _total = this.$data.total;
            var _maxPage = parseInt(_total / this.pageSize);
            this.$data.pages = _total % this.pageSize == 0 ? _maxPage : _maxPage + 1;
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
            $('#btnToExcel').bootstrapExcelExport({ tableSelector: '#tableData' });
        },
        selectRow: function (index) {
            var item = this.data[index];
            item.checked = !item.checked;;
            this.data.$set(index, item);
        }
    },
    computed: {
        maxPageNumber: function () {
            var number = this.pageNumber;
            var maxPage = this.pages;
            var maxNumber = maxPage < number ? maxPage : number;
            return maxNumber;
        }
    },
    watch: {
        args: {
            handler: function () {
                this.loadData();
            },
            deep: true
        }
    },
    created: function () {
        //实例初始化时调用
        this.initColumns();
        if (this.url == "") {
            this.$data.total = this.data.length;
            this.setPages();
        }
        else {
            this.loadData();
        }
    }
})