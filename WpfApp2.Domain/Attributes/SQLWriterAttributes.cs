using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Domain.Attributes
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class PrimaryKeyAttribute : System.Attribute
	{
		public bool IsPrimaryKey { get; set; }
		public PrimaryKeyAttribute()
		{
			IsPrimaryKey = true;
		}
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class IgnoreAttribute : System.Attribute
	{
		public bool IsIgnore { get; set; }
		public IgnoreAttribute()
		{
			IsIgnore = true;
		}
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class UniqueAttribute : System.Attribute
	{
		public bool IsUnique { get; set; }
		public UniqueAttribute()
		{
			IsUnique = true;
		}
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class AutoIncrementAttribute : System.Attribute
	{
		public bool IsAutoIncrement { get; set; }
		public AutoIncrementAttribute()
		{
			IsAutoIncrement = true;
		}
	}

	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public class ColumnAttribute : System.Attribute
	{
		public string Name { get; set; }
		public ColumnAttribute(string name)
		{
			Name = name;
		}
	}

	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
	public class CreateTableAttribute : System.Attribute
	{
		public string Name { get; set; }
		public CreateTableAttribute(string name)
		{
			Name = name;
		}
	}
}
