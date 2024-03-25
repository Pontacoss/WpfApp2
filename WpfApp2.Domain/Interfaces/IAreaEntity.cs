using System.Collections.Generic;
using WpfApp2.Domain.Entities;

namespace WpfApp2.Domain.InterFaces
{
	public interface IAreaEntity 
	{
		string ExtractString(bool language);
		//public abstract void Copy(AreaEntityBase parent);
		//public abstract void ReplaceParamId(Dictionary<string, string> oldNewParamIdPairs);
		//public abstract void DeleteParameter(ParameterEntity p);
	}
}
