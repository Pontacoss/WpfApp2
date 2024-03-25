using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using WpfApp2.Domain.Entities;
using WpfApp2.Domain.InterFaces;
using WpfApp2.Domain.ValueObjects;

namespace WpfApp2.Domain.Entities
{
	public sealed class CompositionEntity 
	{
		[Required]
		public int Id { get; set; }
		public int ParentId { get; set; }
		public int Sequence { get; set; }
		public int Level { get; set; }
		public AreaType AreaType { get; set; } = AreaType.None;
		public bool IsVariable { get; set; } = false;
		public bool IsHide { get; set; } = false;
		public int ContentId { get; set; } = 0;
		[StringLength(30)]
		public string ParamList { get; set; } = String.Empty;
		public string Group { get; set; } = String.Empty;
		public int ParentNodeId { get; set; } = 0;

		public IAreaEntity AreaData { get; set; }

		public CompositionEntity() { }

		public CompositionEntity(CompositionEntity BaseEntity, int newTemplateId,Dictionary<int,int> OldNewParamIdPairs, Dictionary<int, int> OldNewCompIdPairs)
		{
			ParentId = newTemplateId;
			Sequence = BaseEntity.Sequence;
			Level = BaseEntity.Level;
			AreaType = BaseEntity.AreaType;
			IsVariable = BaseEntity.IsVariable;
			IsHide = BaseEntity.IsHide;
			ParentNodeId = BaseEntity.ParentNodeId == 0 ? 0 : OldNewCompIdPairs[BaseEntity.ParentNodeId];

			if (BaseEntity.ParamList != String.Empty)
			{
				List<string> strings = BaseEntity.ParamList.Split('\u002C').ToList();
				var ints = strings.Select(x => OldNewParamIdPairs[Convert.ToInt32(x)]);
				ParamList = string.Join(",", ints);
			}

			
		}

	}
}
