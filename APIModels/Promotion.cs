namespace API.APIModels;

public class Promotion
{
    public string Id { get; set; } = default!;
    public PromotionType PromotionType { get; set; } = default!;
    public string? Description { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<string> ListSelectedStore { get; set; } = default!;
    public List<string> ListItem { get; set; } = default!;
}
