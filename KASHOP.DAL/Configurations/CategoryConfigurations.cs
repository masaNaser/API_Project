using KASHOP.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KASHOP.DAL.Configurations
{
    public class CategoryConfigurations : AuditableEntityConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {

           base.Configure(builder);
        }
    }
}
