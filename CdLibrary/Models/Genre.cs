using System.ComponentModel.DataAnnotations;

namespace CdLibrary.Models;

public class Genre
{
    [Key]
    public int Id { get; set; }
    [StringLength(60, MinimumLength = 2)]
    public string Name { get; set; } = null!;
}