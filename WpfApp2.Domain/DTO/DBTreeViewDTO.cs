using System;
using System.Collections.Generic;
using System.Linq;
using WpfApp2.Domain.Entities;

namespace WpfApp2.Domain.DTO
{
    public sealed class DBTreeViewDTO
    {
        public int Id;
        public int DBId { get; } = 0;
        public string Name { get; }
        public bool IsPrimary { get; }
        public List<DBTreeViewDTO> Children { get; set; }
            = new List<DBTreeViewDTO>();

        private DBTreeViewDTO(DBParameterEntity dbp)
        {
            Id = dbp.Id;
            DBId = dbp.DBId;
            Name = dbp.Name.DisplayValue;
            IsPrimary = dbp.IsPrimaryKey;
#if DEBUG
            constructorCounter++;
            seq = constructorCounter;
            Console.WriteLine("DBTreeViewDTO Constructor " + constructorCounter);
#endif
        }

        private DBTreeViewDTO(DBListEntity parent, List<DBParameterEntity> children)
        {
            DBId = parent.Id;
            Name = parent.Name.DisplayValue;
            foreach (var child in children)
            {
                Children.Add(new DBTreeViewDTO(child));
            }
#if DEBUG
            constructorCounter++;
            seq = constructorCounter;
            Console.WriteLine("DBTreeViewDTO Constructor " + constructorCounter);
#endif
        }

        private static int constructorCounter = 0;
        private int seq = 0;

        ~DBTreeViewDTO()
        {
            this.Children = null;
#if DEBUG
            Console.WriteLine("DBTreeViewDTO Destructor " + seq);
#endif
        }

        public static List<DBTreeViewDTO> GetDTO
            (
                List<DBListEntity> dbList,
                List<DBParameterEntity> parameters
            )
        {
            var list = new List<DBTreeViewDTO>();
            foreach (var node in dbList)
            {
                DBTreeViewDTO p = new DBTreeViewDTO
                    (
                        node,
                        parameters.Where(x => x.DBId == node.Id).ToList()
                    );
                list.Add(p);
            }
            return list;
        }
    }
}
