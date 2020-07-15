using System;
using System.Collections.Generic;
using System.Linq;
using HotChocolate.Types;
using HotChocolate.Types.Relay;

namespace HotChocolateServer
{   
    public class Post
    {
        public Guid? Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
    }
    public class Comment
    {
        public Guid? Id { get; set; }
        public string PosterName { get; set; }
        public string Content { get; set; }
        public Guid PostId { get; set; }
    }
    
    public class PostType : ObjectType<Post>
    {
    }

    public class CommentType : ObjectType<Comment>
    {
    }

    public class Query
    {
        private readonly DataStore _db;

        public Query(DataStore db)
        {
            _db = db;
        }

        [UsePaging(SchemaType = typeof(PostType))]
        [UseFiltering]
        public IQueryable<Post> Posts => _db.Posts;

    }
}