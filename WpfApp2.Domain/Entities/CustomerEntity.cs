using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Domain.ValueObjects;

namespace WpfApp2.Domain.Entities
{
	public sealed class CustomerEntity 
	{
		public int Id { get; set; }

		[StringLength(20)]
		public string CustomerName { get; set; }

		public CustomerEntity() { }
		public CustomerEntity(string name)
		{
			CustomerName = name;
		}
	}
}
