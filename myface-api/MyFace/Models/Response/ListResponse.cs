using System.Collections.Generic;
using System.Linq;
using MyFace.Models.Database;
using MyFace.Models.Request;

namespace MyFace.Models.Response
{
    public class ListResponse<T>
    {
        private readonly string _path;
        private readonly string _searchTerm;
        
        public IEnumerable<T> Items { get; }
        public int TotalNumberOfItems { get; }
        public int Page { get; }
        public int PageSize { get; }

        public string NextPage
        {
            get
            {
                if (!HasNextPage())
                {
                    return null;
                }
                
                var url = $"/{_path}?page={Page + 1}&pageNumber={PageSize}";
                if (_searchTerm != null)
                {
                    url += $"&search={_searchTerm}";
                }

                return url;
            }
        }

        public string PreviousPage
        {
            get
            {
                if (Page <= 1)
                {
                    return null;
                }
                
                var url = $"/{_path}?page={Page - 1}&pageNumber={PageSize}";
                if (_searchTerm != null)
                {
                    url += $"&search={_searchTerm}";
                }

                return url;
            }
        }

        public ListResponse(SearchRequest search, IEnumerable<T> items, int totalNumberOfItems, string path)
        {
            Items = items;
            TotalNumberOfItems = totalNumberOfItems;
            Page = search.Page;
            PageSize = search.PageSize;
            _path = path;
            _searchTerm = search.Search;
        }
        
        private bool HasNextPage()
        {
            return Page * PageSize < TotalNumberOfItems;
        }
    }

    public class PostListResponse : ListResponse<PostResponse>
    {
        private PostListResponse(SearchRequest search, IEnumerable<PostResponse> items, int totalNumberOfItems) 
            : base(search, items, totalNumberOfItems, "posts") { }

        public static PostListResponse Create(SearchRequest search, IEnumerable<Post> posts, int totalNumberOfItems)
        {
            var postModels = posts.Select(post => new PostResponse(post));
            return new PostListResponse(search, postModels, totalNumberOfItems);
        }
    }
    
    public class UserListResponse : ListResponse<UserResponse>
    {
        private UserListResponse(SearchRequest search, IEnumerable<UserResponse> items, int totalNumberOfItems) 
            : base(search, items, totalNumberOfItems, "users") { }
        
        public static UserListResponse Create(SearchRequest search, IEnumerable<User> users, int totalNumberOfItems)
        {
            var userModels = users.Select(user => new UserResponse(user));
            return new UserListResponse(search, userModels, totalNumberOfItems);
        }
    }
    
    public class InteractionListResponse : ListResponse<InteractionResponse>
    {
        private InteractionListResponse(SearchRequest search, IEnumerable<InteractionResponse> items, int totalNumberOfItems) 
            : base(search, items, totalNumberOfItems, "interactions") { }
        
        public static InteractionListResponse Create(SearchRequest search, IEnumerable<Interaction> interactions, int totalNumberOfItems)
        {
            var interactionModels = interactions.Select(i => new InteractionResponse(i));
            return new InteractionListResponse(search, interactionModels, totalNumberOfItems);
        }
    }
}