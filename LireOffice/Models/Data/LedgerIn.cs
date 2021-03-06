﻿using System;

namespace LireOffice.Models
{
    public class LedgerIn : EntityData
    {
        public string AccountId { get; set; }
        public string AccountInId { get; set; }
        public string EmployeeId { get; set; }
        public DateTime LedgerDate { get; set; }
        public string ReferenceId { get; set; }
        public string Description { get; set; }
        public decimal Value { get; set; }
        public string ValueString { get; set; }
        public bool IsPosted { get; set; }
    }
}