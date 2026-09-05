using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Models
{
    public class AuditableEntity
    {
        public string? CreatedById { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? UpdatedById { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public ApplicationUser? CreatedBy { get; set; }
        public ApplicationUser? UpdatedBy { get; set; }
    }
}
