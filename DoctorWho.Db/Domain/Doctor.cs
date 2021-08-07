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
    }
}