using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Cryptography;
using System.Windows.Documents;
using WpfApp2.Domain.DTO;
using WpfApp2.Domain.ValueObjects;

namespace WpfApp2.Domain.Entities
{
	public sealed class ParameterEntity 
	{
		public int Id { get; set; }

		public int ParentId { get; set; }

		[StringLength(10)]
		public ParamName NameJP { get; set; } = new ParamName(String.Empty);

		[StringLength(20)]
		public ParamName NameEN { get; set; } = new ParamName(String.Empty);
		public int TableId { get; set; }
		public int SelectDBParamId { get; set; }
		public int Sequence { get; set; }
		public InputMethod InputMethod { get; set; }
		public bool IsVariable { get; set; }=false;

		[StringLength(20)]
		public string Group { get; set; } = String.Empty;

		[StringLength(20)]
		public string Value { get; set; } = String.Empty;

		public int FilterParamId { get; set; }
	
		public ParameterEntity() { }

		public ParameterEntity(ParameterDTO pv)
		{
			Id= pv.Id;
			ParentId = pv.ParentId;
			NameJP = new ParamName(pv.NameJP);
			NameEN = new ParamName(pv.NameEN);
			FilterParamId = pv.FilterParamId;
			SelectDBParamId = pv.SelectDBParamId;
			TableId = pv.TableId;
			Value = pv.Value;
			Group = pv.Group;
			IsVariable = pv.IsVariable;
			InputMethod = pv.InputMethod;
			Sequence = pv.Sequence;
		}

		public ParameterEntity(int parentId, int sequence)
		{
			ParentId = parentId;
			Sequence = sequence;
			NameJP = new ParamName(string.Empty);
			NameEN = new ParamName(string.Empty);
			TableId = 0;
			SelectDBParamId = 0;
			InputMethod = InputMethod.Manual;
			IsVariable = false;
			Group = string.Empty;
			Value = string.Empty;
			FilterParamId = 0;
		}

		public ParameterEntity(int parentId,string nameJP, int inputMethod)
		{
			ParentId = parentId;
			NameJP = new ParamName(nameJP);
			InputMethod =new InputMethod(inputMethod);
		}

		public ParameterEntity(TemplateEntity parent)
		{
			ParentId = parent.Id;
		}

		public ParameterEntity(ParameterEntity baseEntity, int baseId)
		{
			ParentId = baseId;
			NameJP = baseEntity.NameJP;
			NameEN = baseEntity.NameEN;
			FilterParamId = baseEntity.FilterParamId;
			SelectDBParamId = baseEntity.SelectDBParamId;
			TableId = baseEntity.TableId;
			Value = baseEntity.Value;
			Group = baseEntity.Group;
			IsVariable = baseEntity.IsVariable;
			InputMethod = baseEntity.InputMethod;
		}

	}
}
