Vue.component('v-modal', {
    template: '#vModal',
    props: {
        okText: {
            type: String,
            default: '确 定'
        },
        cancelText: {
            type: String,
            default: '取 消'
        },
        title: {
            type: String,
            default: ''
        },
        show: {
            required: true,
            type: Boolean,
            twoWay: true
        },
        width: {
            default: null
        },
        callback: {
            type: Object,
            default: function () { return {} }
        },
        effect: {
            type: String,
            default: null
        },
        backdrop: {
            type: Boolean,
            default: true
        },
        large: {
            type: Boolean,
            default: false
        },
        small: {
            type: Boolean,
            default: false
        }
    },
    computed: {
        optionalWidth: function () {
            if (this.width === null) {
                return null;
            } else if (Number.isInteger(this.width)) {
                return this.width + 'px';
            }
            return this.width;
        }
    },
    watch: {
        show: function (val) {
            var el = this.$el;
            var body = document.body;
            var scrollBarWidth = this.getScrollBarWidth();
            if (val) {
                $(el).find('.modal-content').focus();
                el.style.display = 'block';
                setTimeout(function () { $(el).addClass('in') }, 0);
                $(body).addClass('modal-open')
                if (scrollBarWidth !== 0) {
                    body.style.paddingRight = scrollBarWidth + 'px';
                }
                if (this.backdrop) {
                    $(el).on('click', function (e) {
                        if (e.target === el) this.show = false;
                    })
                }
            } else {
                body.style.paddingRight = null;
                $(body).removeClass('modal-open');
                $(el).removeClass('in').on('transitionend', function () {
                    $(el).off('click transitionend');
                    el.style.display = 'none';
                });
            }

        },
        methods: {
            close: function () {
                this.show = false;
            },
            getScrollBarWidth: function () {
                if (document.documentElement.scrollHeight <= document.documentElement.clientHeight) {
                    return 0;
                }
                var inner = document.createElement('p');
                inner.style.width = '100%';
                inner.style.height = '200px';

                var outer = document.createElement('div');
                outer.style.position = 'absolute';
                outer.style.top = '0px';
                outer.style.left = '0px';
                outer.style.visibility = 'hidden';
                outer.style.width = '200px';
                outer.style.height = '150px';
                outer.style.overflow = 'hidden';
                outer.appendChild(inner);

                document.body.appendChild(outer);
                var w1 = inner.offsetWidth;
                outer.style.overflow = 'scroll';
                var w2 = inner.offsetWidth;
                if (w1 === w2) w2 = outer.clientWidth;
                document.body.removeChild(outer);
                return (w1 - w2);
            }
        }
    }
})