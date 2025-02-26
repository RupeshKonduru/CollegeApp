using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CollegeApp.Data.Config
{
    public class StudentConfig : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.ToTable(nameof(Student));
            builder.HasKey(t => t.Id);
            builder.Property(x => x.Id).UseIdentityColumn();
            builder.Property(n => n.StdName).IsRequired().HasMaxLength(100);
            builder.Property(n => n.Address).IsRequired(false).HasMaxLength(500);
            builder.Property(n => n.Email).IsRequired().HasMaxLength(250);

            // Add Data
            builder.HasData(new List<Student>() {
             new Student
                {
                    Id = 1,
                    StdName = "Sudharshan K",
                    Email = "ksudharshan1010@gmail.com",
                    Address = "Anantapur",
                    DOB = DateTime.Parse("12/05/1993")
                },
                new Student
                {
                    Id = 2,
                    StdName = "Gireesh",
                    Email = "gireesh@gmail.com",
                    Address = "USA",
                    DOB = DateTime.Parse("09/05/1991")
                }
            });
        }
    }
}
