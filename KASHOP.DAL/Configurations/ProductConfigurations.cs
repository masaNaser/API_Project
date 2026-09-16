using KASHOP.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KASHOP.DAL.Configurations
{
    public class ProductConfigurations : AuditableEntityConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            //مسؤول عن استدعاء AuditableEntityConfiguration
            base.Configure(builder);

            builder.HasOne(c => c.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(c => c.CategoryId);

            builder.HasOne(p => p.Brand)
                   .WithMany(b => b.Products)
                   .HasForeignKey(p => p.BrandId)
                   .OnDelete(DeleteBehavior.Restrict);

          //  builder.HasOne(x => x.CreatedBy)
          //  //المستخدم الواحد بقدر ينشئ اكثر من برودكت 
          // .WithMany()
          // .HasForeignKey(x => x.CreatedById)
          // //لو حدا حذف المستخدم من الداتابيز
          // //، لا تحذف البيانات اللي أنشأها هاد المستخدم تلقائياً
          // //(Cascade Delete)،
          // //وأظهر خطأ يمنع الحذف
          // .OnDelete(DeleteBehavior.Restrict);

          //  builder.HasOne(x => x.UpdatedBy)
          //.WithMany()
          //.HasForeignKey(x => x.UpdatedById)
          //.OnDelete(DeleteBehavior.Restrict);
        }

    }
}
