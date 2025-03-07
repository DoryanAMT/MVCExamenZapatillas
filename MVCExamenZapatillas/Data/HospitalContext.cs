using Microsoft.EntityFrameworkCore;
using MVCExamenZapatillas.Models;

namespace MVCExamenZapatillas.Data
{
    public class HospitalContext:DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options)
            :base(options){ }
        public DbSet <ImagenesZapa> ImagenesZapas{ get; set; }
        public DbSet <Zapa> Zapas { get; set; }
    }
}
