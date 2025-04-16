using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer.Dtos
{
    public record BookUpdateDto
       (
        [Required]
        [MaxLength(50)]
        string Title,
        [Required]
        string Author,
        [Required]
        double Price,
        [Required]
        [MaxLength(50)]
        string? BookImage,
        IFormFile? Image,
        [Required]
        DateTime PublishDate,
        [MaxLength(255)]
        string ISBN,
        [Required]
        int NbrOfCopy
        );
}
