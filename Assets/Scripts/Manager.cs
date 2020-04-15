using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    // Portals: We manually pass the portal objects in the scene to this manager, in numerical order (0-half:player1, (half+1)-end:player2)
    [SerializeField] Portal[] mPortals;
    private int numOfPortals;

    // Start is called before the first frame update
    void Start()
    {
        numOfPortals = mPortals.Length;
        AssignPortalPairs();
        foreach (Portal portal in mPortals)
        {
            portal.CalculateSpawnInfo();
        }

        //mPortals[4].CalculateDestinationInfo();
        //mPortals[5].CalculateDestinationInfo();

        Debug.Log(transform.right);
    }

    private void AssignPortalPairs()
    {
        // TESTING DELETE
        //mPortals[4].AssignDestinationPortal(mPortals[5]);
        //mPortals[5].AssignDestinationPortal(mPortals[4]);

        for (int i = 0; i < (numOfPortals / 2); i++)
        {
            mPortals[i].AssignDestinationPortal(mPortals[numOfPortals / 2 + i]);
            mPortals[numOfPortals / 2 + i].AssignDestinationPortal(mPortals[i]);
        }
    }
}
