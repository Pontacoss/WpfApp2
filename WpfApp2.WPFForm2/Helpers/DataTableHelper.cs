using SQLWriter;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Domain;
using WpfApp2.Domain.Entities;


namespace WpfApp2.WPFForm2.Helpers
{
	public static class DataTableHelper
	{
		public static void DeleteDataTable(int dbid)
		{
			var f = new SQLFilter(nameof(DBParameterEntity.DBId), dbid);
			var list1 = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<DBParameterEntity>(f);

			foreach (var i in list1)
			{
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Delete(i);
			}

			var f1 = new SQLFilter(nameof(DBParameterEntity.DBId), dbid);
			var list2 = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<DBValueEntity>(f1);

			foreach (var i in list2)
			{
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Delete(i);
			}
		}

		public static DataTable GetDataTable(int id)
		{
			throw new NotImplementedException();
		}

		public static void SaveDataTable(DataTable dt, int dbid)
		{
			foreach (DataColumn col in dt.Columns)
			{
				var dbParam = new DBParameterEntity(dbid, col.ColumnName);
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(dbParam);

				var j = 0;
				foreach (DataRow rw in dt.Rows)
				{
					//SQLiteHelper.Execute(new DBValueEntity(dbid, j++, dbParam.Id, rw[dbParam.Name].ToString()));
					SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(new DBValueEntity(dbid, j++, dbParam.Id, rw[dbParam.Name.Value].ToString()));
				}
			}
		}
	}
}
