using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Post.Query.Domain.Entities
{
    [Table("Comment")]
    public class CommentEntity
    {
        [Key]
        public Guid Id { get; set; }

        public string Username { get; set; }

        public DateTime Date { get; set; }

        public string Comment { get; set; }

        public bool IsEdited { get; set; }

        public Guid PostId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public virtual PostEntity Post { get; set; }
    }
}