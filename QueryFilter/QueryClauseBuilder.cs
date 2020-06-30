using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LTIQueryParser
{
    public class QueryClauseBuilder
    {

        public static void BuildClause(ref QueryFilterDTO filterInstance, ref ParameterExpression parameter)
        {
            var member = Expression.Property(parameter, filterInstance.Field);
            var propertyType = ((PropertyInfo)member.Member).PropertyType;
            var converter = TypeDescriptor.GetConverter(propertyType);
            var propertyValue = converter.ConvertFromInvariantString(filterInstance.Value);

            var expressionConstant = Expression.Constant(propertyValue);
            var propertyField = Expression.Call(member, "ToUpper", null);
            filterInstance.LtiClause = getConditionClause(filterInstance, expressionConstant, propertyField);

        }

        private static Expression getConditionClause(QueryFilterDTO filterInstance, ConstantExpression expressionConstant, MethodCallExpression propertyField)
        {
            Expression clause;
            var filterPredicate = (ConditionalOperators)SearchOperators.Instance[filterInstance.SearchPredicate];
            switch (filterPredicate)
            {
                case ConditionalOperators.Equal:
                    clause = Expression.Equal(propertyField, expressionConstant);
                    break;
                case ConditionalOperators.NotEqual:
                    clause = Expression.NotEqual(propertyField, expressionConstant);
                    break;
                case ConditionalOperators.GreaterThan:
                    clause = Expression.GreaterThan(propertyField, expressionConstant);
                    break;
                case ConditionalOperators.GreaterThanOrEqual:
                    clause = Expression.GreaterThanOrEqual(propertyField, expressionConstant);
                    break;
                case ConditionalOperators.LessThan:
                    clause = Expression.LessThan(propertyField, expressionConstant);
                    break;
                case ConditionalOperators.LessThanOrEqual:
                    clause = Expression.LessThanOrEqual(propertyField, expressionConstant);
                    break;
                default:
                    MethodInfo contains = typeof(string).GetMethod("Contains");
                    clause = Expression.Call(propertyField, contains, expressionConstant);
                    break;
            }
            return clause;
        }
    }
}
