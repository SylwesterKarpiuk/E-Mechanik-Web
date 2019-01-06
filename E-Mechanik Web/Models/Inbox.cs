using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Mechanik_Web.Models
{
    public class Inbox : IEntity
    {
        public virtual ICollection<Message> Messages { get; set; }
    }
}