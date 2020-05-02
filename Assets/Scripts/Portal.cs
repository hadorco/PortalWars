using ExitGames.Client.Photon;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class Portal : MonoBehaviourPun
{
    #region Member Variables

    public Vector3 mSpawnPosition;
    public Vector2 mSpawnVelocity;
    public int mId { get; set; }

    // Cached References
    private PhotonView mPhotonView;
    private PortalContainer mPortalContainer;

    [SerializeField] Portal mDestinationPortal = null;

    #endregion

    #region Unity Lifetime Functions

    void Awake()
    {
        mPhotonView = GetComponent<PhotonView>();
        
        // Portals are children of the PortalContainer object upon instantiation
    }

    private void Start()
    {
        //string fdsfs = gameObject.transform.parent.name;
        //mPortalContainer = GetComponentInParent<PortalContainer>();
        PhotonNetwork.AllocateViewID(mPhotonView);
    }

    #endregion

    #region Public Functions

    // Called by the Manager class on startup
    public void CalculateSpawnInfo()
    {
        //mPhotonView.RPC("RpcCalculateSpawnInfo", RpcTarget.AllBuffered);

        Vector3 positionOffset = new Vector3(0, 0, 0);
        Vector2 newVelocity = new Vector2(0, 0);
        switch (gameObject.tag)
        {
            case "PortalTop":
                positionOffset.y = -1;
                newVelocity.y = -1;
                break;
            case "PortalBottom":
                positionOffset.y = 1;
                newVelocity.y = 1;
                break;
            case "PortalLeft":
                positionOffset.x = 1f;
                newVelocity.x = 1;
                break;
            case "PortalRight":
                positionOffset.x = -1;
                newVelocity.x = -1;
                break;
            default:
                break;
        }

        mSpawnPosition = transform.position + positionOffset;
        mSpawnVelocity = newVelocity;
    }

    // Called by the Manager class on startup
    public void AssignDestinationPortal()
    {
        Portal destinationPortal = null;
        if (mId % 2 != 0) // odd
        {
            destinationPortal = GameObject.Find("portal" + (mId + 1)).GetComponent<Portal>();
        }
        else // even
        {
            destinationPortal = GameObject.Find("portal" + (mId - 1)).GetComponent<Portal>();
        }

        mDestinationPortal = destinationPortal;
        //mDestinationPortal = destinationPortal;
        //int portalId = destinationPortal.mId;
        //mPhotonView.RPC("RpcAssignDestinationPortal", RpcTarget.AllBuffered);
    }

    #endregion

    #region Private Functions

    private void SpawnClone(string projectileTypeName)
    {
        GameObject projectileClone = (GameObject)Instantiate(Resources.Load("Prefabs\\" + projectileTypeName), mSpawnPosition, Quaternion.identity);

        Projectile projectile = projectileClone.GetComponent<Projectile>();
        projectile.SetSpriteDirection(new Vector3(1, 1, 1) * mSpawnVelocity.x);
        projectile.SetVelocity(mSpawnVelocity);
    }

    /*void IPunInstantiateMagicCallback.OnPhotonInstantiate(PhotonMessageInfo info)
    {
        PortalContainer portalContainer = GameObject.Find("PortalContainer").GetComponent<PortalContainer>();
        if (PhotonNetwork.IsMasterClient)
        {

            mPortalDictionary[1].mId = 1;
            mPortalDictionary[2].mId = 2;
            mPortalDictionary[1].name = "portal1";
            mPortalDictionary[2].name = "portal2";
            mPortalDictionary[2].tag = "PortalRight";
            mPortalDictionary[1].CalculateSpawnInfo();
            mPortalDictionary[2].CalculateSpawnInfo();
            mPortalDictionary[1].AssignDestinationPortal();
            mPortalDictionary[2].AssignDestinationPortal();
            portalContainer.AddPortalToDictionary(this);
        }
        else
        {
            Portal blueprint = portalContainer.GetPortalById(mId);
            mSpawnPosition = blueprint.mSpawnPosition;
            mSpawnVelocity = blueprint.mSpawnVelocity;
            mDestinationPortal = blueprint.mDestinationPortal;
        }

    }

    void IPunObservable.OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(gameObject.name);
            stream.SendNext(mSpawnPosition);
            stream.SendNext(mSpawnVelocity);
            //stream.SendNext(mSpawnVelocity.x);
            //stream.SendNext(mSpawnVelocity.y);
        }
        else
        {
            gameObject.name = (string)stream.ReceiveNext();
            mSpawnPosition = ((Vector3)stream.ReceiveNext());
            mSpawnVelocity = (Vector2)stream.ReceiveNext();
            //mSpawnVelocity.x = (float)stream.ReceiveNext();
            //mSpawnVelocity.y = (float)stream.ReceiveNext();
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (mPhotonView.IsMine)
        {
            Projectile projectile = collision.GetComponent<Projectile>();
            if (projectile != null)
            {
                string projectileTypeName = projectile.GetType().Name;
                mPhotonView.RPC("RpcSpawnClone", RpcTarget.AllBuffered, projectileTypeName);
            }
        }
    }

    #endregion


    [PunRPC]
    public void RpcAssignDestinationPortal()
    {
        Portal destinationPortal = null;
        if (mId % 2 != 0) // odd
        {
            destinationPortal = GameObject.Find("portal" + (mId + 1)).GetComponent<Portal>();
        }
        else // even
        {
            destinationPortal = GameObject.Find("portal" + (mId - 1)).GetComponent<Portal>();
        }

        mDestinationPortal = destinationPortal;

        //PortalContainer portalContainer = GameObject.Find("PortalContainer").GetComponent<PortalContainer>();
        //Portal destinationPortal = portalContainer.GetPortalById(destinationPortalId);
        //mDestinationPortal = destinationPortal;
        ////Portal destinationPortal = (Portal)rpcParams[0];
        ////mDestinationPortal = destinationPortal;
        ////Portal g = Instantiate(Resources.Load("Prefabs\\Portal"), new Vector3(0, 3, 0), Quaternion.identity) as Portal;
        //Debug.Log("Assigning destination portal: " + gameObject.name + "-> " + mDestinationPortal.name);
    }

    [PunRPC]
    public void RpcCalculateSpawnInfo()
    {
        Vector3 positionOffset = new Vector3(0, 0, 0);
        Vector2 newVelocity = new Vector2(0, 0);
        switch (gameObject.tag)
        {
            case "PortalTop":
                positionOffset.y = -1;
                newVelocity.y = -1;
                break;
            case "PortalBottom":
                positionOffset.y = 1;
                newVelocity.y = 1;
                break;
            case "PortalLeft":
                positionOffset.x = 1f;
                newVelocity.x = 1;
                break;
            case "PortalRight":
                positionOffset.x = -1;
                newVelocity.x = -1;
                break;
            default:
                break;
        }

        mSpawnPosition = transform.position + positionOffset;
        mSpawnVelocity = newVelocity;
    }

    [PunRPC]
    public void RpcSpawnClone(string projectileTypeName)
    {
        //if (mPhotonView.IsMine)
        {
            mDestinationPortal.SpawnClone(projectileTypeName);
        }
    }

    

    // Serialization/deserialization methods to be used by Photon
    /*public static object Deserialize(byte[] arrBytes)
    {
        using (var memStream = new MemoryStream())
        {
            var binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            var obj = binForm.Deserialize(memStream);
            return (Portal)obj;
        }
    }

    public static byte[] Serialize(object obj)
    {
        BinaryFormatter bf = new BinaryFormatter();
        using (var ms = new MemoryStream())
        {
            bf.Serialize(ms, obj);
            return ms.ToArray();
        }
    }*/
}
