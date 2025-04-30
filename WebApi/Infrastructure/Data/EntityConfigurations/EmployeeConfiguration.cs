using CoffeeSharp.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Infrastructure.Data.EntityConfigurations
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> entity)
        {
            entity.HasKey(employee => employee.Id)
                .HasName("employees_pkey");

            entity.ToTable("employees");

            entity.Property(employee => employee.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");

            entity.Property(employee => employee.BranchId)
                .HasColumnName("branch_id");

            entity.Property(employee => employee.Name)
                .HasMaxLength(100)
                .HasColumnName("name");

            entity.Property(employee => employee.UserName)
                .HasMaxLength(30)
                .HasColumnName("user_name");

            entity.Property(employee => employee.PasswordHash)
                .HasColumnName("password_hash");

            entity.Property(employee => employee.RoleId)
                .HasColumnName("role_id");

            entity.HasOne(employee => employee.Branch)
                .WithMany(branch => branch.Employees)
                .HasForeignKey(employee => employee.BranchId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("employees_branch_id_fkey");

            entity.HasOne(employee => employee.Role)
                .WithMany(role => role.Employees)
                .HasForeignKey(employee => employee.RoleId)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("employees_role_id_fkey");

            entity.HasIndex(employee => employee.Name);

            entity.HasIndex(employee => employee.UserName)
                 .IsUnique();

        }
    }
}
