using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogicLayer
{
    public record BookDto
    (
        [Required]
        [MaxLength(50)]
        string Title,
        [Required]
        string Author,
        [Required]
        double Price,
        string? ImageUrl,
        string? ImageLocalPath,
        [Required]
        DateTime PublishDate,
        [MaxLength(255)]
        string ISBN,
        [Required]
        int NbrOfCopy
    );
    
}
