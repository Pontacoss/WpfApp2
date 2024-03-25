using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WpfApp2.Domain.Entities;

namespace WpfApp2.WPFForm2.ViewModels
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class RevisionSelectorViewModel : BindableBase, IDialogAware
    {
        public event Action<IDialogResult> RequestClose;

        private ObservableCollection<TemplateEntity> _entities = new ObservableCollection<TemplateEntity>();
        public ObservableCollection<TemplateEntity> Entities
        {
            get { return _entities; }
            set { SetProperty(ref _entities, value); }
        }

        private TemplateEntity _selectedRevision;
        public TemplateEntity SelectedRevision
        {
            get { return _selectedRevision; }
            set { SetProperty(ref _selectedRevision, value); }
        }

        public string Title => "Revision Selector";

        public DelegateCommand SelectRevisionButton_Click { get; }
        public DelegateCommand PreviewButton_Click { get; }

        private IDialogService _dialogService;
        public RevisionSelectorViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;
            SelectRevisionButton_Click = new DelegateCommand(SelectRevisionButton_ClickExecute);
            PreviewButton_Click = new DelegateCommand(PreviewButton_ClickExecute);

#if DEBUG
            constructorCounter++;
            Console.WriteLine("RevisionSelectorViewModel Constructor" + constructorCounter);
#endif

        }

        private static int constructorCounter = 0;
        private static int destructorCounter = 0;

        ~RevisionSelectorViewModel()
        {
#if DEBUG
            destructorCounter++;
            Console.WriteLine("RevisionSelectorViewModel Destructor" + destructorCounter);
#endif
        }

        private void SelectRevisionButton_ClickExecute()
        {
            //if (Status.CanSelect(SelectedRevision.Status) == false) return;
            //SelectedRevision.Status = Status.Editing;
            //SQLWriterFactories.CreateEntity(EnumRdbms.SQLite).Save(SelectedRevision);

            var p = new DialogParameters();
            p.Add(nameof(SelectedRevision), SelectedRevision);
            RequestClose?.Invoke(new DialogResult(ButtonResult.OK, p));
        }

        private void PreviewButton_ClickExecute()
        {
            if (PreviewWindowViewModel.PreviewWindows.Count > 1) return;
            IDialogParameters p = new DialogParameters();
            p.Add(nameof(SelectedRevision), SelectedRevision);
            RequestClose?.Invoke(new DialogResult(ButtonResult.Retry, p));
        }

        public bool CanCloseDialog() => true;

        public void OnDialogClosed()
        {
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Entities.AddRange(parameters.GetValue<List<TemplateEntity>>(nameof(Entities)));
        }

    }
}
