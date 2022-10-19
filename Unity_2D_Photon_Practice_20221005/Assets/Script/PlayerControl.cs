using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Cinemachine;
using UnityEngine.UI;

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
        private TextMeshProUGUI textApple;
        private int countApple;
        private int countAppleMax = 5;
        private CanvasGroup groupGame;
        private TextMeshProUGUI textWinner;
        private Button btnBackToLobby;

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(1, 0, 0.2f, 0.35f);
            Gizmos.DrawCube(transform.position + groundoffset, groundsize);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.name.Contains("ī�G"))
            {
                Destroy(collision.gameObject);

                textApple.text = (++countApple).ToString();

                if (countApple >= countAppleMax) Win();
            }
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

            textApple = transform.Find("�e�����a�W��/ī�G�ƶq").GetComponent<TextMeshProUGUI>();
            groupGame = GameObject.Find("�e���C������").GetComponent<CanvasGroup>();
            textWinner = GameObject.Find("��Ӫ�").GetComponent<TextMeshProUGUI>();

            btnBackToLobby = GameObject.Find("��^�C���j�U").GetComponent<Button>();
            btnBackToLobby.onClick.AddListener(() =>
            {
                if (photonView.IsMine)
                {
                    PhotonNetwork.LeaveRoom();
                    PhotonNetwork.LoadLevel("�C���j�U");
                }
            });
        }

        private void Update()
        {
            Move();
            CheckGround();
            Jump();
            BackToTop();
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

        private void Win()
        {
            groupGame.alpha = 1;
            groupGame.interactable = true;
            groupGame.blocksRaycasts = true;

            textWinner.text = "��Ӫ̡G" + photonView.Owner.NickName;

            DestroyObject();
        }

        private void DestroyObject()
        {
            GameObject[] apples = GameObject.FindGameObjectsWithTag("ī�G");
            for (int i = 0; i < apples.Length; i++) Destroy(apples[i]);
            Destroy(FindObjectOfType<SpawnApple>().gameObject);
        }

        private void BackToTop()
        {
            if (transform.position.y < -25)
            {
                transform.position = new Vector3(0, 1, 0);
            }
        }
    }
}