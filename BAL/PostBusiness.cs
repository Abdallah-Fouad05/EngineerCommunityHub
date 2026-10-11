using DAL;
using DAL.DTOs;

namespace BAL
{
    public class PostBusiness
    {
        
        public static async Task<List<PostDTO>> GetAllPosts()
        {
            try
            {
                return await PostData.GetAllPosts();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all posts.", ex);
            }
        }

        public static async Task<List<PostDTO>> GetAllPostsByUserId(int id)
        {
            try
            {
                return await PostData.GetPostsByUserId(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while fetching all posts.", ex);
            }
        }

        // ============================
        // GET BY ID
        // ============================
        public static async Task<PostDTO?> GetPostById(int id)
        {
            try
            {
                return await PostData.GetPostById(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while fetching post with ID {id}.", ex);
            }
        }

        // ============================
        // CREATE
        // ============================
        public static async Task<(bool success, int postId)> CreatePost(Post_Created post)
        {
            try
            {
                return await PostData.CreatePost(post);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Service error while creating post.", ex);
            }
        }

        // ============================
        // UPDATE
        // ============================
        public static async Task<int> UpdatePost(Post_Updated post)
        {
            try
            {
                return await PostData.UpdatePost(post);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while updating post with ID {post.PostId}.", ex);
            }
        }

        // ============================
        // DELETE
        // ============================
        public static async Task<int> DeletePost(int postId)
        {
            try
            {
                return await PostData.DeletePost(postId);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Service error while deleting post with ID {postId}.", ex);
            }
        }
    }
}