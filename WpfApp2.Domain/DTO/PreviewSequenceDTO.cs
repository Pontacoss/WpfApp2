using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp2.Domain.Entities;

namespace WpfApp2.Domain.DTO
{
	public sealed class PreviewSequenceDTO
	{
		public int RowIndex { get; }
		public CompositionEntity Composition { get; }
		public PreviewSequenceDTO(int rowIndex, CompositionEntity composition)
		{
			RowIndex = rowIndex;
			Composition = composition;
		}
	}
}
