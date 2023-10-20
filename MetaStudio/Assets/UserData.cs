using Defective.JSON;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;

public class UserData : MonoBehaviour
{
    public string id,user_name, user_address, user_email;

    // Start is called before the first frame update
    private void Awake()
    {
        string userdetails = Application.persistentDataPath + "/" + "current_user.dat";
        using (FileStream file = File.Open(userdetails, FileMode.Open))
        {
            object loadedData = new BinaryFormatter().Deserialize(file);
            var a = new JSONObject(loadedData.ToString());
            print(a.ToString());
            user_name = a.GetField("User Name").ToString().Trim('"');
            user_address = a.GetField("User Address").ToString().Trim('"');
            user_email = a.GetField("User Email").ToString().Trim('"');
            id = a.GetField("Id").ToString().Trim('"');
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
