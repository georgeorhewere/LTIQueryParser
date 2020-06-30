using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        public SanitizeFilter()
        {
            queryOperators.Add("=", "=");
            queryOperators.Add("!=", "<>");
            queryOperators.Add(">", ">");
            queryOperators.Add(">=", ">=");
            queryOperators.Add("<", "<");
            queryOperators.Add("<=", "<=");
            queryOperators.Add("~", "LIKE");
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
                // check for logical
                checkForLogical(Filter);
                //check for operators          
                //checkForOperator(Filter);
                foreach(var p in filterParams)
                {
                    Console.WriteLine($"Field:{p.Field} Operator:{p.QOperator} Value: {p.Value}" );
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
            var hasLogical = filter.ToUpper().Contains(AND_Operator) || filter.ToUpper().Contains(OR_Operator);            
            if (hasLogical)
            {
                
                //check for single use
                Console.WriteLine("Multiple Operator with standard logical spacing");
                var startIndex = 0;
                var count = 0;

                if (filter.ToUpper().Contains(AND_Operator) && filter.ToUpper().Contains(OR_Operator))
                {
                    Console.WriteLine("Use only one logical operator in the filter");

                }
                else
                {
                    var usesAnd = filter.Contains(AND_Operator);
                    var _defaultOperator = usesAnd ? AND_Operator : OR_Operator;

                    var filterList = filter.Split(new string[] { _defaultOperator },StringSplitOptions.RemoveEmptyEntries);

                    foreach(var filt in filterList)
                    {
                        checkForOperator(filt);
                    }

                    //while (startIndex >= 0)
                    //{
                    //    //Console.WriteLine($"start index {startIndex}");
                    //    var indexes = filter.IndexOf(_defaultOperator, startIndex);                                             
                    //    logicalOperators.Add(indexes, _defaultOperator);

                    //    if (indexes < 0)
                    //        break;
                        
                    //    startIndex = indexes + 1;
                    //}

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
                        filterParam.Value = splitOnOperator[1];
                        filterParam.QOperator = tt.First();
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
    }


    public class QueryFilterDTO
    {
        public string Field { get; set; }
        public string QOperator { get; set; }
        public string Value { get; set; }

    }
}
