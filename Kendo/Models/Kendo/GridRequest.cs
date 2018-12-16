using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
              

        public WhereClouse ToStatement()
        {
            var builder = new StringBuilder();

            if (!filters.Any())
                return null;
            if (logic == null)
                return null;

            var whereClouse = new WhereClouse();
            string currentLogic = this.logic;
            for (int i = 0; i < filters.Length; i++)
            {
                if(currentLogic != null)
                {
                    builder.Append(" ( ");
                    if(filters[i].@operator != null)
                    {
                        builder.Append($" {filters[i].field} {ToSqlOperator(filters[i].@operator)} @{filters[i].field} ");
                        whereClouse.AddParamter($"@{filters[i].field}", filters[i].value);
                    }
                    else
                    {
                        if (i > 0) currentLogic = filters[i].logic;
                        if (filters[i].filters.Length >= 1)
                        {
                            builder.Append($" {filters[i].filters[0].field} {ToSqlOperator(filters[i].filters[0].@operator)} @{filters[i].filters[0].field}1 ");
                            whereClouse.AddParamter($"@{filters[i].filters[0].field}1", filters[i].filters[0].value);
                        }
                        if (filters[i].filters.Length == 2)
                        {
                            builder.Append($" {currentLogic} {filters[i].filters[1].field} {ToSqlOperator(filters[i].filters[1].@operator)} @{filters[i].filters[1].field}2 ");
                            whereClouse.AddParamter($"@{filters[i].filters[1].field}2", filters[i].filters[1].value);
                        }
                    }
                    builder.Append(" ) ");
                }
                else
                {
                    builder.Append($"( {filters[i].field} {ToSqlOperator(filters[i].@operator)} @{filters[i].field} )");
                    whereClouse.AddParamter($"@{filters[i].field}", filters[i].value);
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

            whereClouse.SqlStatement = builder.ToString();
            return whereClouse;
        }
        
        public string ToSqlOperator(string kendoOperator)
        {
            if (SimpleOperators.ContainsKey(kendoOperator))
                return SimpleOperators[kendoOperator];
            if (TextualOperators.Contains(kendoOperator))
                return "LIKE";

            throw new InvalidCastException($"invalid or not supported operator {kendoOperator}");
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
        private enum OperatorTypes
        {
            Simple,
            Textual
        }

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
        
    }

    public class WhereClouse
    {
        public WhereClouse()
        {
            Paramters = new List<KeyValuePair<string, object>>();
        }
        public string SqlStatement { get; set; }
        public List<KeyValuePair<string, object>> Paramters { get; set; }
        public void AddParamter(string key, object value) => Paramters.Add(new KeyValuePair<string, object>(key, value));
    }
}