using LogBookReader.Additions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace LogBookReader
{
    internal class ExpressionEventLogCreator
    {
        private Expression _result;
        private readonly ParameterExpression _parameter = Expression.Parameter(typeof(Models.EventLog));

        internal bool CommentIsFilled { get; set; }

        internal void FillExpression(ViewModel.PropertyFilters propertyFiltersViewModel, TimeSpan startPeriodTime, TimeSpan endPeriodTime)
        {
            AddExpression<Filters.FilterAppCodes>(propertyFiltersViewModel.FilterAppCodes, "AppCode");
            AddExpression<Filters.FilterComputerCodes>(propertyFiltersViewModel.FilterComputerCodes, "ComputerCode");
            AddExpression<Filters.FilterEventCodes>(propertyFiltersViewModel.FilterEventCodes, "EventCode");
            AddExpression<Filters.FilterUserCodes>(propertyFiltersViewModel.FilterUserCodes, "UserCode");

            if (propertyFiltersViewModel.StartPeriodDate.Date != new DateTime(1, 1, 1))
            {
                AddExpression("Date",
                              ComparsionType.GreaterThanOrEqual,
                              GetDateSQLite(propertyFiltersViewModel.StartPeriodDate.Date, startPeriodTime));
            }

            if (propertyFiltersViewModel.EndPeriodDate.Date != new DateTime(1, 1, 1))
            {
                AddExpression("Date",
                              ComparsionType.LessThanOrEqual,
                              GetDateSQLite(propertyFiltersViewModel.EndPeriodDate.Date, endPeriodTime));
            }

            if (CommentIsFilled)
                AddExpression("Comment", ComparsionType.NotEqual, "");


        }

        internal void AddExpression(string field, ComparsionType comparsionType, long rightValue)
            => AddExpression(field, comparsionType, Expression.Constant(rightValue));

        internal void AddExpression(string field, ComparsionType comparsionType, DateTime rightValue)
            => AddExpression(field, comparsionType, Expression.Constant(rightValue));

        internal void AddExpression(string field, ComparsionType comparsionType, string rightValue)
            => AddExpression(field, comparsionType, Expression.Constant(rightValue));

        internal void AddExpression(string field, ComparsionType comparsionType, Expression rightValue)
        {
            Expression resultExpression = null;

            Expression leftValue = Expression.Property(_parameter, field);

            switch (comparsionType)
            {
                case ComparsionType.Equal:
                    resultExpression = Expression.Equal(leftValue, rightValue);
                    break;
                case ComparsionType.NotEqual:
                    resultExpression = Expression.NotEqual(leftValue, rightValue);
                    break;
                case ComparsionType.LessThanOrEqual:
                    resultExpression = Expression.LessThanOrEqual(leftValue, rightValue);
                    break;
                case ComparsionType.GreaterThanOrEqual:
                    resultExpression = Expression.GreaterThanOrEqual(leftValue, rightValue);
                    break;
                default:
                    throw new NotImplementedException($"Реализация сравнения {comparsionType} не реализована.");
            }

            SetResultExpression(resultExpression);
        }

        internal void AddExpression<T>(List<T> listData, string field) where T : IFilters.IFilterBase
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

        internal void AddExpression<T>(ICollectionView listData, string field) where T : IFilters.IFilterBase
        {
            AddExpression(listData.Cast<T>().ToList(), field);
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

        private static long GetDateSQLite(DateTime date, TimeSpan time)
        {
            long dateSqlite = date.DateToSQLite();
            dateSqlite += (long)time.TotalMilliseconds * 10;

            return dateSqlite;
        }

    }
}
