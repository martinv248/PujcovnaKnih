//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PujcovnaKnih.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;

    public partial class Users
    {
        public int ID { get; set; }
        [Required, StringLength(50)]
        public string FName { get; set; }
        [Required, StringLength(50)]
        public string LName { get; set; }
        [EmailAddress(ErrorMessage = "E-mailov� adresa nen� platn�") ,Required, StringLength(50)]
        public string Email { get; set; }
        [Required, StringLength(50)]
        public string Password { get; set; }
    }

    
}
