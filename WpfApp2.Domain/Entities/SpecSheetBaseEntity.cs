using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Domain.ValueObjects;

namespace WpfApp2.Domain.Entities
{
	public sealed class SpecSheetBaseEntity 
	{
		public int Id { get; set; }

		[StringLength(40)]
		public SpecSheetName SpecSheetName { get; set; }
		public int ModelId { get; set; }
		public int TemplateCombinationId { get; set; }
		public int DevelopmentOrderId { get; set; }
		public Date CreationDate { get; set; } = new Date(DateTime.MinValue);
		public Date IssueDate { get; set; } = new Date(DateTime.MinValue);


		public SpecSheetBaseEntity() { }
		public SpecSheetBaseEntity(string specSheetName, int developmentOrderId, int modelId, int templateCombinationId)
		{
			SpecSheetName = new SpecSheetName(specSheetName);
			DevelopmentOrderId = developmentOrderId;
			ModelId = modelId;
			TemplateCombinationId = templateCombinationId;
		}
	}
}
