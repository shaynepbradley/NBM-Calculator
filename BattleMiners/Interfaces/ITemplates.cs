using BattleMiners.Classes;

namespace BattleMiners.Interfaces;

public interface ITemplates
{
    public Dictionary<int, Template> GetTemplates();

    public Template? GetTemplateById(int templateId);
}