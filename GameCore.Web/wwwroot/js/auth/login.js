/**
 * 登入頁面 JavaScript
 * 處理登入表單驗證、OAuth 登入、密碼顯示切換等功能
 */

// DOM 元素快取
const elements = {
    form: document.getElementById('loginForm'),
    email: document.getElementById('email'),
    password: document.getElementById('password'),
    passwordToggle: document.getElementById('passwordToggle'),
    eyeIcon: document.getElementById('eyeIcon'),
    rememberMe: document.getElementById('rememberMe'),
    loginBtn: document.getElementById('loginBtn'),
    loginSpinner: document.getElementById('loginSpinner'),
    googleLogin: document.getElementById('googleLogin'),
    facebookLogin: document.getElementById('facebookLogin'),
    discordLogin: document.getElementById('discordLogin')
};

// 表單驗證規則
const validation = {
    email: {
        required: true,
        message: '請輸入電子郵件或帳號'
    },
    password: {
        required: true,
        minLength: 6,
        message: '請輸入密碼'
    }
};

// 登入嘗試限制
let loginAttempts = 0;
const maxAttempts = 5;
const lockoutTime = 15 * 60 * 1000; // 15分鐘

/**
 * 初始化登入頁面
 */
document.addEventListener('DOMContentLoaded', function() {
    initializeEventListeners();
    checkLoginStatus();
    loadRememberedEmail();
    checkLockoutStatus();
});

/**
 * 初始化事件監聽器
 */
function initializeEventListeners() {
    // 表單提交事件
    if (elements.form) {
        elements.form.addEventListener('submit', handleFormSubmit);
    }

    // 密碼顯示切換
    if (elements.passwordToggle) {
        elements.passwordToggle.addEventListener('click', togglePasswordVisibility);
    }

    // 即時驗證
    if (elements.email) {
        elements.email.addEventListener('blur', () => validateField('email'));
        elements.email.addEventListener('input', () => hideError('emailError'));
    }

    if (elements.password) {
        elements.password.addEventListener('blur', () => validateField('password'));
        elements.password.addEventListener('input', () => hideError('passwordError'));
    }

    // OAuth 登入按鈕
    if (elements.googleLogin) {
        elements.googleLogin.addEventListener('click', () => handleOAuthLogin('google'));
    }

    if (elements.facebookLogin) {
        elements.facebookLogin.addEventListener('click', () => handleOAuthLogin('facebook'));
    }

    if (elements.discordLogin) {
        elements.discordLogin.addEventListener('click', () => handleOAuthLogin('discord'));
    }

    // 鍵盤事件
    document.addEventListener('keydown', handleKeyboardEvents);
}

/**
 * 處理表單提交
 * @param {Event} event - 表單提交事件
 */
async function handleFormSubmit(event) {
    event.preventDefault();
    
    // 檢查是否被鎖定
    if (isLockedOut()) {
        showToast('登入嘗試過多，請稍後再試', 'error');
        return;
    }

    // 驗證表單
    if (!validateForm()) {
        return;
    }

    // 設定載入狀態
    setLoading(elements.loginBtn, true);

    try {
        const formData = {
            email: elements.email.value.trim(),
            password: elements.password.value,
            rememberMe: elements.rememberMe?.checked || false
        };

        // 發送登入請求
        const response = await apiRequest('/api/user/login', {
            method: 'POST',
            body: JSON.stringify(formData)
        });

        // 登入成功
        loginAttempts = 0; // 重置嘗試次數
        localStorage.removeItem('lockoutTime');
        
        // 記住郵箱
        if (formData.rememberMe) {
            localStorage.setItem('rememberedEmail', formData.email);
        } else {
            localStorage.removeItem('rememberedEmail');
        }

        showToast('登入成功！正在跳轉...', 'success');
        
        // 延遲跳轉以顯示成功訊息
        setTimeout(() => {
            const redirectUrl = new URLSearchParams(window.location.search).get('returnUrl') || '/';
            window.location.href = redirectUrl;
        }, 1500);

    } catch (error) {
        // 登入失敗
        loginAttempts++;
        
        // 檢查是否需要鎖定
        if (loginAttempts >= maxAttempts) {
            localStorage.setItem('lockoutTime', Date.now().toString());
            showToast(`登入失敗次數過多，帳戶已被鎖定 ${lockoutTime / 60000} 分鐘`, 'error');
        } else {
            const remainingAttempts = maxAttempts - loginAttempts;
            showToast(`${error.message}（剩餘嘗試次數：${remainingAttempts}）`, 'error');
        }
    } finally {
        setLoading(elements.loginBtn, false);
    }
}

/**
 * 驗證整個表單
 * @returns {boolean} 表單是否有效
 */
function validateForm() {
    let isValid = true;

    // 驗證各個欄位
    Object.keys(validation).forEach(field => {
        if (!validateField(field)) {
            isValid = false;
        }
    });

    return isValid;
}

/**
 * 驗證單個欄位
 * @param {string} fieldName - 欄位名稱
 * @returns {boolean} 欄位是否有效
 */
function validateField(fieldName) {
    const element = elements[fieldName];
    const rules = validation[fieldName];
    const value = element?.value?.trim() || '';

    // 清除之前的錯誤
    hideError(`${fieldName}Error`);

    // 必填驗證
    if (rules.required && !value) {
        showError(`${fieldName}Error`, rules.message);
        return false;
    }

    // 長度驗證
    if (rules.minLength && value.length < rules.minLength) {
        showError(`${fieldName}Error`, `密碼長度至少需要 ${rules.minLength} 個字符`);
        return false;
    }

    // 電子郵件格式驗證（如果看起來像郵箱）
    if (fieldName === 'email' && value.includes('@') && !validateEmail(value)) {
        showError(`${fieldName}Error`, '請輸入有效的電子郵件格式');
        return false;
    }

    return true;
}

/**
 * 切換密碼顯示/隱藏
 */
function togglePasswordVisibility() {
    const isPassword = elements.password.type === 'password';
    
    elements.password.type = isPassword ? 'text' : 'password';
    elements.eyeIcon.textContent = isPassword ? '🙈' : '👁️';
    
    // 無障礙支援
    elements.passwordToggle.setAttribute('aria-label', 
        isPassword ? '隱藏密碼' : '顯示密碼');
}

/**
 * 處理 OAuth 登入
 * @param {string} provider - OAuth 提供者 (google, facebook, discord)
 */
async function handleOAuthLogin(provider) {
    try {
        // 防止重複點擊
        const button = elements[`${provider}Login`];
        if (button.disabled) return;
        
        button.disabled = true;
        
        // 記錄 OAuth 登入嘗試
        console.log(`開始 ${provider} OAuth 登入流程`);
        
        // 構建 OAuth URL
        const returnUrl = new URLSearchParams(window.location.search).get('returnUrl') || '/';
        const oauthUrl = `/api/auth/oauth/${provider}?returnUrl=${encodeURIComponent(returnUrl)}`;
        
        // 跳轉到 OAuth 端點
        window.location.href = oauthUrl;
        
    } catch (error) {
        console.error(`${provider} OAuth 登入失敗:`, error);
        showToast(`${provider} 登入失敗，請稍後再試`, 'error');
        
        // 重新啟用按鈕
        setTimeout(() => {
            elements[`${provider}Login`].disabled = false;
        }, 2000);
    }
}

/**
 * 檢查當前登入狀態
 */
async function checkLoginStatus() {
    try {
        const response = await apiRequest('/api/user/me');
        
        // 如果已經登入，跳轉到首頁
        if (response.success) {
            const returnUrl = new URLSearchParams(window.location.search).get('returnUrl') || '/';
            window.location.href = returnUrl;
        }
    } catch (error) {
        // 未登入，繼續顯示登入頁面
        console.log('用戶未登入');
    }
}

/**
 * 載入記住的郵箱
 */
function loadRememberedEmail() {
    const rememberedEmail = localStorage.getItem('rememberedEmail');
    if (rememberedEmail && elements.email) {
        elements.email.value = rememberedEmail;
        if (elements.rememberMe) {
            elements.rememberMe.checked = true;
        }
    }
}

/**
 * 檢查鎖定狀態
 */
function checkLockoutStatus() {
    const lockoutTime = localStorage.getItem('lockoutTime');
    if (lockoutTime) {
        const lockoutEnd = parseInt(lockoutTime) + lockoutTime;
        if (Date.now() < lockoutEnd) {
            const remainingTime = Math.ceil((lockoutEnd - Date.now()) / 60000);
            showToast(`帳戶已被鎖定，剩餘時間：${remainingTime} 分鐘`, 'warning');
            
            // 禁用表單
            if (elements.loginBtn) {
                elements.loginBtn.disabled = true;
            }
            
            // 設定倒計時
            const countdown = setInterval(() => {
                const remaining = Math.ceil((lockoutEnd - Date.now()) / 60000);
                if (remaining <= 0) {
                    clearInterval(countdown);
                    localStorage.removeItem('lockoutTime');
                    loginAttempts = 0;
                    if (elements.loginBtn) {
                        elements.loginBtn.disabled = false;
                    }
                    showToast('帳戶鎖定已解除', 'success');
                }
            }, 60000);
        } else {
            // 鎖定時間已過
            localStorage.removeItem('lockoutTime');
            loginAttempts = 0;
        }
    }
}

/**
 * 檢查是否被鎖定
 * @returns {boolean} 是否被鎖定
 */
function isLockedOut() {
    const lockoutTime = localStorage.getItem('lockoutTime');
    if (lockoutTime) {
        const lockoutEnd = parseInt(lockoutTime) + lockoutTime;
        return Date.now() < lockoutEnd;
    }
    return false;
}

/**
 * 處理鍵盤事件
 * @param {KeyboardEvent} event - 鍵盤事件
 */
function handleKeyboardEvents(event) {
    // Enter 鍵提交表單
    if (event.key === 'Enter' && event.target.tagName === 'INPUT') {
        event.preventDefault();
        elements.form.dispatchEvent(new Event('submit'));
    }
    
    // Tab 鍵循環焦點
    if (event.key === 'Tab') {
        handleTabNavigation(event);
    }
}

/**
 * 處理 Tab 鍵導航
 * @param {KeyboardEvent} event - 鍵盤事件
 */
function handleTabNavigation(event) {
    const focusableElements = elements.form.querySelectorAll(
        'input:not([disabled]), button:not([disabled]), select:not([disabled]), textarea:not([disabled]), [tabindex]:not([tabindex="-1"])'
    );
    
    const firstElement = focusableElements[0];
    const lastElement = focusableElements[focusableElements.length - 1];
    
    if (event.shiftKey) {
        // Shift + Tab (往前)
        if (document.activeElement === firstElement) {
            event.preventDefault();
            lastElement.focus();
        }
    } else {
        // Tab (往後)
        if (document.activeElement === lastElement) {
            event.preventDefault();
            firstElement.focus();
        }
    }
}

/**
 * 表單重置功能
 */
function resetForm() {
    if (elements.form) {
        elements.form.reset();
    }
    
    // 清除所有錯誤訊息
    Object.keys(validation).forEach(field => {
        hideError(`${field}Error`);
    });
    
    // 重置密碼顯示狀態
    if (elements.password) {
        elements.password.type = 'password';
        elements.eyeIcon.textContent = '👁️';
    }
}

// 導出函數供其他腳本使用
window.loginPageUtils = {
    resetForm,
    validateForm,
    checkLoginStatus
};

// 頁面可見性變化時重新檢查登入狀態
document.addEventListener('visibilitychange', function() {
    if (!document.hidden) {
        checkLoginStatus();
    }
});

// 監聽主題變化以適應樣式
window.addEventListener('themeChanged', function(event) {
    console.log('主題已變更:', event.detail.isDark ? '深色' : '淺色');
});

console.log('登入頁面 JavaScript 已載入完成');