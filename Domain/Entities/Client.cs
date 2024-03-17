using Domain.Entities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    [BsonCollection("Client")]
    public class Client : Document
    {
        public string? Name { get; set; }
        public string? CPF { get; set; }
        public string? Adress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? DDD { get; set; }
        public string? Phone { get; set; }
        public string? Active { get; set; }
    }
}