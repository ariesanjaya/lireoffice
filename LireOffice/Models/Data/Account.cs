﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LireOffice.Models
{
    public class Account : EntityData
    {
        public string ReferenceId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public string Description { get; set; }
        public decimal Balance { get; set; }
    }
}
