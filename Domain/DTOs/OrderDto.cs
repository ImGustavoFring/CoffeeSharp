using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class OrderDto
    {
        public long Id { get; set; }
        public long ClientId { get; set; }
        public string? ClientNote { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DoneAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public DateTime? ExpectedIn { get; set; }
        public long BranchId { get; set; }
    }
}
