//imports SQLite library
using Microsoft.Data.Sqlite;
string connectionString = "Data Source=students.db";

//creates a connection to the SQLite database
var connection = new SqliteConnection(connectionString);

connection.Open();

//creates a command to create a table in the database
var command = connection.CreateCommand();
command.CommandText = @"
CREATE TABLE Students (
    StudentId INTEGER,
    Name TEXT,
    Course TEXT,
    Grade TEXT
)";