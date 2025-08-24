using GameCore.Core.Entities;
using GameCore.Core.Interfaces;
using GameCore.Core.Services;
using GameCore.Core.DTOs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCore.Core.Services
{
    public class ForumService : IForumService
    {
        private readonly IForumRepository _forumRepository;
        private readonly IPostRepository _postRepository;
        private readonly IPostReplyRepository _postReplyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ForumService> _logger;

        public ForumService(
            IForumRepository forumRepository,
            IPostRepository postRepository,
            IPostReplyRepository postReplyRepository,
            IUnitOfWork unitOfWork,
            ILogger<ForumService> logger)
        {
            _forumRepository = forumRepository;
            _postRepository = postRepository;
            _postReplyRepository = postReplyRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
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
                    PostCount = f.PostCount,
                    LastPostAt = f.LastPostAt,
                    CreatedAt = f.CreatedAt,
                    IsActive = f.IsActive
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取論壇列表失敗");
                return Enumerable.Empty<ForumDto>();
            }
        }

        public async Task<ForumDto> GetForumByIdAsync(int forumId)
        {
            try
            {
                var forum = await _forumRepository.GetByIdAsync(forumId);
                if (forum == null) return null;

                return new ForumDto
                {
                    Id = forum.Id,
                    Name = forum.Name,
                    Description = forum.Description,
                    PostCount = forum.PostCount,
                    LastPostAt = forum.LastPostAt,
                    CreatedAt = forum.CreatedAt,
                    IsActive = forum.IsActive
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取論壇失敗: {ForumId}", forumId);
                return null;
            }
        }

        public async Task<PostListResult> GetPostsAsync(int forumId, int page = 1, int pageSize = 20)
        {
            try
            {
                var posts = await _postRepository.GetByForumIdAsync(forumId, page, pageSize);
                var totalCount = await _postRepository.GetCountByForumIdAsync(forumId);

                var postDtos = posts.Select(p => new PostListDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content.Length > 200 ? p.Content.Substring(0, 200) + "..." : p.Content,
                    AuthorId = p.AuthorId,
                    AuthorName = p.Author?.Username ?? "未知用戶",
                    ForumId = p.ForumId,
                    ForumName = p.Forum?.Name ?? "未知論壇",
                    ViewCount = p.ViewCount,
                    LikeCount = p.LikeCount,
                    ReplyCount = p.ReplyCount,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    LastReplyAt = p.LastReplyAt
                });

                return new PostListResult
                {
                    Success = true,
                    Posts = postDtos,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取貼文列表失敗: {ForumId}", forumId);
                return new PostListResult
                {
                    Success = false,
                    Message = "獲取貼文列表失敗"
                };
            }
        }

        public async Task<PostDetailResult> GetPostByIdAsync(int postId)
        {
            try
            {
                var post = await _postRepository.GetByIdAsync(postId);
                if (post == null)
                {
                    return new PostDetailResult
                    {
                        Success = false,
                        Message = "貼文不存在"
                    };
                }

                // 增加瀏覽次數
                post.ViewCount++;
                _postRepository.Update(post);
                await _unitOfWork.SaveChangesAsync();

                var postDto = new PostDetailDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Content = post.Content,
                    AuthorId = post.AuthorId,
                    AuthorName = post.Author?.Username ?? "未知用戶",
                    ForumId = post.ForumId,
                    ForumName = post.Forum?.Name ?? "未知論壇",
                    ViewCount = post.ViewCount,
                    LikeCount = post.LikeCount,
                    ReplyCount = post.ReplyCount,
                    Status = post.Status,
                    CreatedAt = post.CreatedAt,
                    UpdatedAt = post.UpdatedAt,
                    LastReplyAt = post.LastReplyAt
                };

                return new PostDetailResult
                {
                    Success = true,
                    Post = postDto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取貼文詳情失敗: {PostId}", postId);
                return new PostDetailResult
                {
                    Success = false,
                    Message = "獲取貼文詳情失敗"
                };
            }
        }

        public async Task<PostCreateResult> CreatePostAsync(PostCreateDto createDto)
        {
            try
            {
                var post = new Post
                {
                    Title = createDto.Title,
                    Content = createDto.Content,
                    AuthorId = createDto.AuthorId,
                    ForumId = createDto.ForumId,
                    Status = PostStatus.Active,
                    ViewCount = 0,
                    LikeCount = 0,
                    ReplyCount = 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _postRepository.Add(post);
                await _unitOfWork.SaveChangesAsync();

                // 更新論壇統計
                await UpdateForumStatistics(createDto.ForumId);

                _logger.LogInformation("新貼文創建成功: {PostId}, 作者: {AuthorId}", post.Id, post.AuthorId);

                return new PostCreateResult
                {
                    Success = true,
                    Message = "貼文創建成功",
                    PostId = post.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "貼文創建失敗: 作者 {AuthorId}", createDto.AuthorId);
                return new PostCreateResult
                {
                    Success = false,
                    Message = "貼文創建失敗"
                };
            }
        }

        public async Task<PostUpdateResult> UpdatePostAsync(int postId, int authorId, PostUpdateDto updateDto)
        {
            try
            {
                var post = await _postRepository.GetByIdAsync(postId);
                if (post == null)
                {
                    return new PostUpdateResult
                    {
                        Success = false,
                        Message = "貼文不存在"
                    };
                }

                if (post.AuthorId != authorId)
                {
                    return new PostUpdateResult
                    {
                        Success = false,
                        Message = "無權限修改此貼文"
                    };
                }

                post.Title = updateDto.Title;
                post.Content = updateDto.Content;
                post.UpdatedAt = DateTime.UtcNow;

                _postRepository.Update(post);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("貼文更新成功: {PostId}", postId);

                return new PostUpdateResult
                {
                    Success = true,
                    Message = "貼文更新成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "貼文更新失敗: {PostId}", postId);
                return new PostUpdateResult
                {
                    Success = false,
                    Message = "貼文更新失敗"
                };
            }
        }

        public async Task<PostDeleteResult> DeletePostAsync(int postId, int authorId)
        {
            try
            {
                var post = await _postRepository.GetByIdAsync(postId);
                if (post == null)
                {
                    return new PostDeleteResult
                    {
                        Success = false,
                        Message = "貼文不存在"
                    };
                }

                if (post.AuthorId != authorId)
                {
                    return new PostDeleteResult
                    {
                        Success = false,
                        Message = "無權限刪除此貼文"
                    };
                }

                post.Status = PostStatus.Deleted;
                post.UpdatedAt = DateTime.UtcNow;

                _postRepository.Update(post);
                await _unitOfWork.SaveChangesAsync();

                // 更新論壇統計
                await UpdateForumStatistics(post.ForumId);

                _logger.LogInformation("貼文刪除成功: {PostId}", postId);

                return new PostDeleteResult
                {
                    Success = true,
                    Message = "貼文刪除成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "貼文刪除失敗: {PostId}", postId);
                return new PostDeleteResult
                {
                    Success = false,
                    Message = "貼文刪除失敗"
                };
            }
        }

        public async Task<ReplyListResult> GetRepliesAsync(int postId, int page = 1, int pageSize = 20)
        {
            try
            {
                var replies = await _postReplyRepository.GetByPostIdAsync(postId, page, pageSize);
                var totalCount = await _postReplyRepository.GetCountByPostIdAsync(postId);

                var replyDtos = replies.Select(r => new ReplyListDto
                {
                    Id = r.Id,
                    Content = r.Content,
                    AuthorId = r.AuthorId,
                    AuthorName = r.Author?.Username ?? "未知用戶",
                    PostId = r.PostId,
                    ParentReplyId = r.ParentReplyId,
                    LikeCount = r.LikeCount,
                    CreatedAt = r.CreatedAt,
                    UpdatedAt = r.UpdatedAt
                });

                return new ReplyListResult
                {
                    Success = true,
                    Replies = replyDtos,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "獲取回覆列表失敗: {PostId}", postId);
                return new ReplyListResult
                {
                    Success = false,
                    Message = "獲取回覆列表失敗"
                };
            }
        }

        public async Task<ReplyCreateResult> CreateReplyAsync(ReplyCreateDto createDto)
        {
            try
            {
                var reply = new PostReply
                {
                    Content = createDto.Content,
                    AuthorId = createDto.AuthorId,
                    PostId = createDto.PostId,
                    ParentReplyId = createDto.ParentReplyId,
                    LikeCount = 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                _postReplyRepository.Add(reply);
                await _unitOfWork.SaveChangesAsync();

                // 更新貼文統計
                await UpdatePostStatistics(createDto.PostId);

                _logger.LogInformation("新回覆創建成功: {ReplyId}, 作者: {AuthorId}", reply.Id, reply.AuthorId);

                return new ReplyCreateResult
                {
                    Success = true,
                    Message = "回覆創建成功",
                    ReplyId = reply.Id
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "回覆創建失敗: 作者 {AuthorId}", createDto.AuthorId);
                return new ReplyCreateResult
                {
                    Success = false,
                    Message = "回覆創建失敗"
                };
            }
        }

        public async Task<ReplyUpdateResult> UpdateReplyAsync(int replyId, int authorId, ReplyUpdateDto updateDto)
        {
            try
            {
                var reply = await _postReplyRepository.GetByIdAsync(replyId);
                if (reply == null)
                {
                    return new ReplyUpdateResult
                    {
                        Success = false,
                        Message = "回覆不存在"
                    };
                }

                if (reply.AuthorId != authorId)
                {
                    return new ReplyUpdateResult
                    {
                        Success = false,
                        Message = "無權限修改此回覆"
                    };
                }

                reply.Content = updateDto.Content;
                reply.UpdatedAt = DateTime.UtcNow;

                _postReplyRepository.Update(reply);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("回覆更新成功: {ReplyId}", replyId);

                return new ReplyUpdateResult
                {
                    Success = true,
                    Message = "回覆更新成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "回覆更新失敗: {ReplyId}", replyId);
                return new ReplyUpdateResult
                {
                    Success = false,
                    Message = "回覆更新失敗"
                };
            }
        }

        public async Task<ReplyDeleteResult> DeleteReplyAsync(int replyId, int authorId)
        {
            try
            {
                var reply = await _postReplyRepository.GetByIdAsync(replyId);
                if (reply == null)
                {
                    return new ReplyDeleteResult
                    {
                        Success = false,
                        Message = "回覆不存在"
                    };
                }

                if (reply.AuthorId != authorId)
                {
                    return new ReplyDeleteResult
                    {
                        Success = false,
                        Message = "無權限刪除此回覆"
                    };
                }

                _postReplyRepository.Delete(reply);
                await _unitOfWork.SaveChangesAsync();

                // 更新貼文統計
                await UpdatePostStatistics(reply.PostId);

                _logger.LogInformation("回覆刪除成功: {ReplyId}", replyId);

                return new ReplyDeleteResult
                {
                    Success = true,
                    Message = "回覆刪除成功"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "回覆刪除失敗: {ReplyId}", replyId);
                return new ReplyDeleteResult
                {
                    Success = false,
                    Message = "回覆刪除失敗"
                };
            }
        }

        public async Task<SearchResult> SearchPostsAsync(string keyword, int page = 1, int pageSize = 20)
        {
            try
            {
                var posts = await _postRepository.SearchAsync(keyword, page, pageSize);
                var totalCount = await _postRepository.GetSearchCountAsync(keyword);

                var postDtos = posts.Select(p => new PostListDto
                {
                    Id = p.Id,
                    Title = p.Title,
                    Content = p.Content.Length > 200 ? p.Content.Substring(0, 200) + "..." : p.Content,
                    AuthorId = p.AuthorId,
                    AuthorName = p.Author?.Username ?? "未知用戶",
                    ForumId = p.ForumId,
                    ForumName = p.Forum?.Name ?? "未知論壇",
                    ViewCount = p.ViewCount,
                    LikeCount = p.LikeCount,
                    ReplyCount = p.ReplyCount,
                    Status = p.Status,
                    CreatedAt = p.CreatedAt,
                    UpdatedAt = p.UpdatedAt,
                    LastReplyAt = p.LastReplyAt
                });

                return new SearchResult
                {
                    Success = true,
                    Posts = postDtos,
                    TotalCount = totalCount,
                    Page = page,
                    PageSize = pageSize,
                    TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
                    Keyword = keyword
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "搜尋貼文失敗: {Keyword}", keyword);
                return new SearchResult
                {
                    Success = false,
                    Message = "搜尋失敗"
                };
            }
        }

        public async Task<bool> LikePostAsync(int postId, int userId)
        {
            try
            {
                var post = await _postRepository.GetByIdAsync(postId);
                if (post == null) return false;

                // 檢查是否已經點讚
                var existingLike = await _postRepository.GetLikeAsync(postId, userId);
                if (existingLike != null)
                {
                    // 取消點讚
                    _postRepository.RemoveLike(existingLike);
                    post.LikeCount = Math.Max(0, post.LikeCount - 1);
                }
                else
                {
                    // 添加點讚
                    var like = new PostLike
                    {
                        PostId = postId,
                        UserId = userId,
                        CreatedAt = DateTime.UtcNow
                    };
                    _postRepository.AddLike(like);
                    post.LikeCount++;
                }

                _postRepository.Update(post);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("貼文點讚狀態更新: {PostId}, 用戶: {UserId}", postId, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "貼文點讚失敗: {PostId}, 用戶: {UserId}", postId, userId);
                return false;
            }
        }

        public async Task<bool> BookmarkPostAsync(int postId, int userId)
        {
            try
            {
                var post = await _postRepository.GetByIdAsync(postId);
                if (post == null) return false;

                // 檢查是否已經收藏
                var existingBookmark = await _postRepository.GetBookmarkAsync(postId, userId);
                if (existingBookmark != null)
                {
                    // 取消收藏
                    _postRepository.RemoveBookmark(existingBookmark);
                }
                else
                {
                    // 添加收藏
                    var bookmark = new PostBookmark
                    {
                        PostId = postId,
                        UserId = userId,
                        CreatedAt = DateTime.UtcNow
                    };
                    _postRepository.AddBookmark(bookmark);
                }

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("貼文收藏狀態更新: {PostId}, 用戶: {UserId}", postId, userId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "貼文收藏失敗: {PostId}, 用戶: {UserId}", postId, userId);
                return false;
            }
        }

        public async Task<bool> IncrementViewCountAsync(int postId)
        {
            try
            {
                var post = await _postRepository.GetByIdAsync(postId);
                if (post == null) return false;

                post.ViewCount++;
                _postRepository.Update(post);
                await _unitOfWork.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "增加貼文瀏覽次數失敗: {PostId}", postId);
                return false;
            }
        }

        private async Task UpdateForumStatistics(int forumId)
        {
            try
            {
                var forum = await _forumRepository.GetByIdAsync(forumId);
                if (forum == null) return;

                var postCount = await _postRepository.GetCountByForumIdAsync(forumId);
                var lastPost = await _postRepository.GetLastPostByForumIdAsync(forumId);

                forum.PostCount = postCount;
                forum.LastPostAt = lastPost?.CreatedAt;

                _forumRepository.Update(forum);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新論壇統計失敗: {ForumId}", forumId);
            }
        }

        private async Task UpdatePostStatistics(int postId)
        {
            try
            {
                var post = await _postRepository.GetByIdAsync(postId);
                if (post == null) return;

                var replyCount = await _postReplyRepository.GetCountByPostIdAsync(postId);
                var lastReply = await _postReplyRepository.GetLastReplyByPostIdAsync(postId);

                post.ReplyCount = replyCount;
                post.LastReplyAt = lastReply?.CreatedAt;

                _postRepository.Update(post);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "更新貼文統計失敗: {PostId}", postId);
            }
        }
    }
}