using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class EmployeeRoleConfiguration : IEntityTypeConfiguration<EmployeeRole>
    {
        public void Configure(EntityTypeBuilder<EmployeeRole> entity)
        {
            entity.HasKey(employeeRole => employeeRole.Id)
                .HasName("employee_roles_pkey");

            entity.ToTable("employee_roles");

            entity.Property(employeeRole => employeeRole.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(employeeRole => employeeRole.Name)
                .HasMaxLength(20)
                .HasColumnName("name");

            entity.HasIndex(employeeRole => employeeRole.Name)
                .IsUnique();
        }
    }
}
