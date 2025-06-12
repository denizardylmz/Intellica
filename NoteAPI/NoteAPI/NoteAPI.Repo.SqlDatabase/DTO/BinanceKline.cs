using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteAPI.Repo.SqlDatabase.DTO
{
    [Table("BinanceKlines")]
    public class BinanceKline
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Symbol { get; set; } = string.Empty;

        [Required]
        public DateTime OpenTime { get; set; }

        [Required]
        [Column(TypeName = "decimal(30,8)")]
        public decimal OpenPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(30,8)")]
        public decimal HighPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(30,8)")]
        public decimal LowPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(30,8)")]
        public decimal ClosePrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(30,8)")]
        public decimal Volume { get; set; }

        [Required]
        public DateTime CloseTime { get; set; }

        [Required]
        [Column(TypeName = "decimal(30,8)")]
        public decimal QuoteVolume { get; set; }

        [Required]
        public int TradeCount { get; set; }

        [Required]
        [Column(TypeName = "decimal(30,8)")]
        public decimal TakerBuyBaseVolume { get; set; }

        [Required]
        [Column(TypeName = "decimal(30,8)")]
        public decimal TakerBuyQuoteVolume { get; set; }
    }
}
