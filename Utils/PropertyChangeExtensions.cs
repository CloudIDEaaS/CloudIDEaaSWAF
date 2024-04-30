using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Collections.Specialized;
using System.Collections;

namespace Utils
{
	public static class PropertyChangeExtensions
	{
		public static void Raise(this INotifyPropertyChanged obj, PropertyChangedEventHandler propertyChangedDelegate, string propertyName)
		{
			if (propertyChangedDelegate != null)
			{
				propertyChangedDelegate(obj, new PropertyChangedEventArgs(propertyName));
			}
		}

		public static void DelayRaise(this INotifyPropertyChanged obj, string propertyName, int milliseconds, Func<PropertyChangedEventHandler> funcGetHandler)
		{
			var timer = new OneTimeTimer(milliseconds);

			timer.Start(() =>
			{
				var propertyChangedDelegate = funcGetHandler();

				if (propertyChangedDelegate != null)
				{
					propertyChangedDelegate(obj, new PropertyChangedEventArgs(propertyName));
				}
			});
		}

		public static PropertyNotifier Notify(this INotifyPropertyChanged obj, PropertyChangingEventHandler propertyChangingDelegate, PropertyChangedEventHandler propertyChangedDelegate,  params string[] properties)
		{
			return new PropertyNotifier(obj, propertyChangingDelegate, propertyChangedDelegate, properties);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action)
		{
			return new PropertyNotifier(obj, collectionChangedHandler, action);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, IList changedItems)
		{
			return new PropertyNotifier(obj, collectionChangedHandler, action, changedItems);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, object changedItem)
		{
			return new PropertyNotifier(obj, collectionChangedHandler, action, changedItem);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, IList newItems, IList oldItems)
		{
			return new PropertyNotifier(obj, collectionChangedHandler, action, newItems, oldItems);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
		{
			return new PropertyNotifier(obj, collectionChangedHandler, action, changedItems, startingIndex);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, object changedItem, int index)
		{
			return new PropertyNotifier(obj, collectionChangedHandler, action, changedItem, index);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, object newItem, object oldItem)
		{
			return new PropertyNotifier(obj, collectionChangedHandler, action, newItem, oldItem);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex)
		{
			return new PropertyNotifier(obj, collectionChangedHandler, action, newItems, oldItems, startingIndex);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex)
		{
			return new PropertyNotifier(obj, collectionChangedHandler, action, changedItems, index, oldIndex);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex)
		{
			return new PropertyNotifier(obj, collectionChangedHandler, action, changedItem, index, oldIndex);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, object newItem, object oldItem, int index)
		{
			return new PropertyNotifier(obj, collectionChangedHandler, action, newItem, oldItem, index);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, params string[] properties)
		{
			return new PropertyNotifier(obj, propertyChangingHandler, propertyChangedHandler, collectionChangedHandler, action, properties);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, IList changedItems, params string[] properties)
		{
			return new PropertyNotifier(obj, propertyChangingHandler, propertyChangedHandler, collectionChangedHandler, action, changedItems.Cast<object>().ToList(), properties);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, object changedItem, params string[] properties)
		{
			return new PropertyNotifier(obj, propertyChangingHandler, propertyChangedHandler, collectionChangedHandler, action, changedItem, properties);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, IList newItems, IList oldItems, params string[] properties)
		{
			return new PropertyNotifier(obj, propertyChangingHandler, propertyChangedHandler, collectionChangedHandler, action, newItems, oldItems, properties);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, IList changedItems, int startingIndex, params string[] properties)
		{
			return new PropertyNotifier(obj, propertyChangingHandler, propertyChangedHandler, collectionChangedHandler, action, changedItems.Cast<object>().ToList(), startingIndex, properties);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, object changedItem, int index, params string[] properties)
		{
			return new PropertyNotifier(obj, propertyChangingHandler, propertyChangedHandler, collectionChangedHandler, action, changedItem, index, properties);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, object newItem, object oldItem, params string[] properties)
		{
			return new PropertyNotifier(obj, propertyChangingHandler, propertyChangedHandler, collectionChangedHandler, action, newItem, oldItem, properties);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex, params string[] properties)
		{
			return new PropertyNotifier(obj, propertyChangingHandler, propertyChangedHandler, collectionChangedHandler, action, newItems, oldItems, startingIndex, properties);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex, params string[] properties)
		{
			return new PropertyNotifier(obj, propertyChangingHandler, propertyChangedHandler, collectionChangedHandler, action, changedItems.Cast<object>().ToList(), index, oldIndex, properties);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex, params string[] properties)
		{
			return new PropertyNotifier(obj, propertyChangingHandler, propertyChangedHandler, collectionChangedHandler, action, changedItem, index, oldIndex, properties);
		}

		public static PropertyNotifier Notify(this INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, object newItem, object oldItem, int index, params string[] properties)
		{
			return new PropertyNotifier(obj, propertyChangingHandler, propertyChangedHandler, collectionChangedHandler, action, newItem, oldItem, index, properties);
		}
	}

	public class PropertyNotifier : IDisposable
	{
		private INotifyPropertyChanged propertyChangedObject;
		private INotifyCollectionChanged collectionChangedObject;
		private PropertyChangedEventHandler propertyChangedHandler;
		private string[] properties;
		private NotifyCollectionChangedEventHandler collectionChangedHandler;
		private NotifyCollectionChangedEventArgs collectionChangedEventArgs;

		public PropertyNotifier(INotifyPropertyChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, params string[] properties)
		{
			this.propertyChangedObject = obj;
			this.properties = properties;
			this.propertyChangedHandler = propertyChangedHandler;

			if (propertyChangingHandler != null)
			{
				foreach (var property in properties)
				{
					propertyChangingHandler(obj, new PropertyChangingEventArgs(property));
				}
			}
		}

		public PropertyNotifier(INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action);
		}

		public PropertyNotifier(INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, IList changedItems)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action, changedItems);
		}

		public PropertyNotifier(INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, object changedItem)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action, changedItem);
		}

		public PropertyNotifier(INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, IList newItems, IList oldItems)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action, newItems, oldItems);
		}

		public PropertyNotifier(INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, IList changedItems, int startingIndex)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action, changedItems, startingIndex);
		}

		public PropertyNotifier(INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, object changedItem, int index)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action, changedItem, index);
		}

		public PropertyNotifier(INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, object newItem, object oldItem)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action, newItem, oldItem);
		}

		public PropertyNotifier(INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action, newItems, oldItems, startingIndex);
		}

		public PropertyNotifier(INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action, changedItems, index, oldIndex);
		}

		public PropertyNotifier(INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action, changedItem, index, oldIndex);
		}

		public PropertyNotifier(INotifyCollectionChanged obj, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, object newItem, object oldItem, int index)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index);
		}

		public PropertyNotifier(INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, params string[] properties)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action);
		}

		public PropertyNotifier(INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, IList changedItems, params string[] properties)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action, changedItems);
		}

		public PropertyNotifier(INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, object changedItem, params string[] properties)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action, changedItem);
		}

		public PropertyNotifier(INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, IList newItems, IList oldItems, params string[] properties)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action, newItems, oldItems);
		}

		public PropertyNotifier(INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, IList changedItems, int startingIndex, params string[] properties)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action, changedItems, startingIndex);
		}

		public PropertyNotifier(INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, object changedItem, int index, params string[] properties)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action, changedItem, index);
		}

		public PropertyNotifier(INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, object newItem, object oldItem, params string[] properties)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action, newItem, oldItem);
		}

		public PropertyNotifier(INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, IList newItems, IList oldItems, int startingIndex, params string[] properties)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action, newItems, oldItems, startingIndex);
		}

		public PropertyNotifier(INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, IList changedItems, int index, int oldIndex, params string[] properties)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action, changedItems, index, oldIndex);
		}

		public PropertyNotifier(INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, object changedItem, int index, int oldIndex, params string[] properties)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action, changedItem, index, oldIndex);
		}

		public PropertyNotifier(INotifyCollectionChanged obj, PropertyChangingEventHandler propertyChangingHandler, PropertyChangedEventHandler propertyChangedHandler, NotifyCollectionChangedEventHandler collectionChangedHandler, NotifyCollectionChangedAction action, object newItem, object oldItem, int index, params string[] properties)
		{
			this.collectionChangedObject = obj;
			this.collectionChangedHandler = collectionChangedHandler;
			this.collectionChangedEventArgs = new NotifyCollectionChangedEventArgs(action, newItem, oldItem, index);
		}

		public void Dispose()
		{
			if (propertyChangedHandler != null)
			{
				foreach (var property in properties.Reverse())
				{
					propertyChangedHandler(propertyChangedObject, new PropertyChangedEventArgs(property));
				}
			}

			if (collectionChangedHandler != null)
			{
				collectionChangedHandler(collectionChangedObject, collectionChangedEventArgs);
			}
		}
	}
}
