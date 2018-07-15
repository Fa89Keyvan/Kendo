using System.Collections.Generic;

namespace System.Linq
{
    public static class LinqExtentions
    {
        public static IOrderedEnumerable<TSource> OrderBy<TSource>
            (this IEnumerable<TSource> source,Sort sort)
        {
            sort = sort ?? new Sort() { Field = string.Empty, Direction = SortDirection.AscSort };

            if (string.IsNullOrEmpty(sort.Field))
                return source.OrderBy(x => x.GetType().GetProperties()[0].GetValue(x, null));

            return (sort.Direction == SortDirection.AscSort)
                ? source.OrderBy(x => x.GetType().GetProperty(sort.Field).GetValue(x, null))
                : source.OrderByDescending(x => x.GetType().GetProperty(sort.Field).GetValue(x, null));
        }

        
    }
    public class SortDirection
    {
        private static SortDirection _ascSort = new SortDirection() { Direction = "ASC" };
        private static SortDirection _dscSort = new SortDirection() { Direction = "DSC" };

        public static SortDirection AscSort => _ascSort;
        public static SortDirection DscSort => _dscSort;

        private SortDirection() { }

        public string Direction { get; private set; }

        public static bool operator ==(SortDirection right, SortDirection left)
        {
            return right.Direction == left.Direction;
        }
        public static bool operator !=(SortDirection right, SortDirection left)
        {
            return right.Direction != left.Direction;
        }

        public override bool Equals(object obj)
        {
            var direction = obj as SortDirection;
            return direction != null &&
                   Direction == direction.Direction;
        }

        public override string ToString()
        {
            return base.ToString();
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
    public class Sort
    {
        public string Field { get; set; }
        public SortDirection Direction { get; set; }

    }
}