using System.ComponentModel.DataAnnotations;

namespace Notes.Domain;

public class Note 
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string? Title { get; set; }

    [MaxLength(144)]
    public string? Description { get; set; }

    [Required]
    public string? Author { get; set; }

    [Required]
    public int Collection { get; set; }

    public DateTime? CreationDate { get; set; }

    public DateTime? FinalDate { get; set; }
}
