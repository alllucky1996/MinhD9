namespace FashionGo.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OrderDetail")]
    public partial class OrderDetail
    {
        [Key]
        [Column(Order = 0)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int OrderId { get; set; }

        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ProductId { get; set; }


        [Display(Name = "Giá bán")]
        public double PriceAfter  { get; set; }

        [Display(Name = "Giảm giá")]
        public double Discount { get; set; }

        [Display(Name = "Số lượng")]
        public int? Amount { get; set; }
        [Display(Name = "Số lượng S")]
        public int? S { get; set; }
        [Display(Name = "Số lượng M")]
        public int? M { get; set; }
        [Display(Name = "Số lượng L")]
        public int? L { get; set; }
        [Display(Name = "Số lượng XL")]
        public int? XL { get; set; }

        [Column(TypeName = "ntext")]
        [Display(Name = "Ghi chú")]
        public string Note { get; set; }

        public virtual Order Order { get; set; } 

        public virtual Product Product { get; set; }
        [NotMapped]
        public string SoLuong { get
            {
                string sl = "";
                if (S == null)
                {
                    if (M == null)
                    {
                        if (L == null)
                        {
                            if (XL == null) return "";
                             sl+="XL:"+XL.ToString();
                        }
                        sl += "L:" + L.ToString();
                    }
                    sl += "M:" + M.ToString();
                }
                sl += "S:" + S.ToString();
                return sl;
            }
        }
    }
}
