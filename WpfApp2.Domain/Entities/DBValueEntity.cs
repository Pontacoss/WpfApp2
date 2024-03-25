using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Domain.Entities
{
	public sealed class DBValueEntity
	{
		public int Id { get; set; }

		public int DBId { get; set; }
		public int Rowindex { get; set; }
		public int ParameterId { get; set; }

		[StringLength(20)]
		public string Value { get; set; }

		public DBValueEntity() { }
		public DBValueEntity(int dBId, int rowindex, int parameterId, string value)
		{
			DBId = dBId;
			Rowindex = rowindex;
			ParameterId = parameterId;
			Value = value;
		}
	}
}
