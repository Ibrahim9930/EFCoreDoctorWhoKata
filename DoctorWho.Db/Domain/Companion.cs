﻿using System;
using System.Collections.Generic;

namespace DoctorWho.Db.Domain
{
    public class Companion
    {
        public Guid CompanionId { get; set; }
        public string CompanionName { get; set; }
        public int WhoPlayed { get; set; }
        public ICollection<Episode> Episodes { get; set; }
    }
}