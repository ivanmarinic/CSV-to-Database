using System;
using System.Collections.Generic;

#nullable disable

namespace spanApp.Models
{
    public partial class Podaci
    {
        public int DataId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
    }
}
