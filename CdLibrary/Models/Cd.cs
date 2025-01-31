using System.ComponentModel.DataAnnotations;

namespace CdLibrary.Models;

public class Cd
{
    [Key]
    public int Id { get; set; }
    [StringLength(60, MinimumLength = 2)]
    [Required]
    public required string Name { get; set; }
    [StringLength(60, MinimumLength = 2)]
    public string? Artist { get; set; }
    [StringLength(200, MinimumLength = 2)]
    [Required]
    public required string Description { get; set; }
    [Range(1, 100)]
    public required Genre? Genre { get; set; }

    [DataType(DataType.Date)]
    public DateTime purchaseDate { get; set; }
}