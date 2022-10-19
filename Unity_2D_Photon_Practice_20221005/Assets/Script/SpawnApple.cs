using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace KuanLun
{
    public class SpawnApple : MonoBehaviour
    {
        [SerializeField, Header("ī�G")]
        private GameObject prefabApple;
        [SerializeField, Header("�ͦ��W�v"), Range(0, 5)]
        private float intervalSpawn = 2.5f;
        [SerializeField, Header("�ͦ��I")]
        private Transform[] spawnPoints;

        private void Awake()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                InvokeRepeating("Spawn", 0, intervalSpawn);
            }
        }

        private void Spawn()
        {
            int random = Random.Range(0, spawnPoints.Length);
            PhotonNetwork.Instantiate(prefabApple.name, spawnPoints[random].position, Quaternion.identity);
        }
    }
}