using PhotoSharing.Core.Models.Auth;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoSharing.Core.Models.Social
{
    public class Friendship
    {
        /// <summary>
        /// Id from request was sent
        /// </summary>
        [Column("user_first_id")]
        public Guid UserFirstId { get; set; }
        public User UserFirst { get; set; }


        /// <summary>
        /// ID to who the request was sent
        /// </summary>
        [Column("user_second_id")]
        public Guid UserSecondId { get; set; }
        public User UserSecond { get; set; }
    }
}
