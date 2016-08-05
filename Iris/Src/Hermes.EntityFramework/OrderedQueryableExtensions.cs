﻿using System;
using System.Collections.Generic;
using System.Linq;

using Hermes.EntityFramework.Queries;
using Hermes.Persistence;

namespace Hermes.EntityFramework
{
    public static class OrderedQueryableExtensions
    {
        public static Hermes.Queries.PagedResult<TEntity> ToPagedResult<TEntity>(this IOrderedQueryable<TEntity> queryExpression, int pageNumber, int pageSize)
        {
            List<TEntity> results = queryExpression
                .Skip(NumberOfRecordsToSkip(pageNumber, pageSize))
                .Take(pageSize)
                .ToList();

            return new Hermes.Queries.PagedResult<TEntity>(results, pageNumber, pageSize, queryExpression.Count());
        }

        public static Hermes.Queries.PagedResult<TResult> ToPagedResult<TEntity, TResult>(this IOrderedQueryable<TEntity> queryExpression, Converter<TEntity, TResult> Converter, int pageNumber, int pageSize)
        {
            List<TResult> results = queryExpression
                .Skip(NumberOfRecordsToSkip(pageNumber, pageSize))
                .Take(pageSize)
                .ToList()
                .ConvertAll(Converter);

            return new Hermes.Queries.PagedResult<TResult>(results, pageNumber, pageSize, queryExpression.Count());
        }

        private static int NumberOfRecordsToSkip(int pageNumber, int selectSize)
        {
            Mandate.ParameterCondition(pageNumber > 0, "pageNumber", "Page number must be larger than zero.");
            int adjustedPageNumber = pageNumber - 1; //we adjust for the fact that sql server starts at page 0

            return selectSize * adjustedPageNumber;
        }
    }
}
