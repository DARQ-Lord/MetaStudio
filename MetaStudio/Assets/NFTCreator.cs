using Defective.JSON;
using Newtonsoft.Json.Linq;
using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class NFTCreator : MonoBehaviour
{
    public Dropdown m_Dropdown;
    List<string> m_DropOptions;
    public UserData userdata;
    public TMP_InputField name, desc;
    public GameObject menu;
    public Camera Cam;
    public Button nftBTN;
    public GameObject screen;

    void Awake()
    {
        m_Dropdown.ClearOptions();
        StartCoroutine(CollectionRequest("http://192.168.29.227/rest/mycollection/"+ userdata.user_address+"/"));
    }
    public IEnumerator CollectionRequest(string url)
    {
        var req = new UnityWebRequest(url, "GET");
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
            if (a.GetField("code").ToString() != "")
            {
                var collections = a.GetField("Collections");
                m_DropOptions = new List<string>(collections.count);
                foreach (var item in collections)
                {
                    var scenarioData = item.GetField("Collection Name").ToString().Replace("\"", "");
                    m_DropOptions.Add(scenarioData);
                }
                m_Dropdown.AddOptions(m_DropOptions);
            }
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    byte[] CamCapture()
    {
        RenderTexture.active = Cam.targetTexture;

        Cam.Render();

        Texture2D Image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height, TextureFormat.RGB24,false);
        Image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0);
        Image.Apply();
        
        var Bytes = Image.EncodeToPNG();
        Destroy(Image);
        return Bytes;
    }
    public void closeMenu()
    {
        menu.SetActive(false);
        name.text = "";
        desc.text = "";
    }
    public void OpenMenu()
    {
        m_Dropdown.ClearOptions();
        StartCoroutine(CollectionRequest("http://ec2-13-234-114-103.ap-south-1.compute.amazonaws.com/rest/mycollection/" + userdata.user_address + "/"));
        menu.SetActive(true);
    }
    public void createNFT()
    {
        var nname = name.text;
        var ndesc = desc.text;
        var useradd = userdata.user_address;
        var cname = m_Dropdown.options[m_Dropdown.value].text;
        print(nname);
        print(ndesc);
        if (nname.Length > 0 && ndesc.Length > 0)
        {
            nftBTN.interactable = false;
            screen.SetActive(true);
            var XX = CamCapture();
            WWWForm form = new WWWForm();
            form.AddField("nftName", nname);
            form.AddField("useraddress", useradd);
            form.AddField("nftDescription", ndesc);
            form.AddField("collectionname", cname);
            form.AddBinaryData("nftFile", XX);
            UnityWebRequest req = UnityWebRequest.Post("http://192.168.29.227/Profile/CreateNFT", form);
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
                nftBTN.interactable = true;
                screen.SetActive(false);
            }
            else
            {
                Debug.Log("Form upload complete!");
                nftBTN.interactable = true;
                screen.SetActive(false);
                closeMenu();
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
