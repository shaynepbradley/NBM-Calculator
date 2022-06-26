using UpgradeCalculator.Classes;

public class ConstructionCards : Card
{
    private readonly ILogger<ConstructionCards> _logger;

    public ConstructionCards(ILogger<ConstructionCards> logger)
    {
        _logger = logger;
    }

    public void InitializeAsync(string userId)
    {

    }
}