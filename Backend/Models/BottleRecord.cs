using System;

namespace KuwagataApp.Backend.Models
{
    public enum MediaType { Kinshi, Mat, Wood }

    public class BottleRecord
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string UserId { get; set; } = string.Empty; // ユーザーごと
        
        public Guid BeetleId { get; set; } // 紐づく幼虫
        public Beetle? Beetle { get; set; }

        public DateTime ExchangeDate { get; set; } = DateTime.UtcNow; // 交換日・投入日
        public MediaType MediaType { get; set; } = MediaType.Kinshi; // 菌糸かマットか
        public string? MediaName { get; set; } // 銘柄 (例: レビンGなど)
        
        public decimal? Weight { get; set; } // 交換時の幼虫体重(g)
        public decimal? HeadWidth { get; set; } // 頭幅(mm)
        
        public string? Notes { get; set; } // メモ
    }
}
