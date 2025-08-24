using GameCore.Core.DTOs;

namespace GameCore.Core.Services
{
    /// <summary>
    /// 社交服務介面 - 完整實現通知、聊天、群組、封鎖功能
    /// 提供多來源通知投遞、私訊聊天、群組管理、使用者封鎖等完整社交功能
    /// 嚴格按照規格要求實現通知狀態管理和群組互動邏輯
    /// </summary>
    public interface ISocialService
    {
        #region 通知系統

        /// <summary>
        /// 建立通知並投遞給收件人
        /// 建立 Notifications → 批次 Notification_Recipients（每收件者一筆，is_read 初始 0）
        /// </summary>
        /// <param name="createDto">建立通知請求</param>
        /// <returns>操作結果和通知資訊</returns>
        Task<SocialServiceResult<NotificationDto>> CreateNotificationAsync(CreateNotificationDto createDto);

        /// <summary>
        /// 取得使用者通知列表
        /// 支援分頁、篩選和排序
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <param name="unreadOnly">是否只顯示未讀</param>
        /// <param name="sourceId">來源類型篩選</param>
        /// <returns>分頁通知列表</returns>
        Task<SocialPagedResult<NotificationDto>> GetUserNotificationsAsync(
            int userId, int page = 1, int pageSize = 20, bool unreadOnly = false, int? sourceId = null);

        /// <summary>
        /// 標記通知為已讀
        /// 前台開啟通知列表/點開 → 更新 is_read=1、read_at
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="markReadDto">標記已讀請求</param>
        /// <returns>標記結果</returns>
        Task<SocialServiceResult> MarkNotificationsAsReadAsync(int userId, MarkNotificationReadDto markReadDto);

        /// <summary>
        /// 標記所有通知為已讀
        /// 批量更新使用者的所有未讀通知
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>標記結果</returns>
        Task<SocialServiceResult> MarkAllNotificationsAsReadAsync(int userId);

        /// <summary>
        /// 取得通知統計資訊
        /// 計算總數、未讀數、來源分布等統計資料
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>通知統計資訊</returns>
        Task<NotificationStatsDto> GetNotificationStatsAsync(int userId);

        /// <summary>
        /// 刪除通知
        /// 軟刪除使用者的特定通知
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="notificationIds">通知ID列表</param>
        /// <returns>刪除結果</returns>
        Task<SocialServiceResult> DeleteNotificationsAsync(int userId, List<int> notificationIds);

        /// <summary>
        /// 取得通知來源列表
        /// 返回系統中所有可用的通知來源類型
        /// </summary>
        /// <returns>通知來源列表</returns>
        Task<List<NotificationSourceDto>> GetNotificationSourcesAsync();

        /// <summary>
        /// 取得通知行為列表
        /// 返回系統中所有可用的通知行為類型
        /// </summary>
        /// <returns>通知行為列表</returns>
        Task<List<NotificationActionDto>> GetNotificationActionsAsync();

        #endregion

        #region 聊天系統

        /// <summary>
        /// 發送私訊
        /// Chat_Message（user→user 或客服 manager_id）
        /// </summary>
        /// <param name="senderId">發送者ID</param>
        /// <param name="sendMessageDto">發送訊息請求</param>
        /// <returns>操作結果和訊息資訊</returns>
        Task<SocialServiceResult<ChatMessageDto>> SendChatMessageAsync(int senderId, SendChatMessageDto sendMessageDto);

        /// <summary>
        /// 取得聊天對話列表
        /// 返回使用者的所有聊天對話，包含最後訊息和未讀數
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>分頁對話列表</returns>
        Task<SocialPagedResult<ChatConversationDto>> GetChatConversationsAsync(int userId, int page = 1, int pageSize = 20);

        /// <summary>
        /// 取得聊天訊息歷史
        /// 返回兩個使用者之間的聊天記錄
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="partnerId">對話對象ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>分頁訊息列表</returns>
        Task<SocialPagedResult<ChatMessageDto>> GetChatMessagesAsync(int userId, int partnerId, int page = 1, int pageSize = 50);

        /// <summary>
        /// 標記聊天訊息為已讀
        /// 更新特定對話中的訊息已讀狀態
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="markReadDto">標記已讀請求</param>
        /// <returns>標記結果</returns>
        Task<SocialServiceResult> MarkChatMessagesAsReadAsync(int userId, MarkChatReadDto markReadDto);

        /// <summary>
        /// 刪除聊天訊息
        /// 軟刪除使用者的特定聊天訊息
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="messageIds">訊息ID列表</param>
        /// <returns>刪除結果</returns>
        Task<SocialServiceResult> DeleteChatMessagesAsync(int userId, List<int> messageIds);

        /// <summary>
        /// 搜尋聊天訊息
        /// 在使用者的聊天記錄中搜尋特定關鍵字
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="keyword">搜尋關鍵字</param>
        /// <param name="partnerId">對話對象ID (可選)</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>搜尋結果</returns>
        Task<SocialPagedResult<ChatMessageDto>> SearchChatMessagesAsync(
            int userId, string keyword, int? partnerId = null, int page = 1, int pageSize = 20);

        #endregion

        #region 群組管理

        /// <summary>
        /// 建立群組
        /// 建立 Groups + 建立者加入 Group_Member
        /// </summary>
        /// <param name="creatorId">建立者ID</param>
        /// <param name="createDto">建立群組請求</param>
        /// <returns>操作結果和群組資訊</returns>
        Task<SocialServiceResult<GroupDto>> CreateGroupAsync(int creatorId, CreateGroupDto createDto);

        /// <summary>
        /// 更新群組資訊
        /// 更新群組名稱等基本資訊 (群組管理員限定)
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="groupId">群組ID</param>
        /// <param name="updateDto">更新群組請求</param>
        /// <returns>操作結果和更新後群組資訊</returns>
        Task<SocialServiceResult<GroupDto>> UpdateGroupAsync(int userId, int groupId, UpdateGroupDto updateDto);

        /// <summary>
        /// 加入群組
        /// 加入 Group_Member
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="joinDto">加入群組請求</param>
        /// <returns>加入結果</returns>
        Task<SocialServiceResult> JoinGroupAsync(int userId, JoinGroupDto joinDto);

        /// <summary>
        /// 退出群組
        /// 移除 Group_Member 記錄
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="groupId">群組ID</param>
        /// <returns>退出結果</returns>
        Task<SocialServiceResult> LeaveGroupAsync(int userId, int groupId);

        /// <summary>
        /// 邀請加入群組
        /// 批量邀請使用者加入群組 (群組成員限定)
        /// </summary>
        /// <param name="inviterId">邀請者ID</param>
        /// <param name="inviteDto">邀請加入群組請求</param>
        /// <returns>邀請結果</returns>
        Task<SocialServiceResult> InviteToGroupAsync(int inviterId, InviteToGroupDto inviteDto);

        /// <summary>
        /// 移除群組成員
        /// 從群組中移除特定成員 (群組管理員限定)
        /// </summary>
        /// <param name="adminId">管理員ID</param>
        /// <param name="removeDto">移除成員請求</param>
        /// <returns>移除結果</returns>
        Task<SocialServiceResult> RemoveFromGroupAsync(int adminId, RemoveFromGroupDto removeDto);

        /// <summary>
        /// 設定群組管理員
        /// 設定或取消群組成員的管理員權限 (群組管理員限定)
        /// </summary>
        /// <param name="adminId">管理員ID</param>
        /// <param name="setAdminDto">設定管理員請求</param>
        /// <returns>設定結果</returns>
        Task<SocialServiceResult> SetGroupAdminAsync(int adminId, SetGroupAdminDto setAdminDto);

        /// <summary>
        /// 取得使用者群組列表
        /// 返回使用者參與的所有群組
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>分頁群組列表</returns>
        Task<SocialPagedResult<GroupDto>> GetUserGroupsAsync(int userId, int page = 1, int pageSize = 20);

        /// <summary>
        /// 取得群組詳細資訊
        /// 包含群組基本資訊、成員列表、最近活動等
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="groupId">群組ID</param>
        /// <returns>群組詳細資訊</returns>
        Task<GroupDto?> GetGroupDetailAsync(int userId, int groupId);

        /// <summary>
        /// 取得群組成員列表
        /// 返回群組的所有成員資訊
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="groupId">群組ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>分頁成員列表</returns>
        Task<SocialPagedResult<GroupMemberDto>> GetGroupMembersAsync(int userId, int groupId, int page = 1, int pageSize = 50);

        /// <summary>
        /// 搜尋群組
        /// 根據群組名稱搜尋公開群組
        /// </summary>
        /// <param name="keyword">搜尋關鍵字</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>搜尋結果</returns>
        Task<SocialPagedResult<GroupDto>> SearchGroupsAsync(string keyword, int page = 1, int pageSize = 20);

        #endregion

        #region 群組聊天

        /// <summary>
        /// 發送群組聊天
        /// 群聊 Group_Chat
        /// </summary>
        /// <param name="senderId">發送者ID</param>
        /// <param name="sendChatDto">發送群組聊天請求</param>
        /// <returns>操作結果和聊天資訊</returns>
        Task<SocialServiceResult<GroupChatDto>> SendGroupChatAsync(int senderId, SendGroupChatDto sendChatDto);

        /// <summary>
        /// 取得群組聊天記錄
        /// 返回群組的聊天訊息歷史
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="groupId">群組ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>分頁聊天記錄</returns>
        Task<SocialPagedResult<GroupChatDto>> GetGroupChatMessagesAsync(int userId, int groupId, int page = 1, int pageSize = 50);

        /// <summary>
        /// 刪除群組聊天訊息
        /// 軟刪除群組聊天訊息 (發送者或群組管理員)
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="messageIds">訊息ID列表</param>
        /// <returns>刪除結果</returns>
        Task<SocialServiceResult> DeleteGroupChatMessagesAsync(int userId, List<int> messageIds);

        /// <summary>
        /// 搜尋群組聊天訊息
        /// 在群組聊天記錄中搜尋特定關鍵字
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="groupId">群組ID</param>
        /// <param name="keyword">搜尋關鍵字</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>搜尋結果</returns>
        Task<SocialPagedResult<GroupChatDto>> SearchGroupChatMessagesAsync(
            int userId, int groupId, string keyword, int page = 1, int pageSize = 20);

        #endregion

        #region 封鎖系統

        /// <summary>
        /// 封鎖群組成員
        /// 群封鎖 Group_Block（同群同人唯一）
        /// </summary>
        /// <param name="blockerId">封鎖者ID</param>
        /// <param name="blockDto">封鎖使用者請求</param>
        /// <returns>封鎖結果</returns>
        Task<SocialServiceResult> BlockUserInGroupAsync(int blockerId, BlockUserDto blockDto);

        /// <summary>
        /// 解除群組封鎖
        /// 移除 Group_Block 記錄
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="unblockDto">解除封鎖請求</param>
        /// <returns>解除封鎖結果</returns>
        Task<SocialServiceResult> UnblockUserInGroupAsync(int userId, UnblockUserDto unblockDto);

        /// <summary>
        /// 取得群組封鎖列表
        /// 返回群組中的封鎖記錄 (群組管理員限定)
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="groupId">群組ID</param>
        /// <param name="page">頁碼</param>
        /// <param name="pageSize">每頁筆數</param>
        /// <returns>分頁封鎖列表</returns>
        Task<SocialPagedResult<GroupBlockDto>> GetGroupBlocksAsync(int userId, int groupId, int page = 1, int pageSize = 20);

        /// <summary>
        /// 檢查使用者是否被封鎖
        /// 檢查特定使用者在群組中是否被封鎖
        /// </summary>
        /// <param name="groupId">群組ID</param>
        /// <param name="userId">使用者ID</param>
        /// <returns>是否被封鎖</returns>
        Task<bool> IsUserBlockedInGroupAsync(int groupId, int userId);

        #endregion

        #region 權限檢查

        /// <summary>
        /// 檢查群組權限
        /// 驗證使用者在群組中的權限
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="groupId">群組ID</param>
        /// <param name="action">操作類型</param>
        /// <returns>是否有權限</returns>
        Task<bool> CheckGroupPermissionAsync(int userId, int groupId, string action);

        /// <summary>
        /// 檢查是否為群組管理員
        /// 驗證使用者是否為指定群組的管理員
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="groupId">群組ID</param>
        /// <returns>是否為管理員</returns>
        Task<bool> IsGroupAdminAsync(int userId, int groupId);

        /// <summary>
        /// 檢查是否為群組成員
        /// 驗證使用者是否為指定群組的成員
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <param name="groupId">群組ID</param>
        /// <returns>是否為成員</returns>
        Task<bool> IsGroupMemberAsync(int userId, int groupId);

        #endregion

        #region 實時通知整合

        /// <summary>
        /// 發送實時通知
        /// 整合 SignalR 發送即時通知給線上使用者
        /// </summary>
        /// <param name="userIds">接收者ID列表</param>
        /// <param name="notificationType">通知類型</param>
        /// <param name="data">通知資料</param>
        /// <returns>發送結果</returns>
        Task<SocialServiceResult> SendRealtimeNotificationAsync(List<int> userIds, string notificationType, object data);

        /// <summary>
        /// 發送實時聊天訊息
        /// 整合 SignalR 發送即時聊天訊息給線上使用者
        /// </summary>
        /// <param name="receiverId">接收者ID</param>
        /// <param name="message">聊天訊息</param>
        /// <returns>發送結果</returns>
        Task<SocialServiceResult> SendRealtimeChatMessageAsync(int receiverId, ChatMessageDto message);

        /// <summary>
        /// 發送實時群組聊天訊息
        /// 整合 SignalR 發送即時群組聊天訊息給群組成員
        /// </summary>
        /// <param name="groupId">群組ID</param>
        /// <param name="message">群組聊天訊息</param>
        /// <returns>發送結果</returns>
        Task<SocialServiceResult> SendRealtimeGroupChatMessageAsync(int groupId, GroupChatDto message);

        #endregion

        #region 統計分析

        /// <summary>
        /// 取得社交統計資訊
        /// 計算使用者的社交活躍度統計
        /// </summary>
        /// <param name="userId">使用者ID</param>
        /// <returns>社交統計資訊</returns>
        Task<SocialStatsDto> GetSocialStatsAsync(int userId);

        /// <summary>
        /// 取得群組活躍度統計
        /// 計算群組的活躍度指標和排名
        /// </summary>
        /// <param name="limit">限制筆數</param>
        /// <returns>群組活躍度排名</returns>
        Task<List<GroupActivityDto>> GetGroupActivityRankingAsync(int limit = 10);

        #endregion

        #region 資料清理

        /// <summary>
        /// 清理過期通知
        /// 刪除超過指定天數的已讀通知
        /// </summary>
        /// <param name="daysToKeep">保留天數</param>
        /// <returns>清理結果</returns>
        Task<SocialServiceResult> CleanupOldNotificationsAsync(int daysToKeep = 30);

        /// <summary>
        /// 清理過期聊天記錄
        /// 刪除超過指定天數的聊天記錄
        /// </summary>
        /// <param name="daysToKeep">保留天數</param>
        /// <returns>清理結果</returns>
        Task<SocialServiceResult> CleanupOldChatMessagesAsync(int daysToKeep = 90);

        #endregion
    }

    #region 輔助類別

    /// <summary>
    /// 社交統計 DTO
    /// </summary>
    public class SocialStatsDto
    {
        /// <summary>總通知數</summary>
        public int TotalNotifications { get; set; }

        /// <summary>未讀通知數</summary>
        public int UnreadNotifications { get; set; }

        /// <summary>聊天對話數</summary>
        public int ChatConversations { get; set; }

        /// <summary>未讀聊天數</summary>
        public int UnreadChats { get; set; }

        /// <summary>參與群組數</summary>
        public int JoinedGroups { get; set; }

        /// <summary>管理群組數</summary>
        public int AdminGroups { get; set; }

        /// <summary>群組未讀訊息數</summary>
        public int UnreadGroupChats { get; set; }

        /// <summary>本週發送訊息數</summary>
        public int WeeklyMessagesSent { get; set; }

        /// <summary>本月社交活躍度分數</summary>
        public decimal MonthlyActivityScore { get; set; }
    }

    /// <summary>
    /// 群組活躍度 DTO
    /// </summary>
    public class GroupActivityDto
    {
        /// <summary>群組ID</summary>
        public int GroupId { get; set; }

        /// <summary>群組名稱</summary>
        public string? GroupName { get; set; }

        /// <summary>成員數量</summary>
        public int MemberCount { get; set; }

        /// <summary>本週訊息數</summary>
        public int WeeklyMessages { get; set; }

        /// <summary>活躍成員數</summary>
        public int ActiveMembers { get; set; }

        /// <summary>活躍度分數</summary>
        public decimal ActivityScore { get; set; }

        /// <summary>最後活動時間</summary>
        public DateTime? LastActivityAt { get; set; }
    }

    #endregion
}