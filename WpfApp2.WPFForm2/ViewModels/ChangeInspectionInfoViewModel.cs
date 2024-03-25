using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using WpfApp2.Domain.Entities;

namespace WpfApp2.WPFForm2.ViewModels
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class ChangeInspectionInfoViewModel : BindableBase, IDialogAware
    {
        #region プロパティ設定	
        private InspectionEntity _entity = new InspectionEntity();
        public InspectionEntity Entity
        {
            get { return _entity; }
            set { _entity = value; }
        }

        private string _clause = string.Empty;
        public string Clause
        {
            get => _clause;
            set { SetProperty(ref _clause, value); }
        }

        private string _nameJP = string.Empty;
        public string NameJP
        {
            get => _nameJP;
            set { SetProperty(ref _nameJP, value); }
        }

        private string _nameEN = string.Empty;
        public string NameEN
        {
            get => _nameEN;
            set { SetProperty(ref _nameEN, value); }
        }

        private bool _typeTest = false;
        public bool TypeTest
        {
            get => _typeTest;
            set { SetProperty(ref _typeTest, value); }
        }

        private bool _routinTest = false;
        public bool RoutinTest
        {
            get => _routinTest;
            set { SetProperty(ref _routinTest, value); }
        }

        private string _addNewButtonName = string.Empty;
        public string AddNewButtonName
        {
            get => _addNewButtonName;
            set { SetProperty(ref _addNewButtonName, value); }
        }
        #endregion

        public ChangeInspectionInfoViewModel()
        {
            CancelButton_Click = new DelegateCommand(CancelButton_ClickExcute);
            ChangeButton_Click = new DelegateCommand(ChangeButton_ClickExcute);

#if DEBUG
            constructorCounter++;
            Console.WriteLine("ChangeInspectionInfoViewModel Constructor" + constructorCounter);
#endif

        }

        private static int constructorCounter = 0;
        private static int destructorCounter = 0;

        ~ChangeInspectionInfoViewModel()
        {
#if DEBUG
            destructorCounter++;
            Console.WriteLine("ChangeInspectionInfoViewModel Destructor" + destructorCounter);
#endif
        }

        public string Title => "試験情報変更";
        public event Action<IDialogResult> RequestClose;

        public DelegateCommand ChangeButton_Click { get; }
        private void ChangeButton_ClickExcute()
        {
            InspectionEntity newEntity;

            newEntity = new InspectionEntity(Entity.ParentId, Clause, NameJP, NameEN, TypeTest, RoutinTest, Entity.Id);

            var p = new DialogParameters();
            p.Add(nameof(Entity), newEntity);
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, p));
        }

        public DelegateCommand CancelButton_Click { get; }
        private void CancelButton_ClickExcute()
        {
            RequestClose?.Invoke(new DialogResult(ButtonResult.Cancel, null));
        }

        public bool CanCloseDialog() => true;
        public void OnDialogClosed() { }
        public void OnDialogOpened(IDialogParameters parameters)
        {
            Entity = parameters.GetValue<InspectionEntity>(nameof(Entity));
            if (Entity.Id == 0)
            {
                AddNewButtonName = "新規追加";
            }
            else AddNewButtonName = "更新";

            Clause = Entity.Clause.Value;
            NameJP = Entity.NameJP.Value;
            NameEN = Entity.NameEN.Value;
            TypeTest = Entity.TypeTest;
            RoutinTest = Entity.RoutinTest;
        }
    }
}
