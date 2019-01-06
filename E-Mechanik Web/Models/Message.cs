using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Mechanik_Web.Models
{
    public class Message : IEntity
    {
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Text { get; set; }
        public DateTime SendTime { get; set; }
    }
}