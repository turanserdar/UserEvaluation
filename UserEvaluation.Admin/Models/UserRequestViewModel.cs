using UserEvaluation.Data.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace UserEvaluation.Models
{
    public class UserRequestViewModel
    {
        [DisplayName("Request Number")]
        public int Id { get; set; }

        [Required(ErrorMessage = "Zorunlu alan")]
        [DisplayName("Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Zorunlu alan")]
        [DisplayName("Surname")]
        public string Surname { get; set; }


        [DisplayName("Description")]
        public string Description { get; set; }

        [DisplayName("File")]
        [Required(ErrorMessage = "Zorunlu alan")]
        public IFormFile File { get; set; }
        public string FileStr { get; set; }

        [DisplayName("Kullanıcı Id")]
        public int? UserId { get; set; }

        [DisplayName("Positive?")]
        public bool IsPositive { get; set; }

        [DisplayName("Is Evalauated?")]
        public bool IsEvaluated { get; set; }

       

        [DisplayName("Admin Description")]
        public string AdminDescription { get; set; }

        [DisplayName("Request Date")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Evaluated Date")]
        public DateTime? UpdatedDate { get; set; }


    }
}
