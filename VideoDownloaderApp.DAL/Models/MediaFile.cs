using System.ComponentModel.DataAnnotations;

namespace VideoDownloaderApp.DAL.Models;

public class MediaFile
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public string MIME { get; set; } = null!;
}
