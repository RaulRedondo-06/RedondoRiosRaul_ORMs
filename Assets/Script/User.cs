using SQLite4Unity3d;

public class User
{
    [PrimaryKey, AutoIncrement]
    public int UserID { get; set; }

    [Unique]
    public string Username { get; set; }

    public string Password { get; set; }
}