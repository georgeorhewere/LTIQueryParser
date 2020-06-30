using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Dynamic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LTIQueryParser
{
    public class SanitizeFilter
    {
        private Hashtable queryOperators = new Hashtable();
        private Hashtable logicalOperators = new Hashtable();
        List<QueryFilterDTO> filterParams = new List<QueryFilterDTO>();
        bool hasLogical;
        bool isAndOp;
        public SanitizeFilter()
        {
            queryOperators.Add("=", 1);
            queryOperators.Add("!=", 2);
            queryOperators.Add(">", 3);
            queryOperators.Add(">=", 4);
            queryOperators.Add("<", 5);
            queryOperators.Add("<=", 6);
            queryOperators.Add("~", 7);
        }
        public string Fields { get; set; }
        public string Filter { get; set; }

        public void processFilter()
        {
            Console.WriteLine($"Process Filter");
            Console.WriteLine($"{Filter}");
            //split logical;
            if (!string.IsNullOrEmpty(Filter))
            {                
             
                SearchFilterParser parser = new SearchFilterParser(Filter);
                var predicate = parser.GetSearchFilter();              
                var result = ResourceList.GetList().Where(predicate).ToList();
                
                foreach(var item in result)
                {
                    Console.WriteLine($"Name : {item.name }");
                    Console.WriteLine($"Description:  {item.description} ");
                    Console.WriteLine($"Url: {item.url}");
                    Console.WriteLine($"");
                }


            }
            else
            {
                Console.WriteLine($"No filter found");
            }

        }

        private void checkForLogical(string filter)
        {
            string AND_Operator = " AND ";
            string OR_Operator = " OR ";
            hasLogical = filter.ToUpper().Contains(AND_Operator) || filter.ToUpper().Contains(OR_Operator);            
            if (hasLogical)
            {
                
                //check for single use
                Console.WriteLine("Multiple Operator with standard logical spacing");            
                if (filter.ToUpper().Contains(AND_Operator) && filter.ToUpper().Contains(OR_Operator))
                {
                    Console.WriteLine("Use only one logical operator in the filter");

                }
                else
                {
                    var usesAnd = isAndOp = filter.Contains(AND_Operator);
                    var _defaultOperator = usesAnd ? AND_Operator : OR_Operator;
                    var filterList = filter.Split(new string[] { _defaultOperator },StringSplitOptions.RemoveEmptyEntries);

                    foreach(var filt in filterList)
                    {
                        checkForOperator(filt);
                    }

                    

                }
            }
            else
            {
                ////check for moe than one operator
                var numOfOperators = queryOperators.Keys.Cast<string>().ToArray();
                int count = 0;
                var splitOperators = filter.Split((string[])numOfOperators, StringSplitOptions.RemoveEmptyEntries);
                count +=  splitOperators.Length;               
                if (count.Equals(2))
                {
                    Console.WriteLine("Single Operator");
                    checkForOperator(filter);
                }
                else
                {
                    //raise error for multiple
                    Console.WriteLine("Multiple Operator without standard logical spacing" );
                }

            }

        }

        private void checkForOperator(string filter)
        {
            if (queryOperators.Keys.Cast<string>().Any(filter.Contains))
            {
                var filterParam = new QueryFilterDTO();
                var tt = queryOperators.Keys.Cast<string>().Where(filter.Contains).ToArray();
                var splitOnOperator = filter.Split(tt, StringSplitOptions.RemoveEmptyEntries);

                if (splitOnOperator.Count() == 2)
                {
                    filterParam.Field = splitOnOperator[0];
                    if (splitOnOperator[1].Last().Equals('\'') && splitOnOperator[1].First().Equals('\''))
                    {
                        filterParam.Value = splitOnOperator[1].Replace("'", "").Trim().ToUpper();
                        filterParam.SearchPredicate = tt.First();
                       // Console.WriteLine($"Found operator Field { filterParam.Field } ,Operator { filterParam.QOperator }, Value { filterParam.Value } ");

                        filterParams.Add(filterParam);
                    }
                    else
                    {
                        Console.WriteLine($"Values should be defined with quotes '{splitOnOperator[1]}' ");
                    }
                }
                else
                {
                    Console.WriteLine($"Unable to resolve filter field and value on {Filter}");
                }
               
                

            }
            else
            {
                Console.WriteLine($"No operator found");
            }
        }


        private Expression ExpressionBuilder(QueryFilterDTO filter, ParameterExpression pe)
        {
            
            var member = Expression.Property(pe, filter.Field);
            var propertyType = ((PropertyInfo)member.Member).PropertyType;
            var converter = TypeDescriptor.GetConverter(propertyType);
            var propertyValue = converter.ConvertFromInvariantString(filter.Value.Replace("'", "").Trim().ToUpper());
            var expressionConstant = Expression.Constant(propertyValue);
            var propertyField = Expression.Call(member, "ToUpper", null);
            Expression body;
            //case operators

            var op = (ConditionalOperators)queryOperators[filter.SearchPredicate];
            switch (op)
            {
                case ConditionalOperators.Equal:
                    body = Expression.Equal(propertyField, expressionConstant);
                    break;
                case ConditionalOperators.NotEqual:
                    body = Expression.NotEqual(propertyField, expressionConstant);
                    break;
                case ConditionalOperators.GreaterThan:
                    body = Expression.GreaterThan(propertyField, expressionConstant);
                    break;
                case ConditionalOperators.GreaterThanOrEqual:
                    body = Expression.GreaterThanOrEqual(propertyField, expressionConstant);
                    break;
                case ConditionalOperators.LessThan:
                    body = Expression.LessThan(propertyField, expressionConstant);
                    break;
                case ConditionalOperators.LessThanOrEqual:
                    body = Expression.LessThanOrEqual(propertyField, expressionConstant);
                    break;
                default:
                    MethodInfo contains = typeof(string).GetMethod("Contains");
                    body = Expression.Call(propertyField, contains, expressionConstant);
                        break;
            }




            return body;
            //Console.WriteLine(body.ToString());
            //var qry = Expression.Lambda<Func<ResourceSet, bool>>(body, pe);
            //var result = ResourceList.GetList().Where(qry).ToList();

        }

    }


    public class QueryFilterDTO
    {
        public string Field { get; set; }
        public string SearchPredicate { get; set; }
        public string Value { get; set; }
        public Expression LtiClause { get; set; }

    }

   
}
