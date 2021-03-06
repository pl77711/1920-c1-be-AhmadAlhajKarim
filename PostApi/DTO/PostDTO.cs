using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PostApi.DTO
{
    public class PostDTO
    {

        [Required]
        public string Title { set; get; }
        [Required]
        public string Location { set; get; }
        public DateTime Date { get; set; }
        public string Picture { get; set; }
        public bool Reserved { get; set; }
    }
}
