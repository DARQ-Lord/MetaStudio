using Defective.JSON;
using Oculus.Voice;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
public class ArtRoomManager : MonoBehaviour
{
    public AppVoiceExperience _voiceExperience;
    public TMPro.TMP_Text text;
    public string apikey;
    public GameObject canvas;

    public class ImageGenData
    {
        public string prompt;
        public int n = 1;
        public string size = "1024x1024";
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void onTrans(string data)
    {
        StartCoroutine(OnTranscriptionCompleted());
    }
    public IEnumerator OnTranscriptionCompleted()
    {
        yield return new WaitForSeconds(1.0f);
        StartCoroutine(GenerateResponse(text.text));
    }
    IEnumerator GenerateResponse(string text)
    {
        yield return new WaitForSeconds(1.0f);
        var data = new ImageGenData();
        data.prompt= text;
        StartCoroutine(Request("https://api.openai.com/v1/images/generations", data));

    }
    public IEnumerator Request(string url, ImageGenData data)
    {
        string json = JsonUtility.ToJson(data);
        Debug.Log("Data " + json);
        var req = new UnityWebRequest(url, "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        req.SetRequestHeader("Content-Type", "application/json");
        req.SetRequestHeader("Authorization", "Bearer " + apikey);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        yield return req.SendWebRequest();
        if (req.isNetworkError)
        {
            Debug.Log(req.error);
        }
        else
        {
            Debug.Log(req.downloadHandler.text);
            JSONObject a = new JSONObject(req.downloadHandler.text);
            if (a.GetField("created").ToString() != "")
            {
                var imageurl = a.GetField("data")[0].GetField("url").ToString();
                print(imageurl);
                StartCoroutine(DownloadImage(imageurl.Replace("\"","")));
            }

        }
    }

    IEnumerator DownloadImage(string MediaUrl)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(MediaUrl);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
        {
            Debug.Log(request.error);
        }
        else
        {
            var texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
            canvas.GetComponent<Renderer>().material.mainTexture = texture;
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("*** Pressed Space bar ***");
            ActivateWit();
        }
    }
    public void Logout()
    {
        File.Delete(Application.persistentDataPath + "/" + "current_user.dat");
        SceneManager.LoadScene("Login");
    }
    public void ActivateWit()
    {
        _voiceExperience.Activate();
    }
}
