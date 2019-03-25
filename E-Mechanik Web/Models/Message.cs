using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace E_Mechanik_Web.Models
{
    public class Message : IEntity
    {
        [Display(Name = "Nadawca")]
        public string SenderName { get; set; }
        [Display(Name = "Odbiorca")]
        public string ReceiverName { get; set; }
        [Required]
        [Display(Name = "Temat")]
        public string Subject { get; set; }
        [Required]
        [Display(Name = "Wiadomość")]
        [DataType(DataType.MultilineText)]
        public string Text { get; set; }
        [Display(Name = "Wiadomość została wysłana")]
        public string SendTime { get; set; }
    }
}