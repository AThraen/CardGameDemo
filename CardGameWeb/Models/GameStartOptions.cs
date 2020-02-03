using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CardGameWeb.Models
{
    public class GameStartOptions
    {
        [Display(Name ="Player Name")]
        [Required]
        public string PlayerName { get; set; }

        public bool ComputerPlayer { get; set; }
    }
}
