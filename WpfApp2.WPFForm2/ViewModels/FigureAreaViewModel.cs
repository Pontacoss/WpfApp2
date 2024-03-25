using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using WpfApp2.Domain.DTO;
using WpfApp2.Domain.Entities;
using WpfApp2.WPFForm2.Interfaces;

namespace WpfApp2.WPFForm2.ViewModels
{
    [RegionMemberLifetime(KeepAlive = false)]
    public class FigureAreaViewModel : BindableBase, INavigationAware, ICompositionRegionElements
    {
        private FigureAreaEntity _targetEntity;
        public FigureAreaEntity TargetEntity
        {
            get { return _targetEntity; }
            set { SetProperty(ref _targetEntity, value); }
        }
        private float _canvusHeight;
        public float CanvusHeight
        {
            get { return _canvusHeight; }
            set { SetProperty(ref _canvusHeight, value); }
        }
        private float _canvusWidth;
        public float CanvusWidth
        {
            get { return _canvusWidth; }
            set { SetProperty(ref _canvusWidth, value); }
        }

        private float _imageHeight = 100;
        public float ImageHeight
        {
            get { return _imageHeight; }
            set { SetProperty(ref _imageHeight, value); }
        }

        private float _imageWidth = 100;
        public float ImageWidth
        {
            get { return _imageWidth; }
            set { SetProperty(ref _imageWidth, value); }
        }

        private float _magnification = 100;
        public float Magnification
        {
            get { return _magnification; }
            set { SetProperty(ref _magnification, value); }
        }

        private string _fileName;
        public string FileName
        {
            get { return _fileName; }
            set { SetProperty(ref _fileName, value); }
        }

        private string _figureFile = string.Empty;
        public string FigureFile
        {
            get { return _figureFile; }
            set { SetProperty(ref _figureFile, value); }
        }

        private float ImageActualWidth;
        private float ImageActualHeight;

        public FigureAreaViewModel()
        {
            GetFigureButton_Click = new DelegateCommand(GetFigureButton_ClickExecute);
            SliderValueChanged = new DelegateCommand(SliderValueChangedExecute);
#if DEBUG
            constructorCounter++;
            Console.WriteLine("FigureAreaViewModel Constructor" + constructorCounter);
#endif

        }

        private static int constructorCounter = 0;
        private static int destructorCounter = 0;

        ~FigureAreaViewModel()
        {
#if DEBUG
            destructorCounter++;
            Console.WriteLine("FigureAreaViewModel Destructor" + destructorCounter);
#endif
        }

        public DelegateCommand SliderValueChanged { get; }
        private void SliderValueChangedExecute()
        {
            CanvusHeight = Convert.ToInt32(10 * Magnification);
            CanvusWidth = Convert.ToInt32(10 * Magnification);
            //ImageWidth = ImageActualWidth* Magnification / 100;
            //ImageHeight = ImageActualHeight* Magnification / 100;
        }

        public DelegateCommand GetFigureButton_Click { get; }
        private void GetFigureButton_ClickExecute()
        {
            var ofDialog = new OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Title = "インポートする画像ファイルの選択",
                Filter = "画像ファイル | *.bmp;*.jpg;*.png;*.tiff;*.gif;*.icon"
            };
            ImageActualWidth = ImageWidth;
            ImageActualHeight = ImageHeight;


            if (ofDialog.ShowDialog() == true)
            {
                FileName = ofDialog.FileName;
            }

        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            TargetEntity = navigationContext.Parameters.GetValue<FigureAreaEntity>(nameof(TargetEntity));
            FileName = TargetEntity.FilePath;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return false;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void AddParameter(ParamTreeDTO pt)
        {

        }

        public void SaveAreaData()
        {

        }
    }
}
