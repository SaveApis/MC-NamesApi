#region

using MySqlConnector;

#endregion

MySqlConnectionStringBuilder builder = new MySqlConnectionStringBuilder
{
    Server = "saveapis.com",
    Database = "mcnamesapi",
    Password = "yourPassword",
    UserID = "yourUsername",
    Port = 3306,
    Pooling = true,
    ApplicationName = "McNames REST-Api",
    AllowUserVariables = true
};
Console.WriteLine(builder.ToString());