using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LTIQueryParser
{
    public class FilterBase
    {
        private string AND_Operator = " AND ";
        private string OR_Operator = " OR ";
        //var _defaultOperator = usesAnd ? AND_Operator : OR_Operator;

        private string filterQuery;
        public bool UsesCondition { 
            get { 
                return filterQuery.ToUpper().Contains(AND_Operator) || filterQuery.ToUpper().Contains(OR_Operator); 
            } 
        }
        public bool UsesAndCondition { get { return filterQuery.Contains(AND_Operator); } }
        public bool HasMultipleLogical { get { return filterQuery.ToUpper().Contains(AND_Operator) && filterQuery.ToUpper().Contains(OR_Operator); } }
        public FilterBase(string filterQry)
        {
            filterQuery = filterQry;
        }
        public string FilterQuery { get { return filterQuery; } }
        public string DefaultLogicalPredicate { get { return UsesAndCondition ? AND_Operator : OR_Operator;  } }


    }
}
