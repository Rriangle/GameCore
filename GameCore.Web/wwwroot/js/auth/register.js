/**
 * 註冊頁面 JavaScript
 * 處理多步驟註冊表單、即時驗證、密碼強度檢測等功能
 */

// DOM 元素快取
const elements = {
    form: document.getElementById('registerForm'),
    steps: document.querySelectorAll('.form-step'),
    stepIndicators: document.querySelectorAll('.step'),
    
    // Step 1 elements
    userName: document.getElementById('userName'),
    userAccount: document.getElementById('userAccount'),
    email: document.getElementById('email'),
    password: document.getElementById('password'),
    confirmPassword: document.getElementById('confirmPassword'),
    passwordToggle: document.getElementById('passwordToggle'),
    eyeIcon: document.getElementById('eyeIcon'),
    passwordStrength: document.getElementById('passwordStrength'),
    
    // Step 2 elements
    nickName: document.getElementById('nickName'),
    gender: document.getElementById('gender'),
    dateOfBirth: document.getElementById('dateOfBirth'),
    cellphone: document.getElementById('cellphone'),
    idNumber: document.getElementById('idNumber'),
    address: document.getElementById('address'),
    userIntroduce: document.getElementById('userIntroduce'),
    charCount: document.getElementById('charCount'),
    
    // Step 3 elements
    agreeTerms: document.getElementById('agreeTerms'),
    newsletter: document.getElementById('newsletter'),
    
    // Navigation buttons
    nextStep1: document.getElementById('nextStep1'),
    nextStep2: document.getElementById('nextStep2'),
    backStep1: document.getElementById('backStep1'),
    backStep2: document.getElementById('backStep2'),
    registerBtn: document.getElementById('registerBtn'),
    
    // Summary elements
    summaryUserName: document.getElementById('summaryUserName'),
    summaryUserAccount: document.getElementById('summaryUserAccount'),
    summaryEmail: document.getElementById('summaryEmail'),
    summaryNickName: document.getElementById('summaryNickName'),
    summaryCellphone: document.getElementById('summaryCellphone')
};

// 當前步驟
let currentStep = 1;

// 表單驗證規則
const validation = {
    // Step 1
    userName: {
        required: true,
        minLength: 2,
        maxLength: 50,
        pattern: /^[\u4e00-\u9fa5a-zA-Z0-9_]+$/,
        message: '使用者名稱只能包含中文、英文、數字和下劃線，長度2-50字符'
    },
    userAccount: {
        required: true,
        minLength: 4,
        maxLength: 20,
        pattern: /^[a-zA-Z0-9_]+$/,
        message: '登入帳號只能包含英文、數字和下劃線，長度4-20字符'
    },
    email: {
        required: true,
        pattern: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
        message: '請輸入有效的電子郵件地址'
    },
    password: {
        required: true,
        minLength: 8,
        message: '密碼長度至少需要8個字符'
    },
    confirmPassword: {
        required: true,
        message: '請確認密碼'
    },
    
    // Step 2
    nickName: {
        required: true,
        minLength: 1,
        maxLength: 50,
        message: '暱稱長度不能超過50字符'
    },
    gender: {
        required: true,
        message: '請選擇性別'
    },
    dateOfBirth: {
        required: true,
        message: '請選擇出生日期'
    },
    cellphone: {
        required: true,
        pattern: /^09\d{8}$/,
        message: '請輸入有效的手機號碼（如：0912345678）'
    },
    idNumber: {
        required: true,
        pattern: /^[A-Z]\d{9}$/,
        message: '請輸入有效的身分證字號（如：A123456789）'
    },
    address: {
        required: true,
        minLength: 5,
        message: '請輸入完整地址'
    },
    
    // Step 3
    agreeTerms: {
        required: true,
        message: '請同意服務條款和隱私政策'
    }
};

// 已使用的用戶名和帳號快取（模擬檢查）
const usedNames = new Set();
const usedAccounts = new Set();

/**
 * 初始化註冊頁面
 */
document.addEventListener('DOMContentLoaded', function() {
    initializeEventListeners();
    updateStepIndicator();
    setupPasswordStrengthMeter();
    setupCharacterCounter();
    setMaxDate();
});

/**
 * 初始化事件監聽器
 */
function initializeEventListeners() {
    // 表單提交事件
    if (elements.form) {
        elements.form.addEventListener('submit', handleFormSubmit);
    }

    // 步驟導航按鈕
    if (elements.nextStep1) {
        elements.nextStep1.addEventListener('click', () => goToStep(2));
    }
    if (elements.nextStep2) {
        elements.nextStep2.addEventListener('click', () => goToStep(3));
    }
    if (elements.backStep1) {
        elements.backStep1.addEventListener('click', () => goToStep(1));
    }
    if (elements.backStep2) {
        elements.backStep2.addEventListener('click', () => goToStep(2));
    }

    // 密碼顯示切換
    if (elements.passwordToggle) {
        elements.passwordToggle.addEventListener('click', togglePasswordVisibility);
    }

    // 即時驗證事件
    setupRealtimeValidation();

    // 字符計數器
    if (elements.userIntroduce) {
        elements.userIntroduce.addEventListener('input', updateCharacterCount);
    }

    // 鍵盤事件
    document.addEventListener('keydown', handleKeyboardEvents);
}

/**
 * 設定即時驗證
 */
function setupRealtimeValidation() {
    Object.keys(validation).forEach(fieldName => {
        const element = elements[fieldName];
        if (element) {
            // 失去焦點時驗證
            element.addEventListener('blur', () => validateField(fieldName));
            
            // 輸入時清除錯誤（除了特殊欄位）
            if (!['confirmPassword', 'agreeTerms'].includes(fieldName)) {
                element.addEventListener('input', () => hideError(`${fieldName}Error`));
            }
            
            // 特殊處理
            if (fieldName === 'password') {
                element.addEventListener('input', updatePasswordStrength);
            }
            if (fieldName === 'confirmPassword') {
                element.addEventListener('input', validatePasswordMatch);
            }
            if (fieldName === 'userName' || fieldName === 'userAccount') {
                element.addEventListener('blur', () => checkUniqueness(fieldName));
            }
        }
    });
}

/**
 * 設定密碼強度計
 */
function setupPasswordStrengthMeter() {
    if (elements.password) {
        elements.password.addEventListener('input', updatePasswordStrength);
    }
}

/**
 * 設定字符計數器
 */
function setupCharacterCounter() {
    if (elements.userIntroduce && elements.charCount) {
        updateCharacterCount();
    }
}

/**
 * 設定出生日期最大值為今天
 */
function setMaxDate() {
    if (elements.dateOfBirth) {
        const today = new Date().toISOString().split('T')[0];
        elements.dateOfBirth.max = today;
        
        // 設定最小年齡為13歲
        const minDate = new Date();
        minDate.setFullYear(minDate.getFullYear() - 13);
        elements.dateOfBirth.max = minDate.toISOString().split('T')[0];
    }
}

/**
 * 前往指定步驟
 * @param {number} step - 目標步驟
 */
async function goToStep(step) {
    // 驗證當前步驟
    if (step > currentStep && !await validateCurrentStep()) {
        return;
    }

    // 更新步驟
    currentStep = step;
    
    // 更新 DOM
    updateStepDisplay();
    updateStepIndicator();
    
    // 如果是步驟3，更新摘要
    if (step === 3) {
        updateSummary();
    }
    
    // 聚焦到第一個輸入欄位
    focusFirstInput();
    
    // 滾動到頂部
    document.querySelector('.auth-card').scrollIntoView({ 
        behavior: 'smooth',
        block: 'start'
    });
}

/**
 * 更新步驟顯示
 */
function updateStepDisplay() {
    elements.steps.forEach((step, index) => {
        if (index + 1 === currentStep) {
            step.classList.add('active');
        } else {
            step.classList.remove('active');
        }
    });
}

/**
 * 更新步驟指示器
 */
function updateStepIndicator() {
    elements.stepIndicators.forEach((indicator, index) => {
        const stepNumber = index + 1;
        
        if (stepNumber < currentStep) {
            indicator.classList.add('completed');
            indicator.classList.remove('active');
        } else if (stepNumber === currentStep) {
            indicator.classList.add('active');
            indicator.classList.remove('completed');
        } else {
            indicator.classList.remove('active', 'completed');
        }
    });
}

/**
 * 聚焦到當前步驟的第一個輸入欄位
 */
function focusFirstInput() {
    const currentStepElement = document.getElementById(`step${currentStep}`);
    const firstInput = currentStepElement?.querySelector('input, select, textarea');
    if (firstInput) {
        setTimeout(() => firstInput.focus(), 100);
    }
}

/**
 * 驗證當前步驟
 * @returns {Promise<boolean>} 是否通過驗證
 */
async function validateCurrentStep() {
    let isValid = true;
    
    const fieldsToValidate = getFieldsForStep(currentStep);
    
    for (const fieldName of fieldsToValidate) {
        if (!await validateField(fieldName)) {
            isValid = false;
        }
    }
    
    return isValid;
}

/**
 * 獲取指定步驟需要驗證的欄位
 * @param {number} step - 步驟號
 * @returns {string[]} 欄位名稱陣列
 */
function getFieldsForStep(step) {
    switch (step) {
        case 1:
            return ['userName', 'userAccount', 'email', 'password', 'confirmPassword'];
        case 2:
            return ['nickName', 'gender', 'dateOfBirth', 'cellphone', 'idNumber', 'address'];
        case 3:
            return ['agreeTerms'];
        default:
            return [];
    }
}

/**
 * 驗證單個欄位
 * @param {string} fieldName - 欄位名稱
 * @returns {Promise<boolean>} 欄位是否有效
 */
async function validateField(fieldName) {
    const element = elements[fieldName];
    const rules = validation[fieldName];
    
    if (!element || !rules) return true;
    
    let value = element.value?.trim() || '';
    
    // 特殊處理 checkbox
    if (element.type === 'checkbox') {
        value = element.checked;
    }
    
    // 清除之前的錯誤
    hideError(`${fieldName}Error`);
    
    // 必填驗證
    if (rules.required) {
        if (element.type === 'checkbox' && !value) {
            showError(`${fieldName}Error`, rules.message);
            return false;
        }
        if (element.type !== 'checkbox' && !value) {
            showError(`${fieldName}Error`, rules.message);
            return false;
        }
    }
    
    // 如果欄位為空且不是必填，跳過其他驗證
    if (!value && !rules.required) {
        return true;
    }
    
    // 長度驗證
    if (rules.minLength && value.length < rules.minLength) {
        showError(`${fieldName}Error`, `${getFieldDisplayName(fieldName)}長度至少需要${rules.minLength}個字符`);
        return false;
    }
    
    if (rules.maxLength && value.length > rules.maxLength) {
        showError(`${fieldName}Error`, `${getFieldDisplayName(fieldName)}長度不能超過${rules.maxLength}個字符`);
        return false;
    }
    
    // 格式驗證
    if (rules.pattern && !rules.pattern.test(value)) {
        showError(`${fieldName}Error`, rules.message);
        return false;
    }
    
    // 特殊驗證
    switch (fieldName) {
        case 'confirmPassword':
            if (value !== elements.password.value) {
                showError('confirmPasswordError', '兩次輸入的密碼不一致');
                return false;
            }
            break;
            
        case 'dateOfBirth':
            if (!validateAge(value)) {
                showError('dateOfBirthError', '年齡必須滿13歲');
                return false;
            }
            break;
            
        case 'idNumber':
            if (!validateTaiwanId(value)) {
                showError('idNumberError', '身分證字號格式不正確');
                return false;
            }
            break;
    }
    
    return true;
}

/**
 * 檢查用戶名/帳號唯一性
 * @param {string} fieldName - 欄位名稱
 */
async function checkUniqueness(fieldName) {
    const element = elements[fieldName];
    const value = element.value.trim();
    
    if (!value) return;
    
    try {
        // 模擬 API 檢查（實際應該調用後端 API）
        const response = await apiRequest(`/api/user/check-unique`, {
            method: 'POST',
            body: JSON.stringify({
                field: fieldName,
                value: value
            })
        });
        
        if (!response.isUnique) {
            const displayName = fieldName === 'userName' ? '使用者名稱' : '登入帳號';
            showError(`${fieldName}Error`, `此${displayName}已被使用`);
            return false;
        }
        
    } catch (error) {
        console.warn('無法檢查唯一性:', error);
        // 在離線模式下使用本地快取模擬
        const usedSet = fieldName === 'userName' ? usedNames : usedAccounts;
        if (usedSet.has(value)) {
            const displayName = fieldName === 'userName' ? '使用者名稱' : '登入帳號';
            showError(`${fieldName}Error`, `此${displayName}已被使用`);
            return false;
        }
    }
    
    return true;
}

/**
 * 獲取欄位顯示名稱
 * @param {string} fieldName - 欄位名稱
 * @returns {string} 顯示名稱
 */
function getFieldDisplayName(fieldName) {
    const displayNames = {
        userName: '使用者名稱',
        userAccount: '登入帳號',
        email: '電子郵件',
        password: '密碼',
        confirmPassword: '確認密碼',
        nickName: '暱稱',
        gender: '性別',
        dateOfBirth: '出生日期',
        cellphone: '聯絡電話',
        idNumber: '身分證字號',
        address: '地址',
        userIntroduce: '自我介紹'
    };
    
    return displayNames[fieldName] || fieldName;
}

/**
 * 驗證年齡
 * @param {string} dateString - 日期字串
 * @returns {boolean} 是否滿13歲
 */
function validateAge(dateString) {
    const birthDate = new Date(dateString);
    const today = new Date();
    const age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();
    
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
        return age - 1 >= 13;
    }
    
    return age >= 13;
}

/**
 * 驗證台灣身分證字號
 * @param {string} id - 身分證字號
 * @returns {boolean} 是否有效
 */
function validateTaiwanId(id) {
    if (!/^[A-Z]\d{9}$/.test(id)) return false;
    
    // 英文字母對應數字
    const letterMap = {
        A: 10, B: 11, C: 12, D: 13, E: 14, F: 15, G: 16, H: 17, I: 34, J: 18,
        K: 19, L: 20, M: 21, N: 22, O: 35, P: 23, Q: 24, R: 25, S: 26, T: 27,
        U: 28, V: 29, W: 32, X: 30, Y: 31, Z: 33
    };
    
    const letter = letterMap[id[0]];
    const numbers = id.substring(1).split('').map(n => parseInt(n));
    
    // 檢查碼計算
    const sum = Math.floor(letter / 10) + (letter % 10) * 9 +
                numbers[0] * 8 + numbers[1] * 7 + numbers[2] * 6 + numbers[3] * 5 +
                numbers[4] * 4 + numbers[5] * 3 + numbers[6] * 2 + numbers[7] * 1;
    
    return (10 - (sum % 10)) % 10 === numbers[8];
}

/**
 * 更新密碼強度
 */
function updatePasswordStrength() {
    const password = elements.password.value;
    const strengthInfo = validatePassword(password);
    
    if (!elements.passwordStrength) return;
    
    const strengthFill = elements.passwordStrength.querySelector('.strength-fill');
    const strengthText = elements.passwordStrength.querySelector('.strength-text');
    
    // 更新進度條
    strengthFill.className = `strength-fill ${strengthInfo.strength}`;
    
    // 更新文字
    const strengthLabels = {
        weak: '弱',
        fair: '普通',
        good: '良好',
        strong: '強'
    };
    
    strengthText.textContent = `密碼強度：${strengthLabels[strengthInfo.strength]}`;
}

/**
 * 驗證密碼確認
 */
function validatePasswordMatch() {
    const password = elements.password.value;
    const confirmPassword = elements.confirmPassword.value;
    
    hideError('confirmPasswordError');
    
    if (confirmPassword && password !== confirmPassword) {
        showError('confirmPasswordError', '兩次輸入的密碼不一致');
    }
}

/**
 * 切換密碼顯示/隱藏
 */
function togglePasswordVisibility() {
    const isPassword = elements.password.type === 'password';
    
    elements.password.type = isPassword ? 'text' : 'password';
    elements.confirmPassword.type = isPassword ? 'text' : 'password';
    elements.eyeIcon.textContent = isPassword ? '🙈' : '👁️';
    
    // 無障礙支援
    elements.passwordToggle.setAttribute('aria-label', 
        isPassword ? '隱藏密碼' : '顯示密碼');
}

/**
 * 更新字符計數
 */
function updateCharacterCount() {
    if (elements.userIntroduce && elements.charCount) {
        const count = elements.userIntroduce.value.length;
        elements.charCount.textContent = count;
        
        // 接近限制時改變顏色
        if (count > 180) {
            elements.charCount.style.color = 'var(--error-color)';
        } else if (count > 150) {
            elements.charCount.style.color = 'var(--warning-color)';
        } else {
            elements.charCount.style.color = 'var(--muted)';
        }
    }
}

/**
 * 更新摘要信息
 */
function updateSummary() {
    const summaryFields = {
        summaryUserName: elements.userName.value,
        summaryUserAccount: elements.userAccount.value,
        summaryEmail: elements.email.value,
        summaryNickName: elements.nickName.value,
        summaryCellphone: elements.cellphone.value
    };
    
    Object.entries(summaryFields).forEach(([summaryId, value]) => {
        const element = elements[summaryId];
        if (element) {
            element.textContent = value;
        }
    });
}

/**
 * 處理表單提交
 * @param {Event} event - 表單提交事件
 */
async function handleFormSubmit(event) {
    event.preventDefault();
    
    // 最終驗證
    if (!await validateCurrentStep()) {
        return;
    }
    
    // 設定載入狀態
    setLoading(elements.registerBtn, true);
    
    try {
        // 收集表單資料
        const formData = collectFormData();
        
        // 發送註冊請求
        const response = await apiRequest('/api/user/register', {
            method: 'POST',
            body: JSON.stringify(formData)
        });
        
        // 註冊成功
        showToast('註冊成功！歡迎加入 GameCore！', 'success');
        
        // 延遲跳轉到登入頁面
        setTimeout(() => {
            window.location.href = '/Auth/Login';
        }, 2000);
        
    } catch (error) {
        // 註冊失敗
        showToast(error.message || '註冊失敗，請稍後再試', 'error');
        
        // 如果是唯一性錯誤，回到相應步驟
        if (error.message.includes('使用者名稱') || error.message.includes('帳號')) {
            goToStep(1);
        } else if (error.message.includes('暱稱')) {
            goToStep(2);
        }
        
    } finally {
        setLoading(elements.registerBtn, false);
    }
}

/**
 * 收集表單資料
 * @returns {object} 表單資料
 */
function collectFormData() {
    return {
        // 基本資料
        userName: elements.userName.value.trim(),
        userAccount: elements.userAccount.value.trim(),
        email: elements.email.value.trim(),
        password: elements.password.value,
        
        // 個人資訊
        nickName: elements.nickName.value.trim(),
        gender: elements.gender.value,
        dateOfBirth: elements.dateOfBirth.value,
        cellphone: elements.cellphone.value.trim(),
        idNumber: elements.idNumber.value.trim().toUpperCase(),
        address: elements.address.value.trim(),
        userIntroduce: elements.userIntroduce.value.trim(),
        
        // 其他選項
        newsletter: elements.newsletter.checked
    };
}

/**
 * 處理鍵盤事件
 * @param {KeyboardEvent} event - 鍵盤事件
 */
function handleKeyboardEvents(event) {
    // 在步驟1和2中，Enter鍵前往下一步
    if (event.key === 'Enter' && event.target.tagName === 'INPUT') {
        event.preventDefault();
        
        if (currentStep === 1) {
            goToStep(2);
        } else if (currentStep === 2) {
            goToStep(3);
        } else if (currentStep === 3) {
            elements.form.dispatchEvent(new Event('submit'));
        }
    }
}

// 導出函數供其他腳本使用
window.registerPageUtils = {
    goToStep,
    validateCurrentStep,
    collectFormData
};

console.log('註冊頁面 JavaScript 已載入完成');