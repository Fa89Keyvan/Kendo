using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Kendo.Models.Kendo
{
    public class GridRequest
    {
        //take=20&skip=0&page=1&pageSize=20&sort%5b0%5d%5bfield%5d=Name&sort%5b0%5d%5bdir%5d=asc
        private NameValueCollection _queryString;
        private List<Sort> _sorts;

        public GridRequest(NameValueCollection queryString)
        {
            _queryString = queryString;
            _sorts = new List<Sort>();
        }
        
        public int take => Convert.ToInt32(_queryString["take"]);
        public int skip => Convert.ToInt32(_queryString["skip"]);
        public int pageSize => Convert.ToInt32(_queryString["pageSize"]);
        public int page => Convert.ToInt32(_queryString["page"]);
        public List<Sort> Sorts
        {
            get
            {
                if (_sorts == null || !_sorts.Any())
                    extractSort();

                return _sorts;
            }
        }

        private void extractSort()
        {
            if (_queryString == null || !_queryString.AllKeys.Any(k => k.StartsWith("sort")))
                return;

            //var countsOfSort = _queryString.AllKeys.Where(k => k.StartsWith("sort")).Count() / 2;
            var i = 0;
            while (true)
            {
                var sort = extractSort(i);
                if (sort == null)
                    break;
                _sorts.Add(sort);
                i++;
            }

        }
        private Sort extractSort(int i)
        {
            try
            {
                var field     = _queryString[$"sort[{i}][field]"];
                var direction = (_queryString[$"sort[{i}][dir]"] == "asc") ? SortDirection.AscSort : SortDirection.DscSort;
                return (field == null) ? null : new Sort() { Field = field, Direction = direction };

            }
            catch (Exception)
            {
                return null;
            }
        }
        
    }
    
}