using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTIQueryParser
{
    public class SanitizeFilter
    {
        private Hashtable queryOperators = new Hashtable();

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
                //check for operators          
                if (queryOperators.Keys.Cast<string>().Any(Filter.Contains))
                {
                    var filterParam = new QueryFilterDTO();
                    var tt = queryOperators.Keys.Cast<string>().Where(Filter.Contains).ToArray();
                    var splitOnOperator = Filter.Split(tt, StringSplitOptions.RemoveEmptyEntries);
                    
                    if (splitOnOperator.Count() == 2)
                    {
                        filterParam.Field = splitOnOperator[0];
                        if (splitOnOperator[1].Last().Equals('\'') && splitOnOperator[1].First().Equals('\''))
                        {
                            filterParam.Value = splitOnOperator[1];
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
                    filterParam.QOperator = tt.First();
                    Console.WriteLine($"Found operator Field { filterParam.Field } ,Operator { filterParam.QOperator }, Value { filterParam.Value } ");

                }
                else
                {
                    Console.WriteLine($"No operator found");
                }
            }
            else
            {
                Console.WriteLine($"No filter found");
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
