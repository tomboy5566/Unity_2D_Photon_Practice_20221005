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
        #region �j�U���
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

        #region �ж����
        private TextMeshProUGUI textRoomName;
        private TextMeshProUGUI textRoomPlayer;
        private CanvasGroup groupRoom;
        private Button btnStartGame;
        private Button btnLeaveRoom;
        #endregion

        private void Awake()
        {
            GetLobbyObjectAndEvent();

            textRoomName = GameObject.Find("�ж����ж��W��").GetComponent<TextMeshProUGUI>();
            textRoomPlayer = GameObject.Find("�ж����ж��ж��H��").GetComponent<TextMeshProUGUI>();
            groupRoom = GameObject.Find("�e���ж�").GetComponent<CanvasGroup>();
            btnStartGame = GameObject.Find("�}�l�C��").GetComponent<Button>();
            btnLeaveRoom = GameObject.Find("�h�X�ж�").GetComponent<Button>();

            btnLeaveRoom.onClick.AddListener(LeaveRoom);

            PhotonNetwork.ConnectUsingSettings();
        }

        private void GetLobbyObjectAndEvent()
        {
            inputFieldPlayerName = GameObject.Find("���a�W��").GetComponent<TMP_InputField>();
            inputFieldNewRoomName = GameObject.Find("�Ыةж��W��").GetComponent<TMP_InputField>();
            inputFieldExistRoomName = GameObject.Find("���[�J�����w�ж��W��").GetComponent<TMP_InputField>();

            btnNewRoom = GameObject.Find("�Ыةж�").GetComponent<Button>();
            btnJoinRoom = GameObject.Find("�[�J���w�ж�").GetComponent<Button>();
            btnJoinRandomRoom = GameObject.Find("�[�J�H���ж�").GetComponent<Button>();

            cG = GameObject.Find("�e���D�n").GetComponent<CanvasGroup>();

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

            print("�w�s�u�ܥD��");
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
            print("�Ыةж����\");
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            print("�[�J�ж����\");

            groupRoom.alpha = 1;
            groupRoom.interactable = true;
            groupRoom.blocksRaycasts = true;

            textRoomName.text = "�ж��W�١G" + PhotonNetwork.CurrentRoom.Name;
            textRoomPlayer.text = $"�ж��H��{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            textRoomPlayer.text = $"�ж��H��{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            textRoomPlayer.text = $"�ж��H��{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
        }
    }
}