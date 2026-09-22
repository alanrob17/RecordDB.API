using System.ComponentModel.DataAnnotations;

namespace RecordDB.API.DTOs
{
    /// <summary>
    /// Request body DTO for creating a new Artist (POST /api/artist).
    /// </summary>
    public class CreateArtistDto
    {
        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        public string? Biography { get; set; }
    }
}
