using System.Collections.Generic;
using System.Linq;
using WpfApp2.Domain.Entities;
using WpfApp2.Domain.DTO;
using WpfApp2.Domain.ValueObjects;
using SQLWriter;
using WpfApp2.Domain.InterFaces;

namespace WpfApp2.WPFForm2.Helpers
{
	public class CompositionDTOHelper
	{
		public static CompositionEntity ConvertDTOToComposition(CompositionDTO dto)
		{
			return new CompositionEntity()
			{
				Id = dto.Id,
				ParentId = dto.ParentId,
				Sequence = dto.Sequence,
				Level = dto.Level,
				AreaType = dto.AreaType,
				IsVariable = dto.IsVariable,
				IsHide = dto.IsHide,
				ParentNodeId = dto.ParentNode.Id,
				Group = dto.Group,
				ContentId = dto.AreaDataId,
				AreaData = dto.AreaData,
			};
		}

		public static List<CompositionDTO> ConvertCompositionToDto(TemplateEntity te)
		{
			List<CompositionDTO> dto = new List<CompositionDTO>();
			var top = new CompositionDTO()
			{
				ParentId = te.Id,
				Level = 0,
				IsVariable = false,
				AreaType = AreaType.None,
				IsEnableControl = false,
			};
			dto.Add(top);

			SQLFilter p = new SQLFilter(nameof(CompositionEntity.ParentId), te.Id);
			var comp = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<CompositionEntity>(p)
									.OrderBy(x => x.Level).ThenBy(x => x.Sequence).ToList();

			foreach (var dbValue in comp)
			{
				dbValue.AreaData = SetAreaData(dbValue);
				var searchList = new List<CompositionDTO>() { top };

				while (searchList.Count > 0)
				{
					if (searchList[0].Id == dbValue.ParentNodeId)
					{
						searchList[0].Children.Add(new CompositionDTO(dbValue as CompositionEntity, searchList[0]));
					}
					var children = searchList[0].Children.OrderBy(x => x.Sequence).ToList();
					foreach (var s in children)
					{
						searchList.Add(s);
					}
					searchList.RemoveAt(0);
				}
			}
			return dto;
		}

		public static CompositionDTO CreateNewComposition(CompositionDTO parentNode, string areaType)
		{
			var dto = new CompositionDTO()
			{
				ParentId = parentNode.ParentId,
				ParentNode = parentNode,
				ParentNodeId = parentNode.Id,
				Sequence = parentNode.Children.Count,
				Level = parentNode.Level + 1,
				AreaType = new AreaType(areaType),
				IsVariable = parentNode.IsVariable,
				IsHide = false,
			};

			if (dto.AreaType==AreaType.TextArea)
			{
				dto.AreaData = new TextAreaEntity();
			}
			else if (dto.AreaType == AreaType.TableArea)
			{
				dto.AreaData = new TableAreaEntity();
			}
			else if (dto.AreaType == AreaType.FigureArea)
			{
				dto.AreaData = new FigureAreaEntity();
			}

			dto.AreaDataId = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Insert(dto.AreaData);

			var pc = new CompositionEntity()
			{
				ParentId = dto.ParentId,
				Sequence = dto.Sequence,
				Level = dto.Level,
				AreaType = dto.AreaType,
				IsVariable = dto.IsVariable,
				IsHide = dto.IsHide,
				ParentNodeId = dto.ParentNode.Id,
				Group = dto.Group,
				ContentId = dto.AreaDataId,
				AreaData=dto.AreaData,
			};
			
			dto.Id = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Insert(pc);

			if (string.IsNullOrEmpty(parentNode.Group))
			{
				dto.Group = dto.Id.ToString();
			}
			else
			{
				dto.Group = parentNode.Group + "-" + dto.Id.ToString();
			}

			return dto;
		}

		public static void MoveInTree(CompositionDTO pt, int direction)
		{
			var prev = pt.ParentNode;
			if (prev == null) return;

			var currentIndex = prev.Children.IndexOf(pt);

			if (direction > 0 && currentIndex == 0) return;
			if (direction < 0 && currentIndex == prev.Children.Count - 1) return;

			var pt2 = prev.Children[currentIndex - direction];
			(pt2.Sequence, pt.Sequence) = (pt.Sequence, pt2.Sequence);

			var list = prev.Children.OrderBy(x => x.Sequence).ToList();
			prev.Children.Clear();
			foreach(var child in list)
			{
				prev.Children.Add(child);
			}
		}

		public static void DeleteInTree(CompositionDTO pt)
		{
			var prev = pt.ParentNode;
			var l1 = new List<CompositionDTO> { pt };
			while (l1.Count > 0)
			{
				var pt1 = l1[0];
				foreach (var child in pt1.Children)
				{
					l1.Add(child);
				}
				l1.RemoveAt(0);
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Delete(pt1.AreaData);
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Delete(ConvertDTOToComposition(pt1));
			}
			prev.Children.Remove(pt);
			var seq = 0;
			foreach(var child in prev.Children)
			{
				child.Sequence = seq++;
				SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(ConvertDTOToComposition(child));
			}
		}

		private static IAreaEntity SetAreaData(CompositionEntity ce)
		{
			IAreaEntity areaData;
			if (ce.AreaType==AreaType.TextArea)
			{
				areaData = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TextAreaEntity>(
					new SQLFilter(nameof(TextAreaEntity.Id),ce.ContentId)).Single();
			}
			else if (ce.AreaType == AreaType.TableArea)
			{
				areaData = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TableAreaEntity>(
					new SQLFilter(nameof(TableAreaEntity.Id), ce.ContentId)).Single(); 
			}
			else if (ce.AreaType == AreaType.FigureArea)
			{
				areaData = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<FigureAreaEntity>(
					new SQLFilter(nameof(FigureAreaEntity.Id), ce.ContentId)).Single(); 
			}
			else areaData = null;

			return areaData;
		}

		public static List<CompositionEntity> ConvertFromDtoToTemplateTreeData(List<CompositionDTO> list)
		{
			List<CompositionDTO> searchList = list;

			List<CompositionEntity> saveList = new List<CompositionEntity>();

			while (searchList.Count > 0)
			{
				var firstElement = searchList.First();

				if (firstElement.Level > 0)
				{
					saveList.Add(ConvertDTOToComposition(firstElement));
				}

				if (firstElement.Children.Count > 0)
				{
					var children = firstElement.Children.OrderBy(x => x.Sequence).ToList();
					foreach (var child in children)
					{
						searchList.Add(child);
					}
				}
				searchList.Remove(firstElement);
			}
			return saveList;
		}
	}
}
