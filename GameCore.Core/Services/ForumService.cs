using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Core.DTOs;
using Microsoft.Extensions.Logging;

namespace GameCore.Core.Services
{
    public class ForumService : IForumService
    {
        private readonly IForumRepository _forumRepository;
        private readonly IPostRepository _postRepository;
        private readonly IReplyRepository _replyRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ForumService> _logger;

        public ForumService(
            IForumRepository forumRepository,
            IPostRepository postRepository,
            IReplyRepository replyRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            ILogger<ForumService> logger)
        {
            _forumRepository = forumRepository;
            _postRepository = postRepository;
            _replyRepository = replyRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IEnumerable<ForumDto>> GetActiveForumsAsync()
        {
            try
            {
                var forums = await _forumRepository.GetActiveForumsAsync();
                return forums.Select(f => new ForumDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Description = f.Description,
                    Category = f.Category,
                    IsActive = f.IsActive,
                    PostCount = f.PostCount,
                    LastPostDate = f.LastPostDate,
                    CreatedAt = f.CreatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取活躍論壇失敗");
                return Enumerable.Empty<ForumDto>();
            }
        }

        public async Task<ForumDto?> GetForumByGameAsync(int gameId)
        {
            try
            {
                var forum = await _forumRepository.GetByGameIdAsync(gameId);
                if (forum == null) return null;

                return new ForumDto
                {
                    Id = forum.Id,
                    Name = forum.Name,
                    Description = forum.Description,
                    Category = forum.Category,
                    IsActive = forum.IsActive,
                    PostCount = forum.PostCount,
                    LastPostDate = forum.LastPostDate,
                    CreatedAt = forum.CreatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取遊戲論壇失敗: {GameId}", gameId);
                return null;
            }
        }

        public async Task<PostDto> CreatePostAsync(int userId, PostCreateDto postCreate)
        {
            try
            {
                var post = new Post
                {
                    ForumId = postCreate.ForumId,
                    UserId = userId,
                    Title = postCreate.Title,
                    Content = postCreate.Content,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _postRepository.CreateAsync(post);
                if (result == null) return null;

                return new PostDto
                {
                    Id = result.Id,
                    ForumId = result.ForumId,
                    UserId = result.UserId,
                    Username = result.User.Username,
                    Title = result.Title,
                    Content = result.Content,
                    ViewCount = result.ViewCount,
                    LikeCount = result.LikeCount,
                    ReplyCount = result.ReplyCount,
                    IsActive = result.IsActive,
                    CreatedAt = result.CreatedAt,
                    UpdatedAt = result.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建帖子失敗: {UserId}", userId);
                return null;
            }
        }

        public async Task<IEnumerable<PostDto>> GetPostsByForumAsync(int forumId, int page = 1, int pageSize = 20, string sortBy = "Latest")
        {
            try
            {
                var posts = await _postRepository.GetByForumIdAsync(forumId, page, pageSize, sortBy);
                return posts.Select(p => new PostDto
                {
                    Id = p.Id,
                    ForumId = p.ForumId,
                    UserId = p.UserId,
                    Username = p.User.Username,
                    Title = p.Title,
                    Content = p.Content,
                    ViewCount = p.ViewCount,
                    LikeCount = p.LikeCount,
                    ReplyCount = p.ReplyCount,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取論壇帖子失敗: {ForumId}", forumId);
                return Enumerable.Empty<PostDto>();
            }
        }

        public async Task<PostDto> GetPostAsync(int postId)
        {
            try
            {
                var post = await _postRepository.GetByIdAsync(postId);
                if (post == null) return null;

                // 增加瀏覽次數
                post.ViewCount++;
                await _postRepository.UpdateAsync(post);

                return new PostDto
                {
                    Id = post.Id,
                    ForumId = post.ForumId,
                    UserId = post.UserId,
                    Username = post.User.Username,
                    Title = post.Title,
                    Content = post.Content,
                    ViewCount = post.ViewCount,
                    LikeCount = post.LikeCount,
                    ReplyCount = post.ReplyCount,
                    IsActive = post.IsActive,
                    CreatedAt = post.CreatedAt,
                    UpdatedAt = post.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取帖子詳情失敗: {PostId}", postId);
                return null;
            }
        }

        public async Task<PostDto?> GetPostByIdAsync(int postId)
        {
            try
            {
                var post = await _postRepository.GetByIdAsync(postId);
                if (post == null) return null;

                // 增加瀏覽次數
                post.ViewCount++;
                await _postRepository.UpdateAsync(post);

                return new PostDto
                {
                    Id = post.Id,
                    ForumId = post.ForumId,
                    UserId = post.UserId,
                    Username = post.User.Username,
                    Title = post.Title,
                    Content = post.Content,
                    ViewCount = post.ViewCount,
                    LikeCount = post.LikeCount,
                    ReplyCount = post.ReplyCount,
                    IsActive = post.IsActive,
                    CreatedAt = post.CreatedAt,
                    UpdatedAt = post.UpdatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取帖子詳情失敗: {PostId}", postId);
                return null;
            }
        }

        public async Task<IEnumerable<PostDto>> SearchPostsAsync(string keyword, int page = 1, int pageSize = 20)
        {
            try
            {
                var posts = await _postRepository.SearchAsync(keyword, page, pageSize);
                return posts.Select(p => new PostDto
                {
                    Id = p.Id,
                    ForumId = p.ForumId,
                    UserId = p.UserId,
                    Username = p.User.Username,
                    Title = p.Title,
                    Content = p.Content,
                    ViewCount = p.ViewCount,
                    LikeCount = p.LikeCount,
                    ReplyCount = p.ReplyCount,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "搜尋帖子失敗");
                return Enumerable.Empty<PostDto>();
            }
        }

        public async Task<ReplyDto> CreateReplyAsync(int userId, int postId, string content)
        {
            try
            {
                var reply = new Reply
                {
                    PostId = postId,
                    UserId = userId,
                    Content = content,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _replyRepository.CreateAsync(reply);
                if (result == null) return null;

                return new ReplyDto
                {
                    Id = result.Id,
                    PostId = result.PostId,
                    UserId = result.UserId,
                    Username = result.User.Username,
                    Content = result.Content,
                    ParentReplyId = result.ParentReplyId,
                    LikeCount = result.LikeCount,
                    IsActive = result.IsActive,
                    CreatedAt = result.CreatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建回覆失敗: {UserId}", userId);
                return null;
            }
        }

        public async Task<IEnumerable<ReplyDto>> GetPostRepliesAsync(int postId, int page = 1, int pageSize = 20)
        {
            try
            {
                var replies = await _replyRepository.GetByPostIdAsync(postId, page, pageSize);
                return replies.Select(r => new ReplyDto
                {
                    Id = r.Id,
                    PostId = r.PostId,
                    UserId = r.UserId,
                    Username = r.User.Username,
                    Content = r.Content,
                    ParentReplyId = r.ParentReplyId,
                    LikeCount = r.LikeCount,
                    IsActive = r.IsActive,
                    CreatedAt = r.CreatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取帖子回覆失敗: {PostId}", postId);
                return Enumerable.Empty<ReplyDto>();
            }
        }

        public async Task<bool> LikePostAsync(int userId, int postId)
        {
            try
            {
                return await _postRepository.LikePostAsync(userId, postId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "點讚帖子失敗: {UserId}, {PostId}", userId, postId);
                return false;
            }
        }

        public async Task<bool> UnlikePostAsync(int userId, int postId)
        {
            try
            {
                return await _postRepository.UnlikePostAsync(userId, postId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取消點讚帖子失敗: {UserId}, {PostId}", userId, postId);
                return false;
            }
        }

        public async Task<bool> BookmarkPostAsync(int userId, int postId)
        {
            try
            {
                return await _postRepository.BookmarkPostAsync(userId, postId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "收藏帖子失敗: {UserId}, {PostId}", userId, postId);
                return false;
            }
        }

        public async Task<bool> UnbookmarkPostAsync(int userId, int postId)
        {
            try
            {
                return await _postRepository.UnbookmarkPostAsync(userId, postId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取消收藏帖子失敗: {UserId}, {PostId}", userId, postId);
                return false;
            }
        }

        public async Task<bool> IncrementViewCountAsync(int postId)
        {
            try
            {
                return await _postRepository.IncrementViewCountAsync(postId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "增加帖子瀏覽次數失敗: {PostId}", postId);
                return false;
            }
        }

        public async Task<IEnumerable<PostDto>> GetAllPostsAsync(int page = 1, int pageSize = 20)
        {
            try
            {
                var posts = await _postRepository.GetAllAsync(page, pageSize);
                return posts.Select(p => new PostDto
                {
                    Id = p.Id,
                    ForumId = p.ForumId,
                    UserId = p.UserId,
                    Username = p.User.Username,
                    Title = p.Title,
                    Content = p.Content,
                    ViewCount = p.ViewCount,
                    LikeCount = p.LikeCount,
                    ReplyCount = p.ReplyCount,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取所有帖子失敗");
                return Enumerable.Empty<PostDto>();
            }
        }

        public async Task<bool> UpdatePostAsync(int postId, int userId, string title, string content, string tags)
        {
            try
            {
                var post = await _postRepository.GetByIdAsync(postId);
                if (post == null || post.UserId != userId) return false;

                post.Title = title;
                post.Content = content;
                post.Tags = tags;
                post.UpdatedAt = DateTime.UtcNow;

                return await _postRepository.UpdateAsync(post);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新帖子失敗: {PostId}, {UserId}", postId, userId);
                return false;
            }
        }

        public async Task<bool> DeletePostAsync(int postId, int userId)
        {
            try
            {
                var post = await _postRepository.GetByIdAsync(postId);
                if (post == null || post.UserId != userId) return false;

                return await _postRepository.DeleteAsync(postId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刪除帖子失敗: {PostId}, {UserId}", postId, userId);
                return false;
            }
        }

        public async Task<bool> RemoveBookmarkAsync(int postId, int userId)
        {
            try
            {
                return await _postRepository.RemoveBookmarkAsync(userId, postId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取消收藏帖子失敗: {UserId}, {PostId}", userId, postId);
                return false;
            }
        }

        public async Task<IEnumerable<PostDto>> GetTrendingPostsAsync(int take = 10)
        {
            try
            {
                var posts = await _postRepository.GetTrendingPostsAsync(take);
                return posts.Select(p => new PostDto
                {
                    Id = p.Id,
                    ForumId = p.ForumId,
                    UserId = p.UserId,
                    Username = p.User.Username,
                    Title = p.Title,
                    Content = p.Content,
                    ViewCount = p.ViewCount,
                    LikeCount = p.LikeCount,
                    ReplyCount = p.ReplyCount,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取熱門帖子失敗");
                return Enumerable.Empty<PostDto>();
            }
        }

        public async Task<IEnumerable<PostDto>> GetUserPostsAsync(int userId, int page = 1, int pageSize = 20)
        {
            try
            {
                var posts = await _postRepository.GetByUserIdAsync(userId, page, pageSize);
                return posts.Select(p => new PostDto
                {
                    Id = p.Id,
                    ForumId = p.ForumId,
                    UserId = p.UserId,
                    Username = p.User.Username,
                    Title = p.Title,
                    Content = p.Content,
                    ViewCount = p.ViewCount,
                    LikeCount = p.LikeCount,
                    ReplyCount = p.ReplyCount,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取使用者帖子失敗: {UserId}", userId);
                return Enumerable.Empty<PostDto>();
            }
        }

        public async Task<IEnumerable<PostDto>> GetUserBookmarksAsync(int userId, int page = 1, int pageSize = 20)
        {
            try
            {
                var posts = await _postRepository.GetUserBookmarksAsync(userId, page, pageSize);
                return posts.Select(p => new PostDto
                {
                    Id = p.Id,
                    ForumId = p.ForumId,
                    UserId = p.UserId,
                    Username = p.User.Username,
                    Title = p.Title,
                    Content = p.Content,
                    ViewCount = p.ViewCount,
                    LikeCount = p.LikeCount,
                    ReplyCount = p.ReplyCount,
                    IsActive = p.IsActive,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取使用者收藏失敗: {UserId}", userId);
                return Enumerable.Empty<PostDto>();
            }
        }

        public async Task<IEnumerable<ForumDto>> GetForumsAsync()
        {
            try
            {
                var forums = await _forumRepository.GetAllAsync();
                return forums.Select(f => new ForumDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Description = f.Description,
                    Category = f.Category,
                    IsActive = f.IsActive,
                    PostCount = f.PostCount,
                    LastPostDate = f.LastPostDate,
                    CreatedAt = f.CreatedAt
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取論壇列表失敗");
                return Enumerable.Empty<ForumDto>();
            }
        }

        public async Task<ForumDto?> GetForumByIdAsync(int id)
        {
            try
            {
                var forum = await _forumRepository.GetByIdAsync(id);
                if (forum == null) return null;

                return new ForumDto
                {
                    Id = forum.Id,
                    Name = forum.Name,
                    Description = forum.Description,
                    Category = forum.Category,
                    IsActive = forum.IsActive,
                    PostCount = forum.PostCount,
                    LastPostDate = forum.LastPostDate,
                    CreatedAt = forum.CreatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取論壇詳情失敗: {ForumId}", id);
                return null;
            }
        }

        public async Task<ForumDto> CreateForumAsync(string name, string description, string category, int createdBy)
        {
            try
            {
                var forum = new Forum
                {
                    Name = name,
                    Description = description,
                    Category = category,
                    CreatedBy = createdBy,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _forumRepository.CreateAsync(forum);
                if (result == null) return null;

                return new ForumDto
                {
                    Id = result.Id,
                    Name = result.Name,
                    Description = result.Description,
                    Category = result.Category,
                    IsActive = result.IsActive,
                    PostCount = result.PostCount,
                    LastPostDate = result.LastPostDate,
                    CreatedAt = result.CreatedAt
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "創建論壇失敗: {Name}", name);
                return null;
            }
        }

        public async Task<bool> UpdateForumAsync(int id, string name, string description, string category, bool isActive)
        {
            try
            {
                var forum = await _forumRepository.GetByIdAsync(id);
                if (forum == null) return false;

                forum.Name = name;
                forum.Description = description;
                forum.Category = category;
                forum.IsActive = isActive;
                forum.UpdatedAt = DateTime.UtcNow;

                return await _forumRepository.UpdateAsync(forum);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新論壇失敗: {ForumId}", id);
                return false;
            }
        }

        public async Task<bool> DeleteForumAsync(int id)
        {
            try
            {
                return await _forumRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "刪除論壇失敗: {ForumId}", id);
                return false;
            }
        }

        public async Task<ForumStatsDto> GetForumStatsAsync(int id)
        {
            try
            {
                var forum = await _forumRepository.GetByIdAsync(id);
                if (forum == null) return null;

                return new ForumStatsDto
                {
                    ForumId = forum.Id,
                    TotalPosts = forum.PostCount,
                    TotalReplies = forum.ReplyCount,
                    TotalViews = forum.ViewCount,
                    LastActivity = forum.LastPostDate
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取論壇統計失敗: {ForumId}", id);
                return null;
            }
        }

        public async Task<bool> SubscribeToForumAsync(int forumId, int userId)
        {
            try
            {
                var subscription = new ForumSubscription
                {
                    ForumId = forumId,
                    UserId = userId,
                    SubscribedAt = DateTime.UtcNow
                };

                return await _forumRepository.SubscribeToForumAsync(subscription);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "訂閱論壇失敗: {ForumId}, {UserId}", forumId, userId);
                return false;
            }
        }

        public async Task<bool> UnsubscribeFromForumAsync(int forumId, int userId)
        {
            try
            {
                return await _forumRepository.UnsubscribeFromForumAsync(forumId, userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "取消訂閱論壇失敗: {ForumId}, {UserId}", forumId, userId);
                return false;
            }
        }
    }
} 