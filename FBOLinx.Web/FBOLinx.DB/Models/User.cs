using FBOLinx.Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FBOLinx.DB.Models
{
    public class User
    {
        [Key]
        [Column("OID")]
        public int Oid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        [NotMapped]
        public string Token { get; set; }
        public bool? CopyAlerts { get; set; }
        public long? LoginCount { get;set; }
        public int? AddOnMarginTries { get; set; }
        public bool? GoOverTutorial { get; set; }
        public bool? Active { get; set; } = true;
        public string ResetPasswordToken { get; set; }
        public DateTime? ResetPasswordTokenExpiration { get; set; }
        public UserRoles Role { get; set; }

        [Column("FBOID")]
        public int FboId { get; set; }
        public bool? CopyOrders { get; set; }

        [ForeignKey("FboId")]
        [InverseProperty("Users")]
        public Fbos Fbo { get; set; }

        [Column("GroupID")]
        public int? GroupId { get; set; }

        [ForeignKey("GroupId")]
        [InverseProperty("Users")]
        public Group Group { get; set; }

        public ICollection<AccessTokens> AccessTokens { get; set; }
        public string RoleDescription
        {
            get { return FBOLinx.Core.Utilities.Enum.GetDescription(Role); }
        }
    }
}
