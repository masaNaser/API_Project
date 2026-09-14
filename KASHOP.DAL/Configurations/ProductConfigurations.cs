using KASHOP.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Configurations
{
    public class ProductConfigurations : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.HasOne(c => c.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(c => c.CategoryId);

            builder.HasOne(x => x.CreatedBy)
            //المستخدم الواحد بقدر ينشئ اكثر من برودكت 
           .WithMany()
           .HasForeignKey(x => x.CreatedById)
           //لو حدا حذف المستخدم من الداتابيز
           //، لا تحذف البيانات اللي أنشأها هاد المستخدم تلقائياً
           //(Cascade Delete)،
           //وأظهر خطأ يمنع الحذف
           .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UpdatedBy)
          .WithMany()
          .HasForeignKey(x => x.UpdatedById)
          .OnDelete(DeleteBehavior.Restrict);
        }

    }
}
