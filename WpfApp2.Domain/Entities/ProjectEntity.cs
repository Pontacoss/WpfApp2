using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Domain.Entities
{
	public sealed class ProjectEntity
	{
		public int Id { get; set; }

		public int CustomerId { get; set; }
		public int DevelopmentOrderId { get; set; }

		[StringLength(30)]
		public string ProjectName { get; set; }

		public ProjectEntity() { }

		public ProjectEntity(int developmentOrderId, int customerId, string projectName)
		{
			DevelopmentOrderId = developmentOrderId;
			CustomerId = customerId;
			ProjectName = projectName;
		}
	}
}
