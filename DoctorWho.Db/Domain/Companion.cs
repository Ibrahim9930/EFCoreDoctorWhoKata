﻿using System;
using System.Collections.Generic;

namespace DoctorWho.Db.Domain
{
    public class Companion : CRUDModel
    {
        public int CompanionId { get; set; }
        public string CompanionName { get; set; }
        public int WhoPlayed { get; set; }
        public ICollection<Episode> Episodes { get; set; }

        public Companion(DoctorWhoCoreDbContext context = null) : base(context)
        {
            Episodes = new List<Episode>();
        }

        public static Companion GetCompanionById(int companionId)
        {
            var context = new DoctorWhoCoreDbContext();
            return context.Companions.Find(companionId);
        }
        
        public static Companion GetCompanionById(int companionId,DoctorWhoCoreDbContext context)
        {
            return context.Companions.Find(companionId);
        }
    }
}