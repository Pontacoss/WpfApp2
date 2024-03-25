using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Domain.ValueObjects;

namespace WpfApp2.Domain.Entities
{
	public sealed class DBParameterEntity
	{
		public int Id { get; set; }
		public int DBId { get; set; }

		[StringLength(20)]
		public ParamName Name { get; set; }
		public bool IsPrimaryKey { get; set; }

		public DBParameterEntity()
		{
		}

		public DBParameterEntity(int dBId, string name)
		{
			DBId = dBId;
			Name = new ParamName(name);
		}

		public DBParameterEntity(DBParameterEntity dbp)
		{
			Id = dbp.DBId;
			DBId = dbp.DBId;
			Name = dbp.Name;
			IsPrimaryKey = dbp.IsPrimaryKey;
		}
	}
}
