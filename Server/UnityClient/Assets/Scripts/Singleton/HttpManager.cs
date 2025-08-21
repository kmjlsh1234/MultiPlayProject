using System.Collections;
using UnityEngine;

public enum HTTPMethod
{
    GET,
    POST, 
    PUT, 
    DELETE
}

public class HttpManager : SingletonBase<HttpManager>
{
    private const string host = "http://localhost:8080";

    public override void Init()
    {
        
    }

    public void Request(string url)
    {

    }

    
}
