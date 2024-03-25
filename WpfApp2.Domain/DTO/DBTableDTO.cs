using System.Collections.Generic;
using System.Data;
using System.Linq;
using WpfApp2.Domain.Entities;

namespace WpfApp2.Domain.DTO
{
    public sealed class DBTableDTO
    {
        public DataTable Source { get; private set; }
        public List<DBParameterEntity> ColumnElements { get; private set; } = new List<DBParameterEntity>();

        public DBTableDTO(DataTable dt)
        {
            this.Source = dt;
        }

        public DBTableDTO(int dbid, List<DBParameterEntity> dbParameters, List<DBValueEntity> dbValues)
        {
            Source = new DataTable();
            ColumnElements = dbParameters;
            foreach (var col in ColumnElements)
            {
                Source.Columns.Add(col.Name.Value);
                var values = dbValues.Where(x => x.ParameterId == col.Id).ToList();
                var index = 0;
                for (int i = 0; i < values.Count; i++)
                {
                    if (Source.Rows.Count <= i) Source.Rows.Add();
                    Source.Rows[index++][col.Name.Value] = values[i].Value;
                }
            }
        }
    }
}
