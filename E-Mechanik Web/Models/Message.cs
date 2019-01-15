using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Mechanik_Web.Models
{
    public class Message : IEntity
    {
        public string SenderName { get; set; }
        public string ReceiverName { get; set; }
        [Required]
        public string Subject { get; set; }
        [Required]
        public string Text { get; set; }
        public string SendTime { get; set; }
    }
}