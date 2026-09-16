using KASHOP.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KASHOP.DAL.Configurations
{
    public class BrandConfigurations : AuditableEntityConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {

            base.Configure(builder);
        }
    }
}
