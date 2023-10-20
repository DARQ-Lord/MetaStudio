using Defective.JSON;
using Oculus.Platform.Models;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField  email, password;
    public void Awake()
    {
        if (File.Exists(Application.persistentDataPath + "/current_user.dat"))
        {
            SceneManager.LoadScene("ArtRoom");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public IEnumerator request(UnityWebRequest req)
    {
        using (req)
        {
            Debug.Log(req);
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(req.error);
            }
            else
            {
                Debug.Log("Form upload complete!");
                Debug.Log(req.downloadHandler.text);
                JSONObject a = new JSONObject(req.downloadHandler.text);
                if (a.GetField("code").ToString() == "200")
                {
                    string filepath = Application.persistentDataPath + "/" + "current_user.dat";
                    Debug.Log(filepath);
                    using (FileStream file = File.Create(filepath))
                    {
                        new BinaryFormatter().Serialize(file, a.ToString());
                        file.Close();
                    }
                    SceneManager.LoadScene("ArtRoom");
                }
                else
                {
                    password.text = "";
                    email.text = "";
                }
            }
        }
    }
    public void Login()
    {
        var emailid = email.text;
        var pwd = password.text;
        Debug.Log(emailid);
        Debug.Log(pwd);
        if (emailid.Length > 0 && pwd.Length>0)
        {
            WWWForm form = new WWWForm();
            form.AddField("username", emailid);
            form.AddField("password", pwd);
            UnityWebRequest req = UnityWebRequest.Post("http://ec2-13-234-114-103.ap-south-1.compute.amazonaws.com/Profile/AuthenticateVR", form);
            StartCoroutine(request(req));
        }
    }
    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("*** Pressed Space bar ***");
            var emailid = "infantsamchris@gmail.com";
            var pwd = "Bententen10";
            Debug.Log(emailid);
            Debug.Log(pwd);
            if (emailid.Length > 0 && pwd.Length > 0)
            {
                WWWForm form = new WWWForm();
                form.AddField("username", emailid);
                form.AddField("password", pwd);
                UnityWebRequest req = UnityWebRequest.Post("http://ec2-13-234-114-103.ap-south-1.compute.amazonaws.com/Profile/AuthenticateVR", form);
                StartCoroutine(request(req));
            }
        }
    }
}
