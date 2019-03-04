using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DBReader : MonoBehaviour {
    private string Url = "http://192.168.6.138:8000/ws/iniciosesion/Damian-Almi123";

    

    // Use this for initialization
    void Start () {
    }
    public void GetInfo(string URL)
    {
        StartCoroutine(LoadPost(URL));
    }
    IEnumerator LoadPost(string URL)
    {
        WWW www = new WWW(URL);
        
        yield return www;

        JSONObject f = new JSONObject(www.text);

        accessData(f);
        Debug.Log(www.text);
    }

    void accessData(JSONObject obj)
    {
        switch (obj.type)
        {
            case JSONObject.Type.OBJECT:
                for (int i = 0; i < obj.list.Count; i++)
                {
                    string key = (string)obj.keys[i];
                    JSONObject j = (JSONObject)obj.list[i];
                    Debug.Log(key);
                    accessData(j);
                }
                break;
            case JSONObject.Type.ARRAY:
                foreach (JSONObject j in obj.list)
                {
                    accessData(j);
                }
                break;
            case JSONObject.Type.STRING:
                Debug.Log(obj.str);
                break;
            case JSONObject.Type.NUMBER:
                Debug.Log(obj.n);
                break;
            case JSONObject.Type.BOOL:
                Debug.Log(obj.b);
                break;
            case JSONObject.Type.NULL:
                Debug.Log("NULL");
                break;

        }
    }


    }
