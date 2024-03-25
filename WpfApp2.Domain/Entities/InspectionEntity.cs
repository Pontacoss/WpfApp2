using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Domain.ValueObjects;

namespace WpfApp2.Domain.Entities
{
	public sealed class InspectionEntity 
	{
		public int Id { get; set; }

		public int ParentId { get; set; }

		[StringLength(10)]
		public Name NameJP { get; set; } = new Name(string.Empty);

		[StringLength(20)]
		public Name NameEN { get; set; } = new Name(string.Empty);
		public bool TypeTest { get; set; } = false;
		public bool RoutinTest { get; set; } = false;

		[StringLength(2)]
		public Revision LatestRevision { get; set; } = new Revision(string.Empty);

		[StringLength(16)]
		public Clause Clause { get; set; } = new Clause(string.Empty);

		public InspectionEntity() { }

		public InspectionEntity(int parentId, string clause, string nameJP, string nameEN, bool typeTest = false, bool routinTest = false, int id = 0)
		{
			Id = id;
			ParentId = parentId;
			Clause = new Clause(clause);
			NameJP = new Name(nameJP);
			NameEN = new Name(nameEN);
			TypeTest = typeTest;
			RoutinTest = routinTest;
		}

		public InspectionEntity(StandardEntity parent)
		{
			ParentId = parent.Id;			
		}

		public void GetLatestRevision(List<TemplateEntity> temps)
		{
			var approvedTemp = temps.Where(x => x.Status == Status.Approved);
			if (approvedTemp.Count() == 0) LatestRevision = new Revision(string.Empty);
			else LatestRevision = temps.Where(x => x.Status == Status.Approved)
					.OrderBy(x => x.Revision).Last().Revision;
		}
	}
}
