﻿using System;

namespace DoctorWho.Db.Domain
{
    public class Author : CRUDModel
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; }


    }
}