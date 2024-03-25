using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Domain.DTO;

namespace WpfApp2.WPFForm2.Interfaces
{
	public interface ICompositionRegionElements
	{
		void AddParameter(ParamTreeDTO pt);
		void SaveAreaData();
	}
}
