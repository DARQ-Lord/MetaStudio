using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static ArtRoomManager;

public class CollectionCreator : MonoBehaviour
{
    public UserData userdata;
    public TMP_InputField name, token;
    public GameObject menu;
    public Button clnBtn;
    // Start is called before the first frame update

    public void closeMenu()
    {
        menu.SetActive(false);
        name.text = "";
        token.text = "";
    }
    public void OpenMenu()
    {
        menu.SetActive(true);
    }
    public void createCollection()
    {
        var cname = name.text;
        var ctoken = token.text;
        var useradd = userdata.user_address;
        if (cname.Length > 0 && ctoken.Length > 0)
        {
            clnBtn.interactable = false;
            WWWForm form = new WWWForm();
            form.AddField("UserAddress", useradd);
            form.AddField("collectionToken", ctoken);
            form.AddField("collectionname", cname); 
            UnityWebRequest req = UnityWebRequest.Post("http://ec2-13-234-114-103.ap-south-1.compute.amazonaws.com/Profile/CreateCollection", form);
            StartCoroutine(request(req));
        }
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
                clnBtn.interactable = true;
            }
            else
            {
                Debug.Log("Form upload complete!");
                clnBtn.interactable= true;
                closeMenu();
            }
        }
    }
        // Update is called once per frame
    void Update()
    {
        
    }
}
