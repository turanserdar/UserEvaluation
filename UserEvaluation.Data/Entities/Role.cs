using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UserEvaluation.Data.Entities;

namespace Shoposphere.Data.Entities
{
    public class Role : BaseEntity
    {
        public UserRole UserRole { get; set; }


        #region Relations

        public List<User> Users { get; set; } 

        #endregion
    }
}
