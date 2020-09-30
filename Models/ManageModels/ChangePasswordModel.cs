using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Atlob_Dent.Models.ManageModels
{
    public class ChangePasswordModel
    {
        [Required]
        [StringLength(50,MinimumLength = 6)]
        [DataType(DataType.Password)]
        //[RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,50}")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 6)]
        [DataType(DataType.Password)]
        //[RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,50}")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }

    }
}
