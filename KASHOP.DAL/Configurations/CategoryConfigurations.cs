using KASHOP.DAL.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Configurations
{
    public class CategoryConfigurations : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {

            builder.HasOne(x => x.CreatedBy)
            //المستخدم الواحد بقدر ينشئ اكثر من مستخدم 
           .WithMany()
           .HasForeignKey(x => x.CreatedById)
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UpdatedBy)
          //المستخدم الواحد بقدر ينشئ اكثر من مستخدم 
          .WithMany()
          .HasForeignKey(x => x.UpdatedById)
          .OnDelete(DeleteBehavior.Restrict);
        }
}
