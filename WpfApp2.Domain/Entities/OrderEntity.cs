using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Domain.ValueObjects;

namespace WpfApp2.Domain.Entities
{
	public sealed class OrderEntity 
	{
		public int Id { get; set; }

		public int ParentId { get; set; }
		public int OrderNo { get; set; }

		[StringLength(20)]
		public SpecSheetName SpecSheetId { get; set; }

		[StringLength(2)]
		public Revision SpecSheetRevision { get; set; }
		public SeriesNo SpecSheetSeriesNo { get; set; }

	}
}
