namespace Stock.Application.Features.Categories.DTOs
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } // Nullable olabilir
        // İlişkili Product'ları buraya eklemek genellikle iyi bir fikir değildir,
        // performans sorunlarına yol açabilir ve DTO'yu şişirebilir.
        // İhtiyaç halinde ayrı bir endpoint/DTO düşünülebilir.
    }
} 