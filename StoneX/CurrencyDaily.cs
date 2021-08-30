namespace StoneX
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CurrencyDaily")]
    public partial class CurrencyDaily
    {
        [Key]
        [Column(Order = 0, TypeName = "money")]
        public decimal value { get; set; }

        [ForeignKey("Currency")]
        [StringLength(7)]
        public string currency_id { get; set; }

        [Key]
        [Column(Order = 1, TypeName = "date")]
        public DateTime date { get; set; }

        public virtual Item Currency { get; set; }
    }
}
