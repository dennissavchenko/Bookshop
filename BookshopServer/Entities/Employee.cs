namespace BookShopServer.Entities;

/// <summary>
/// Represents the experience level of an employee.
/// </summary>
public enum Experience
{
    Junior,
    Mid,
    Senior
}

/// <summary>
/// Represents an employee in the bookshop system, including experience, salary,
/// and the associated user account.
/// </summary>
public class Employee
{
    public int UserId { get; set; }
    public Experience Experience { get; set; }
    public double Salary { get; set; }
    public static double MinSalary { get; set; } = 2000.0;
    public virtual User User { get; set; }
}
