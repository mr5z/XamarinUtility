using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace XamarinUtility.Extensions
{
    public static class ObjectExtension
	{
		public static T ToObject<T>(this IDictionary<string, object> source)
			where T : class, new()
		{
			return ToObject<T, object>(source);
		}

		public static T ToObject<T>(this IDictionary<string, string> source)
			where T : class, new()
		{
			return ToObject<T, string>(source);
		}

		private static TReturnType ToObject<TReturnType, TValue>(IDictionary<string, TValue> source)
			where TReturnType : class, new()
		{
			var obj = new TReturnType();
			var type = obj.GetType();
			foreach (var item in source)
			{
				type.GetProperty(item.Key)
					.SetValue(obj, item.Value, null);
			}
			return obj;
		}

		public static IDictionary<string, object> AsDictionary(this object source, BindingFlags bindingAttr = BindingFlags.Public | BindingFlags.Instance)
		{
			return source.GetType().GetProperties(bindingAttr).ToDictionary
			(
				propInfo => propInfo.Name,
				propInfo => propInfo.GetValue(source, null)
			);
		}
	}
}
