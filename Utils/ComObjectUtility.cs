using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using System.ComponentModel;

namespace Utils
{
    public static class ComObjectUtility
    {
        internal static string InspectComObject(object obj)
        {
            var builder = new StringBuilder();

            foreach (PropertyDescriptor descrip in TypeDescriptor.GetProperties(obj))
            {
                if (descrip.Name == "Attribute Name")
                {
                    foreach (PropertyDescriptor descrip2 in TypeDescriptor.GetProperties(descrip))
                    {
                        if (descrip2.Name == "sub attribute Name")
                        {
                        } 
                    }
                }
            }

            return builder.ToString();
        }
    }
}
