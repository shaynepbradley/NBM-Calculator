using System.Text.Json.Serialization;
using AtomicAssets.Models;

namespace UpgradeCalculator.Classes;

public class Lands : Construction
{
    public string resources { get; set; }
}
