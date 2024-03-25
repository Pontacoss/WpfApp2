using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WpfApp2.Domain.DTO;
using WpfApp2.Domain.Entities;
using System.Text;
using WpfApp2.Domain.ValueObjects;
using SQLWriter;

namespace WpfApp2.WPFForm2.Helpers
{
	public class PreviewHelper
	{
		// SpecDataForComp → dummyEntity

		public static List<PreviewSequenceDTO> GetSequence(List<CompositionEntity> comps, List<dummyDTO> sdComps=null)
		{
			List<PreviewSequenceDTO> sequences = new List<PreviewSequenceDTO>();
			Dictionary<int, int> counter = new Dictionary<int, int>();
			Dictionary<int, int> upper = new Dictionary<int, int>();

			var seachList = comps.Where(x => x.ParentNodeId == 0).ToList();
			foreach (var comp in seachList)
			{
				if (sdComps != null)
				{
					var sdComp = sdComps.SingleOrDefault(x => x.BaseCompId == comp.Id);
					upper.Add(comp.Id, sdComp.InputDataTables[0].Rows.Count);
					counter.Add(comp.Id, 0);
				}
				else
				{
					upper.Add(comp.Id, 1);
					counter.Add(comp.Id, 0);
				}
			}

			while (seachList.Count > 0)
			{
				var p = seachList.First();

				sequences.Add(new PreviewSequenceDTO(counter[p.Id], p));
				counter[p.Id]++;
				var children = comps.Where(x => x.ParentNodeId == p.Id).ToList();
				foreach (var child in children)
				{
					sequences.Add(new PreviewSequenceDTO(counter[p.Id], child));
				}
				if (counter[p.Id] >= upper[p.Id])
				{
					seachList.Remove(p);
				}
			}
			return sequences;
		}

		public static Dictionary<string, string> GetEmbeddedParameter(TextAreaEntity te,bool language)
		{
			int[] args = te.Get_Args();
			var dic = new Dictionary<string, string>();
			for (int i = 0; i < args.Count(); i++)
			{
				if (args[i] > 0)
				{
					var p = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<ParameterEntity>(
						new SQLFilter(nameof(ParameterEntity.Id), args[i])).Single();
					var name=string.Empty;
					if (p != null)
						name = language ? p.NameJP.Value : p.NameEN.Value
								== string.Empty ? p.NameJP.Value : p.NameEN.Value;
					dic.Add("{" + i + "}", name);
				}
			}
			return dic;
		}
	}

	public abstract class CompositionPreview
	{
		protected CompositionEntity _comp;
		protected bool _language;

		public abstract UIElement Preview(dummyDTO sdComp, bool language, int variableIndex);
		public abstract StringBuilder HtmlPreview(dummyDTO sdComp, bool language, int variableIndex);
	}

	public class FigureAreaPreview : CompositionPreview
	{
		public FigureAreaPreview(CompositionEntity comp)
		{
			base._comp = comp;
		}

		public override StringBuilder HtmlPreview(dummyDTO sdComp, bool language, int variableIndex)
		{
			return new StringBuilder();
		}

		public override UIElement Preview(dummyDTO sdComp, bool language, int variableIndex)
		{
			return new TextBox();
		}
	}

	public class TextAreaPreview : CompositionPreview
	{
		public TextAreaPreview(CompositionEntity comp)
		{
			base._comp = comp;
		}

		public override StringBuilder HtmlPreview(dummyDTO sdComp, bool language, int variableIndex)
		{
			_language = language;
			
			var ta = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TextAreaEntity>(
						new SQLFilter(nameof(TextAreaEntity.Id), _comp.ContentId)).Single();

			if (ta == null) return new StringBuilder();

			var dic = PreviewHelper.GetEmbeddedParameter(ta, _language);

			var dic2 = new Dictionary<string, string>();
			if (dic.Count > 0)
			{
				if (sdComp != null)
				{
					var row = sdComp.InputDataTables[0].Rows[variableIndex];

					foreach (var key in dic.Keys)
					{
						if (dic[key] == "自動インクリメント") dic2.Add(key, (variableIndex + 1).ToString());
						else dic2.Add(key, row[dic[key]].ToString());
					}
				}
			}

			var dic3 = dic2.Count == 0 ? dic : dic2;

			StringBuilder ss=new StringBuilder();
			
			ss.Append(_language ? ta.Value : ta.ValueEN);
			
			ss.Append("<br><br>");

			foreach (var key in dic3.Keys)
			{
				ss = ss.Replace(key, "<span style=\"color:Red\">【" + dic3[key] + "】</span>");
			}
			

			return ss;
		}

		public override UIElement Preview(dummyDTO sdComp, bool language, int variableIndex)
		{
			_language = language;
			TextBox previewTextBox = new TextBox()
			{
				Margin = new Thickness(5, 5, 5, 5),
				HorizontalAlignment = HorizontalAlignment.Left,
				IsReadOnly = true,
				BorderBrush = Brushes.White,
				TextWrapping = TextWrapping.Wrap,
				FontSize = 18,
				Width = 500,
			};

			if (_comp.IsVariable)
			{
				previewTextBox.Background = Brushes.AliceBlue;
			}
			if (_comp.IsHide)
			{
				previewTextBox.Background = Brushes.LightGray;
			}

			var s = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TextAreaEntity>(
						new SQLFilter(nameof(TextAreaEntity.Id), _comp.ContentId)).Single();

			if (s == null) return previewTextBox;

			var dic = PreviewHelper.GetEmbeddedParameter(s, _language);

			var dic2 = new Dictionary<string, string>();
			if (dic.Count > 0)
			{
				if (sdComp != null)
				{
					var row = sdComp.InputDataTables[0].Rows[variableIndex];

					foreach (var key in dic.Keys)
					{
						if (dic[key] == "自動インクリメント") dic2.Add(key, (variableIndex + 1).ToString());
						else dic2.Add(key, row[dic[key]].ToString());
					}
				}
			}

			var dic3 = dic2.Count == 0 ? dic : dic2;

			var ss = _language ? s.Value : s.ValueEN;
			foreach (var key in dic3.Keys)
			{
				ss = ss.Replace(key, "【" + dic3[key] + "】");
			}
			previewTextBox.Text = ss;

			return previewTextBox;
		}
	}

	public class TableAreaPreview : CompositionPreview
	{
		private DataTable _dt = new DataTable();
		private DataGrid _gr;
		private TableAreaEntity _previewTarget;
		private int[] _rowLabelIdArray;
		private int[] _columnLabelIdArray;
		private int[] _valueLabelIdArray;
		private List<ParameterEntity> _parameterList = new List<ParameterEntity>();


		public TableAreaPreview(CompositionEntity comp)
		{
			base._comp = comp;
		}

		public override UIElement Preview(dummyDTO sdCompe, bool language, int variableIndex)
		{
			_language = language;

			StackPanel stackPanel = new StackPanel()
			{
				Orientation = Orientation.Vertical,
				HorizontalAlignment = HorizontalAlignment.Center,
			};

			TextBox TitleTextBox = new TextBox()
			{
				Margin = new Thickness(5, 5, 5, 0),
				HorizontalAlignment = HorizontalAlignment.Center,
				IsReadOnly = true,
				BorderBrush = Brushes.White,
			};

			_gr = new DataGrid()
			{
				Margin = new Thickness(5, 0, 5, 5),
				AutoGenerateColumns = true,
				IsReadOnly = true,
				HorizontalAlignment = HorizontalAlignment.Left,
			};

			if (_comp.IsVariable)
			{
				stackPanel.Background = Brushes.AliceBlue;
			}
			if (_comp.IsHide)
			{
				stackPanel.Background = Brushes.LightGray;
			}

			_parameterList = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<ParameterEntity>();
			_previewTarget = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<TableAreaEntity>(
						new SQLFilter(nameof(TextAreaEntity.Id), _comp.ContentId)).Single();


			if (_previewTarget == null) return stackPanel;

			if (_language) TitleTextBox.Text = "表.x " + _previewTarget.TitleString;
			else TitleTextBox.Text = "Figure.x " + _previewTarget.TitleStringEN;

			string[] Label_array;
			Label_array = _previewTarget.RowLabels.Split(',');
			if (!string.IsNullOrEmpty(Label_array[0])) _rowLabelIdArray = Label_array.Select(s => int.Parse(s)).ToArray();

			Label_array = _previewTarget.ColumnLabels.Split(',');
			if (!string.IsNullOrEmpty(Label_array[0])) _columnLabelIdArray = Label_array.Select(s => int.Parse(s)).ToArray();

			Label_array = _previewTarget.ValueLabels.Split(',');
			if (!string.IsNullOrEmpty(Label_array[0])) _valueLabelIdArray = Label_array.Select(s => int.Parse(s)).ToArray();

			ConstractColumns();

			ConstractRowLabel();

			if (!_language)
			{
				if (_previewTarget.Direction == false)
				{
					if (_valueLabelIdArray != null) _dt.Columns["項目"].ColumnName = "items";
					if (_columnLabelIdArray == null) _dt.Columns["値"].ColumnName = "values";
				}
			}
			_gr.ItemsSource = _dt.DefaultView;

			stackPanel.Children.Add(TitleTextBox);
			stackPanel.Children.Add(_gr);

			return stackPanel;
		}

		private int ConvertRowNumberToArrayNumber(int x, int y, int z)
		{
			int i = x / y;
			while (i >= z)
			{
				i -= z;
			}
			return i;
		}

		private void ConstractRowLabel()
		{
			int counter = 1;
			if (_rowLabelIdArray != null)
			{
				string[] value_array;

				foreach (var id in _rowLabelIdArray)
				{
					ParameterEntity param = _parameterList.Single(x => x.Id == id);

					if (param.Value != null && param.Value.Length > 0)
					{
						value_array = param.Value.Split(',');
						counter *= value_array.Count();
					}
				}
				if (_previewTarget.Direction == false && _valueLabelIdArray != null) counter *= _valueLabelIdArray.Count();

				for (int i = 0; i < counter; i++)
				{
					int magnification = counter;
					var row = _dt.NewRow();
					foreach (var id in _rowLabelIdArray)
					{
						ParameterEntity param = _parameterList.Single(x => x.Id == id);
						var name = ParamName.GetName(_language, param.NameJP, param.NameEN);
						if (param.Value != null && param.Value.Length > 0)
						{
							value_array = param.Value.Split(',');
							magnification /= value_array.Count();

							if (i % magnification == 0)
							{
								row[name] = value_array[ConvertRowNumberToArrayNumber(i, magnification, value_array.Count())];
							}


						}
					}
					if (_previewTarget.Direction == false && _valueLabelIdArray != null)
					{
						magnification /= _valueLabelIdArray.Count();

						ParameterEntity param2 = _parameterList.Single(x => x.Id ==
										_valueLabelIdArray[ConvertRowNumberToArrayNumber(i, magnification, _valueLabelIdArray.Count())]);
						var name = ParamName.GetName(_language, param2.NameJP, param2.NameEN);
						row["項目"] = name;

					}

					_dt.Rows.Add(row);
				}
			}
			if (counter == 1)
			{
				if (_previewTarget.Direction == false)
				{
					if (_valueLabelIdArray != null)
					{
						foreach (var id in _valueLabelIdArray)
						{
							ParameterEntity param = _parameterList.Single(x => x.Id == id);
							var name = ParamName.GetName(_language, param.NameJP, param.NameEN);
							var row = _dt.NewRow();
							row["項目"] = name;
							_dt.Rows.Add(row);
						}
					}
				}
			}
		}

		private void ConstractColumns()
		{
			if (_rowLabelIdArray != null)
			{
				foreach (var LabelId in _rowLabelIdArray)
				{
					ParameterEntity param = _parameterList.Single(x => x.Id == LabelId);
					var name = ParamName.GetName(_language, param.NameJP, param.NameEN);
					_dt.Columns.Add(name, Type.GetType("System.String"));
				}
			}

			if (_previewTarget.Direction == false)
			{
				_dt.Columns.Add("項目", Type.GetType("System.String"));
			}

			if (_columnLabelIdArray != null)
			{
				foreach (var LabelId in _columnLabelIdArray)
				{
					ParameterEntity param = _parameterList.Single(x => x.Id == LabelId);
					var name = ParamName.GetName(_language, param.NameJP, param.NameEN); 

					if (param.Value != null && param.Value.Length > 0)
					{
						var value_array = param.Value.Split(',');
						foreach (var val in value_array)
						{
							if (_previewTarget.Direction == true && _valueLabelIdArray != null)
							{
								foreach (var v in _valueLabelIdArray)
								{
									ParameterEntity valParam = _parameterList.Single(x => x.Id == v);
									var valname = ParamName.GetName(_language, valParam.NameJP, valParam.NameEN); 
									_dt.Columns.Add(name + "\n" + val + "\n" + valname, Type.GetType("System.String"));
								}
							}
							else
							{
								_dt.Columns.Add(name + "\n" + val, Type.GetType("System.String"));
							}
						}
					}
					else
					{
						if (_previewTarget.Direction == true && _valueLabelIdArray != null)
						{
							foreach (var v in _valueLabelIdArray)
							{
								ParameterEntity valParam = _parameterList.Single(x => x.Id == v);
								var valname = ParamName.GetName(_language, valParam.NameJP, valParam.NameEN);
								_dt.Columns.Add(name + "\n" + valname, Type.GetType("System.String"));
							}
						}
						else
						{
							_dt.Columns.Add(name, Type.GetType("System.String"));
						}
					}
				}
			}
			else if (_previewTarget.Direction == true && _valueLabelIdArray != null)
			{
				foreach (var v in _valueLabelIdArray)
				{
					ParameterEntity valParam = _parameterList.SingleOrDefault(x => x.Id == v);
					if (valParam == null) continue;
					var valname = ParamName.GetName(_language, valParam.NameJP, valParam.NameEN);
					_dt.Columns.Add(valname, Type.GetType("System.String"));
				}
			}
			else if (_previewTarget.Direction == false)
			{
				_dt.Columns.Add("値", Type.GetType("System.String"));
			}
		}

		public override StringBuilder HtmlPreview(dummyDTO sdComp, bool language, int variableIndex)
		{
			return new StringBuilder();
		}
	}
}
