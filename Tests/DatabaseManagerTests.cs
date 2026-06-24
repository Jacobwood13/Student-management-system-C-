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
        
        // Setup: Create the database table in our blank in-memory DB
        using (var command = _keepAliveConnection.CreateCommand())
        {
            // This makes sure the Student table exists before any test runs
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Students (
                    Id INTEGER PRIMARY KEY,
                    Name TEXT NOT NULL,
                    Course TEXT NOT NULL,
                    Grade TEXT NOT NULL
                );";
            command.ExecuteNonQuery();
        }

        _dbManager = new DatabaseManager(_inMemoryConnectionString);
    }

    // --- TEST 1: Insert ---
    [Fact]
    public void InsertStudent_ShouldSuccessfullySaveToDatabase()
    {
        var newStudent = new Student(1, "Alice Smith", "Computer Science", 'A');
        
        bool result = _dbManager.InsertStudent(newStudent);
        Assert.True(result);
    }

    // --- TEST 2: Get Existing Student ---
    [Fact]
    public void GetStudentById_ShouldReturnCorrectStudent_WhenStudentExists()
    {
        var expectedStudent = new Student(2, "Bob Jones", "Mathematics", 'B');
        _dbManager.InsertStudent(expectedStudent);

        Student? actualStudent = _dbManager.GetStudentById(2);

        Assert.NotNull(actualStudent);
        Assert.Equal("Bob Jones", actualStudent.Name);
        Assert.Equal("Mathematics", actualStudent.Course);
    }

    // --- TEST 3: Get Non-Existent Student ---
    [Fact]
    public void GetStudentById_ShouldReturnNull_WhenStudentDoesNotExist()
    {
        Student? result = _dbManager.GetStudentById(999);

        Assert.Null(result);
    }

    public void Dispose()
    {
        _keepAliveConnection.Close();
        _keepAliveConnection.Dispose();
    }
}