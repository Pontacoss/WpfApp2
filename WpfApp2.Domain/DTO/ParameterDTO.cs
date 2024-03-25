using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp2.Domain.Entities;
using WpfApp2.Domain.ValueObjects;

namespace WpfApp2.Domain.DTO
{
    public sealed class ParameterDTO : INotifyPropertyChanged
    {
        public ParameterDTO(int parentId, int sequence)
        {
            ParentId = parentId;
            Sequence = sequence;
            NameJP = string.Empty;
            NameEN = string.Empty;
            TableId = 0;
            SelectDBParamId = 0;
            InputMethod = InputMethod.Manual;
            IsVariable = false;
            Group = string.Empty;
            Value = string.Empty;
            TableName = string.Empty;
            Select = string.Empty;
            FilteringCondition = string.Empty;
            FilterParamId = 0;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected bool RaisePropertyChanged<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;

            field = value;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        public ParameterDTO(ParameterEntity entity)
        {
            Id = entity.Id;
            ParentId = entity.ParentId;
            Sequence = entity.Sequence;
            NameJP = entity.NameJP.Value;
            NameEN = entity.NameEN.Value;
            TableId = entity.TableId;
            SelectDBParamId = entity.SelectDBParamId;
            InputMethod = entity.InputMethod;
            IsVariable = entity.IsVariable;
            Group = entity.Group;
            Value = entity.Value;
            FilterParamId = entity.FilterParamId;
        }


        private int _id;
        public int Id
        {
            get { return _id; }
            set { RaisePropertyChanged(ref _id, value); }
        }
        private int _parentId;
        public int ParentId
        {
            get { return _parentId; }
            set { RaisePropertyChanged(ref _parentId, value); }
        }
        private int _sequence;
        public int Sequence
        {
            get { return _sequence; }
            set { RaisePropertyChanged(ref _sequence, value); }
        }

        private string _nameJP = String.Empty;
        public string NameJP
        {
            get => _nameJP;
            set { RaisePropertyChanged(ref _nameJP, value); }
        }

        public string NameEN { get; set; } = String.Empty;
        public int TableId { get; set; }
        public int SelectDBParamId { get; set; }

        private InputMethod _inputMethod;
        public InputMethod InputMethod
        {
            get => _inputMethod;
            set { RaisePropertyChanged(ref _inputMethod, value); }
        }

        private bool _isVariable;
        public bool IsVariable
        {
            get => _isVariable;
            set { RaisePropertyChanged(ref _isVariable, value); }
        }

        private string _group = String.Empty;
        public string Group
        {
            get => _group;
            set { RaisePropertyChanged(ref _group, value); }
        }

        private string _value = String.Empty;
        public string Value
        {
            get => _value;
            set { RaisePropertyChanged(ref _value, value); }
        }

        private string _tableName = String.Empty;
        public string TableName
        {
            get => _tableName;
            set { RaisePropertyChanged(ref _tableName, value); }
        }

        private string _select = String.Empty;
        public string Select
        {
            get => _select;
            set { RaisePropertyChanged(ref _select, value); }
        }

        private string _filteringCondition = String.Empty;
        public string FilteringCondition
        {
            get => _filteringCondition;
            set { RaisePropertyChanged(ref _filteringCondition, value); }
        }

        private int _filterParamId;
        public int FilterParamId
        {
            get => _filterParamId;
            set { RaisePropertyChanged(ref _filterParamId, value); }
        }
    }
}
