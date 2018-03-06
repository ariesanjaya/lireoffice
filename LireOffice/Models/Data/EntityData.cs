using System;

namespace LireOffice.Models
{
    public abstract class EntityData
    {
        public EntityData()
        {
            Id = Guid.NewGuid().ToString();
            Version = 1;
            IsDeleted = false;
            CreatedAt = DateTime.Now;
            UpdatedAt = DateTime.Now;
        }

        public string Id { get; set; }
        public int Version { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}