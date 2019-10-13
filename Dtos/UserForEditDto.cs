using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Witter.Dtos
{
    public class UserForEditDto
    {
        [Required]
        [MinLength(4)]
        public string Password { get; set; }
    }
}
