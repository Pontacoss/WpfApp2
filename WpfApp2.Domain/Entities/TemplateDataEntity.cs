using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using WpfApp2.Domain.ValueObjects;

namespace WpfApp2.Domain.Entities
{
	public sealed class TemplateDataEntity 
	{
		public int Id { get; set; }
		public int TemplateCombinationId { get; set; }
		[StringLength(10)]
		public string SeriesNo { get; set; }
		public DataTable OrderDataTable { get; set; }
		public Status Status { get; set; } = new Status(0);

		public TemplateDataEntity() { }
		public TemplateDataEntity(int templateCombinationId, string seriesNo)
		{
			TemplateCombinationId = templateCombinationId;
			SeriesNo = seriesNo;
		}
	}
}
