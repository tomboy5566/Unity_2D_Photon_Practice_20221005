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
        [SerializeField, Header("移動速度"), Range(0, 50)]
        private float movespeed = 3.5f;
        [Header("檢查地板資料")]
        [SerializeField] private Vector3 groundoffset;
        [SerializeField] private Vector3 groundsize;
        [SerializeField, Header("跳躍高度"), Range(0, 5000)]
        private float jumpheight = 500f;

        private Rigidbody2D rig;
        private Animator ani;
        private string parWalk = "跑步開關";
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
            if (collision.name.Contains("蘋果"))
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

            textApple = transform.Find("畫布玩家名稱/蘋果數量").GetComponent<TextMeshProUGUI>();
            groupGame = GameObject.Find("畫布遊戲介面").GetComponent<CanvasGroup>();
            textWinner = GameObject.Find("獲勝者").GetComponent<TextMeshProUGUI>();

            btnBackToLobby = GameObject.Find("返回遊戲大廳").GetComponent<Button>();
            btnBackToLobby.onClick.AddListener(() =>
            {
                if (photonView.IsMine)
                {
                    PhotonNetwork.LeaveRoom();
                    PhotonNetwork.LoadLevel("遊戲大廳");
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
            transform.Find("畫布玩家名稱/名稱介面").GetComponent<TextMeshProUGUI>().text = photonView.Owner.NickName;
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

            textWinner.text = "獲勝者：" + photonView.Owner.NickName;

            DestroyObject();
        }

        private void DestroyObject()
        {
            GameObject[] apples = GameObject.FindGameObjectsWithTag("蘋果");
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