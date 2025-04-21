using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTOs
{
    public class FeedbackDto
    {
        public long Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public long? RatingId { get; set; }
        public long? OrderId { get; set; }
    }
}
