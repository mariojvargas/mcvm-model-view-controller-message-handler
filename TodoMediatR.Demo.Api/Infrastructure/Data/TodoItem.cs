using System.ComponentModel.DataAnnotations;

namespace TodoApiMediatR.Demo.Api.Infrastructure.Data
{
    public class TodoItem
    {
        [Key]
        public long Id { get; set; }

        [Required]
        [StringLength(256)]
        public string Name { get; set; }

        public bool IsComplete { get; set; }
    }
}
