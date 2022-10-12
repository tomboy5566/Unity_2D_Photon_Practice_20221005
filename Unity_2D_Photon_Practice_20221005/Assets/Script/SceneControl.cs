using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace KuanLun
{
    public class SceneControl : MonoBehaviourPunCallbacks
    {
        [SerializeField, Header("玩家預製物")]
        private GameObject prefabPlayer;

        private void Awake()
        {
            InitializePlayer();
        }

        private void InitializePlayer()
        {
            Vector3 pos = Vector3.zero;
            pos.x = Random.Range(-4, 5);
            pos.y = 0;
            PhotonNetwork.Instantiate(prefabPlayer.name, pos, Quaternion.identity);
        }
    }
}