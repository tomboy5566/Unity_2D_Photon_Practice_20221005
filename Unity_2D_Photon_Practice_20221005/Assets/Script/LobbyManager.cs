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
        #region 大廳資料
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
        #endregion

        #region 房間資料
        private TextMeshProUGUI textRoomName;
        private TextMeshProUGUI textRoomPlayer;
        private CanvasGroup groupRoom;
        private Button btnStartGame;
        private Button btnLeaveRoom;
        #endregion

        private void Awake()
        {
            GetLobbyObjectAndEvent();

            textRoomName = GameObject.Find("房間內房間名稱").GetComponent<TextMeshProUGUI>();
            textRoomPlayer = GameObject.Find("房間內房間房間人數").GetComponent<TextMeshProUGUI>();
            groupRoom = GameObject.Find("畫布房間").GetComponent<CanvasGroup>();
            btnStartGame = GameObject.Find("開始遊戲").GetComponent<Button>();
            btnLeaveRoom = GameObject.Find("退出房間").GetComponent<Button>();

            btnLeaveRoom.onClick.AddListener(LeaveRoom);

            PhotonNetwork.ConnectUsingSettings();
        }

        private void GetLobbyObjectAndEvent()
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

        private void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();

            groupRoom.alpha = 0;
            groupRoom.interactable = false;
            groupRoom.blocksRaycasts = false;
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

            groupRoom.alpha = 1;
            groupRoom.interactable = true;
            groupRoom.blocksRaycasts = true;

            textRoomName.text = "房間名稱：" + PhotonNetwork.CurrentRoom.Name;
            textRoomPlayer.text = $"房間人數{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            textRoomPlayer.text = $"房間人數{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            textRoomPlayer.text = $"房間人數{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
        }
    }
}