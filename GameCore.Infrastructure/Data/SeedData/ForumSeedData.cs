using GameCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameCore.Infrastructure.Data.SeedData
{
    /// <summary>
    /// 論壇假資料
    /// </summary>
    public static class ForumSeedData
    {
        /// <summary>
        /// 建立論壇假資料
        /// </summary>
        /// <param name="modelBuilder">ModelBuilder</param>
        public static void SeedForumData(this ModelBuilder modelBuilder)
        {
            // 遊戲資料
            modelBuilder.Entity<Game>().HasData(
                new Game { GameId = 1, Name = "英雄聯盟", Genre = "MOBA", CreatedAt = DateTime.UtcNow.AddDays(-365) },
                new Game { GameId = 2, Name = "原神", Genre = "RPG", CreatedAt = DateTime.UtcNow.AddDays(-300) },
                new Game { GameId = 3, Name = "Steam 綜合", Genre = "綜合", CreatedAt = DateTime.UtcNow.AddDays(-200) },
                new Game { GameId = 4, Name = "手機遊戲", Genre = "手遊", CreatedAt = DateTime.UtcNow.AddDays(-150) },
                new Game { GameId = 5, Name = "綜合討論", Genre = "綜合", CreatedAt = DateTime.UtcNow.AddDays(-100) },
                new Game { GameId = 6, Name = "心情板", Genre = "社群", CreatedAt = DateTime.UtcNow.AddDays(-50) }
            );

            // 論壇版面資料
            modelBuilder.Entity<Forum>().HasData(
                new Forum { ForumId = 1, GameId = 1, Name = "英雄聯盟", Description = "版本情報、電競賽事、教學攻略", CreatedAt = DateTime.UtcNow.AddDays(-365) },
                new Forum { ForumId = 2, GameId = 2, Name = "原神", Description = "角色配隊、抽卡心得、世界探索", CreatedAt = DateTime.UtcNow.AddDays(-300) },
                new Forum { ForumId = 3, GameId = 3, Name = "Steam 綜合", Description = "促銷情報、遊戲心得、實況討論", CreatedAt = DateTime.UtcNow.AddDays(-200) },
                new Forum { ForumId = 4, GameId = 4, Name = "手機遊戲", Description = "Android / iOS 手遊討論", CreatedAt = DateTime.UtcNow.AddDays(-150) },
                new Forum { ForumId = 5, GameId = 5, Name = "綜合討論", Description = "硬體外設、雜談灌水、求助問答", CreatedAt = DateTime.UtcNow.AddDays(-100) },
                new Forum { ForumId = 6, GameId = 6, Name = "心情板", Description = "日常、告白、碎碎念、抱怨", CreatedAt = DateTime.UtcNow.AddDays(-50) }
            );

            // 主題資料
            var threadTitles = new[]
            {
                "平民向武器替代表（附表格）",
                "改版後坦克裝推薦",
                "首抽角色 CP 分析",
                "實測 120 抽紀錄",
                "打野動線更新（S 賽季）",
                "速刷日常路線（含地圖）",
                "Steam 夏促清單精選",
                "本周活動懶人包",
                "入門三天上手指南",
                "冷門角機體解構",
                "手機省電設定大全",
                "新手求助：如何快速上手？",
                "攻略分享：通關技巧大公開",
                "情報速報：最新更新內容",
                "閒聊：大家最喜歡的角色",
                "活動：限時挑戰賽",
                "同人：自製角色插畫",
                "抽卡：歐皇曬卡時間",
                "更新：版本更新日誌",
                "Bug：發現的遊戲問題"
            };

            var threadContents = new[]
            {
                "這篇文章將為大家介紹一些平民向的武器替代方案，希望能幫助到預算有限的玩家。\n\n## 武器推薦清單\n\n1. **初級武器**\n   - 攻擊力：100-150\n   - 價格：1000-3000 金幣\n   - 適合新手使用\n\n2. **中級武器**\n   - 攻擊力：150-250\n   - 價格：5000-15000 金幣\n   - 性價比最高\n\n3. **高級武器**\n   - 攻擊力：250-400\n   - 價格：20000-50000 金幣\n   - 適合進階玩家\n\n希望這份清單對大家有幫助！",

                "隨著最新版本的更新，坦克裝備有了很大的變化。以下是個人推薦的裝備搭配：\n\n### 核心裝備\n- **防禦頭盔**：提供基礎防護\n- **護甲**：增加生存能力\n- **盾牌**：格擋傷害\n\n### 可選裝備\n- **生命寶石**：增加血量\n- **恢復藥水**：持續回血\n\n這個搭配在實戰中表現相當不錯，推薦給大家試試！",

                "經過長時間的測試和分析，我整理出了首抽角色的 CP 值排行：\n\n## S 級角色\n1. **角色A** - 綜合能力最強\n2. **角色B** - 輸出能力突出\n\n## A 級角色\n1. **角色C** - 平衡性好\n2. **角色D** - 特殊技能強\n\n## B 級角色\n1. **角色E** - 適合新手\n2. **角色F** - 需要配合\n\n建議新手優先選擇 S 級角色開始遊戲。",

                "這次抽卡運氣不錯，120 抽的結果如下：\n\n### 抽卡結果\n- **SSR**：3 張\n- **SR**：15 張\n- **R**：102 張\n\n### 詳細清單\n1. SSR 角色A\n2. SSR 角色B\n3. SSR 角色C\n\n整體來說運氣還算可以，希望下次能抽到更多 SSR！",

                "S 賽季的打野動線有了重大更新，以下是新的路線規劃：\n\n## 新動線特點\n1. **更快的清野速度**\n2. **更好的經濟效益**\n3. **更強的支援能力**\n\n## 推薦路線\n1. 藍方：藍 → 三狼 → 紅 → 石頭人\n2. 紅方：紅 → 石頭人 → 藍 → 三狼\n\n這個路線在測試中表現優異，推薦給所有打野玩家。",

                "這裡分享一個速刷日常的路線，包含詳細的地圖標記：\n\n### 路線規劃\n1. **起點**：主城傳送點\n2. **第一站**：每日任務 NPC\n3. **第二站**：材料收集點\n4. **第三站**：經驗副本\n5. **終點**：獎勵領取處\n\n### 時間預估\n- 總時間：約 30 分鐘\n- 收益：大量經驗和金幣\n\n這個路線可以大大提高日常效率！",

                "Steam 夏季促銷即將開始，以下是我精選的遊戲清單：\n\n## 必買遊戲\n1. **遊戲A** - 原價 $60，現價 $15\n2. **遊戲B** - 原價 $40，現價 $10\n3. **遊戲C** - 原價 $30，現價 $8\n\n## 推薦理由\n- 遊戲品質高\n- 折扣力度大\n- 適合多人遊玩\n\n不要錯過這次機會！",

                "本周活動懶人包來了！\n\n## 活動時間\n- 開始：7月1日 00:00\n- 結束：7月7日 23:59\n\n## 活動內容\n1. **登入獎勵**：每日登入送好禮\n2. **任務挑戰**：完成任務獲得積分\n3. **限時副本**：特殊獎勵等你拿\n\n## 獎勵清單\n- 稀有道具\n- 限定角色\n- 大量金幣\n\n記得準時參加哦！",

                "新手入門三天上手指南：\n\n## 第一天\n1. 完成新手教程\n2. 熟悉基本操作\n3. 加入新手公會\n\n## 第二天\n1. 完成主線任務\n2. 嘗試多人模式\n3. 學習進階技巧\n\n## 第三天\n1. 挑戰困難副本\n2. 參與公會活動\n3. 制定長期目標\n\n按照這個指南，三天就能上手！",

                "今天來分析一下冷門角的機體構造：\n\n## 機體特點\n- **攻擊力**：中等\n- **防禦力**：較高\n- **速度**：較慢\n- **特殊能力**：獨特\n\n## 使用技巧\n1. 善用特殊能力\n2. 配合隊友\n3. 選擇合適時機\n\n雖然冷門，但用得好還是很強的！",

                "手機省電設定大全：\n\n## 系統設定\n1. 降低螢幕亮度\n2. 關閉不必要的通知\n3. 使用省電模式\n\n## 遊戲設定\n1. 降低畫質\n2. 關閉特效\n3. 減少音效\n\n## 其他技巧\n1. 關閉背景應用\n2. 使用 WiFi 而非 4G\n3. 定期清理快取\n\n這些設定可以大大延長遊戲時間！"
            };

            var threadData = new List<GameCore.Domain.Entities.Thread>();
            var random = new Random(42); // 固定種子以確保可重現性

            for (int i = 1; i <= 100; i++)
            {
                var forumId = random.Next(1, 7);
                var authorUserId = random.Next(1, 21);
                var title = threadTitles[random.Next(threadTitles.Length)] + (random.Next(100) < 20 ? "（含數據圖表）" : "");
                var createdAt = DateTime.UtcNow.AddDays(-random.Next(1, 30));

                threadData.Add(new GameCore.Domain.Entities.Thread
                {
                    ThreadId = i,
                    ForumId = forumId,
                    AuthorUserId = authorUserId,
                    Title = title,
                    Status = "normal",
                    CreatedAt = createdAt,
                    UpdatedAt = createdAt.AddHours(random.Next(1, 24))
                });
            }

            modelBuilder.Entity<GameCore.Domain.Entities.Thread>().HasData(threadData);

            // 主題回覆資料
            var postData = new List<ThreadPost>();
            var postContents = new[]
            {
                "感謝分享！這個攻略對我很有幫助。",
                "我也來分享一下我的經驗，希望對大家有用。",
                "這個方法確實有效，我試過了。",
                "有沒有人遇到過類似的問題？",
                "樓主說得很對，支持一下！",
                "這個技巧太實用了，謝謝分享。",
                "我也有類似的想法，樓主說得很清楚。",
                "這個攻略寫得很詳細，新手也能看懂。",
                "感謝樓主的無私分享！",
                "這個方法我之前也試過，效果不錯。",
                "有沒有人能詳細解釋一下這個機制？",
                "樓主的分析很到位，學到了很多。",
                "這個技巧確實很實用，推薦給大家。",
                "感謝樓主的經驗分享！",
                "這個攻略對我幫助很大，謝謝！",
                "樓主說得很對，我也遇到過這種情況。",
                "這個方法值得一試，謝謝分享。",
                "樓主的分析很透徹，學到了。",
                "這個技巧確實有效，我驗證過了。",
                "感謝樓主的詳細說明！"
            };

            var postId = 1;
            foreach (var thread in threadData)
            {
                // 每個主題的第一個回覆（主題內容）
                var contentIndex = (thread.ThreadId - 1) % threadContents.Length;
                postData.Add(new ThreadPost
                {
                    Id = postId++,
                    ThreadId = thread.ThreadId,
                    AuthorUserId = thread.AuthorUserId,
                    ContentMd = threadContents[contentIndex],
                    Status = "normal",
                    CreatedAt = thread.CreatedAt,
                    UpdatedAt = thread.CreatedAt
                });

                // 每個主題的額外回覆
                var replyCount = random.Next(0, 8);
                for (int j = 0; j < replyCount; j++)
                {
                    var replyAuthorId = random.Next(1, 21);
                    var replyContent = postContents[random.Next(postContents.Length)];
                    var replyTime = thread.CreatedAt.AddHours(random.Next(1, 48));

                    postData.Add(new ThreadPost
                    {
                        Id = postId++,
                        ThreadId = thread.ThreadId,
                        AuthorUserId = replyAuthorId,
                        ContentMd = replyContent,
                        Status = "normal",
                        CreatedAt = replyTime,
                        UpdatedAt = replyTime
                    });
                }
            }

            modelBuilder.Entity<ThreadPost>().HasData(postData);

            // 反應資料（讚）
            var reactionData = new List<Reaction>();
            var reactionId = 1;

            foreach (var thread in threadData)
            {
                var likeCount = random.Next(0, 15);
                for (int i = 0; i < likeCount; i++)
                {
                    var userId = random.Next(1, 21);
                    reactionData.Add(new Reaction
                    {
                        Id = reactionId++,
                        UserId = userId,
                        TargetType = "thread",
                        TargetId = thread.ThreadId,
                        Kind = "like",
                        CreatedAt = thread.CreatedAt.AddHours(random.Next(1, 24))
                    });
                }
            }

            // 回覆的讚
            foreach (var post in postData.Where(p => p.ParentPostId == null))
            {
                var likeCount = random.Next(0, 8);
                for (int i = 0; i < likeCount; i++)
                {
                    var userId = random.Next(1, 21);
                    reactionData.Add(new Reaction
                    {
                        Id = reactionId++,
                        UserId = userId,
                        TargetType = "thread_post",
                        TargetId = post.Id,
                        Kind = "like",
                        CreatedAt = post.CreatedAt.AddHours(random.Next(1, 12))
                    });
                }
            }

            modelBuilder.Entity<Reaction>().HasData(reactionData);

            // 收藏資料
            var bookmarkData = new List<Bookmark>();
            var bookmarkId = 1;

            foreach (var thread in threadData)
            {
                var bookmarkCount = random.Next(0, 10);
                for (int i = 0; i < bookmarkCount; i++)
                {
                    var userId = random.Next(1, 21);
                    bookmarkData.Add(new Bookmark
                    {
                        Id = bookmarkId++,
                        UserId = userId,
                        TargetType = "thread",
                        TargetId = thread.ThreadId,
                        CreatedAt = thread.CreatedAt.AddHours(random.Next(1, 48))
                    });
                }
            }

            // 論壇的收藏
            for (int forumId = 1; forumId <= 6; forumId++)
            {
                var bookmarkCount = random.Next(5, 20);
                for (int i = 0; i < bookmarkCount; i++)
                {
                    var userId = random.Next(1, 21);
                    bookmarkData.Add(new Bookmark
                    {
                        Id = bookmarkId++,
                        UserId = userId,
                        TargetType = "forum",
                        TargetId = forumId,
                        CreatedAt = DateTime.UtcNow.AddDays(-random.Next(1, 30))
                    });
                }
            }

            modelBuilder.Entity<Bookmark>().HasData(bookmarkData);
        }
    }
} 