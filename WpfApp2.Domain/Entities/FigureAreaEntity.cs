using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Domain.Entities;
using WpfApp2.Domain.InterFaces;

namespace WpfApp2.Domain.Entities
{
	public sealed class FigureAreaEntity : IAreaEntity
	{
		public int Id { get; set; }

		[StringLength(150)]
		public string FilePath { get; set;} = string.Empty;

		[StringLength(20)]
		public string TitleJP { get; set; } = string.Empty;

		[StringLength(40)]
		public string TitleEN { get; set; } = string.Empty;

		public FigureAreaEntity()
		{
		}

		public FigureAreaEntity(FigureAreaEntity BaseEntity, Dictionary<int, int> OldNewParamIdPairs)
		{
			FilePath = BaseEntity.FilePath;
			TitleJP = BaseEntity.TitleJP;
			TitleEN = BaseEntity.TitleEN;
		}

		public FigureAreaEntity(int id)
		{
			Id = id;
		}

		public string ExtractString(bool language)
		{
			return string.Empty;
		}
	}
}
