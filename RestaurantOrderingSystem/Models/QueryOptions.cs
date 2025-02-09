using System.Linq.Expressions;

namespace RestaurantOrderingSystem.Models
{
    public class QueryOptions<T> where T : class
    {
        //This is an expression (a LINQ function) that defines how to sort the query results.
        public Expression<Func<T, Object>> OrderBy { get; set; } = null!;

        //This allows defining a filter condition dynamically.
        public Expression<Func<T, bool>> Where { get; set; } = null!;




        private string[] includes = Array.Empty<string>();

        public string Includes
        {
            set => includes = value.Replace(" ", "").Split(',');
        }

        public string[] GetIncludes() => includes;




        public bool HasWhere => Where != null; //Returns true if a filter condition is set.
        public bool HasOrderBy => OrderBy != null; //HasOrderBy: Returns true if a sorting order is set.
    }
}