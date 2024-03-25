using System.ComponentModel.DataAnnotations;
using WpfApp2.Domain.ValueObjects;

namespace WpfApp2.Domain.Entities
{
	public sealed class DBListEntity
	{
		public int Id { get; set; }

		[StringLength(20)]
		public Name Name { get; set; }

		public DBListEntity(){}

		public DBListEntity(string name)
		{
			Name = new Name(name);
		}
	}
}
