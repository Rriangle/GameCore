/**
 * GameCore 寵物系統 JavaScript 實作
 * 
 * 功能說明：
 * - 可愛史萊姆寵物的完整互動系統
 * - Canvas 2D 動畫與粒子效果
 * - 狀態管理與 API 整合
 * - 音效系統 (使用 Howler.js)
 * - 冷卻系統與防重複點擊
 * 
 * 技術特點：
 * - 純 JavaScript (ES6+) 實作
 * - 無外部動畫函式庫依賴
 * - 60fps 流暢動畫
 * - 響應式設計
 * 
 * @author GameCore Team
 * @version 1.0.0
 */

class GameCorePetSystem {
    constructor(containerId = 'gc-pet-card') {
        // DOM 元素參照
        this.container = document.getElementById(containerId);
        this.canvas = document.getElementById('gc-pet-canvas');
        this.ctx = this.canvas.getContext('2d');
        
        // 寵物狀態數據
        this.petData = {
            name: '史萊姆',
            level: 1,
            experience: 0,
            experienceMax: 50,
            hunger: 100,
            mood: 100,
            energy: 100,
            cleanliness: 100,
            health: 100,
            skinColor: '#f6d84a',
            isAnimating: false,
            lastInteraction: null
        };
        
        // 使用者點數
        this.userPoints = 0;
        
        // 動畫狀態
        this.animationState = {
            tick: 0,
            eyeBlinkCounter: 0,
            isBlinking: false,
            breathPhase: 0,
            idleTimer: 0,
            currentAnimation: null,
            particles: []
        };
        
        // 互動冷卻時間 (毫秒)
        this.cooldownTime = 3000;
        this.lastActionTime = 0;
        
        // 音效系統 (需要 Howler.js)
        this.sounds = {};
        
        // API 端點配置
        this.apiEndpoints = {
            getPet: '/api/pet',
            interact: '/api/pet/interact',
            recolor: '/api/pet/recolor',
            updateName: '/api/pet/name',
            getBalance: '/api/wallet/balance',
            getStatus: '/api/pet/status',
            canAdventure: '/api/pet/can-adventure',
            getCooldown: '/api/pet/cooldown'
        };
        
        this.initialize();
    }
    
    /**
     * 初始化寵物系統
     */
    async initialize() {
        console.log('🐾 初始化 GameCore 寵物系統...');
        
        // 設定 Canvas
        this.setupCanvas();
        
        // 初始化音效
        this.initializeSounds();
        
        // 綁定事件監聽器
        this.bindEventListeners();
        
        // 載入寵物資料
        await this.loadPetData();
        
        // 開始動畫循環
        this.startAnimationLoop();
        
        // 定期重新整理狀態
        setInterval(() => this.refreshPetStatus(), 30000); // 每30秒
        
        console.log('✅ 寵物系統初始化完成');
    }
    
    /**
     * 設定 Canvas 基本參數
     */
    setupCanvas() {
        // 設定高解析度顯示
        const dpr = window.devicePixelRatio || 1;
        const rect = this.canvas.getBoundingClientRect();
        
        this.canvas.width = rect.width * dpr;
        this.canvas.height = rect.height * dpr;
        
        this.ctx.scale(dpr, dpr);
        this.ctx.imageSmoothingEnabled = false; // 像素風格
        
        // Canvas 樣式設定
        this.canvas.style.width = rect.width + 'px';
        this.canvas.style.height = rect.height + 'px';
    }
    
    /**
     * 初始化音效系統
     */
    initializeSounds() {
        // 檢查是否有 Howler.js
        if (typeof Howl === 'undefined') {
            console.warn('⚠️ Howler.js 未載入，音效功能將被停用');
            return;
        }
        
        // 定義音效檔案
        const soundFiles = {
            feed: '/sounds/pet/feed.mp3',
            bath: '/sounds/pet/bath.mp3',
            play: '/sounds/pet/play.mp3',
            rest: '/sounds/pet/rest.mp3',
            levelUp: '/sounds/pet/levelup.mp3',
            click: '/sounds/ui/click.mp3'
        };
        
        // 載入音效檔案
        Object.keys(soundFiles).forEach(key => {
            this.sounds[key] = new Howl({
                src: [soundFiles[key]],
                volume: 0.5,
                preload: true,
                onloaderror: () => {
                    console.warn(`⚠️ 無法載入音效檔案: ${soundFiles[key]}`);
                }
            });
        });
    }
    
    /**
     * 綁定事件監聽器
     */
    bindEventListeners() {
        // 互動按鈕
        const actionButtons = this.container.querySelectorAll('.gc-actions button[data-act]');
        actionButtons.forEach(button => {
            button.addEventListener('click', (e) => {
                e.preventDefault();
                const action = button.getAttribute('data-act');
                this.performAction(action);
            });
            
            // 新增按鈕懸停效果
            button.addEventListener('mouseenter', () => {
                button.style.transform = 'scale(1.05)';
            });
            
            button.addEventListener('mouseleave', () => {
                button.style.transform = 'scale(1)';
            });
        });
        
        // 冒險按鈕
        const adventureButton = document.getElementById('gc-adv');
        if (adventureButton) {
            adventureButton.addEventListener('click', (e) => {
                e.preventDefault();
                this.startAdventure();
            });
        }
        
        // Canvas 點擊事件 (撫摸寵物)
        this.canvas.addEventListener('click', (e) => {
            this.petSlime(e);
        });
        
        // 滑鼠移動追蹤 (寵物視線跟隨)
        this.canvas.addEventListener('mousemove', (e) => {
            this.updateEyeTracking(e);
        });
    }
    
    /**
     * 載入寵物資料
     */
    async loadPetData() {
        try {
            // 同時載入寵物狀態和使用者餘額
            const [petResponse, balanceResponse] = await Promise.all([
                fetch(this.apiEndpoints.getPet),
                fetch(this.apiEndpoints.getBalance)
            ]);
            
            if (petResponse.ok) {
                const petData = await petResponse.json();
                this.updatePetData(petData);
            } else {
                // 如果 API 未準備好，使用示範資料
                console.warn('⚠️ 寵物 API 未連線，使用示範資料');
                this.loadDemoData();
            }
            
            if (balanceResponse.ok) {
                const balanceData = await balanceResponse.json();
                this.userPoints = balanceData.balance || 0;
            } else {
                console.warn('⚠️ 錢包 API 未連線，使用示範資料');
                this.userPoints = 2500; // 示範用點數
            }
            
            this.updateUI();
            
        } catch (error) {
            console.error('❌ 載入寵物資料時發生錯誤:', error);
            this.loadDemoData();
        }
    }
    
    /**
     * 載入示範資料
     */
    loadDemoData() {
        this.petData = {
            name: '史萊姆',
            level: 5,
            experience: 120,
            experienceMax: 200,
            hunger: 85,
            mood: 92,
            energy: 78,
            cleanliness: 90,
            health: 95,
            skinColor: '#f6d84a',
            isAnimating: false,
            lastInteraction: null
        };
        this.userPoints = 2500;
        this.updateUI();
        this.logMessage('載入示範資料完成 🎮', 'info');
    }
    
    /**
     * 更新寵物資料
     */
    updatePetData(newData) {
        Object.assign(this.petData, newData);
        
        // 確保數值在合理範圍內
        ['hunger', 'mood', 'energy', 'cleanliness', 'health'].forEach(stat => {
            this.petData[stat] = Math.max(0, Math.min(100, this.petData[stat]));
        });
    }
    
    /**
     * 更新使用者介面
     */
    updateUI() {
        // 更新寵物資訊
        const nameElement = document.getElementById('gc-pet-name');
        const levelElement = document.getElementById('gc-pet-lv');
        const xpElement = document.getElementById('gc-pet-xp');
        const xpMaxElement = document.getElementById('gc-pet-xpmax');
        const coinsElement = document.getElementById('gc-coins');
        
        if (nameElement) nameElement.textContent = this.petData.name;
        if (levelElement) levelElement.textContent = this.petData.level;
        if (xpElement) xpElement.textContent = this.petData.experience;
        if (xpMaxElement) xpMaxElement.textContent = this.petData.experienceMax;
        if (coinsElement) coinsElement.textContent = this.userPoints.toLocaleString();
        
        // 更新狀態條
        this.updateStatBar('hunger', this.petData.hunger);
        this.updateStatBar('mood', this.petData.mood);
        this.updateStatBar('energy', this.petData.energy);
        this.updateStatBar('clean', this.petData.cleanliness);
        this.updateStatBar('health', this.petData.health);
    }
    
    /**
     * 更新狀態條
     */
    updateStatBar(statName, value) {
        const barElement = document.getElementById(`bar-${statName}`);
        const textElement = document.getElementById(`txt-${statName}`);
        
        if (barElement) {
            barElement.style.width = `${value}%`;
            
            // 根據數值設定顏色
            const parentBar = barElement.parentElement;
            parentBar.classList.remove('warn', 'bad');
            
            if (value < 20) {
                parentBar.classList.add('bad');
            } else if (value < 40) {
                parentBar.classList.add('warn');
            }
        }
        
        if (textElement) {
            textElement.textContent = Math.round(value);
        }
    }
    
    /**
     * 執行寵物互動
     */
    async performAction(action) {
        // 檢查冷卻時間
        const now = Date.now();
        const timeSinceLastAction = now - this.lastActionTime;
        
        if (timeSinceLastAction < this.cooldownTime) {
            const remainingSeconds = Math.ceil((this.cooldownTime - timeSinceLastAction) / 1000);
            this.logMessage(`請等待 ${remainingSeconds} 秒後再試 ⏰`, 'warn');
            return;
        }
        
        // 防止動畫進行中重複操作
        if (this.petData.isAnimating) {
            this.logMessage('史萊姆正在忙碌中... 🎭', 'warn');
            return;
        }
        
        try {
            // 播放音效
            this.playSound('click');
            
            // 開始動畫
            this.startActionAnimation(action);
            
            // 發送 API 請求
            const response = await fetch(this.apiEndpoints.interact, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ 
                    interactionType: action 
                })
            });
            
            if (response.ok) {
                const result = await response.json();
                
                // 更新寵物狀態
                if (result.pet) {
                    this.updatePetData(result.pet);
                }
                
                // 更新使用者點數
                if (result.points !== undefined) {
                    this.userPoints = result.points;
                }
                
                // 顯示結果訊息
                this.logMessage(result.message || this.getActionMessage(action), 'success');
                
                // 播放對應音效
                this.playSound(action);
                
                // 更新 UI
                this.updateUI();
                
                // 記錄最後互動時間
                this.lastActionTime = now;
                
            } else {
                throw new Error('API 請求失敗');
            }
            
        } catch (error) {
            console.error('❌ 執行互動時發生錯誤:', error);
            
            // 即使 API 失敗也要顯示動畫效果 (示範模式)
            this.simulateActionEffect(action);
            this.logMessage('後端未連線，顯示示範效果 🎪', 'info');
        }
    }
    
    /**
     * 模擬互動效果 (API 未連線時使用)
     */
    simulateActionEffect(action) {
        const effects = {
            Feed: { hunger: 10, message: '餵食史萊姆！飢餓度 +10 🍎' },
            Bath: { cleanliness: 10, message: '幫史萊姆洗澡！清潔度 +10 🛁' },
            Play: { mood: 10, message: '和史萊姆玩耍！心情 +10 🎾' },
            Rest: { energy: 10, message: '讓史萊姆休息！體力 +10 😴' }
        };
        
        const effect = effects[action];
        if (effect) {
            // 模擬狀態變化
            Object.keys(effect).forEach(key => {
                if (key !== 'message' && this.petData[key] !== undefined) {
                    this.petData[key] = Math.min(100, this.petData[key] + effect[key]);
                }
            });
            
            // 檢查是否所有狀態都滿了 (回滿健康度)
            if (this.petData.hunger >= 100 && this.petData.mood >= 100 && 
                this.petData.energy >= 100 && this.petData.cleanliness >= 100) {
                this.petData.health = 100;
                this.triggerHealthRestoreEffect();
            }
            
            this.updateUI();
            this.logMessage(effect.message, 'success');
            this.playSound(action.toLowerCase());
        }
    }
    
    /**
     * 開始冒險
     */
    async startAdventure() {
        try {
            // 檢查是否可以冒險
            const canAdventureResponse = await fetch(this.apiEndpoints.canAdventure);
            
            if (!canAdventureResponse.ok) {
                // 模擬檢查
                if (this.petData.health === 0 || this.petData.hunger === 0 || 
                    this.petData.energy === 0 || this.petData.cleanliness === 0) {
                    this.logMessage('史萊姆太虛弱了，無法冒險！請先照顧好牠 💔', 'warn');
                    return;
                }
            }
            
            // 開始冒險動畫
            this.startAdventureAnimation();
            
            // 模擬冒險結果
            setTimeout(() => {
                const isWin = Math.random() > 0.3; // 70% 勝率
                const expGain = isWin ? 50 : 10;
                const pointsGain = isWin ? 100 : 20;
                
                this.petData.experience += expGain;
                this.userPoints += pointsGain;
                
                // 屬性變化 (冒險會消耗)
                this.petData.hunger = Math.max(0, this.petData.hunger - 20);
                this.petData.energy = Math.max(0, this.petData.energy - 20);
                this.petData.cleanliness = Math.max(0, this.petData.cleanliness - 20);
                
                if (isWin) {
                    this.petData.mood = Math.min(100, this.petData.mood + 30);
                } else {
                    this.petData.mood = Math.max(0, this.petData.mood - 30);
                }
                
                // 檢查升級
                this.checkLevelUp();
                
                this.updateUI();
                
                const message = isWin ? 
                    `🎉 冒險成功！獲得 ${expGain} 經驗值、${pointsGain} 點數` :
                    `😅 冒險失敗，獲得 ${expGain} 經驗值、${pointsGain} 點數`;
                
                this.logMessage(message, isWin ? 'success' : 'warn');
                
                if (isWin) {
                    this.playSound('levelUp');
                    this.triggerVictoryEffect();
                }
                
            }, 3000);
            
        } catch (error) {
            console.error('❌ 冒險時發生錯誤:', error);
            this.logMessage('冒險系統暫時無法使用 🚧', 'warn');
        }
    }
    
    /**
     * 檢查升級
     */
    checkLevelUp() {
        while (this.petData.experience >= this.petData.experienceMax && this.petData.level < 250) {
            this.petData.experience -= this.petData.experienceMax;
            this.petData.level++;
            
            // 計算下一級所需經驗值
            this.petData.experienceMax = this.calculateRequiredExp(this.petData.level);
            
            // 升級效果
            this.triggerLevelUpEffect();
            this.logMessage(`🎊 恭喜！史萊姆升級到 Lv.${this.petData.level}！`, 'success');
            this.playSound('levelUp');
        }
    }
    
    /**
     * 計算所需經驗值
     */
    calculateRequiredExp(level) {
        if (level <= 10) {
            return 40 * level + 60;
        } else if (level <= 100) {
            return Math.floor(0.8 * level * level + 380);
        } else {
            return Math.floor(285.69 * Math.pow(1.06, level));
        }
    }
    
    /**
     * 撫摸寵物
     */
    petSlime(event) {
        const rect = this.canvas.getBoundingClientRect();
        const x = event.clientX - rect.left;
        const y = event.clientY - rect.top;
        
        // 創建撫摸效果
        this.createPetEffect(x, y);
        
        // 小幅增加心情
        this.petData.mood = Math.min(100, this.petData.mood + 1);
        this.updateStatBar('mood', this.petData.mood);
        
        this.logMessage('史萊姆很開心！💕', 'success');
    }
    
    /**
     * 創建撫摸效果
     */
    createPetEffect(x, y) {
        for (let i = 0; i < 5; i++) {
            this.animationState.particles.push({
                x: x,
                y: y,
                vx: (Math.random() - 0.5) * 4,
                vy: -Math.random() * 3 - 1,
                life: 1.0,
                color: '#ff69b4',
                type: 'heart'
            });
        }
    }
    
    /**
     * 開始動畫循環
     */
    startAnimationLoop() {
        const animate = () => {
            this.updateAnimation();
            this.renderPet();
            requestAnimationFrame(animate);
        };
        
        animate();
    }
    
    /**
     * 更新動畫狀態
     */
    updateAnimation() {
        this.animationState.tick++;
        
        // 呼吸動畫
        this.animationState.breathPhase += 0.05;
        
        // 眨眼動畫
        this.animationState.eyeBlinkCounter++;
        if (this.animationState.eyeBlinkCounter > 180 + Math.random() * 120) { // 3-5秒隨機眨眼
            this.animationState.isBlinking = true;
            this.animationState.eyeBlinkCounter = 0;
        }
        
        if (this.animationState.isBlinking) {
            if (this.animationState.eyeBlinkCounter > 10) { // 眨眼持續約1/6秒
                this.animationState.isBlinking = false;
            }
        }
        
        // 隨機 Idle 動作
        this.animationState.idleTimer++;
        if (this.animationState.idleTimer > 300 + Math.random() * 600) { // 5-15秒
            this.triggerRandomIdleAction();
            this.animationState.idleTimer = 0;
        }
        
        // 更新粒子
        this.updateParticles();
    }
    
    /**
     * 更新粒子效果
     */
    updateParticles() {
        this.animationState.particles = this.animationState.particles.filter(particle => {
            particle.x += particle.vx;
            particle.y += particle.vy;
            particle.vy += 0.1; // 重力
            particle.life -= 0.02;
            
            return particle.life > 0;
        });
    }
    
    /**
     * 繪製寵物
     */
    renderPet() {
        const ctx = this.ctx;
        const canvas = this.canvas;
        
        // 清除畫布
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        
        // 繪製背景
        this.drawBackground(ctx);
        
        // 繪製史萊姆
        this.drawSlime(ctx);
        
        // 繪製粒子效果
        this.drawParticles(ctx);
        
        // 繪製狀態指示器 (如果健康度很低)
        if (this.petData.health < 20) {
            this.drawHealthWarning(ctx);
        }
    }
    
    /**
     * 繪製背景
     */
    drawBackground(ctx) {
        const gradient = ctx.createLinearGradient(0, 0, 0, this.canvas.height);
        gradient.addColorStop(0, '#87CEEB');
        gradient.addColorStop(1, '#98FB98');
        
        ctx.fillStyle = gradient;
        ctx.fillRect(0, 0, this.canvas.width, this.canvas.height);
        
        // 簡單的草地
        ctx.fillStyle = '#90EE90';
        ctx.fillRect(0, this.canvas.height - 30, this.canvas.width, 30);
    }
    
    /**
     * 繪製史萊姆
     */
    drawSlime(ctx) {
        const centerX = this.canvas.width / 2;
        const centerY = this.canvas.height / 2 + 10;
        
        // 呼吸效果
        const breathScale = 1 + Math.sin(this.animationState.breathPhase) * 0.03;
        
        // 史萊姆身體
        ctx.save();
        ctx.translate(centerX, centerY);
        ctx.scale(breathScale, breathScale);
        
        // 主體 (橢圓形)
        ctx.fillStyle = this.petData.skinColor;
        ctx.beginPath();
        ctx.ellipse(0, 0, 35, 30, 0, 0, Math.PI * 2);
        ctx.fill();
        
        // 陰影
        ctx.fillStyle = 'rgba(0,0,0,0.1)';
        ctx.beginPath();
        ctx.ellipse(0, 25, 30, 8, 0, 0, Math.PI * 2);
        ctx.fill();
        
        // 高光
        if (this.petData.cleanliness > 70) {
            ctx.fillStyle = 'rgba(255,255,255,0.3)';
            ctx.beginPath();
            ctx.ellipse(-10, -15, 8, 12, -0.3, 0, Math.PI * 2);
            ctx.fill();
        }
        
        // 眼睛
        this.drawEyes(ctx);
        
        // 嘴巴 (根據心情變化)
        this.drawMouth(ctx);
        
        // 污漬 (如果清潔度低)
        if (this.petData.cleanliness < 50) {
            this.drawDirtSpots(ctx);
        }
        
        ctx.restore();
        
        // 狀態表情
        this.drawStatusIndicators(ctx, centerX, centerY);
    }
    
    /**
     * 繪製眼睛
     */
    drawEyes(ctx) {
        const eyeHeight = this.animationState.isBlinking ? 2 : 6;
        
        // 左眼
        ctx.fillStyle = '#000';
        ctx.fillRect(-15, -10, 6, eyeHeight);
        
        // 右眼
        ctx.fillRect(9, -10, 6, eyeHeight);
        
        // 眼神光 (如果沒有眨眼)
        if (!this.animationState.isBlinking && this.petData.health > 20) {
            ctx.fillStyle = '#fff';
            ctx.fillRect(-13, -8, 2, 2);
            ctx.fillRect(11, -8, 2, 2);
        }
    }
    
    /**
     * 繪製嘴巴
     */
    drawMouth(ctx) {
        ctx.strokeStyle = '#000';
        ctx.lineWidth = 2;
        ctx.lineCap = 'round';
        
        if (this.petData.mood >= 80) {
            // 開心 - 笑臉
            ctx.beginPath();
            ctx.arc(0, 5, 8, 0.2 * Math.PI, 0.8 * Math.PI);
            ctx.stroke();
        } else if (this.petData.mood >= 50) {
            // 普通 - 直線
            ctx.beginPath();
            ctx.moveTo(-6, 8);
            ctx.lineTo(6, 8);
            ctx.stroke();
        } else {
            // 不開心 - 皺眉
            ctx.beginPath();
            ctx.arc(0, 15, 8, -0.8 * Math.PI, -0.2 * Math.PI);
            ctx.stroke();
        }
        
        // 飢餓時的特殊表情
        if (this.petData.hunger < 30) {
            ctx.fillStyle = '#ff6b6b';
            ctx.font = '12px Arial';
            ctx.textAlign = 'center';
            ctx.fillText('...', 0, 20);
        }
    }
    
    /**
     * 繪製污漬
     */
    drawDirtSpots(ctx) {
        ctx.fillStyle = 'rgba(139, 69, 19, 0.6)';
        
        // 隨機位置的小污點
        const spots = [
            { x: 15, y: -5, size: 3 },
            { x: -20, y: 10, size: 2 },
            { x: 10, y: 15, size: 2.5 }
        ];
        
        spots.forEach(spot => {
            ctx.beginPath();
            ctx.arc(spot.x, spot.y, spot.size, 0, Math.PI * 2);
            ctx.fill();
        });
    }
    
    /**
     * 繪製狀態指示器
     */
    drawStatusIndicators(ctx, centerX, centerY) {
        // 疲累指示器
        if (this.petData.energy < 20) {
            ctx.fillStyle = '#666';
            ctx.font = 'bold 16px Arial';
            ctx.textAlign = 'center';
            
            // 繪製 Z 字符 (睡眠)
            const zOffset = Math.sin(this.animationState.tick * 0.1) * 3;
            ctx.fillText('Z', centerX + 25, centerY - 20 + zOffset);
            ctx.fillText('z', centerX + 35, centerY - 30 + zOffset);
        }
        
        // 愛心 (心情很好時)
        if (this.petData.mood >= 90) {
            this.drawHeart(ctx, centerX + 30, centerY - 25, '#ff69b4');
        }
    }
    
    /**
     * 繪製愛心
     */
    drawHeart(ctx, x, y, color) {
        ctx.fillStyle = color;
        ctx.beginPath();
        ctx.moveTo(x, y + 5);
        ctx.bezierCurveTo(x, y, x - 5, y, x - 5, y + 2.5);
        ctx.bezierCurveTo(x - 5, y + 5, x, y + 7.5, x, y + 10);
        ctx.bezierCurveTo(x, y + 7.5, x + 5, y + 5, x + 5, y + 2.5);
        ctx.bezierCurveTo(x + 5, y, x, y, x, y + 5);
        ctx.fill();
    }
    
    /**
     * 繪製粒子效果
     */
    drawParticles(ctx) {
        this.animationState.particles.forEach(particle => {
            ctx.save();
            ctx.globalAlpha = particle.life;
            
            if (particle.type === 'heart') {
                this.drawHeart(ctx, particle.x, particle.y, particle.color);
            } else {
                ctx.fillStyle = particle.color;
                ctx.beginPath();
                ctx.arc(particle.x, particle.y, 3, 0, Math.PI * 2);
                ctx.fill();
            }
            
            ctx.restore();
        });
    }
    
    /**
     * 繪製健康警告
     */
    drawHealthWarning(ctx) {
        ctx.fillStyle = 'rgba(255, 0, 0, 0.3)';
        ctx.fillRect(0, 0, this.canvas.width, this.canvas.height);
        
        ctx.fillStyle = '#ff0000';
        ctx.font = 'bold 14px Arial';
        ctx.textAlign = 'center';
        ctx.fillText('⚠️ 健康狀況危險！', this.canvas.width / 2, 20);
    }
    
    /**
     * 開始動作動畫
     */
    startActionAnimation(action) {
        this.petData.isAnimating = true;
        
        const animations = {
            Feed: () => this.animateFeed(),
            Bath: () => this.animateBath(),
            Play: () => this.animatePlay(),
            Rest: () => this.animateRest()
        };
        
        const animationFunction = animations[action];
        if (animationFunction) {
            animationFunction();
        }
        
        // 2秒後結束動畫狀態
        setTimeout(() => {
            this.petData.isAnimating = false;
        }, 2000);
    }
    
    /**
     * 餵食動畫
     */
    animateFeed() {
        // 創建食物粒子
        for (let i = 0; i < 10; i++) {
            setTimeout(() => {
                this.animationState.particles.push({
                    x: this.canvas.width / 2 - 20,
                    y: this.canvas.height / 2 - 30,
                    vx: Math.random() * 2 - 1,
                    vy: Math.random() * 2 + 1,
                    life: 1.0,
                    color: '#ffeb3b',
                    type: 'food'
                });
            }, i * 100);
        }
    }
    
    /**
     * 洗澡動畫
     */
    animateBath() {
        // 創建泡泡效果
        for (let i = 0; i < 15; i++) {
            setTimeout(() => {
                this.animationState.particles.push({
                    x: this.canvas.width / 2 + (Math.random() - 0.5) * 60,
                    y: this.canvas.height / 2 + (Math.random() - 0.5) * 40,
                    vx: (Math.random() - 0.5) * 2,
                    vy: -Math.random() * 3 - 1,
                    life: 1.5,
                    color: '#87ceeb',
                    type: 'bubble'
                });
            }, i * 80);
        }
    }
    
    /**
     * 玩耍動畫
     */
    animatePlay() {
        // 創建星星和愛心效果
        for (let i = 0; i < 12; i++) {
            setTimeout(() => {
                this.animationState.particles.push({
                    x: this.canvas.width / 2,
                    y: this.canvas.height / 2,
                    vx: (Math.random() - 0.5) * 8,
                    vy: -Math.random() * 6 - 2,
                    life: 1.2,
                    color: i % 2 === 0 ? '#ff69b4' : '#ffd700',
                    type: i % 2 === 0 ? 'heart' : 'star'
                });
            }, i * 150);
        }
    }
    
    /**
     * 休息動畫
     */
    animateRest() {
        // 創建 Z 字睡眠效果
        for (let i = 0; i < 6; i++) {
            setTimeout(() => {
                this.animationState.particles.push({
                    x: this.canvas.width / 2 + 30,
                    y: this.canvas.height / 2 - 20,
                    vx: Math.random() * 2 - 1,
                    vy: -Math.random() * 2 - 1,
                    life: 2.0,
                    color: '#666',
                    type: 'sleep'
                });
            }, i * 300);
        }
    }
    
    /**
     * 冒險動畫
     */
    startAdventureAnimation() {
        this.petData.isAnimating = true;
        this.logMessage('史萊姆出發冒險了！⚔️', 'info');
        
        // 創建冒險特效
        for (let i = 0; i < 20; i++) {
            setTimeout(() => {
                this.animationState.particles.push({
                    x: this.canvas.width / 2,
                    y: this.canvas.height / 2,
                    vx: (Math.random() - 0.5) * 10,
                    vy: (Math.random() - 0.5) * 10,
                    life: 1.5,
                    color: '#ff6b35',
                    type: 'adventure'
                });
            }, i * 50);
        }
        
        setTimeout(() => {
            this.petData.isAnimating = false;
        }, 3000);
    }
    
    /**
     * 觸發隨機 Idle 動作
     */
    triggerRandomIdleAction() {
        const actions = [
            () => this.createPetEffect(this.canvas.width / 2, this.canvas.height / 2),
            () => this.animationState.breathPhase += 1,
            () => {
                // 搖頭動作
                for (let i = 0; i < 3; i++) {
                    setTimeout(() => {
                        this.animationState.particles.push({
                            x: this.canvas.width / 2 + (i % 2 === 0 ? -10 : 10),
                            y: this.canvas.height / 2,
                            vx: 0,
                            vy: 0,
                            life: 0.5,
                            color: 'rgba(255,255,255,0.5)',
                            type: 'idle'
                        });
                    }, i * 200);
                }
            }
        ];
        
        const randomAction = actions[Math.floor(Math.random() * actions.length)];
        randomAction();
    }
    
    /**
     * 觸發升級效果
     */
    triggerLevelUpEffect() {
        // 創建升級爆炸效果
        for (let i = 0; i < 25; i++) {
            this.animationState.particles.push({
                x: this.canvas.width / 2,
                y: this.canvas.height / 2,
                vx: (Math.random() - 0.5) * 12,
                vy: (Math.random() - 0.5) * 12,
                life: 1.5,
                color: '#ffd700',
                type: 'levelup'
            });
        }
    }
    
    /**
     * 觸發健康回復效果
     */
    triggerHealthRestoreEffect() {
        // 創建治療光環效果
        for (let i = 0; i < 15; i++) {
            this.animationState.particles.push({
                x: this.canvas.width / 2,
                y: this.canvas.height / 2,
                vx: Math.cos(i * 0.4) * 3,
                vy: Math.sin(i * 0.4) * 3,
                life: 2.0,
                color: '#00ff7f',
                type: 'heal'
            });
        }
        
        this.logMessage('史萊姆完全恢復了！✨', 'success');
    }
    
    /**
     * 觸發勝利效果
     */
    triggerVictoryEffect() {
        // 創建慶祝煙火效果
        for (let i = 0; i < 30; i++) {
            setTimeout(() => {
                this.animationState.particles.push({
                    x: this.canvas.width / 2 + (Math.random() - 0.5) * 80,
                    y: this.canvas.height / 2 + (Math.random() - 0.5) * 60,
                    vx: (Math.random() - 0.5) * 6,
                    vy: -Math.random() * 4 - 2,
                    life: 1.8,
                    color: ['#ff6b35', '#f7931e', '#ffd23f', '#06ffa5', '#3b82f6'][Math.floor(Math.random() * 5)],
                    type: 'firework'
                });
            }, i * 30);
        }
    }
    
    /**
     * 視線追蹤
     */
    updateEyeTracking(event) {
        // 簡化版本 - 可以之後擴展為真正的視線跟隨
        const rect = this.canvas.getBoundingClientRect();
        const mouseX = event.clientX - rect.left;
        const mouseY = event.clientY - rect.top;
        
        // 存儲滑鼠位置供繪製時使用
        this.mousePosition = { x: mouseX, y: mouseY };
    }
    
    /**
     * 播放音效
     */
    playSound(soundName) {
        if (this.sounds[soundName] && this.sounds[soundName].state() === 'loaded') {
            this.sounds[soundName].play();
        }
    }
    
    /**
     * 取得動作訊息
     */
    getActionMessage(action) {
        const messages = {
            Feed: '餵食史萊姆！好香的食物～ 🍎',
            Bath: '史萊姆洗得好乾淨！閃閃發亮 ✨',
            Play: '和史萊姆一起玩耍！真開心 🎾',
            Rest: '史萊姆睡得好香甜～ 😴'
        };
        
        return messages[action] || '互動成功！';
    }
    
    /**
     * 記錄訊息到日誌
     */
    logMessage(message, type = 'info') {
        const logContainer = document.getElementById('gc-log');
        if (!logContainer) return;
        
        const logItem = document.createElement('li');
        logItem.textContent = `${new Date().toLocaleTimeString()} ${message}`;
        
        // 根據類型設定樣式
        const typeClasses = {
            success: '',
            warn: 'warn',
            error: 'bad',
            info: ''
        };
        
        if (typeClasses[type]) {
            logItem.classList.add(typeClasses[type]);
        }
        
        // 添加到日誌頂部
        logContainer.insertBefore(logItem, logContainer.firstChild);
        
        // 限制日誌數量
        while (logContainer.children.length > 20) {
            logContainer.removeChild(logContainer.lastChild);
        }
        
        // 淡入效果
        logItem.style.opacity = '0';
        setTimeout(() => {
            logItem.style.transition = 'opacity 0.3s ease';
            logItem.style.opacity = '1';
        }, 10);
    }
    
    /**
     * 重新整理寵物狀態
     */
    async refreshPetStatus() {
        await this.loadPetData();
    }
    
    /**
     * 銷毀寵物系統
     */
    destroy() {
        // 清理事件監聽器
        this.canvas.removeEventListener('click', this.petSlime);
        this.canvas.removeEventListener('mousemove', this.updateEyeTracking);
        
        // 停止音效
        Object.values(this.sounds).forEach(sound => {
            if (sound.unload) sound.unload();
        });
        
        console.log('🐾 寵物系統已清理');
    }
}

// 全域變數，供外部存取
window.GameCorePetSystem = GameCorePetSystem;

// DOM 載入完成後自動初始化
document.addEventListener('DOMContentLoaded', () => {
    if (document.getElementById('gc-pet-card')) {
        window.petSystem = new GameCorePetSystem();
    }
});

