using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoSharing.Core.Models.Auth
{
    // TODO: add enums of roles
    [Table("roles", Schema = "auth")]
    public class Role : BaseEntity
    {
        [Column("name")]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
