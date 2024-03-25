using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp2.Domain.DTO
{
	public sealed class ParamTreeDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Value { get; set; }
		public IList<ParamTreeDTO> Children { get; set; }

		public ParamTreeDTO()
		{
#if DEBUG
			constructorCounter++;
			seq = constructorCounter;
			Console.WriteLine("ParamTreeDTO Constructor " + constructorCounter);
#endif

		}

		private static int constructorCounter = 0;
		private int seq = 0;

		~ParamTreeDTO()
		{
			this.Children = null;
#if DEBUG
			Console.WriteLine("ParamTreeDTO Destructor " + seq);
#endif
		}
	

		public ParamTreeDTO(int id,string name,string value)
		{
			Id = id;
			Name = name;
			Value = value;
#if DEBUG
			constructorCounter++;
			seq = constructorCounter;
			Console.WriteLine("ParamTreeDTO Constructor " + constructorCounter);
#endif
		}
	}
}
