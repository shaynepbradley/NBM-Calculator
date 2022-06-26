using UpgradeCalculator.Classes;

namespace UpgradeCalculator.Interfaces;

public interface ITemplates
{
    public Dictionary<int, Template> GetTemplates();

    public Template? GetTemplateById(int templateId);
}