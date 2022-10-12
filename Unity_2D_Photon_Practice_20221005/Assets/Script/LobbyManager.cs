using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Photon.Realtime;

namespace KuanLun
{
    public class LobbyManager : MonoBehaviourPunCallbacks
    {
        private TMP_InputField inputFieldPlayerName;
        private TMP_InputField inputFieldNewRoomName;
        private TMP_InputField inputFieldExistRoomName;

        private string namePlayer;
        private string nameNewRoom;
        private string nameExistRoom;

        private Button btnNewRoom;
        private Button btnJoinRoom;
        private Button btnJoinRandomRoom;

        private CanvasGroup cG;

        private void Awake()
        {
            inputFieldPlayerName = GameObject.Find("玩家名稱").GetComponent<TMP_InputField>();
            inputFieldNewRoomName = GameObject.Find("創建房間名稱").GetComponent<TMP_InputField>();
            inputFieldExistRoomName = GameObject.Find("欲加入的指定房間名稱").GetComponent<TMP_InputField>();

            btnNewRoom = GameObject.Find("創建房間").GetComponent<Button>();
            btnJoinRoom = GameObject.Find("加入指定房間").GetComponent<Button>();
            btnJoinRandomRoom = GameObject.Find("加入隨機房間").GetComponent<Button>();

            cG = GameObject.Find("畫布主要").GetComponent<CanvasGroup>();

            inputFieldPlayerName.onEndEdit.AddListener((input) => namePlayer = input);
            inputFieldNewRoomName.onEndEdit.AddListener((input) => nameNewRoom = input);
            inputFieldExistRoomName.onEndEdit.AddListener((input) => nameExistRoom = input);

            btnNewRoom.onClick.AddListener(CreateNewRoom);
            btnJoinRoom.onClick.AddListener(JoinRoom);
            btnJoinRandomRoom.onClick.AddListener(JoinRandomRoom);

            PhotonNetwork.ConnectUsingSettings();
        }

        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            print("已連線至主機");
            cG.interactable = true;
            cG.blocksRaycasts = true;
        }

        private void CreateNewRoom()
        {
            RoomOptions ro = new RoomOptions();
            ro.MaxPlayers = 20;
            ro.IsVisible = true;

            PhotonNetwork.CreateRoom(nameNewRoom, ro);
        }

        private void JoinRoom()
        {
            PhotonNetwork.JoinRoom(nameExistRoom);
        }

        private void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            print("創建房間成功");
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            print("加入房間成功");
        }
    }
}