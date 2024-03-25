using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Domain.ValueObjects;

namespace WpfApp2.Domain.Entities
{
	public sealed class ModelEntity 
	{
		public int Id { get; set; }

		/// <summary>
		/// 型式
		/// </summary>
		[StringLength(10)]
		public ModelName ModelName { get; set; }

		[StringLength(2)]
		public Revision LatestRevision { get; set; } = new Revision(string.Empty);

		public ModelEntity() {}

		public ModelEntity(string modelName)
		{
			ModelName = new ModelName(modelName);
		}

		public static void GetAndSettingLatestRevision(List<ModelEntity> me, List<TemplateCombinationEntity> ssc)
		{
			foreach (var entity in me)
			{
				var list = ssc.Where(x => x.ModelId == entity.Id).OrderBy(x => x.Revision);
				if (list.Any())
					entity.LatestRevision = list.Last().Revision;
				else
					entity.LatestRevision = null;
			}
		}

		public void UpdateLatestRev(Revision revision)
		{
			if (LatestRevision == null) LatestRevision = new Revision(revision.Value);
			else if (LatestRevision.CompareTo(revision) < 0)
				LatestRevision = new Revision(revision.Value);
		}
	}
}
