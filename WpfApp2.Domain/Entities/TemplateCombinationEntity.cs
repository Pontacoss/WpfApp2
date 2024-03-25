using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Domain.DTO;
using WpfApp2.Domain.ValueObjects;

namespace WpfApp2.Domain.Entities
{
	public sealed class TemplateCombinationEntity 
	{
		public int Id { get; set; }

		public int ModelId { get; set; }

		[StringLength(2)]
		public Revision Revision { get; set; } = new Revision("");
		public List<int> TemplateCombination { get; set; } = new List<int>();

		public TemplateCombinationEntity() { }
		public TemplateCombinationEntity(int modelId, Revision revision, List<int> list)
		{
			ModelId = modelId;
			Revision = revision;
			TemplateCombination = list;
		}
	}
}
