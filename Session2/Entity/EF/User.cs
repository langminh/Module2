namespace Session2.Entity.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class User
    {
        public int ID { get; set; }

        public int RoleID { get; set; }

        [Required]
        [StringLength(150)]
        public string Email { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        public int? OfficeID { get; set; }

        [Column(TypeName = "date")]
        public DateTime? Birthdate { get; set; }

        public bool? Active { get; set; }

        public virtual Office Office { get; set; }

        public virtual Role Role { get; set; }
    }
}
