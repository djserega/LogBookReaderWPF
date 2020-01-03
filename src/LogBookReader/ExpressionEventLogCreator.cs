using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace LogBookReader
{
    internal class ExpressionEventLogCreator
    {
        private Expression _result;
        private readonly ParameterExpression _parameter = Expression.Parameter(typeof(Models.EventLog));

        internal bool CommentIsFilled { get; set; }

        internal void AddExpression(string field)
        {
            Expression resultExpression = null;

            if (field == "Comment")
            {
                if (CommentIsFilled)
                    resultExpression = Expression.NotEqual(Expression.Property(_parameter, "Comment"), Expression.Constant(""));
            }

            SetResultExpression(resultExpression);
        }

        internal void AddExpression<T>(ObservableCollection<T> listData, string field) where T : IFilters.IFilterBase
        {
            Expression resultExpression = null;

            int countCheckedElement = listData.Count(f => f.IsChecked);
            if (countCheckedElement > 0 && countCheckedElement != listData.Count)
            {
                foreach (T item in listData.Where(f => f.IsChecked))
                {
                    Expression currentExpression = Expression.Equal(Expression.Property(_parameter, field), Expression.Constant(item.Code));

                    if (resultExpression == null)
                        resultExpression = currentExpression;
                    else
                        resultExpression = Expression.Or(resultExpression, currentExpression);
                }
            }

            SetResultExpression(resultExpression);
        }

        private void SetResultExpression(Expression resultExpression)
        {
            if (resultExpression == null)
                return;

            if (_result == null)
                _result = resultExpression;
            else
                _result = Expression.AndAlso(_result, resultExpression);
        }

        internal Expression<Func<Models.EventLog, bool>> GetResult()
        {
            if (_result == null)
                return null;

            var expression = Expression.Lambda<Func<Models.EventLog, bool>>(_result, _parameter);

            return expression;
        }
    }
}
