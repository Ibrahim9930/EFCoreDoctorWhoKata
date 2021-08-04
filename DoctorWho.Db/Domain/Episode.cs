using System;
using System.Collections.Generic;

namespace DoctorWho.Db.Domain
{
    public class Episode
    {
        public Guid EpisodeId { get; set; }
        public int SeriesNumber { get; set; }
        public int EpisodeNumber { get; set; }
        public string EpisodeType { get; set; }
        public string Title { get; set; }
        public DateTime EpisodeDate { get; set; }
        public string Notes { get; set; }

        public List<Enemy> Enemies { get; set; }
        public List<Companion> Companions { get; set; }
        
        public Guid AuthorId { get; set; }
        public Author Author { get; set; }
        
        public Guid DoctorId { get; set; }
        public Doctor Doctor { get; set; }       
    }
}