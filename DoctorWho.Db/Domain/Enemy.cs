using System;
using System.Collections.Generic;

namespace DoctorWho.Db.Domain
{
    public class Enemy : CRUDModel
    {
        public int EnemyId { get; set; }
        public string EnemyName { get; set; }
        public string Description { get; set; }
        public ICollection<Episode> Episodes { get; set; }

        public Enemy(DoctorWhoCoreDbContext context = null) : base(context)
        {
        }
    }
}