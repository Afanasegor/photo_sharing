using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoSharing.Core.Models
{
    public abstract class BaseEntity
    {
        [Column("id")]
        public Guid Id { get; set; }

        [Column("created")]
        public DateTime? Created { get; set; }

        [Column("modified")]
        public DateTime? Modified { get; set; }
    }
}
