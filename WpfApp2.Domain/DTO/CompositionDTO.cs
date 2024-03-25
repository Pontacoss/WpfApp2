using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp2.Domain.Entities;
using WpfApp2.Domain.InterFaces;
using WpfApp2.Domain.ValueObjects;

namespace WpfApp2.Domain.DTO
{
    public class CompositionDTO : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int Level { get; set; }
        public string Group { get; set; } = string.Empty;
        public int AreaDataId { get; set; }
        public int ParentNodeId { get; set; }

        public IAreaEntity AreaData { get; set; }
        public CompositionDTO ParentNode { get; set; }
        public ObservableCollection<CompositionDTO> Children { get; set; } = new ObservableCollection<CompositionDTO>();

        public event PropertyChangedEventHandler PropertyChanged;
        protected bool RaisePropertyChanged<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        private int _sequence;
        public int Sequence
        {
            get => _sequence;
            set { RaisePropertyChanged(ref _sequence, value); }
        }

        private bool _isHide;
        public bool IsHide
        {
            get => _isHide;
            set { RaisePropertyChanged(ref _isHide, value); }
        }

        private bool _isVariable;
        public bool IsVariable
        {
            get => _isVariable;
            set
            { RaisePropertyChanged(ref _isVariable, value); }
        }

        private bool _isEnableControl = true;
        public bool IsEnableControl
        {
            get => _isEnableControl;
            set
            { RaisePropertyChanged(ref _isEnableControl, value); }
        }

        private AreaType _areaType;
        public AreaType AreaType
        {
            get => _areaType;
            set { RaisePropertyChanged(ref _areaType, value); }
        }

        public CompositionDTO() { }


        public CompositionDTO(CompositionEntity pc, CompositionDTO parentNode)
        {
            Id = pc.Id;
            ParentNode = parentNode;
            ParentNodeId = parentNode.Id;
            ParentId = pc.ParentId;
            Sequence = pc.Sequence;
            Level = pc.Level;
            AreaType = pc.AreaType;
            IsVariable = pc.IsVariable;
            IsHide = pc.IsHide;
            Group = pc.Group;
            AreaDataId = pc.ContentId;
            AreaData = pc.AreaData;
            IsEnableControl = true;

            if (string.IsNullOrEmpty(parentNode.Group))
            {
                Group = this.Id.ToString();
            }
            else
            {
                Group = parentNode.Group + "-" + this.Id.ToString();
            }

        }


    }
}
