using Shoposphere.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UserEvaluation.Data.Entities
{
  public  class UserRequest : BaseEntity
    {            

            [Required]
            public string Name { get; set; }

            [Required]
            public string Surname { get; set; }

            [StringLength(1000)]
            public string Message { get; set; }

            [StringLength(1000)]
            public string AdminMessage { get; set; }

            [Required]
            public byte[] File { get; set; }


            public int? UserId { get; set; }
            public User User { get; set; }

            public bool IsPositive { get; set; }

            public bool IsEvaluated { get; set; }
        }
    }

