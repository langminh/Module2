namespace Session3.Entity.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Log
    {
        public int ID { get; set; }

        public int? UserID { get; set; }

        public DateTime? TimeLogin { get; set; }

        public DateTime? TimeLogout { get; set; }

        public int? CrashID { get; set; }

        [StringLength(250)]
        public string Description { get; set; }

        public virtual Crash Crash { get; set; }

        public virtual User User { get; set; }
    }
}
