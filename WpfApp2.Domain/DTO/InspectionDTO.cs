using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using WpfApp2.Domain.Entities;
using WpfApp2.Domain.ValueObjects;

namespace WpfApp2.Domain.DTO
{
    public sealed class InspectionDTOComparer : IEqualityComparer<InspectionDTO>
    {
        public bool Equals(InspectionDTO x, InspectionDTO y)
        {
            return x.Id == y.Id;
        }

        public int GetHashCode(InspectionDTO obj)
        {
            return obj.Id.GetHashCode();
        }
    }

    public class InspectionDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected bool RaisePropertyChanged<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value)) return false;

            field = value;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        public static bool CombinationEqals(List<InspectionDTO> a, List<InspectionDTO> b)
        {
            if (a.Count != b.Count) return false;
            else
            {
                for (int i = 0; i < a.Count; i++)
                {
                    if (a[i].Id != b[i].Id) return false;
                    if (a[i].SelectedRevision != b[i].SelectedRevision) return false;
                }
            }
            return true;
        }

        //public static List<InspectionDTO> ConvertToInspectionDTOList(List<InspectionEntity> ie)
        //{
        //	List<InspectionDTO> inspectionDTOs = new List<InspectionDTO>();
        //	foreach(var entity in ie)
        //	{
        //		inspectionDTOs.Add(new InspectionDTO(entity));
        //	}
        //	return inspectionDTOs;
        //}

        public int Id { get; }
        public int ParentId { get; }
        //public string Sorter { get; }
        public string Clause { get; }

        private string _nameJP;
        public string NameJP
        {
            get => _nameJP;
            set { RaisePropertyChanged(ref _nameJP, value); }
        }
        private string _nameEN;
        public string NameEN
        {
            get => _nameEN;
            set { RaisePropertyChanged(ref _nameEN, value); }
        }

        private bool _typeTest = false;
        public bool TypeTest
        {
            get => _typeTest;
            set { RaisePropertyChanged(ref _typeTest, value); }
        }

        private bool _routinTest = false;
        public bool RoutinTest
        {
            get => _routinTest;
            set { RaisePropertyChanged(ref _routinTest, value); }
        }

        private Revision _latestRevision;
        public Revision LatestRevision
        {
            get => _latestRevision;
            set { RaisePropertyChanged(ref _latestRevision, value); }
        }

        private Revision _selectedRevision;
        public Revision SelectedRevision
        {
            get => _selectedRevision;
            set
            {
                RaisePropertyChanged(ref _selectedRevision, value);
                if (LatestRevision.CompareTo(SelectedRevision) <= 0) Message = string.Empty;
                else Message = "最新ではありません。";
            }
        }

        private string _message = string.Empty;
        public string Message
        {
            get => _message;
            set { RaisePropertyChanged(ref _message, value); }
        }

        public InspectionDTO(InspectionEntity ie)
        {
            Id = ie.Id;
            ParentId = ie.ParentId;
            NameJP = ie.NameJP.Value;
            NameEN = ie.NameEN.Value;
            Clause = ie.Clause.Value;
            TypeTest = ie.TypeTest;
            RoutinTest = ie.RoutinTest;
            LatestRevision = ie.LatestRevision;
            SelectedRevision = ie.LatestRevision;
        }

        public InspectionDTO(InspectionEntity ie, TemplateEntity te)
        {
            Id = ie.Id;
            ParentId = ie.ParentId;
            NameJP = ie.NameJP.Value;
            NameEN = ie.NameEN.Value;
            Clause = ie.Clause.Value;
            TypeTest = ie.TypeTest;
            RoutinTest = ie.RoutinTest;
            LatestRevision = ie.LatestRevision;
            SelectedRevision = te.Revision;
        }
    }
}
