using System;
using System.Collections.Generic;
using System.Reflection;

namespace GestUAB.Tests
{
	/// <summary>Comparison class.</summary>
	public static class Compare
	{
		/// <summary>Compare the public instance properties. Uses deep comparison.</summary>
		/// <param name="self">The reference object.</param>
		/// <param name="to">The object to compare.</param>
		/// <param name="ignore">Ignore property with name.</param>
		/// <typeparam name="T">Type of objects.</typeparam>
		/// <returns><see cref="bool">True</see> if both objects are equal, else <see cref="bool">false</see>.</returns>
		public static bool Equals<T> (T self, T to, params string[] ignore) where T : class
		{
			if (self != null && to != null) {
				var type = self.GetType ();
				var ignoreList = new List<string> (ignore);
				foreach (var pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
					if (ignoreList.Contains (pi.Name)) {
						continue;
					}

					var selfValue = type.GetProperty (pi.Name).GetValue (self, null);
					var toValue = type.GetProperty (pi.Name).GetValue (to, null);

					if (pi.PropertyType.IsClass && !(pi.PropertyType.Module.ScopeName.Equals ("CommonLanguageRuntimeLibrary") ||
						pi.PropertyType.Module.ScopeName.Equals ("mscorlib.dll"))) {
						// Check of "CommonLanguageRuntimeLibrary" is needed because string is also a class
						if (Equals (selfValue, toValue, ignore)) {
							continue;
						}

						return false;
					}

					if (selfValue != toValue && (selfValue == null || !selfValue.Equals (toValue))) {
						return false;
					}
				}

				return true;
			}

			return self == to;
		}
//		public static bool PublicInstancePropertiesEqual<T>(this T self, T to, params string[] ignore) where T : class
//		{
//			if (self != null && to != null)
//			{
//				var type = typeof(T);
//				var ignoreList = new List<string>(ignore);
//				var unequalProperties =
//					from pi in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
//						where !ignoreList.Contains(pi.Name)
//						let selfValue = type.GetProperty(pi.Name).GetValue(self, null)
//						let toValue = type.GetProperty(pi.Name).GetValue(to, null)
//						where selfValue != toValue && (selfValue == null || !selfValue.Equals(toValue))
//						select selfValue;
//				return !unequalProperties.Any();
//			}
//			return self == to;
//		}
	}
}

