using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTIQueryParser
{
    public class SearchOperators
    {
        private static Hashtable queryOperators;

        public static Hashtable Instance
        {
            get
            {
                if(queryOperators == null)
                {
                    initializeOperators();
                }
                return queryOperators;
            }
        }


        private static void initializeOperators()
        {
            queryOperators = new Hashtable();
            queryOperators.Add("=", ConditionalOperators.Equal);
            queryOperators.Add("!=", ConditionalOperators.NotEqual);
            queryOperators.Add(">", ConditionalOperators.GreaterThan);
            queryOperators.Add(">=", ConditionalOperators.GreaterThanOrEqual);
            queryOperators.Add("<", ConditionalOperators.LessThan);
            queryOperators.Add("<=", ConditionalOperators.LessThanOrEqual);
            queryOperators.Add("~", ConditionalOperators.Contains);
        }
    }
}
