using System.ComponentModel.DataAnnotations;

namespace RecordDB.API.DTOs
{
    /// <summary>
    /// Request body DTO for patching disc length only (PATCH /api/disc/{id}/length).
    /// </summary>
    public class UpdateDiscLengthDto
    {
        public int? Length { get; set; }
    }
}
