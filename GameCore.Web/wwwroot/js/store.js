/**
 * GameCore 官方商城 JavaScript 功能
 * 包含購物車管理、商品互動、表單驗證等功能
 */

// 全域變數
let cart = JSON.parse(localStorage.getItem('gamecore_cart')) || [];
let wishlist = JSON.parse(localStorage.getItem('gamecore_wishlist')) || [];

// 購物車管理
const CartManager = {
    /**
     * 添加商品到購物車
     * @param {Object} product - 商品資訊
     * @param {number} quantity - 數量
     */
    addToCart: function (product, quantity = 1) {
        const existingItem = cart.find(item => item.id === product.id);

        if (existingItem) {
            existingItem.quantity += quantity;
        } else {
            cart.push({
                id: product.id,
                name: product.name,
                price: product.price,
                imageUrl: product.imageUrl,
                quantity: quantity
            });
        }

        this.saveCart();
        this.updateCartCount();
        this.showToast('商品已加入購物車', 'success');
    },

    /**
     * 從購物車移除商品
     * @param {number} productId - 商品ID
     */
    removeFromCart: function (productId) {
        cart = cart.filter(item => item.id !== productId);
        this.saveCart();
        this.updateCartCount();
        this.showToast('商品已從購物車移除', 'info');
    },

    /**
     * 更新購物車商品數量
     * @param {number} productId - 商品ID
     * @param {number} quantity - 新數量
     */
    updateQuantity: function (productId, quantity) {
        const item = cart.find(item => item.id === productId);
        if (item) {
            if (quantity <= 0) {
                this.removeFromCart(productId);
            } else {
                item.quantity = quantity;
                this.saveCart();
                this.updateCartCount();
            }
        }
    },

    /**
     * 清空購物車
     */
    clearCart: function () {
        cart = [];
        this.saveCart();
        this.updateCartCount();
        this.showToast('購物車已清空', 'info');
    },

    /**
     * 取得購物車總計
     * @returns {number} 總計金額
     */
    getTotal: function () {
        return cart.reduce((total, item) => total + (item.price * item.quantity), 0);
    },

    /**
     * 取得購物車商品總數
     * @returns {number} 商品總數
     */
    getItemCount: function () {
        return cart.reduce((total, item) => total + item.quantity, 0);
    },

    /**
     * 儲存購物車到本地儲存
     */
    saveCart: function () {
        localStorage.setItem('gamecore_cart', JSON.stringify(cart));
    },

    /**
     * 更新購物車數量顯示
     */
    updateCartCount: function () {
        const cartCount = document.getElementById('cartCount');
        if (cartCount) {
            cartCount.textContent = this.getItemCount();
        }
    }
};

// 願望清單管理
const WishlistManager = {
    /**
     * 添加商品到願望清單
     * @param {Object} product - 商品資訊
     */
    addToWishlist: function (product) {
        if (!wishlist.find(item => item.id === product.id)) {
            wishlist.push({
                id: product.id,
                name: product.name,
                price: product.price,
                imageUrl: product.imageUrl
            });
            this.saveWishlist();
            this.showToast('商品已加入願望清單', 'success');
        } else {
            this.showToast('商品已在願望清單中', 'info');
        }
    },

    /**
     * 從願望清單移除商品
     * @param {number} productId - 商品ID
     */
    removeFromWishlist: function (productId) {
        wishlist = wishlist.filter(item => item.id !== productId);
        this.saveWishlist();
        this.showToast('商品已從願望清單移除', 'info');
    },

    /**
     * 檢查商品是否在願望清單中
     * @param {number} productId - 商品ID
     * @returns {boolean} 是否在願望清單中
     */
    isInWishlist: function (productId) {
        return wishlist.some(item => item.id === productId);
    },

    /**
     * 儲存願望清單到本地儲存
     */
    saveWishlist: function () {
        localStorage.setItem('gamecore_wishlist', JSON.stringify(wishlist));
    }
};

// 表單驗證
const FormValidator = {
    /**
     * 驗證必填欄位
     * @param {string} value - 欄位值
     * @param {string} fieldName - 欄位名稱
     * @returns {boolean} 是否有效
     */
    required: function (value, fieldName) {
        if (!value || value.trim() === '') {
            this.showError(`${fieldName} 為必填欄位`);
            return false;
        }
        return true;
    },

    /**
     * 驗證電子郵件格式
     * @param {string} email - 電子郵件
     * @returns {boolean} 是否有效
     */
    email: function (email) {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        if (!emailRegex.test(email)) {
            this.showError('請輸入有效的電子郵件地址');
            return false;
        }
        return true;
    },

    /**
     * 驗證電話號碼格式
     * @param {string} phone - 電話號碼
     * @returns {boolean} 是否有效
     */
    phone: function (phone) {
        const phoneRegex = /^09\d{8}$/;
        if (!phoneRegex.test(phone)) {
            this.showError('請輸入有效的台灣手機號碼');
            return false;
        }
        return true;
    },

    /**
     * 顯示錯誤訊息
     * @param {string} message - 錯誤訊息
     */
    showError: function (message) {
        this.showToast(message, 'error');
    },

    /**
     * 顯示 Toast 訊息
     * @param {string} message - 訊息內容
     * @param {string} type - 訊息類型
     */
    showToast: function (message, type) {
        const toast = $(`
            <div class="toast align-items-center text-white bg-${type === 'error' ? 'danger' : type} border-0" role="alert">
                <div class="d-flex">
                    <div class="toast-body">${message}</div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
                </div>
            </div>
        `);

        $('.toast-container').append(toast);
        const bsToast = new bootstrap.Toast(toast[0]);
        bsToast.show();

        // 自動移除
        setTimeout(() => {
            toast.remove();
        }, 3000);
    }
};

// 商品互動功能
const ProductInteractions = {
    /**
     * 初始化商品頁面互動
     */
    init: function () {
        this.bindAddToCart();
        this.bindWishlistToggle();
        this.bindQuantityControls();
        this.bindImageGallery();
    },

    /**
     * 綁定加入購物車按鈕
     */
    bindAddToCart: function () {
        $(document).on('click', '.btn-add-to-cart', function (e) {
            e.preventDefault();

            const button = $(this);
            const productCard = button.closest('.product-card');
            const productId = parseInt(button.data('product-id'));
            const productName = button.data('product-name');
            const productPrice = parseFloat(button.data('product-price'));
            const productImage = button.data('product-image');

            // 取得數量
            let quantity = 1;
            const quantityInput = productCard.find('.quantity-input');
            if (quantityInput.length) {
                quantity = parseInt(quantityInput.val()) || 1;
            }

            // 加入購物車
            CartManager.addToCart({
                id: productId,
                name: productName,
                price: productPrice,
                imageUrl: productImage
            }, quantity);

            // 按鈕動畫效果
            button.addClass('btn-success').text('已加入購物車');
            setTimeout(() => {
                button.removeClass('btn-success').text('加入購物車');
            }, 2000);
        });
    },

    /**
     * 綁定願望清單切換
     */
    bindWishlistToggle: function () {
        $(document).on('click', '.btn-wishlist', function (e) {
            e.preventDefault();

            const button = $(this);
            const productId = parseInt(button.data('product-id'));
            const productName = button.data('product-name');
            const productPrice = parseFloat(button.data('product-price'));
            const productImage = button.data('product-image');

            if (WishlistManager.isInWishlist(productId)) {
                WishlistManager.removeFromWishlist(productId);
                button.removeClass('btn-danger').addClass('btn-outline-secondary');
                button.html('<i class="fas fa-heart"></i>');
            } else {
                WishlistManager.addToWishlist(productId);
                button.removeClass('btn-outline-secondary').addClass('btn-danger');
                button.html('<i class="fas fa-heart"></i>');
            }
        });
    },

    /**
     * 綁定數量控制
     */
    bindQuantityControls: function () {
        $(document).on('click', '.quantity-btn', function () {
            const button = $(this);
            const input = button.siblings('.quantity-input');
            let value = parseInt(input.val()) || 1;

            if (button.hasClass('quantity-minus')) {
                value = Math.max(1, value - 1);
            } else if (button.hasClass('quantity-plus')) {
                value = Math.min(99, value + 1);
            }

            input.val(value);
        });

        $(document).on('change', '.quantity-input', function () {
            let value = parseInt($(this).val()) || 1;
            value = Math.max(1, Math.min(99, value));
            $(this).val(value);
        });
    },

    /**
     * 綁定圖片畫廊
     */
    bindImageGallery: function () {
        $(document).on('click', '.product-thumbnail', function () {
            const mainImage = $(this).closest('.product-gallery').find('.product-main-image');
            const thumbnailSrc = $(this).attr('src');

            mainImage.attr('src', thumbnailSrc);
            $(this).addClass('active').siblings().removeClass('active');
        });
    }
};

// 購物車頁面功能
const CartPage = {
    /**
     * 初始化購物車頁面
     */
    init: function () {
        this.renderCart();
        this.bindEvents();
    },

    /**
     * 渲染購物車內容
     */
    renderCart: function () {
        const cartContainer = $('#cartItems');
        if (!cartContainer.length) return;

        if (cart.length === 0) {
            cartContainer.html(`
                <div class="empty-state">
                    <i class="fas fa-shopping-cart"></i>
                    <h3>購物車是空的</h3>
                    <p>快來選購您喜歡的商品吧！</p>
                    <a href="/StoreMvc" class="btn btn-primary">開始購物</a>
                </div>
            `);
            return;
        }

        let html = '';
        cart.forEach(item => {
            html += `
                <div class="cart-item" data-product-id="${item.id}">
                    <div class="row align-items-center">
                        <div class="col-md-2">
                            <img src="${item.imageUrl || '/images/placeholder-product.jpg'}" 
                                 alt="${item.name}" class="cart-item-image">
                        </div>
                        <div class="col-md-4">
                            <h6 class="mb-1">${item.name}</h6>
                            <p class="text-muted mb-0">單價：NT$ ${item.price.toLocaleString()}</p>
                        </div>
                        <div class="col-md-2">
                            <div class="quantity-control">
                                <button class="quantity-btn quantity-minus">-</button>
                                <input type="number" class="quantity-input" value="${item.quantity}" min="1" max="99">
                                <button class="quantity-btn quantity-plus">+</button>
                            </div>
                        </div>
                        <div class="col-md-2 text-center">
                            <strong>NT$ ${(item.price * item.quantity).toLocaleString()}</strong>
                        </div>
                        <div class="col-md-2 text-end">
                            <button class="btn btn-outline-danger btn-sm remove-item">
                                <i class="fas fa-trash"></i>
                            </button>
                        </div>
                    </div>
                </div>
            `;
        });

        cartContainer.html(html);
        this.updateSummary();
    },

    /**
     * 更新購物車摘要
     */
    updateSummary: function () {
        const subtotal = CartManager.getTotal();
        const shipping = subtotal > 1000 ? 0 : 100;
        const total = subtotal + shipping;

        $('#cartSubtotal').text(`NT$ ${subtotal.toLocaleString()}`);
        $('#cartShipping').text(`NT$ ${shipping.toLocaleString()}`);
        $('#cartTotal').text(`NT$ ${total.toLocaleString()}`);
    },

    /**
     * 綁定事件
     */
    bindEvents: function () {
        // 移除商品
        $(document).on('click', '.remove-item', function () {
            const cartItem = $(this).closest('.cart-item');
            const productId = parseInt(cartItem.data('product-id'));

            CartManager.removeFromCart(productId);
            CartPage.renderCart();
        });

        // 更新數量
        $(document).on('change', '.quantity-input', function () {
            const cartItem = $(this).closest('.cart-item');
            const productId = parseInt(cartItem.data('product-id'));
            const quantity = parseInt($(this).val());

            CartManager.updateQuantity(productId, quantity);
            CartPage.renderCart();
        });

        // 清空購物車
        $(document).on('click', '#clearCart', function () {
            if (confirm('確定要清空購物車嗎？')) {
                CartManager.clearCart();
                CartPage.renderCart();
            }
        });
    }
};

// 結帳頁面功能
const CheckoutPage = {
    /**
     * 初始化結帳頁面
     */
    init: function () {
        this.bindFormValidation();
        this.bindPaymentMethodToggle();
        this.bindPlaceOrder();
    },

    /**
     * 綁定表單驗證
     */
    bindFormValidation: function () {
        $('#checkoutForm').on('submit', function (e) {
            e.preventDefault();

            if (CheckoutPage.validateForm()) {
                CheckoutPage.submitOrder();
            }
        });
    },

    /**
     * 驗證表單
     * @returns {boolean} 是否有效
     */
    validateForm: function () {
        const required = ['fullName', 'email', 'phone', 'address'];
        let isValid = true;

        required.forEach(field => {
            const value = $(`#${field}`).val();
            if (!FormValidator.required(value, $(`#${field}`).attr('placeholder'))) {
                isValid = false;
            }
        });

        // 驗證電子郵件
        const email = $('#email').val();
        if (email && !FormValidator.email(email)) {
            isValid = false;
        }

        // 驗證電話
        const phone = $('#phone').val();
        if (phone && !FormValidator.phone(phone)) {
            isValid = false;
        }

        return isValid;
    },

    /**
     * 綁定付款方式切換
     */
    bindPaymentMethodToggle: function () {
        $('input[name="paymentMethod"]').on('change', function () {
            const method = $(this).val();
            $('.payment-details').hide();
            $(`#${method}Details`).show();
        });
    },

    /**
     * 提交訂單
     */
    submitOrder: function () {
        const submitBtn = $('#placeOrderBtn');
        const originalText = submitBtn.text();

        submitBtn.prop('disabled', true).html('<span class="loading"></span> 處理中...');

        // 模擬 API 呼叫
        setTimeout(() => {
            submitBtn.prop('disabled', false).text(originalText);

            // 清空購物車
            CartManager.clearCart();

            // 跳轉到訂單確認頁面
            window.location.href = '/StoreMvc/OrderConfirmation?orderId=12345';
        }, 2000);
    }
};

// 搜尋功能
const SearchManager = {
    /**
     * 初始化搜尋功能
     */
    init: function () {
        this.bindSearchForm();
        this.bindSearchSuggestions();
    },

    /**
     * 綁定搜尋表單
     */
    bindSearchForm: function () {
        $('#searchForm').on('submit', function (e) {
            e.preventDefault();

            const query = $('#searchInput').val().trim();
            if (query) {
                SearchManager.performSearch(query);
            }
        });
    },

    /**
     * 執行搜尋
     * @param {string} query - 搜尋關鍵字
     */
    performSearch: function (query) {
        // 這裡可以實作 AJAX 搜尋
        console.log('搜尋:', query);

        // 模擬搜尋結果
        this.showSearchResults(query, []);
    },

    /**
     * 顯示搜尋結果
     * @param {string} query - 搜尋關鍵字
     * @param {Array} results - 搜尋結果
     */
    showSearchResults: function (query, results) {
        const container = $('#searchResults');
        if (!container.length) return;

        if (results.length === 0) {
            container.html(`
                <div class="empty-state">
                    <i class="fas fa-search"></i>
                    <h3>找不到相關商品</h3>
                    <p>試試其他關鍵字或瀏覽所有商品</p>
                </div>
            `);
        } else {
            // 渲染搜尋結果
            // 這裡可以實作結果渲染邏輯
        }
    },

    /**
     * 綁定搜尋建議
     */
    bindSearchSuggestions: function () {
        const searchInput = $('#searchInput');
        if (!searchInput.length) return;

        // 實作搜尋建議功能
        // 這裡可以添加自動完成或搜尋建議
    }
};

// 工具函數
const Utils = {
    /**
     * 格式化價格
     * @param {number} price - 價格
     * @returns {string} 格式化後的價格
     */
    formatPrice: function (price) {
        return `NT$ ${price.toLocaleString()}`;
    },

    /**
     * 顯示 Toast 訊息
     * @param {string} message - 訊息內容
     * @param {string} type - 訊息類型
     */
    showToast: function (message, type = 'info') {
        const toast = $(`
            <div class="toast align-items-center text-white bg-${type} border-0" role="alert">
                <div class="d-flex">
                    <div class="toast-body">${message}</div>
                    <button type="button" class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button>
                </div>
            </div>
        `);

        $('.toast-container').append(toast);
        const bsToast = new bootstrap.Toast(toast[0]);
        bsToast.show();

        // 自動移除
        setTimeout(() => {
            toast.remove();
        }, 3000);
    },

    /**
     * 防抖函數
     * @param {Function} func - 要執行的函數
     * @param {number} wait - 等待時間
     * @returns {Function} 防抖後的函數
     */
    debounce: function (func, wait) {
        let timeout;
        return function executedFunction(...args) {
            const later = () => {
                clearTimeout(timeout);
                func(...args);
            };
            clearTimeout(timeout);
            timeout = setTimeout(later, wait);
        };
    }
};

// 頁面載入完成後初始化
$(document).ready(function () {
    // 初始化購物車數量
    CartManager.updateCartCount();

    // 根據頁面類型初始化不同功能
    const path = window.location.pathname;

    if (path.includes('/Cart')) {
        CartPage.init();
    } else if (path.includes('/Checkout')) {
        CheckoutPage.init();
    } else if (path.includes('/Search')) {
        SearchManager.init();
    }

    // 初始化商品互動
    ProductInteractions.init();

    // 全域錯誤處理
    $(document).ajaxError(function (event, xhr, settings, error) {
        console.error('AJAX 錯誤:', error);
        Utils.showToast('發生錯誤，請稍後再試', 'error');
    });
});

// 全域函數，供 HTML 直接呼叫
window.addToCart = function (productId, productName, productPrice, productImage) {
    CartManager.addToCart({
        id: productId,
        name: productName,
        price: productPrice,
        imageUrl: productImage
    });
};

window.toggleWishlist = function (productId, productName, productPrice, productImage) {
    if (WishlistManager.isInWishlist(productId)) {
        WishlistManager.removeFromWishlist(productId);
    } else {
        WishlistManager.addToWishlist(productId);
    }
}; 