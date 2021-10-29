using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportStore.Models.Dtos
{
    public class UserLoginDTO
    {
        [Required]
        [StringLength(150)]
        public String Username { get; set; }

        [Required]
        [StringLength(20)]
        public String Password { get; set; }
    }
}
