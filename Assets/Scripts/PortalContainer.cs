using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalContainer : MonoBehaviour
{
    private Dictionary<int, Portal> mPortalDictionary;
    
    void Awake()
    {
        mPortalDictionary = new Dictionary<int, Portal>();
    }

    public void AddPortalToDictionary(Portal portal)
    {
        mPortalDictionary[portal.mId] = portal;
    }

    public void CreatePortals(int numOfPortals)
    {
        Portal portal1 = PhotonNetwork.InstantiateSceneObject("Prefabs\\Portal", new Vector3(-11.5f, 1.5f, 0f), Quaternion.identity).GetComponent<Portal>();
        Portal portal2 = PhotonNetwork.InstantiateSceneObject("Prefabs\\Portal", new Vector3(12.5f, -3.5f, 0f), Quaternion.identity).GetComponent<Portal>();
        mPortalDictionary[1].transform.parent = gameObject.transform;
        mPortalDictionary[2].transform.parent = gameObject.transform;
        mPortalDictionary[1].mId = 1;
        mPortalDictionary[2].mId = 2;
        mPortalDictionary[1].name = "portal1";
        mPortalDictionary[2].name = "portal2";
        mPortalDictionary[2].tag = "PortalRight";
        mPortalDictionary[1].CalculateSpawnInfo();
        mPortalDictionary[2].CalculateSpawnInfo();
        mPortalDictionary[1].AssignDestinationPortal();
        mPortalDictionary[2].AssignDestinationPortal();
    }

    public Portal GetPortalById(int portalId)
    {
        return mPortalDictionary[portalId];
    }

    public void AssignDestinationPortals()
    {
        Portal[] portals = GetComponentsInChildren<Portal>();

        foreach (Portal portal in portals)
        {

        }
    }
}
