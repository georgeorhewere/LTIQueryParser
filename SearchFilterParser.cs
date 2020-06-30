using LTIQueryParser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LTIQueryParser
{
    public class SearchFilterParser
    {
       
        
        private IEnumerable<QueryFilterDTO> filterExpressions;
        private ParameterExpression expressionType = Expression.Parameter(typeof(ResourceSet));
        private FilterBase baseFilter;

        public SearchFilterParser(string filter_param)
        {
            if (!string.IsNullOrEmpty(filter_param))
            {
                baseFilter = new FilterBase(filter_param);                
                filterExpressions = QueryClause.GetClauses(baseFilter, expressionType);       

            }
            else
            {
                Console.WriteLine($"No filter found");
            }

        }

        public Expression<Func<ResourceSet, bool>> GetSearchFilter()
        {
            Expression predicateBody = filterExpressions.First().LtiClause;
            if (baseFilter.UsesCondition)
            {
                foreach (var exp in filterExpressions.Skip(1))
                {
                    if (baseFilter.UsesAndCondition)
                    {
                        predicateBody = Expression.And(predicateBody, exp.LtiClause);
                    }
                    else
                    {
                        predicateBody = Expression.Or(predicateBody, exp.LtiClause);
                    }

                }

            }
            return Expression.Lambda<Func<ResourceSet, bool>>(predicateBody, expressionType);          
        }


    }
}
