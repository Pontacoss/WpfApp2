using System.Collections.Generic;
using System.Data;

namespace WpfApp2.Domain.DTO
{
    public sealed class dummyDTO
    {
        public int BaseCompId { get; set; }
        public List<DataTable> InputDataTables { get; set; } = new List<DataTable>();
    }
}
