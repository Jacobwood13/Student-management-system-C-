using Xunit;
using Microsoft.Data.Sqlite;

namespace StudentManagementSystem.Tests;

public class DatabaseManagerTests : IDisposable
{
    private readonly string _inMemoryConnectionString = "Data Source=:memory:";
    private readonly SqliteConnection _keepAliveConnection;
    private readonly DatabaseManager _dbManager;

    public DatabaseManagerTests()
    {
        _keepAliveConnection = new SqliteConnection(_inMemoryConnectionString);
        _keepAliveConnection.Open();
        _dbManager = new DatabaseManager(_inMemoryConnectionString);
    }

    [Fact]
    public void InsertStudent_ShouldSuccessfullySaveToDatabase()
    {
        var newStudent = new Student(1, "Alice Smith", "Computer Science", 'A');
        
        bool result = _dbManager.InsertStudent(newStudent);
        Assert.True(result);
    }

    public void Dispose()
    {
        _keepAliveConnection.Close();
        _keepAliveConnection.Dispose();
    }
}