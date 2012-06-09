
using System;
using System.Collections.Generic;
namespace CMB.Phone
{
 
    public class Country
    {
        public string Name { get; set; }
        public string ISOCode { get; set; }
        public int LanguageCode { get; set; }
        public List<Bank> Banks { get; set; }
    }

    public class Service
    {
        public Guid ServiceID { get; set; }
        public ServiceType Type { get; set; }
        public string NationalNumber { get; set; }
        public string InternationalNumber { get; set; }
        public Bank Parent { get; set; }

    }

    public enum ServiceType
    {
        debit = 0,
        credit = 1,
        info = 2,
        customer_private = 3,
        customer_enterprise = 4
    }

    public class Bank
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public List<Service> Services { get; set; }
        public Country Parent { get; set; }

    }
}
