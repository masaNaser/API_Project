using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KASHOP.DAL.Models;

namespace KASHOP.DAL.Configurations
{
        public class CartConfigurations : IEntityTypeConfiguration<CartItem>
        {
            public void Configure(EntityTypeBuilder<CartItem> builder)
            {
                builder.HasOne(c => c.User)
                       .WithMany()
                       .HasForeignKey(c => c.UserId)
                       .OnDelete(DeleteBehavior.Cascade);

                builder.HasOne(c => c.Product)
                       .WithMany()
                       .HasForeignKey(c => c.ProductId)
                       .OnDelete(DeleteBehavior.Cascade);

                // نفس المنتج ما بينضاف مرتين لنفس المستخدم (بنزيد الـ Count بدالها)
                builder.HasIndex(c => new { c.UserId, c.ProductId }).IsUnique();
            }
        }
    
}
