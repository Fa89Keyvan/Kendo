using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kendo.Models.Kendo
{
    public class DataSourceRequest
    {
        public int? take     { get; set; }
        public int? skip     { get; set; }
        public int? pageSize { get; set; }
        public int? page     { get; set; }
        public filter filter { get; set; }
        public KendoSort[] sort { get; set; }

    }
    public class KendoSort
    {
        public string field { get; set; }
        public string dir { get; set; }
    }
    public class filter
    {
        #region ' Kendo '

        public string logic { get; set; }
        public string field { get; set; }
        public string value { get; set; }
        public string @operator { get; set; }
        public filter[] filters { get; set; } 

        #endregion

        

        public string ToStatement()
        {
            var builder = new StringBuilder();

            if (!filters.Any())
                return String.Empty;

            if (logic == null)
                return String.Empty;

            for (int i = 0; i < filters.Length; i++)
            {
                if(filters[i].logic != null)
                {
                    builder.Append(" ( ");
                    if (filters[i].filters.Length >= 1)
                        builder.Append($" {filters[i].filters[0].field} {filters[i].filters[0].@operator} {filters[i].filters[0].value} ");
                    if (filters[i].filters.Length == 2)
                        builder.Append($" {filters[i].logic} {filters[i].filters[1].field} {filters[i].filters[1].@operator} {filters[i].filters[1].value} ");
                    builder.Append(" ) ");
                }
                else
                {
                    builder.Append($"( {filters[i].field} {filters[i].@operator} {filters[i].value} )");
                }
                if(i < filters.Length - 1)
                {
                    if(filters[i].logic != null)
                        builder.Append(filters[i].logic);
                    else
                    {
                        if(i == 0)
                            builder.Append(logic);
                    }
                }

            }

            return builder.ToString();

        }

        #region ' Constants '

        private static readonly Dictionary<string, string> SimpleOperators = new Dictionary<string, string>
        {
            { "eq" , "="  },
            { "lt" , "<"  },
            { "gt" , ">"  },
            { "lte", "<=" },
            { "gte", ">=" },
            { "neq", "!=" }
        };

        private static readonly string[] TextualOperators = new string[]
        {
            "startswith","contains","doesnotcontain","endswith"
        };

        #endregion
        private OperatorTypes OperatorType
        {
            get
            {
                if (SimpleOperators.ContainsKey(@operator))
                    return OperatorTypes.Simple;
                if (TextualOperators.Contains(@operator))
                    return OperatorTypes.Textual;

                throw new InvalidCastException("invalid or not supported operator");
            }
        }
        private enum OperatorTypes
        {
            Simple,
            Textual
        }
    }
}