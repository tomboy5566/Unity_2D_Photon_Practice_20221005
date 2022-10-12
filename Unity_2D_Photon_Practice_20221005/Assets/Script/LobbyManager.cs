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

            PhotonNetwork.ConnectUsingSettings();
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

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            print("�Ыةж����\");
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            print("�[�J�ж����\");
        }
    }
}