using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomRateLimiter.Domain.Entities
{
    public class Ticket
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }
        public required DateTimeOffset Date { get; set; }
    }
}
