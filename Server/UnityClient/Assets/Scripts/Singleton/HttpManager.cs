using Newtonsoft.Json;
using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using static System.Net.WebRequestMethods;

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

    public void SendRequest<T>(string uri, T requestBody, HTTPMethod method, Action<UnityWebRequest> callBack)
    {
        StartCoroutine(HTTPRequest(uri, requestBody, method, callBack));
    }

    IEnumerator HTTPRequest<T>(string uri, T requestBody, HTTPMethod method, Action<UnityWebRequest> callBack)
    {
        string detailUri = host + uri;

        Debug.Log("endPoint : " + detailUri);

        using (UnityWebRequest req = new UnityWebRequest(detailUri, method.ToString()))
        {
            req.downloadHandler = new DownloadHandlerBuffer();

            SetRequestHeader(req, uri);
            SetRequestBody(req, requestBody);

            yield return req.SendWebRequest();

            callBack.Invoke(req);
        }
    }


    public void SetRequestHeader(UnityWebRequest req, string uri)
    {
        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("Accept", "*/*");
    }

    private void SetRequestBody<T>(UnityWebRequest req, T requestBody)
    {
        if (requestBody == null) return;
        string jsonData = JsonConvert.SerializeObject(requestBody);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonData);
        req.uploadHandler = new UploadHandlerRaw(bodyRaw);
    }


}
