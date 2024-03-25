using System.ComponentModel.DataAnnotations;

namespace WpfApp2.Domain.Entities
{
	public sealed class DevelopmentOrderEntity 
	{
		public int Id { get; set; }

		[StringLength(11)]
		public string DevelopmentOrderNo { get; set; }

		public DevelopmentOrderEntity() { }
		public DevelopmentOrderEntity(string developmentOrderNo)
		{
			DevelopmentOrderNo = developmentOrderNo;
		}
	}
}
