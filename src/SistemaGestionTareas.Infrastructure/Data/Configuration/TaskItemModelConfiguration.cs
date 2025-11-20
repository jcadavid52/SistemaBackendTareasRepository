using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SistemaGestionTareas.ApplicationCore.Common.Contants;
using SistemaGestionTareas.ApplicationCore.Entities;
using SistemaGestionTareas.Infrastructure.AuthProviders.Identity;

namespace SistemaGestionTareas.Infrastructure.Data.Configuration
{
    public class TaskItemModelConfiguration : IEntityTypeConfiguration<TaskItem>
    {
        public void Configure(EntityTypeBuilder<TaskItem> builder)
        {
            builder.HasKey(taskItem => taskItem.Id);
            builder.Ignore(t => t.User);

            builder.Property(taskItem => taskItem.Title)
                .IsRequired()
                .HasMaxLength(PropertieLengthTaskItem.TitleLength);

            builder.Property(taskItem => taskItem.Description)
                .IsRequired()
                .HasMaxLength(PropertieLengthTaskItem.DescriptionLength);

            builder.HasOne<ApplicationIdentityUser>()
                .WithMany()
                .HasForeignKey(taskItem => taskItem.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
