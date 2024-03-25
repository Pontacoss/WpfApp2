using System.Collections.Generic;
using WpfApp2.Domain.Entities;
using WpfApp2.Domain.DTO;

using System.Linq;
using SQLWriter;

namespace WpfApp2.WPFForm2.Helpers
{
	public class TextParamDTOHelper
	{
		public static List<TextParamDTO> GetDataGridSource(TextAreaEntity te)
		{
			List<TextParamDTO> textParams = new List<TextParamDTO>();

			var pArray = te.Get_Args();
			var i = 0;
			foreach (var arg in pArray)
			{
				if (arg > 0)
				{
					var p = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<ParameterEntity>
						(new SQLFilter(nameof(ParameterEntity.Id),arg)).SingleOrDefault();
					if (p == null) textParams.Add(new TextParamDTO(i, string.Empty, 0, string.Empty)); 
					else textParams.Add(new TextParamDTO(i, p.NameJP.Value, p.Id,p.Value));
				}
				else if(arg == 0) textParams.Add(new TextParamDTO(i, "", 0, ""));
				i++;
			}

			return textParams;
		}
	}
}
