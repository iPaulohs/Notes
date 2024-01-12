using System.ComponentModel.DataAnnotations;

namespace Notes.Domain;

public class Collection
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? Title { get; set; }

    [MaxLength(144)]
    public string? Description { get; set; }

    [Required]
    public string? Author { get; set; }

    public DateTime? CreationDate { get; set; }
}
