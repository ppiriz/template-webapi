using System;

namespace webApi.template.Models
{
    /// <summary>
    /// Represents a customer
    /// </summary>
    public class Record
    {
        /// <summary>
        /// Name of customer
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Address of customer
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Customers date of birth
        /// </summary>
        public DateTime DoB { get; set; }
    }
}