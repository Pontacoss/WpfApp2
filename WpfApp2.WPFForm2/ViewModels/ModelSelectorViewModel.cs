using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using SQLWriter;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using WpfApp2.Domain.DTO;
using WpfApp2.Domain.Entities;

namespace WpfApp2.WPFForm2.ViewModels
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class ModelSelectorViewModel : BindableBase, IDialogAware
    {
        #region Property Setting
        private string _modelNameTextPrev;
        public string ModelNameTextPrev
        {
            get => _modelNameTextPrev;
            set { SetProperty(ref _modelNameTextPrev, value); }
        }
        private string _modelNameTextMid;
        public string ModelNameTextMid
        {
            get => _modelNameTextMid;
            set { SetProperty(ref _modelNameTextMid, value); }
        }
        private string _modelNameTextBack;
        public string ModelNameTextBack
        {
            get => _modelNameTextBack;
            set { SetProperty(ref _modelNameTextBack, value); }
        }
        private string _selectedModelMidAlphabet;
        public string SelectedModelMidAlphabet
        {
            get => _selectedModelMidAlphabet;
            set { SetProperty(ref _selectedModelMidAlphabet, value); }
        }
        private ProjectListDTO _targetDevOrder;
        public ProjectListDTO TargetDevOrder
        {
            get => _targetDevOrder;
            set { SetProperty(ref _targetDevOrder, value); }
        }

        private ObservableCollection<ModelEntity> _modelList = new ObservableCollection<ModelEntity>();
        public ObservableCollection<ModelEntity> ModelList
        {
            get => _modelList;
            set { SetProperty(ref _modelList, value); }
        }

        private ObservableCollection<string> _modelMidAlphabet = new ObservableCollection<string>();
        public ObservableCollection<string> ModelMidAlphabet
        {
            get => _modelMidAlphabet;
            set { SetProperty(ref _modelMidAlphabet, value); }
        }

        private ModelEntity _selectedModel;
        public ModelEntity SelectedModel
        {
            get => _selectedModel;
            set { SetProperty(ref _selectedModel, value); }
        }
        private SolidColorBrush _modelNameTextPrevBackground
            = new BrushConverter().ConvertFrom("White") as SolidColorBrush;
        public SolidColorBrush ModelNameTextPrevBackground
        {
            get => _modelNameTextPrevBackground;
            set { SetProperty(ref _modelNameTextPrevBackground, value); }
        }
        private SolidColorBrush _modelNameTextMidBackground
            = new BrushConverter().ConvertFrom("White") as SolidColorBrush;
        public SolidColorBrush ModelNameTextMidBackground
        {
            get => _modelNameTextMidBackground;
            set { SetProperty(ref _modelNameTextMidBackground, value); }
        }
        private SolidColorBrush _modelNameTextBackBackground
            = new BrushConverter().ConvertFrom("White") as SolidColorBrush;
        public SolidColorBrush ModelNameTextBackBackground
        {
            get => _modelNameTextBackBackground;
            set { SetProperty(ref _modelNameTextBackBackground, value); }
        }
        private SolidColorBrush _modelMidAlphabetBackground
            = new BrushConverter().ConvertFrom("White") as SolidColorBrush;
        public SolidColorBrush ModelMidAlphabetBackground
        {
            get => _modelMidAlphabetBackground;
            set { SetProperty(ref _modelMidAlphabetBackground, value); }
        }
        #endregion

        public string Title { get; set; }
        public event Action<IDialogResult> RequestClose;

        #region コンストラクター
        public ModelSelectorViewModel()
        {
            CreateNewModelButtonClick = new DelegateCommand(CreateNewModelButtonClickExecute);
            SelectModelButtonClick = new DelegateCommand(SelectModelButtonClickExecute);
            ModelMidAlphabet.Add("V");
            ModelMidAlphabet.Add("VD");
#if DEBUG
            constructorCounter++;
            Console.WriteLine("ModelSelectorViewModel Constructor" + constructorCounter);
#endif

        }

        private static int constructorCounter = 0;
        private static int destructorCounter = 0;

        ~ModelSelectorViewModel()
        {
#if DEBUG
            destructorCounter++;
            Console.WriteLine("ModelSelectorViewModel Destructor" + destructorCounter);
#endif
        }
        #endregion


        public DelegateCommand SelectModelButtonClick { get; }
        private void SelectModelButtonClickExecute()
        {
            IDialogParameters p = new DialogParameters();
            p.Add(nameof(SelectedModel), SelectedModel);
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, p));
        }

        public DelegateCommand CreateNewModelButtonClick { get; }
        private void CreateNewModelButtonClickExecute()
        {
            bool rackFlag = false;
            ModelNameTextPrevBackground = new BrushConverter().ConvertFrom("White") as SolidColorBrush;
            ModelNameTextMidBackground = new BrushConverter().ConvertFrom("White") as SolidColorBrush;
            ModelNameTextBackBackground = new BrushConverter().ConvertFrom("White") as SolidColorBrush;
            ModelMidAlphabetBackground = new BrushConverter().ConvertFrom("White") as SolidColorBrush;

            if (ModelNameTextPrev == null || ModelNameTextPrev.Length < 3)
            {
                ModelNameTextPrevBackground = new BrushConverter().ConvertFrom("Pink") as SolidColorBrush;
                rackFlag = true;
            }
            if (ModelNameTextMid == null || ModelNameTextMid.Length < 2)
            {
                ModelNameTextMidBackground = new BrushConverter().ConvertFrom("Pink") as SolidColorBrush;
                rackFlag = true;
            }
            if (ModelNameTextBack == null || ModelNameTextBack.Length < 3)
            {
                ModelNameTextBackBackground = new BrushConverter().ConvertFrom("Pink") as SolidColorBrush;
                rackFlag = true;
            }
            if (rackFlag)
            {
                MessageBox.Show("文字数が足りません。");
                return;
            }
            if (string.IsNullOrEmpty(SelectedModelMidAlphabet))
            {
                ModelMidAlphabetBackground = new BrushConverter().ConvertFrom("Pink") as SolidColorBrush;
                MessageBox.Show("アルファベットを選択してください。");
                return;
            }
            var newModelName = "MAP-" + ModelNameTextPrev + "-"
                + ModelNameTextMid + SelectedModelMidAlphabet + ModelNameTextBack;

            SQLFilter p1 = new SQLFilter(nameof(ModelEntity.ModelName), newModelName);
            var s = SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<ModelEntity>(p1);
            ModelEntity selectedModel;
            if (s.Count > 0)
            {
                var result = MessageBox.Show("この型式は既に登録されています。\n『"
                    + newModelName + "』を使用しますか？", "型式選択", MessageBoxButton.OKCancel);
                if (result == MessageBoxResult.Cancel) return;
                selectedModel = s.First();
            }
            else
            {
                selectedModel = new ModelEntity(newModelName);
                SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(selectedModel);
            }

            IDialogParameters p = new DialogParameters();
            p.Add(nameof(SelectedModel), selectedModel);
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, p));
        }

        #region IDialogAwear
        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            TargetDevOrder = parameters.GetValue<ProjectListDTO>(nameof(TargetDevOrder));
            ModelList.AddRange(SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Load<ModelEntity>());
            Title = TargetDevOrder.DevelopmentOrderNo + " : " + TargetDevOrder.CustomerName + " : " + TargetDevOrder.ProjectName;
        }
        #endregion
    }

}
