namespace HotChocolateServer
{
    
    public class Mutation
    {
        /*private readonly DataStore _db;

        public Mutation(DataStore db)
        {
            _db = db;
        }

        public async Task<Post> SavePost(Post post)
        {
            if (post.Id != null)
                _db.Update(post);
            else
                _db.Add(post);
            await _db.SaveChangesAsync();
            return post;
        }

        public async Task<bool> DeletePost(Guid id)
        {
            var post = await _db.Posts.FindAsync(id);
            _db.Remove(post);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<Comment> SaveComment(Comment comment)
        {
            if (comment.Id != null)
                _db.Update(comment);
            else
                _db.Add(comment);
            await _db.SaveChangesAsync();
            return comment;
        }
        public async Task<bool> DeleteComment(Guid id)
        {
            var comment = _db.Comments.FindAsync(id);
            _db.Remove(comment);
            await _db.SaveChangesAsync();
            return true;
        }*/
    }
}