using System;
using System.Collections.Generic;

namespace DoctorWho.Db.Domain
{
    public class Doctor : CRUDModel
    {
        public int DoctorId { get; set; }
        public int DoctorNumber { get; set; }
        public string DoctorName { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime FirstEpisodeDate { get; set; }
        public DateTime LastEpisodeDate { get; set; }
        public List<Episode> Episodes { get; set; }

        public Doctor(DoctorWhoCoreDbContext context = null) : base(context)
        {
        }
        public static IEnumerable<Doctor> GetAllDoctors()
        {
            var context = new DoctorWhoCoreDbContext();
            return context.Doctors;
        }
        public static IEnumerable<Doctor> GetAllDoctors(DoctorWhoCoreDbContext context)
        {
            return context.Doctors;
        }
    }
}