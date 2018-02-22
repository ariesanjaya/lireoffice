﻿using System;
using LiteDB;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Models
{
    public abstract class EntityData
    {
        public EntityData()
        {
            Id = ObjectId.NewObjectId();
            Version = 1;
            IsDeleted = false;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public ObjectId Id { get; set; }
        public int Version { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}