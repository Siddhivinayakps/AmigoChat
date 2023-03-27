using System.Collections.Generic;

[System.Serializable]
public class Users
{
    public List<User> users;
}

[System.Serializable]
public class User
{
    public string userId, interests;
    public int isAvailable;
    public string[] interestArray;
}
