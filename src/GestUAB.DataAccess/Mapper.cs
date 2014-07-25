using System;
using System.Collections.Generic;
using System.Globalization;

namespace GestUAB.DataAccess
{
	public static class Mapper
	{


		public static T ToStatic<T> (object expando)
		{
			var entity = Activator.CreateInstance<T> ();

			//ExpandoObject implements dictionary
			var properties = expando as IDictionary<string, object>; 

			if (properties == null)
				return entity;

			foreach (var entry in properties) {
				var propertyInfo = entity.GetType ().GetProperty (entry.Key);
				object value = null;
				var entryType = entry.Value.GetType ();
				var propertyType = propertyInfo.PropertyType;
				value = entry.Value;
				if (propertyType == entryType) {
					value = entry.Value;
                } else if (propertyType.IsEnum) {
                    value = Enum.ToObject (propertyType, Convert.ChangeType (value, typeof(int)));
				} else if (propertyType != typeof(string) && entryType != typeof(string)) {
					value = Convert.ChangeType (value, entryType);
				}
//                else if(propertyType != typeof(string) && entryType == typeof(string)){
//					if (propertyType == typeof(DateTime)) {
//						value = DateTime.ParseExact ((string)entry.Value, "yyyy-MM-ddTHH:mm:ss.fffffffzzz" , CultureInfo.InvariantCulture);
//					} 
//				}
				if (propertyInfo != null) {
					propertyInfo.SetValue (entity, value, null);
				}
			}
			return entity;
		}
	}
}

