using UnityEngine;
using System.Net;
using System.IO;
using Newtonsoft.Json;

public static class APIHelper
{
    public static UserData GetUserData(string userId)
    {
        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(string.Format(EVConstants.URL_USERDATA_TEST, userId));
        HttpWebResponse response = (HttpWebResponse)request.GetResponse();

        StreamReader reader = new StreamReader(response.GetResponseStream());
        string json = reader.ReadToEnd();

        Debug.Log($"<color=yellow>received json: {json}</color>");

        return JsonUtility.FromJson<UserData>(json);
    }
}
