using OneForAll.Core.ORM;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UnitTest.Host
{
    /// <summary>
    /// 订单
    /// </summary>
    public class Order
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        [AutoIncrement]
        public long Id { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [Required]
        [StringLength(200)]
        public string ProductName { get; set; }

        /// <summary>
        /// 总价
        /// </summary>
        [Required]
        [Column(TypeName = "decimal(18,3)")]
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>
        [Required]
        [Column(TypeName = "datetime")]
        public DateTime CreateTime { get; set; }
    }
}
