using System;
using System.ComponentModel.DataAnnotations;

namespace SportStore.Models.Results
{
    public class UserResult
    {
        public Guid UserId { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Email { get; set; }
    }
}
