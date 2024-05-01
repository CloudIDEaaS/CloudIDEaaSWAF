using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Utils;

namespace Utils.ConditionPoints
{
    public static class Extensions
    {
        public static void GenerateWeightedList(this List<IConditionItem> conditionItems, IServiceProvider serviceProvider, Action<IConditionItem> addItem, int targetCount, IEnumerable<InsertBetweenAttribute> insertBetweens = null, float terminationWeight = -1)
        {
            var orderIndexedDictionary = new Dictionary<int, IConditionItem>();
            var balancedList = new List<IConditionItem>();
            var targetList = new List<IConditionItem>();
            Queue<IConditionItem> balancedListQueue;
            List<IConditionItem> nonIndexedConditionItems;

            foreach (var conditionItem in conditionItems.Where(i => i.OrderKind == OrderKind.Specific))
            {
                orderIndexedDictionary.Add(conditionItem.OrderNumber, conditionItem);
            }

            nonIndexedConditionItems = conditionItems.Where(i => !orderIndexedDictionary.Values.Any(v => v == i)).ToList();

            if (conditionItems.Any(i => i.Weight != -1))
            {
                foreach (var conditionItem in nonIndexedConditionItems.Where(i => i.Weight != -1))
                {
                    var weightedCount = (int)Math.Round(((float)targetCount) * conditionItem.Weight);

                    for (var x = 0; x < weightedCount; x++)
                    {
                        balancedList.Add(conditionItem);
                    }
                }
            }
            else if (nonIndexedConditionItems.Any(i => i.NextKind == NextKind.Terminates))
            {
                var itemCount = conditionItems.Count;
                var terminationCount = conditionItems.Count(i => i.NextKind == NextKind.Terminates);
                var nonTerminationCount = itemCount - terminationCount;
                var totalTerminationWeight = terminationCount * terminationWeight;
                var totalNonTerminationWeight = 1 - totalTerminationWeight;
                var nonTerminationWeight = totalNonTerminationWeight / nonTerminationCount;
                var terminationWeightedCount = (int)Math.Round(((float)targetCount) * terminationWeight);
                var nonTerminationWeightedCount = (int)Math.Round(((float)targetCount) * nonTerminationWeight);

                Debug.Assert(terminationWeight != -1);

                foreach (var conditionItem in nonIndexedConditionItems)
                {
                    if (conditionItem.NextKind == NextKind.Terminates)
                    {
                        for (var x = 0; x < terminationWeightedCount; x++)
                        {
                            balancedList.Add(conditionItem);
                        }
                    }
                    else
                    {
                        for (var x = 0; x < nonTerminationWeightedCount; x++)
                        {
                            balancedList.Add(conditionItem);
                        }
                    }
                }
            }
            else
            {
                balancedList.AddRange(nonIndexedConditionItems);
            }

            balancedListQueue = new Queue<IConditionItem>(balancedList.RandomExpand(targetCount));

            for (var x = 1; x < targetCount + 1; x++)
            {
                IConditionItem conditionItem;

                if (orderIndexedDictionary.ContainsKey(x))
                {
                    var item = orderIndexedDictionary[x];

                    conditionItem = item;
                }
                else
                {
                    var queueItem = balancedListQueue.Dequeue();

                    conditionItem = queueItem;
                }

                targetList.Add(conditionItem);

                if (conditionItem.NextKind == NextKind.GoToOrderNumber)
                {
                    targetList.Add(conditionItem.NextItem);
                }
                else if (conditionItem.NextKind == NextKind.Terminates)
                {
                    break;
                }
            }

            targetList = targetList.Take(targetCount).ToList();

            if (insertBetweens != null && insertBetweens.Count() > 0)
            {
                for (var x = targetList.Count - 1; x > 0; x--)
                {
                    foreach (var insertBetween in insertBetweens.Reverse())
                    {
                        var type = insertBetween.InsertType;
                        var list = (ConditionPointBase)Activator.CreateInstance(type);

                        list.GenerateItems();
                        list.ServiceProvider = serviceProvider;

                        if (list.ConditionPointType == ConditionPointKind.RandomListSource)
                        {
                            var conditionItem = list.GetConditionLists().Single().Randomize().First();

                            targetList.Insert(x, conditionItem);
                        }
                    }
                }
            }

            targetList.ForEach(i => addItem(i));
        }

        public static T GetItem<T>(this IConditionItem item)
        {
            return (T)item.Item;
        }

        public static string GetItemName(this IListConditionPoint conditionPoint)
        {
            var type = conditionPoint.GetType();

            if (type.HasCustomAttribute<ItemNameAttribute>())
            {
                var itemNameAttribute = type.GetCustomAttribute<ItemNameAttribute>();

                return itemNameAttribute.Name;
            }
            else
            {
                return type.Name;
            }
        }

        public static bool HasHandler(this IConditionItem conditionItem)
        {
            return conditionItem.StepHandler != null;
        }

        public static bool HasActionPoint(this IConditionItem conditionItem)
        {
            return conditionItem.ActionPointType != null;
        }

        public static HandlerResult ExecuteHandler(this IConditionItem conditionItem, string phase, IEnumerable<IConditionItem> conditionItems, params KeyValuePair<string, object>[] parms)
        {
            return conditionItem.StepHandler.Handle(phase, conditionItems, conditionItem, parms);
        }

        public static void Navigate(this TestsBase testsBase, IServiceProvider serviceProvider, string phase, Action<string, IEnumerable<IEnumerable<IConditionItem>>> navigateCallback)
        {
            var conditionGroups = testsBase.GetConditionGroups(serviceProvider);

            navigateCallback(phase, conditionGroups);
        }

        public static void Navigate(this Type actionPointType, IServiceProvider serviceProvider, string phase, Action<string, IEnumerable<IEnumerable<IConditionItem>>> navigateCallback)
        {
            var conditionGroups = actionPointType.GetConditionGroups(serviceProvider);

            navigateCallback(phase, conditionGroups);
        }

        public static IEnumerable<IEnumerable<IConditionItem>> GetConditionGroups(this Type actionPointType, IServiceProvider serviceProvider)
        {
            var conditionPoint = (ConditionPointBase) Activator.CreateInstance(actionPointType);
            var conditionItems = new List<IConditionItem>();
            var rootLists = new List<ConditionPointBase>();
            var conditionLists = new List<IEnumerable<IConditionItem>>();
            IEnumerable<IEnumerable<IConditionItem>> conditionGroups;

            conditionPoint.AddDependencies();

            conditionPoint.ServiceProvider = serviceProvider;
            conditionPoint.GenerateItems();

            foreach (var conditionList in conditionPoint.GetConditionLists())
            {
                conditionLists.Add(conditionList);
            }

            conditionGroups = conditionLists.CartesianProduct(true).ToList();

            foreach (var conditionGroup in conditionGroups)
            {
                foreach (var item in conditionGroup)
                {
                    if (item is IOnDemandConditionItem onDemandConditionItem)
                    {
                        onDemandConditionItem.Group = conditionGroup.Where(i => i != item);
                    }
                }
            }

            return conditionGroups;
        }

        public static IServiceProvider BuildServiceProvider(this TestsBase testsBase, IServiceCollection services)
        {
            var assembly = Assembly.GetEntryAssembly();
            var sessionStateTypes = assembly.GetTypes().Where(t => t.Implements<ISessionStateObject>());
            var handlerTypes = assembly.GetTypes().Where(t => t.Implements<IStepHandler>());
            IServiceProvider serviceProvider;

            foreach (var sessionStateType in sessionStateTypes)
            {
                services.AddSingleton(typeof(ISessionStateObject), sessionStateType);
            }

            foreach (var handlerType in handlerTypes)
            {
                services.AddSingleton(handlerType, handlerType);
            }

            serviceProvider = services.BuildServiceProvider();

            return serviceProvider;
        }

        public static IEnumerable<IEnumerable<IConditionItem>> GetConditionGroups(this TestsBase testsBase, IServiceProvider serviceProvider)
        {
            var methodStackItem = testsBase.GetStack().Skip(2).First(m => m.GetMethod().HasCustomAttribute<ConditionPointsAttribute>());
            var method = methodStackItem.GetMethod();
            var conditionPointsAttribute = method.GetCustomAttribute<ConditionPointsAttribute>();
            var conditionPointsClass = conditionPointsAttribute.ConditionPointsClass;
            var classes = conditionPointsClass.GetNestedTypes().Where(t => t.IsClass && t.InheritsFrom<ConditionPointBase>() && t.GetConstructors().Any(c => c.GetParameters().Length == 0));
            var instances = classes.Select(c => Activator.CreateInstance(c)).OfType<ConditionPointBase>();
            var factoringLists = instances.Where(i => i.ConditionPointType.HasFlag(ConditionPointKind.FactoringList)).ToList();
            var conditionItems = new List<IConditionItem>();
            var rootConditionPoints = new List<ConditionPointBase>();
            var conditionLists = new List<IEnumerable<IConditionItem>>();
            IEnumerable<IEnumerable<IConditionItem>> conditionGroups;

            foreach (var factoringList in factoringLists)
            {
                factoringList.AddDependencies();
            }

            rootConditionPoints = factoringLists.Where(f => f.IsRootList(factoringLists.Where(l => !(l == f)))).ToList();

            foreach (var conditionPoint in rootConditionPoints)
            {
                conditionPoint.ServiceProvider = serviceProvider;
                conditionPoint.GenerateItems();

                foreach (var conditionList in conditionPoint.GetConditionLists())
                {
                    conditionLists.Add(conditionList);
                }
            }

            conditionGroups = conditionLists.CartesianProduct(true).ToList();

            foreach (var conditionGroup in conditionGroups)
            {
                foreach (var item in conditionGroup)
                {
                    if (item is IOnDemandConditionItem onDemandConditionItem)
                    {
                        onDemandConditionItem.Group = conditionGroup.Where(i => i != item);
                    }
                }
            }

            return conditionGroups;
        }

        public static void GenerateItems(this ConditionPointBase conditionPoint)
        {
            if (conditionPoint is IComparableListConditionPoint comparableListConditionPoint && comparableListConditionPoint.FanListSetting != null)
            {
                comparableListConditionPoint.FanList();
            }
            else if (conditionPoint is IEnumListConditionPoint enumListConditionPoint)
            {
                enumListConditionPoint.GenerateEnumItems();
            }
        }

        public static bool IsRootList(this ConditionPointBase conditionPoint, IEnumerable<ConditionPointBase> allConditionPoints)
        {
            return !allConditionPoints.SelectMany(p => p.ConditionPointDependencies).Where(d => d.Kind == ConditionPointDependencyKind.ActionPoint).Any(d => d.DependencyType == conditionPoint.GetType());
        }

        public static void AddDependencies(this ConditionPointBase conditionPoint)
        {
            var type = conditionPoint.GetType();

            if (type.HasCustomAttribute<RandomizationCountFromAttribute>())
            {
                var randomizationCountFromAttribute = type.GetCustomAttribute<RandomizationCountFromAttribute>();
                var dependency = new ConditionPointDependency(randomizationCountFromAttribute, randomizationCountFromAttribute.CountType, ConditionPointDependencyKind.CountSource);

                conditionPoint.ConditionPointDependencies.Add(dependency);
            }

            if (type.HasCustomAttribute<InsertBetweenAttribute>())
            {
                var insertBetweenAttributes = type.GetCustomAttributes<InsertBetweenAttribute>();

                foreach (var insertBetweenAttribute in insertBetweenAttributes)
                {
                    var dependency = new ConditionPointDependency(insertBetweenAttribute, insertBetweenAttribute.InsertType, ConditionPointDependencyKind.InsertBetweenSource);

                    conditionPoint.ConditionPointDependencies.Add(dependency);
                }
            }

            if (type.BaseType.GetGenericTypeDefinition() == typeof(EnumListConditionPoint<>))
            {
                var baseType = type.BaseType;
                var enumType = baseType.GetGenericArguments().Single();
                var enumFields = EnumUtils.GetFields(enumType);
                var dependency = new ConditionPointDependency(enumType, ConditionPointDependencyKind.TypeSource, enumFields);
                var actionPointFields = enumFields.Where(f => f.HasCustomAttribute<ConditionActionAttribute>());

                conditionPoint.ConditionPointDependencies.Add(dependency);

                foreach (var actionPointField in actionPointFields)
                {
                    var actionPointAttribute = actionPointField.GetCustomAttribute<ConditionActionAttribute>();
                    dependency = new ConditionPointDependency(actionPointAttribute, actionPointAttribute.ActionPointType, actionPointField, ConditionPointDependencyKind.ActionPoint);

                    conditionPoint.ConditionPointDependencies.Add(dependency);
                }
            }
        }
    }
}
