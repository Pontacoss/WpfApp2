using Prism.Mvvm;
using Prism.Regions;
using SQLWriter;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using WpfApp2.Domain.DTO;
using WpfApp2.Domain.Entities;

using WpfApp2.Domain.ValueObjects;
using WpfApp2.Infrastructure;

using WpfApp2.WPFForm2.Helpers;

namespace WpfApp2.WPFForm2.ViewModels
{
	[RegionMemberLifetime(KeepAlive = false)]
	public class PreviewControlViewModel : BindableBase,INavigationAware
	{
		private bool _isHtml=false;
		public bool IsHtml
		{
			get => _isHtml;
			set { SetProperty(ref _isHtml, value); }
		}

		private TemplateEntity _targetTemplate;
		public TemplateEntity TargetTemplate
		{
			get => _targetTemplate;
			set
			{
				_targetTemplate = value;
				AuthorComment = value.AuthorComment;
				CheckerComment = value.CheckerComment;
				ApproverComment = value.ApproverComment;

				List<dummyDTO> sts = null;

				SQLFilter p = new SQLFilter(nameof(CompositionEntity.ParentId), value.Id);
				var compositions = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite)
					.Load<CompositionEntity>(p).OrderBy(x => x.Level).ThenBy(x => x.Sequence).ToList();
				var sequence = PreviewHelper.GetSequence(compositions, sts);

				GetPreviewElements(true, sts, compositions, sequence, PreviewJP,IsHtml);
				GetPreviewElements(false, sts, compositions, sequence, PreviewEN, IsHtml);
			}
		}

		private string _authorComment = null;
		public string AuthorComment
		{
			get => _authorComment;
			set { SetProperty(ref _authorComment, value); }
		}
		private string _checkerComment = null;
		public string CheckerComment
		{
			get => _checkerComment;
			set { SetProperty(ref _checkerComment, value); }
		}
		private string _approverComment = null;
		public string ApproverComment
		{
			get => _approverComment;
			set { SetProperty(ref _approverComment, value); }
		}
		private ObservableCollection<UIElement> _diffMatchJP = new ObservableCollection<UIElement>();
		public ObservableCollection<UIElement> DiffMatchJP
		{
			get => _diffMatchJP;
			set { SetProperty(ref _diffMatchJP, value); }
		}
		private ObservableCollection<UIElement> _previewJP = new ObservableCollection<UIElement>();
		public ObservableCollection<UIElement> PreviewJP
		{
			get => _previewJP;
			set { SetProperty(ref _previewJP, value); }
		}
		private ObservableCollection<UIElement> _previewEN = new ObservableCollection<UIElement>();
		public ObservableCollection<UIElement> PreviewEN
		{
			get => _previewEN;
			set { SetProperty(ref _previewEN, value); }
		}

		public PreviewControlViewModel()
		{
			PreviewJP.Clear();
			PreviewEN.Clear();

//#if DEBUG
//			constructorCounter++;
//			Console.WriteLine("PreviewControlViewModel Constructor" + constructorCounter);
//#endif

		}

//		private static int constructorCounter = 0;
//		private static int destructorCounter = 0;

//		~PreviewControlViewModel()
//		{
//#if DEBUG
//			destructorCounter++;
//			Console.WriteLine("PreviewControlViewModel Destructor" + destructorCounter);
//#endif
//		}

		private void GetPreviewElements(bool language, List<dummyDTO> sts,
			List<CompositionEntity> compositions, List<PreviewSequenceDTO> sequence,
			ObservableCollection<UIElement> previewPanel,bool isHtml)
		{
			TextBlock TemplateNameTextBlock = new TextBlock();
			var ie = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<InspectionEntity>(
						new SQLFilter(nameof(InspectionEntity.Id), _targetTemplate.ParentId)).Single();
			var name = language? ie.NameJP.Value:ie.NameEN.Value ?? ie.NameJP.Value;
			TemplateNameTextBlock.Inlines.Add(new Bold(new Run(name)));
			TemplateNameTextBlock.FontSize = 16;
			TemplateNameTextBlock.TextWrapping = TextWrapping.Wrap;

			previewPanel.Add(TemplateNameTextBlock);
			StringBuilder sb = new StringBuilder();
			sb.Append("<html><head><meta http-equiv='Content-Type' content='text/html;charset=UTF-8'></head><body><font size=\"4\">");
			WebBrowser wb = new WebBrowser()
			{
				Margin = new Thickness(5, 5, 5, 5),
			};
			
			//var sts = st == null ? null : st.SpecDataForComps;

			foreach (var seq in sequence)
			{
				var sc = sts == null ? null : sts.Single(x => x.BaseCompId == seq.Composition.Id);

				CompositionPreview compositionPreview;
				if (seq.Composition.AreaType == AreaType.TextArea)
				{
					compositionPreview = new TextAreaPreview(seq.Composition);
				}
				else if (seq.Composition.AreaType == AreaType.TableArea)
				{
					compositionPreview = new TableAreaPreview(seq.Composition);
				}
				else if (seq.Composition.AreaType == AreaType.FigureArea)
				{
					compositionPreview = new FigureAreaPreview(seq.Composition);
				}
				else throw new NotImplementedException();

				if(isHtml) sb.Append(compositionPreview.HtmlPreview(sc, language, seq.RowIndex));
				else previewPanel.Add(compositionPreview.Preview(sc, language, seq.RowIndex));
			}
			if (isHtml)
			{
				sb.Append("</font></body></html>");
				sb = sb.Replace("\r\n", "<br>");
				wb.NavigateToString(sb.ToString());
				previewPanel.Add(wb);
				
			}
		}

		public void OnNavigatedTo(NavigationContext navigationContext)
		{
			TargetTemplate = navigationContext.Parameters.GetValue<TemplateEntity>(nameof(TargetTemplate));
		}

		public bool IsNavigationTarget(NavigationContext navigationContext) => false;
		

		public void OnNavigatedFrom(NavigationContext navigationContext)
		{
		}
	}
}
