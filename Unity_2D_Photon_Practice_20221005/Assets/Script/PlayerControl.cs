using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Cinemachine;

namespace KuanLun
{
    public class PlayerControl : MonoBehaviourPunCallbacks
    {
        [SerializeField, Header("���ʳt��"), Range(0, 50)]
        private float movespeed = 3.5f;
        [Header("�ˬd�a�O���")]
        [SerializeField] private Vector3 groundoffset;
        [SerializeField] private Vector3 groundsize;
        [SerializeField, Header("���D����"), Range(0, 5000)]
        private float jumpheight = 500f;

        private Rigidbody2D rig;
        private Animator ani;
        private string parWalk = "�]�B�}��";
        private bool isGround;
        private Transform childCanvas;

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0.2f, 0.35f);
            Gizmos.DrawCube(transform.position + groundoffset, groundsize);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name.Contains("ī�G")) PhotonNetwork.Destroy(collision.gameObject);
        }

        private void Start()
        {
            GameObject.Find("CM").GetComponent<CinemachineVirtualCamera>().Follow = transform;
        }

        private void Awake()
        {
            rig = GetComponent<Rigidbody2D>();
            ani = GetComponent<Animator>();

            childCanvas = transform.GetChild(0);

            if (!photonView.IsMine) enabled = false;

            photonView.RPC("RPCUpdateName", RpcTarget.All);
        }

        private void Update()
        {
            Move();
            CheckGround();
            Jump();
        }

        [PunRPC]
        private void RPCUpdateName()
        {
            transform.Find("�e�����a�W��/�W�٤���").GetComponent<TextMeshProUGUI>().text = photonView.Owner.NickName;
        }

        private void Move()
        {
            float h = Input.GetAxis("Horizontal");
            rig.velocity = new Vector2(movespeed * h, rig.velocity.y);
            ani.SetBool(parWalk, h != 0);

            if (Mathf.Abs(h) < 0) return;
            transform.eulerAngles = new Vector3(0, h < 0 ? 180 : 0, 0);
            childCanvas.localEulerAngles = new Vector3(0, h < 0 ? 180 : 0, 0);
        }

        private void CheckGround()
        {
            Collider2D hit = Physics2D.OverlapBox(transform.position + groundoffset, groundsize, 0);
            isGround = hit;
        }

        private void Jump()
        {
            if (isGround && Input.GetKeyDown(KeyCode.Space))
            {
                rig.AddForce(new Vector2(0, jumpheight));
            }
        }
    }
}