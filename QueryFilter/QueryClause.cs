using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace LTIQueryParser
{
    public class QueryClause
    {
        private static ParameterExpression _expressionType;
        private static List<QueryFilterDTO> queryFilters;
       

        public static IEnumerable<QueryFilterDTO> GetClauses(FilterBase filter, ParameterExpression expressionType)
        {
            _expressionType = expressionType;
            queryFilters = new List<QueryFilterDTO>();
            if (filter.UsesCondition)
            {
                //process multiple
                Console.WriteLine("Multiple Operator with standard logical spacing");
                if (filter.HasMultipleLogical)
                {
                    Console.WriteLine("Use only one logical operator in the filter");

                }
                else
                {
                    
                    var filterList = filter.FilterQuery.Split(new string[] { filter.DefaultLogicalPredicate }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var filt in filterList)
                    {
                        ParseFilter(filt);
                    }
                }

            }
            else
            {

                ////check for moe than one operator
                var numOfOperators = SearchOperators.Instance.Keys.Cast<string>().ToArray();
                int count = 0;
                var splitOperators = filter.FilterQuery.Split((string[])numOfOperators, StringSplitOptions.RemoveEmptyEntries);
                count += splitOperators.Length;
                if (count.Equals(2))
                {                 
                    // process single filter
                    ParseFilter(filter.FilterQuery);
                }
                else
                {
                    //raise error for multiple
                    Console.WriteLine("Multiple Operator without standard logical spacing");
                }
              
            }
            return queryFilters;

        }


        private static void ParseFilter(string filterClause)
        {          

            if (SearchOperators.Instance.Keys.Cast<string>().Any(filterClause.Contains))
            {
                var queryFilter = new QueryFilterDTO();
                var filterPredicate = SearchOperators.Instance.Keys.Cast<string>().Where(filterClause.Contains).ToArray();
                var splitOnOperator = filterClause.Split(filterPredicate, StringSplitOptions.RemoveEmptyEntries);


                if (splitOnOperator.Count() == 2)
                {
                    queryFilter.Field = splitOnOperator[0];
                    if (splitOnOperator[1].Last().Equals('\'') && splitOnOperator[1].First().Equals('\''))
                    {
                        queryFilter.Value = splitOnOperator[1].Replace("'", "").Trim().ToUpper();
                        queryFilter.SearchPredicate = filterPredicate.First();
                        QueryClauseBuilder.BuildClause(ref queryFilter, ref _expressionType);                        
                        queryFilters.Add(queryFilter);
                    }
                    else
                    {
                        Console.WriteLine($"Values should be defined with quotes '{splitOnOperator[1]}' ");
                    }
                }
                else
                {
                    Console.WriteLine($"Unable to resolve filter field and value on {filterClause}");
                }



            }
            else
            {
                Console.WriteLine($"No operator found");
            }
        }

    }
}
