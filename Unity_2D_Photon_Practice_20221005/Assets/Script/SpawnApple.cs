using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace KuanLun
{
    public class SpawnApple : MonoBehaviour
    {
        [SerializeField, Header("카찱")]
        private GameObject prefabApple;
        [SerializeField, Header("Ν┬픚쾤"), Range(0, 5)]
        private float intervalSpawn = 2.5f;
        [SerializeField, Header("Ν┬헕")]
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