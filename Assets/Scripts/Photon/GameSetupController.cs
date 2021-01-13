using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Photon.Pun;

public class GameSetupController : MonoBehaviour
{
    [SerializeField] HealthBar mHealthBarPrefab;
    private Canvas mCanvas;
    private Player mInstantiatedPlayer = null;
    private HealthBar mLeftHealthBar = null;
    private HealthBar mRightHealthBar = null;


    private void Awake()
    {
        mCanvas = GameObject.Find("PingCanvas").GetComponent<Canvas>();
    }

    void Start()
    {
        CreateHealthBars();
        CreatePlayer();
    }

    private void CreateHealthBars()
    {
        mLeftHealthBar = Instantiate(mHealthBarPrefab, new Vector3(-495f, -30f, 0f), Quaternion.identity).GetComponent<HealthBar>();
        mLeftHealthBar.transform.SetParent(mCanvas.transform, false);
        mLeftHealthBar.name = "HealthBarLeft";
        mRightHealthBar = Instantiate(mHealthBarPrefab, new Vector3(305f, -30f, 0f), Quaternion.identity).GetComponent<HealthBar>();
        mRightHealthBar.transform.SetParent(mCanvas.transform, false);
        mRightHealthBar.name = "HealthBarRight";
    }

    private void CreatePlayer()
    {
        mInstantiatedPlayer = PhotonNetwork.Instantiate("Prefabs\\Robot", new Vector3(-3f, 4f, 0f), Quaternion.identity).GetComponent<Player>();
    }
}
