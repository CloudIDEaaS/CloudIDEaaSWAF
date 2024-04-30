using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Utils;

namespace Utils.PortableExecutable
{
    [DebuggerDisplay(" { DebugInfo }")]
    public class ReferencedItem
    {
        private ImageLayoutItem primary;
        public IImageLayoutItemContainer Referenced { get; set; }
        public string ReferencingProperty { get; set; }
        public string ReferenceName { get; set; }

        public ReferencedItem(ImageLayoutItem referenced, ImageLayoutItem primary, string referencingProperty)
        {
            this.primary = primary;
            this.Referenced = referenced;
            this.ReferencingProperty = referencingProperty;
        }

        public ReferencedItem(ImageLayoutItem referenced, ImageLayoutItem primary, string referencingProperty, string referenceName)
        {
            this.primary = primary;
            this.Referenced = referenced;
            this.ReferencingProperty = referencingProperty;
            this.ReferenceName = referenceName;
        }

        public bool ReferencesArray
        {
            get
            {
                if (primary is ImageLayoutItem)
                {
                    var item = (ImageLayoutItem)primary;
                    var arrayCount = item.References.Count(i => i.ReferencingProperty == this.ReferencingProperty);

                    if (arrayCount > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
        }

        public int ArrayIndex
        {
            get
            {
                if (primary is ImageLayoutItem)
                {
                    var item = (ImageLayoutItem)primary;
                    var arrayCount = item.References.Count(i => i.ReferencingProperty == this.ReferencingProperty);

                    if (arrayCount > 0)
                    {
                        if (item.References.Any(i => i == this))
                        {
                            var index = item.References.Where(i => i.ReferencingProperty == this.ReferencingProperty).IndexOf(this);

                            return index;
                        }
                        else
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    return -1;
                }
            }
        }

        public string DebugInfo
        {
            get
            {
                var realItem = primary.Item.GetRealItem();

                if (realItem is IImageLayoutItemTupleContainer)
                {
                    var tupleItem = (IImageLayoutItemTupleContainer)realItem;
                    var pairProperty = tupleItem.TupleValues.Single(p => p.Key == this.ReferencingProperty);
                    var pairReference = tupleItem.TupleValues.Single(p => p.Key == this.ReferenceName);

                    return string.Format(
                        "ReferencedBy: {0}=0x{1:x8}, "
                        + "ReferencedItem: {2}, "
                        + "Value: {3}",
                        this.ReferencingProperty,
                        pairProperty.Value,
                        this.Referenced.Name,
                        pairReference.Value);
                }
                else
                {
                    var propertyValue = realItem.GetPropertyValue<object>(this.ReferencingProperty).ToString();

                    if (!(this.Referenced is IImageLayoutItemGenericContainer) && this.Referenced.Item is IImageLayoutItemTupleContainer)
                    {
                        var tupleValueString = string.Empty;
                        var tupleItem = (IImageLayoutItemTupleContainer)this.Referenced.Item;
                        string referencedName;

                        foreach (var pair in tupleItem.TupleValues)
                        {
                            tupleValueString = tupleValueString.Append(string.Format("{0}: {1}, ", pair.Key, pair.Value));
                        }

                        if (tupleValueString.Length > 2)
                        {
                            tupleValueString = tupleValueString.RemoveEnd(", ");
                        }

                        if (this.ReferencesArray)
                        {
                            if (this.ArrayIndex == -1)
                            {
                                // linot yet added

                                var item = (ImageLayoutItem)primary;
                                var arrayCount = item.References.Count(i => i.ReferencingProperty == this.ReferencingProperty);

                                if (arrayCount > 0)
                                {
                                    referencedName = this.Referenced.Name + string.Format("[{0}]", arrayCount);
                                }
                                else
                                {
                                    referencedName = this.Referenced.Name;
                                }
                            }
                            else
                            {
                                var item = (ImageLayoutItem)primary;

                                referencedName = this.Referenced.Name + string.Format("[{0}]", this.ArrayIndex);
                            }
                        }
                        else
                        {
                            referencedName = this.Referenced.Name;
                        }

                        return string.Format(
                            "ReferencedBy: {0}=0x{1:x8}, "
                            + "ReferencedItem: {2}, "
                            + "{3}",
                            this.ReferencingProperty,
                            propertyValue,
                            referencedName,
                            tupleValueString);
                    }
                    else
                    {
                        return string.Format(
                            "ReferencedBy: {0}=0x{1:x8}, "
                            + "ReferencedItem: {2}",
                            this.ReferencingProperty,
                            propertyValue,
                            this.Referenced.GetDebuggerDisplay());
                    }
                }
            }
        }
    }
}
