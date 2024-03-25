using System;
using System.Collections.Generic;
using System.Linq;
using WpfApp2.Domain.Helpers;
using WpfApp2.Domain.Entities;
using WpfApp2.Domain.ValueObjects;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace WpfApp2.Domain.DTO
{
    public sealed class ProjectListDTO : INotifyPropertyChanged
    {
		public event PropertyChangedEventHandler PropertyChanged;
		private bool RaisePropertyChanged<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
		{
			if (Equals(field, value)) return false;

			field = value;

			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
			return true;
		}

		public static int Count  = 0;
        public int No { get; set; }

		private int _developmentOrderId;
		public int DevelopmentOrderId
		{
			get => _developmentOrderId;
			set { RaisePropertyChanged(ref _developmentOrderId, value); }
		}

		private string _developmentOrderNo;
		public string DevelopmentOrderNo
		{
			get => _developmentOrderNo;
			set { RaisePropertyChanged(ref _developmentOrderNo, value); }
		}

		private string _customerName;
		public string CustomerName
		{
			get => _customerName;
			set { RaisePropertyChanged(ref _customerName, value); }
		}
		
		private string _projectName;
		public string ProjectName
		{
			get => _projectName;
			set { RaisePropertyChanged(ref _projectName, value); }
		}

		private int _modelId;
		public int ModelId
		{
			get => _modelId;
			set { RaisePropertyChanged(ref _modelId, value); }
		}

		private string _modelName;
        public string ModelName
		{
			get => _modelName;
			set { RaisePropertyChanged(ref _modelName, value); }
		}

		private int _specSheetId;
		public int SpecSheetId
		{
			get => _specSheetId;
			set { RaisePropertyChanged(ref _specSheetId, value); }
		}

		private string _specSheetName;
		public string SpecSheetName
		{
			get => _specSheetName;
			set { RaisePropertyChanged(ref _specSheetName, value); }
		}

		private int _templateCombinationId;
		public int TemplateCombinationId
		{
			get => _templateCombinationId;
			set { RaisePropertyChanged(ref _templateCombinationId, value); }
		}

		private Revision _selectedRevision;
		public Revision SelectedRevision
		{
			get => _selectedRevision;
			set { RaisePropertyChanged(ref _selectedRevision, value); }
		}

		private Revision _latestRevision;
		public Revision LatestRevision
		{
			get => _latestRevision;
			set 
			{
				RaisePropertyChanged(ref _latestRevision, value);
				if (LatestRevision.CompareTo(SelectedRevision) > 0) IsOld = true;
				else IsOld = false;
			}
		}

		private bool _isOld = false;
		public bool IsOld
		{
			get => _isOld;
			set { RaisePropertyChanged(ref _isOld, value); }
		}

		private string _seriesNo;
		public string SeriesNo
		{
			get => _seriesNo;
			set { RaisePropertyChanged(ref _seriesNo, value); }
		}
		private string _status;
		public string Status
		{
			get => _status;
			set { RaisePropertyChanged(ref _status, value); }
		}

        public ProjectListDTO() { }

		public ProjectListDTO(int developmentOrderId,
			string developmentOrderNo, string customerName, string projectName,
			int modelId, string modelName, int specSheetId, string specSheetName,
			int templateCombinationId, string lastRevision, string revision, string seriesNo)
		{
			No = ++Count;
			DevelopmentOrderId = developmentOrderId;
			DevelopmentOrderNo = developmentOrderNo;
			CustomerName = customerName;
			ProjectName = projectName;
			ModelId = modelId;
			ModelName = modelName;
			SpecSheetId = specSheetId;
			SpecSheetName = specSheetName;
			TemplateCombinationId = templateCombinationId;
			SelectedRevision = new Revision(revision);
			LatestRevision = new Revision(lastRevision);
			SeriesNo = seriesNo;

//#if DEBUG
//			constructorCounter++;
//			Console.WriteLine("ProjectListDTO Constructor" + constructorCounter);
//#endif

		}

//		private static int constructorCounter = 0;
//		private static int destructorCounter = 0;

//		~ProjectListDTO()
//		{
//#if DEBUG
//			destructorCounter++;
//			Console.WriteLine("ProjectListDTO Destructor" + destructorCounter);
//#endif
//		}

		public static List<ProjectListDTO> SynthesizeTables(
           List<DevelopmentOrderEntity> deo,
           List<CustomerEntity> ce,
           List<ProjectEntity> pe,
           List<ModelEntity> me,
           List<SpecSheetBaseEntity> ssb,
           List<TemplateCombinationEntity> sst,
           List<TemplateDataEntity> ssd)
        {
            ///Customer  1 - 0...* Project
			var list = ce.GroupJoin(pe, x => x.Id, y => y.CustomerId,
				       (a, b) => new { a, b })
				       .SelectMany(x => x.b.DefaultIfEmpty(), (x, y) =>
				   new
				   {
					   DevelopmentOrderId = y == null ? 0 : y.DevelopmentOrderId,
					   CustomerId = x.a.Id,
					   x.a.CustomerName,
					   ProjectId = y == null ? 0 : y.Id,
					   ProjectName = y == null ? string.Empty : y.ProjectName
				   }).ToList();

			/// Model 1 - * SpecSheetBase
			//var list1 = me.Join(ssb, x => x.Id, y => y.ModelId,
			//    (x, y) => new { ModelId=x.Id, ModelName = x.ModelName.Value, SpecSheetBaseId = y.Id, y.SpecSheetId });
			var list1 = me.GroupJoin(ssb, x => x.Id, y => y.ModelId, (a, b) => new { a, b })
					   .SelectMany(x => x.b.DefaultIfEmpty(), (x, y) =>  new
					   {
						   DevelopmentOrderId = y == null ? 0 : y.DevelopmentOrderId,
						   ModelId = x.a.Id,
						   ModelName = x.a.ModelName.Value,
						   LatestRevision=x.a.LatestRevision==null?string.Empty: x.a.LatestRevision.Value,
						   SpecSheetId = y == null ? 0 : y.Id,
						   SpecSheetName = y == null ? String.Empty : y.SpecSheetName.Value,
						   TemplateCombinationId = y == null ? 0 : y.TemplateCombinationId,
					   }).ToList();

			///  list1  1 - 0...* TemplateConbination
			var list2 = list1.GroupJoin(sst, x => x.TemplateCombinationId, y => y.Id,
                   (a, b) => new { a, b })
                   .SelectMany(x => x.b.DefaultIfEmpty(),
                   (x, y) => new
                   {
					   x.a.DevelopmentOrderId,
					   x.a.ModelId,
					   x.a.ModelName,
					   x.a.SpecSheetId,
					   x.a.SpecSheetName ,
					   x.a.TemplateCombinationId,
					   x.a.LatestRevision,
					   SelectedRevision = y == null ? string.Empty : y.Revision.Value
                   }).ToList();

			/// list2  1 - 0...* TemplateData
			var list3 = list2.GroupJoin(ssd, x => x.TemplateCombinationId, y => y.TemplateCombinationId,
                    (a, b) => new { a, b })
                    .SelectMany(x => x.b.DefaultIfEmpty(),
                    (x, y) => new
                    {
						x.a.DevelopmentOrderId,
						x.a.ModelId,
						x.a.ModelName,
						x.a.SpecSheetId,
						x.a.SpecSheetName,
						x.a.TemplateCombinationId,
						x.a.LatestRevision,
						x.a.SelectedRevision,
						SeriesNo =y==null?string.Empty:y.SeriesNo
					 }).ToList();

			/// DevOrder  1  -  1 list
			var list4 = deo.Join(list, x => x.Id, y => y.DevelopmentOrderId,
			   (x, y) => new
			   {
				   x.DevelopmentOrderNo, 
				   DevelopmentOrderId=x.Id,
				   y.CustomerName,
				   y.ProjectName });

            /// list4  1 -  0...*  list3
            var list5= list4.GroupJoin(list3, x => x.DevelopmentOrderId, y => y.DevelopmentOrderId,
					(a, b) => new { a, b })
					.SelectMany(x => x.b.DefaultIfEmpty(),
					(x, y) => new ProjectListDTO(
						x.a.DevelopmentOrderId,
                        x.a.DevelopmentOrderNo,
                        x.a.CustomerName,
                        x.a.ProjectName,
						y == null ? 0 : y.ModelId,
                        y == null ? string.Empty : y.ModelName,
                        y == null ? 0 : y.SpecSheetId,
						y == null ? string.Empty : y.SpecSheetName,
						y == null ? 0 : y.TemplateCombinationId,
						y == null ? string.Empty : y.LatestRevision,
						y == null ? string.Empty : y.SelectedRevision,
                        y == null ? string.Empty : y.SeriesNo
                        )
                    { }).ToList();
			var list6 = list5.OrderByDescending(x => x.SelectedRevision);
			return list6.ToList();//.Distinct(x=>x.DevelopmentOrderId).ToList();
            
		}

	}
}
