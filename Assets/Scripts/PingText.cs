using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PingText : MonoBehaviour
{
    private Text mPingText;

    // Start is called before the first frame update
    void Awake()
    {
        mPingText = GetComponent<Text>();      
    }

    // Update is called once per frame
    void Update()
    {
        mPingText.text = "Ping: " + PhotonNetwork.GetPing();
    }
}
