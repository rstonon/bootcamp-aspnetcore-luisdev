using DevCars.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevCars.API.Persistence.Configurations
{
    public class CarDbConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder
                .HasKey(c => c.Id);

            builder
                .Property(c => c.Brand)
                .HasColumnType("VARCHAR(100)")
                .HasMaxLength(100)
                .HasDefaultValue("PADRÃO");
            //.IsRequired()
            //.HasColumnName("Marca");

            builder
                .Property(C => C.ProductionDate)
                .HasDefaultValueSql("getdate()");


            //builder
            //    .ToTable("tb_Car");
        }
    }
}
