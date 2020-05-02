using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    // TODO: FIX COMMENT - Portals: We manually pass the portal objects in the scene to this manager, in numerical order (0-half:player1, (half+1)-end:player2)
    [SerializeField] Dictionary<int,Portal> mPortalDictionary;
    [SerializeField] GameObject mPortalsTilemapLayer;
    [SerializeField] GameObject mPortalPrefab;

    // Cached References
    private PortalContainer mPortalContainer;

    private int numOfPortals;


    private void Awake()
    {
        mPortalContainer = GameObject.Find("PortalContainer").GetComponent<PortalContainer>();
    }

    void Start()
    {
        /*numOfPortals = mPortals.Length;
        AssignPortalPairs();
        foreach (Portal portal in mPortals)
        {
            portal.CalculateSpawnInfo();
        }*/

        //PortalContainer portalContainer = GameObject.Find("PortalContainer").GetComponent<PortalContainer>();

        //if (PhotonNetwork.IsMasterClient)
        {
            //mPortalContainer.CreatePortals(2);
            Portal portal1 = Instantiate(mPortalPrefab, new Vector3(-11.5f, 1.5f, 0f), Quaternion.identity).GetComponent<Portal>();
            portal1.mId = 1;
            Portal portal2 = Instantiate(mPortalPrefab, new Vector3(12.5f, -3.5f, 0f), Quaternion.identity).GetComponent<Portal>();
            portal2.mId = 2;
            portal1.name = "portal1";
            portal2.name = "portal2";
            portal2.tag = "PortalRight";
            portal1.CalculateSpawnInfo();
            portal2.CalculateSpawnInfo();
            portal1.AssignDestinationPortal();
            portal2.AssignDestinationPortal();
        }
    }

    private void AssignPortalPairs()
    {
        /*for (int i = 0; i < (numOfPortals / 2); i++)
        {
            mPortals[i].AssignDestinationPortal(mPortals[numOfPortals / 2 + i]);
            mPortals[numOfPortals / 2 + i].AssignDestinationPortal(mPortals[i]);
        }*/


    }
}
