namespace StoneX
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Xml.Serialization;

    [Serializable, XmlType("Item")]
    [Table("Currency")]
    public partial class Item
    {
        public Item()
        {    }

        [Key]
        [StringLength(7)]
        [Column("id")]
        [XmlAttribute("ID")]
        public string ID { get; set; }

        [Column("name")]
        [StringLength(50)]
        [XmlElement("Name")]
        public string Name { get; set; }

        [StringLength(3)]
        [Column("num_code")]
        [XmlElement("ISO_Num_Code")]
        public string ISO_Num_Code { get; set; }

        [StringLength(3)]
        [Column("char_code")]
        [XmlElement("ISO_Char_Code")]
        public string ISO_Char_Code { get; set; }

        [Column("nominal")]
        [XmlElement("Nominal")]
        public int? Nominal { get; set; }

        [StringLength(10)]
        [Column("parent_code")]
        [XmlElement("ParentCode")]
        public string ParentCode { get; set; }

        [StringLength(50)]
        [Column("eng_name")]
        [XmlElement("EngName")]
        public string EngName { get; set; }
        
        public Item(string _id, string _name, string _num_code, string _char_code, int? _nominal, string _parent_code, string _eng_name)
        {
            ID = _id;
            Name = _name;
            ISO_Num_Code = _num_code;
            ISO_Char_Code = _char_code;
            Nominal = _nominal;
            ParentCode = _parent_code;
            EngName = _eng_name;
        }
    }
}
