using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using Moq;
using Microsoft.Extensions.DependencyInjection;

namespace Utils.ConditionPoints
{
    public class EnumListConditionPoint<T> : ConditionPointBase, IList<T>, IEnumListConditionPoint where T : Enum
    {
        private List<T> internalList;
        public bool IsReadOnly => false;

        public EnumListConditionPoint(ConditionPointKind conditionPointKind) : base(conditionPointKind)
        {
            this.internalList = new List<T>();
        }

        public void GenerateEnumItems()
        {
            foreach (var item in Enum.GetValues(typeof(T)))
            {
                this.Add((T) item);
            }
        }

        public List<IConditionItem> GenerateConditionItems()
        {
            var conditionItems = new List<IConditionItem>();
            var enumType = typeof(T);
            Type stepHandlerType = null;

            if (enumType.HasCustomAttribute<StepHandlerAttribute>())
            {
                var stepHandlerAttribute = enumType.GetCustomAttribute<StepHandlerAttribute>();
                var handlerType = stepHandlerAttribute.HandlerType;

                stepHandlerType = handlerType;
            }

            foreach (var field in EnumUtils.GetFields<T>())
            {
                Type actionPointType = null;
                float weight = -1;
                int orderNumber = -1;
                int nextOrderNumber = -1;
                OrderKind orderKind = OrderKind.Invalid;
                NextKind nextKind = NextKind.Invalid;
                IStepHandler stepHandler = null;
                string expectedResponsePattern = null;
                int minimumResponseBytes =  -1;
                TimeSpan maximumResponseTimeSpan = TimeSpan.Zero;
                var itemName = this.GetItemName();
                var item = EnumUtils.GetValue<T>(field.Name);

                if (stepHandlerType != null)
                {
                    stepHandler = (IStepHandler) this.ServiceProvider.GetService(stepHandlerType);
                    stepHandler.SessionState = (ISessionStateObject)this.ServiceProvider.GetService<ISessionStateObject>();
                }

                if (field.HasCustomAttribute<OrderAttribute>())
                {
                    var orderAttribute = field.GetCustomAttribute<OrderAttribute>();

                    if (orderAttribute.OrderKind == OrderKind.Specific)
                    {
                        orderNumber = orderAttribute.Order;
                    }

                    orderKind = orderAttribute.OrderKind;
                }

                if (field.HasCustomAttribute<ConditionActionAttribute>())
                {
                    var conditionActionAttribute = field.GetCustomAttribute<ConditionActionAttribute>();

                    actionPointType = conditionActionAttribute.ActionPointType;
                }

                if (field.HasCustomAttribute<NextAttribute>())
                {
                    var nextActionAttribute = field.GetCustomAttribute<NextAttribute>();

                    if (nextActionAttribute.NextKind == NextKind.GoToOrderNumber)
                    {
                        nextOrderNumber = nextActionAttribute.OrderNumber;
                    }

                    nextKind = nextActionAttribute.NextKind;
                }

                if (field.HasCustomAttribute<WeightAttribute>())
                {
                    var weightAttribute = field.GetCustomAttribute<WeightAttribute>();

                    weight = weightAttribute.Weight;
                }

                if (field.HasCustomAttribute<StepHandlerAttribute>())
                {
                    var stepHandlerAttribute = field.GetCustomAttribute<StepHandlerAttribute>();
                    var handlerType = stepHandlerAttribute.HandlerType;

                    stepHandler = (IStepHandler)this.ServiceProvider.GetService(handlerType);
                    stepHandler.SessionState = (ISessionStateObject)this.ServiceProvider.GetService<ISessionStateObject>();
                }

                if (field.HasCustomAttribute<ExpectedResponseAttribute>())
                {
                    var expectedResponseAttribute = field.GetCustomAttribute<ExpectedResponseAttribute>();
                    
                    expectedResponsePattern = expectedResponseAttribute.ExpectedResponsePattern;
                    minimumResponseBytes = expectedResponseAttribute.MinimumResponseBytes;
                    maximumResponseTimeSpan = expectedResponseAttribute.MaximumResponseTimeSpan;
                }

                conditionItems.Add(new ConditionItem<T>(itemName, item)
                {
                    ActionPointType = actionPointType,
                    Weight = weight,
                    OrderNumber = orderNumber,
                    NextOrderNumber = nextOrderNumber,
                    OrderKind = orderKind,
                    StepHandler = stepHandler,
                    ExpectedResponsePattern = expectedResponsePattern,
                    MinimumResponseBytes = minimumResponseBytes,
                    MaximumResponseTimeSpan = maximumResponseTimeSpan,
                    NextKind = nextKind
                });
            }

            foreach (var conditionItem in conditionItems.Where(i => i.NextKind == NextKind.GoToOrderNumber))
            {
                conditionItem.NextItem = conditionItems.Single(i => i.OrderNumber == conditionItem.NextOrderNumber);
            }

            return conditionItems;
        }

        public override List<IList<IConditionItem>> GetConditionLists()
        {
            var conditionLists = new List<IList<IConditionItem>>();

            if (this.ConditionPointDependencies.Any(d => d.Kind == ConditionPointDependencyKind.CountSource))
            {
                var countSourceDependency = this.ConditionPointDependencies.Single(d => d.Kind == ConditionPointDependencyKind.CountSource);
                var countInstance = countSourceDependency.ConditionPointInstance;
                var countConditionPoint = (IListConditionPoint)countInstance;
                var randomizationCountFromAttribute = this.GetType().GetCustomAttribute<RandomizationCountFromAttribute>();

                countInstance.GenerateItems();

                switch (randomizationCountFromAttribute.CountRelationshipKind)
                {
                    case CountRelationshipKind.OrderedCountCrossJoin:

                        var itemName = this.GetItemName();
                        var onDemandList = new OnDemandConditionList<T>();
                        var conditionList = new List<IConditionItem> { new OnDemandConditionItem<OnDemandConditionList<T>>(itemName, onDemandList) };

                        onDemandList.CreateOnDemand += (sender, e) =>
                        {
                            var count = e.List.Group.Single(i => i.Name == countConditionPoint.GetItemName()).GetItem<int>();
                            var itemList = this.GenerateConditionItems();
                            var thisType = this.GetType();
                            var enumType = typeof(T);
                            IEnumerable<InsertBetweenAttribute> insertBetweens = null;
                            float terminationWeight = 0;

                            if (thisType.HasCustomAttribute<InsertBetweenAttribute>())
                            {
                                insertBetweens = thisType.GetCustomAttributes<InsertBetweenAttribute>();
                            }

                            if (enumType.HasCustomAttribute<TerminationWeightAttribute>())
                            {
                                var terminationWeightAttribute = enumType.GetCustomAttribute<TerminationWeightAttribute>();

                                terminationWeight = terminationWeightAttribute.Weight;
                            }

                            itemList.GenerateWeightedList(this.ServiceProvider, e.AddItem, count, insertBetweens, terminationWeight);
                        };

                        conditionLists.Add(conditionList);
                        conditionLists.Add(countInstance.GetConditionLists().Single());

                        break;

                    default:
                        DebugUtils.Break();
                        break;
                }
            }
            else
            {
                var itemList = this.GenerateConditionItems();
                var conditionList = new List<IConditionItem>();

                itemList.GenerateWeightedList(this.ServiceProvider, conditionList.Add, this.Count);

                conditionLists.Add(conditionList);
            }

            return conditionLists;
        }

        public T this[int index]
        {
            get => throw new NotImplementedException();
            set => throw new NotImplementedException();
        }

        public virtual void Add(T item)
        {
            internalList.Add(item);
        }

        public virtual void AddRange(System.Collections.Generic.IEnumerable<T> collection)
        {
            internalList.AddRange(collection);
        }

        public virtual System.Collections.ObjectModel.ReadOnlyCollection<T> AsReadOnly()
        {
            return internalList.AsReadOnly();
        }

        public virtual int BinarySearch(int index, int count, T item, System.Collections.Generic.IComparer<T>? comparer)
        {
            return internalList.BinarySearch(index, count, item, comparer);
        }

        public virtual int BinarySearch(T item)
        {
            return internalList.BinarySearch(item);
        }

        public virtual int BinarySearch(T item, System.Collections.Generic.IComparer<T>? comparer)
        {
            return internalList.BinarySearch(item, comparer);
        }

        public virtual void Clear()
        {
            internalList.Clear();
        }

        public virtual bool Contains(T item)
        {
            return internalList.Contains(item);
        }

        public virtual void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            internalList.CopyTo(index, array, arrayIndex, count);
        }

        public virtual void CopyTo(T[] array)
        {
            internalList.CopyTo(array);
        }

        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            internalList.CopyTo(array, arrayIndex);
        }

        public virtual bool Exists(System.Predicate<T> match)
        {
            return internalList.Exists(match);
        }

        public virtual T Find(System.Predicate<T> match)
        {
            return internalList.Find(match);
        }

        public virtual System.Collections.Generic.List<T> FindAll(System.Predicate<T> match)
        {
            return internalList.FindAll(match);
        }

        public virtual int FindIndex(int startIndex, int count, System.Predicate<T> match)
        {
            return internalList.FindIndex(startIndex, count, match);
        }

        public virtual int FindIndex(int startIndex, System.Predicate<T> match)
        {
            return internalList.FindIndex(startIndex, match);
        }

        public virtual int FindIndex(System.Predicate<T> match)
        {
            return internalList.FindIndex(match);
        }

        public virtual T FindLast(System.Predicate<T> match)
        {
            return internalList.FindLast(match);
        }

        public virtual int FindLastIndex(int startIndex, int count, System.Predicate<T> match)
        {
            return internalList.FindLastIndex(startIndex, count, match);
        }

        public virtual int FindLastIndex(int startIndex, System.Predicate<T> match)
        {
            return internalList.FindLastIndex(startIndex, match);
        }

        public virtual int FindLastIndex(System.Predicate<T> match)
        {
            return internalList.FindLastIndex(match);
        }

        public virtual void ForEach(System.Action<T> action)
        {
            internalList.ForEach(action);
        }

        public virtual System.Collections.Generic.List<T>.Enumerator GetEnumerator()
        {
            return internalList.GetEnumerator();
        }

        public virtual System.Collections.Generic.List<T> GetRange(int index, int count)
        {
            return internalList.GetRange(index, count);
        }

        public virtual int IndexOf(T item)
        {
            return internalList.IndexOf(item);
        }

        public virtual int IndexOf(T item, int index)
        {
            return internalList.IndexOf(item, index);
        }

        public virtual int IndexOf(T item, int index, int count)
        {
            return internalList.IndexOf(item, index, count);
        }

        public virtual void Insert(int index, T item)
        {
            internalList.Insert(index, item);
        }

        public virtual void InsertRange(int index, System.Collections.Generic.IEnumerable<T> collection)
        {
            internalList.InsertRange(index, collection);
        }

        public virtual int LastIndexOf(T item)
        {
            return internalList.LastIndexOf(item);
        }

        public virtual int LastIndexOf(T item, int index)
        {
            return internalList.LastIndexOf(item, index);
        }

        public virtual int LastIndexOf(T item, int index, int count)
        {
            return internalList.LastIndexOf(item, index, count);
        }

        public virtual bool Remove(T item)
        {
            return internalList.Remove(item);
        }

        public virtual int RemoveAll(System.Predicate<T> match)
        {
            return internalList.RemoveAll(match);
        }

        public virtual void RemoveAt(int index)
        {
            internalList.RemoveAt(index);
        }

        public virtual void RemoveRange(int index, int count)
        {
            internalList.RemoveRange(index, count);
        }

        public virtual void Reverse()
        {
            internalList.Reverse();
        }

        public virtual void Reverse(int index, int count)
        {
            internalList.Reverse(index, count);
        }

        public virtual void Sort()
        {
            internalList.Sort();
        }

        public virtual void Sort(System.Collections.Generic.IComparer<T>? comparer)
        {
            internalList.Sort(comparer);
        }

        public virtual void Sort(System.Comparison<T> comparison)
        {
            internalList.Sort(comparison);
        }

        public virtual void Sort(int index, int count, System.Collections.Generic.IComparer<T>? comparer)
        {
            internalList.Sort(index, count, comparer);
        }

        public virtual T[] ToArray()
        {
            return internalList.ToArray();
        }

        public virtual void TrimExcess()
        {
            internalList.TrimExcess();
        }

        public virtual bool TrueForAll(System.Predicate<T> match)
        {
            return internalList.TrueForAll(match);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public virtual int Capacity
        {
            get
            {
                return internalList.Capacity;
            }

            set
            {
                internalList.Capacity = value;
            }
        }

        public virtual int Count
        {
            get
            {
                return internalList.Count;
            }
        }
    }
}
