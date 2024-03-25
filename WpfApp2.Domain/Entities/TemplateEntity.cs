using System;
using System.ComponentModel.DataAnnotations;
using WpfApp2.Domain.ValueObjects;

namespace WpfApp2.Domain.Entities
{
	public sealed class TemplateEntity
	{
		public int Id { get; set; }

		public Status Status { get; set; } = new Status(0);

		[StringLength(2)]
		public Revision Revision { get; set; } = new Revision("-");

		[StringLength(2)]
		public Revision BaseRevision { get; set; } = new Revision(string.Empty);
		public int BaseTemplateId { get; set; } = 0;

		[StringLength(20)]
		public string Author { get; set; } = String.Empty;

		[StringLength(20)]
		public string Checker { get; set; } = String.Empty;

		[StringLength(20)]
		public string Approver { get; set; } = String.Empty;
		public Date IssueDate { get; set; } = new Date(DateTime.MinValue);
		public Date LastUpdate { get; set; } = new Date(DateTime.MinValue);

		[StringLength(200)]
		public string Description { get; set; } = String.Empty;

		[StringLength(1000)]
		public string AuthorComment { get; set; } = String.Empty;

		[StringLength(1000)]
		public string CheckerComment { get; set; } = String.Empty;

		[StringLength(1000)]
		public string ApproverComment { get; set; } = String.Empty;

		public int ParentId { get; set; }

		public TemplateEntity() { }

		public TemplateEntity(InspectionEntity parent)
		{
			ParentId = parent.Id;
		}

		public TemplateEntity(TemplateEntity baseTemplate, Revision maxRev)
		{
			Revision = Revision.GetNextRev(maxRev);
			Status =new Status(0);
			BaseRevision = baseTemplate.Revision;
			BaseTemplateId = baseTemplate.Id;
			ParentId = baseTemplate.ParentId;
			AuthorComment = baseTemplate.AuthorComment + "\n";
			CheckerComment = baseTemplate.CheckerComment + "\n";
			ApproverComment = baseTemplate.ApproverComment + "\n";
		}

		public void SaveTemplate(
			string description,
			Date updateDate,
			string authorComment
			)
		{
			Description= description;
			LastUpdate= updateDate;
			AuthorComment= authorComment;
		}
	}
}
