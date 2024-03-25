using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace WpfApp2.Domain.Entities
{
    public sealed class StandardEntity
    {
        public int Id { get; set; }

        [StringLength(20)]
        public string StandardType { get; set; } = String.Empty;
        [StringLength(20)]
        public string Numbering { get; set; } = String.Empty;

        public static IEnumerable<string> DistinctNameList(List<StandardEntity> entities)
        {
            return entities.Select(x => x.StandardType).Distinct();
        }

        public StandardEntity() { }

    }
}


