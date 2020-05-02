using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyController : MonoBehaviourPunCallbacks
{
    // Serialized fields
    [SerializeField] private GameObject mStartButton = null;
    [SerializeField] private GameObject mCancelButton = null;
    [SerializeField] private int mRoomSise = 0;

    // Cached references

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        mStartButton.SetActive(true);
    }

    public void StartGame()
    {
        mStartButton.SetActive(false);
        mCancelButton.SetActive(true);
        PhotonNetwork.JoinRandomRoom();
        Debug.Log("start game");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Join Fail!!!");
        CreateRoom();
    }

    private void CreateRoom()
    {
        Debug.Log("Create Room");
        int rundomRoomNumber = UnityEngine.Random.Range(1, 100);
        RoomOptions roomOptions = new RoomOptions()
        { IsVisible = true, IsOpen = true, MaxPlayers = (byte)mRoomSise };
        PhotonNetwork.CreateRoom("Room" + roomOptions, roomOptions);
        Debug.Log(rundomRoomNumber);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to create room, trying again");
        CreateRoom();
    }

    public void CancelGame()
    {
        mCancelButton.SetActive(false);
        mStartButton.SetActive(true);
        PhotonNetwork.LeaveRoom();
    }
}