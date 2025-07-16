using System.Text.Json;
using BookShopServer.Entities;

namespace BookShopServer;

/// <summary>
/// Provides functionality to persist and retrieve the minimum employee salary
/// to and from a JSON file.
/// </summary>
public static class MinimumSalaryPersistence
{
    private const string FilePath = "minimumsalary.json";

    /// <summary>
    /// Saves the current value of <c>Employee.MinSalary</c> to a JSON file.
    /// </summary>
    public static void Save()
    {
        var json = JsonSerializer.Serialize(Employee.MinSalary);
        File.WriteAllText(FilePath, json);
    }

    /// <summary>
    /// Loads the minimum salary value from the JSON file and assigns it to <c>Employee.MinSalary</c>.
    /// If the file does not exist, the value remains unchanged.
    /// </summary>
    public static void Load()
    {
        if (File.Exists(FilePath))
        {
            var json = File.ReadAllText(FilePath);
            Employee.MinSalary = JsonSerializer.Deserialize<double>(json);
        }
    }
}
