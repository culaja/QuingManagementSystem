﻿using System;
using System.Collections.Generic;
using Common;
using Common.Commanding;

namespace InMemory
{
    public sealed class InMemoryRepository<T> : IRepository<T> where T: AggregateRoot
    {
        private readonly Dictionary<Guid, T> _cache = new Dictionary<Guid, T>();

        public T AddNew(T aggregateRoot)
        {
            if (_cache.ContainsKey(aggregateRoot.Id))
            {
                throw new AggregateRootWithSameIdAlreadyExists<T>(aggregateRoot.Id);
            }
            
            _cache.Add(aggregateRoot.Id, aggregateRoot);
            return aggregateRoot;
        }

        public T Borrow(Guid aggregateRootId, Func<T, T> transformer) =>
            transformer(ReadAggregateOrThrowIfDoesntExist(aggregateRootId));

        private T ReadAggregateOrThrowIfDoesntExist(Guid aggregateRootId)
        {
            if (!_cache.TryGetValue(aggregateRootId, out var aggregateRoot))
            {
                throw new AggregateRootDoesntExistInRepositoryException<T>(aggregateRootId);
            }

            return aggregateRoot;
        }
        
        public T BorrowEachFor(ISpecification<T> specification, Func<T, T> transformer)
        {
            specification.IsSatisfied().Parameters[0].
        }
    }
}