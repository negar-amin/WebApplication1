using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace WebApplication1.Data.Entities

{
	public interface IMergeable<T>
	{
	}

	public static class MergeableExtensions
	{
		public static void MergeNotNullValues<T>(this T target, T source) where T : IMergeable<T>
		{
			foreach (PropertyInfo property in target.GetType().GetProperties())
			{
				var value = property.GetValue(source);
				if (IsDefaultOne(value)) continue;
				if (value != null)
				{
					property.SetValue(target, value);
				}
			}
		}
		private static bool IsDefaultOne(object value)
		{
			if (value is int intValue && intValue == -1)
				return true;
			if (value is double doubleValue && doubleValue == -1)
				return true;
			if (value is float floatValue && floatValue == -1)
				return true;
			if (value is long longValue && longValue == -1)
				return true;
			if (value is short shortValue && shortValue == -1)
				return true;
			if (value is decimal decimalValue && decimalValue == -1)
				return true;
			//if (value.GetType().IsEnum && value!= null)
			//	object defaultValue = Activator.CreateInstance(value.GetType());
			//	return value.Equals(defaultValue);

			return false;
		}
	}
}
