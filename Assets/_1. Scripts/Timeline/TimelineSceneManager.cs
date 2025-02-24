using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Timeline
{
    public class TimelineSceneManager : MonoBehaviour
    {
        private static TimelineSceneManager instance;
        public static TimelineSceneManager Instance { get { return instance; } }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnEnable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
        }


        private void OnDisable()
        {
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Debug.Log($"씬 로드 이벤트 호출됨, {scene.name}, {mode}");
        }


        private void Start()
        {

        }

        private void Update()
        {
            if (PhotonNetwork.IsMasterClient)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    Debug.Log("씬 이동");
                    PhotonNetwork.LoadLevel("14.PUN_Room");
                }
                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    Debug.Log("이동 씬");
                    PhotonNetwork.LoadLevel("Timeline");
                }
            }
        }
    }
}

//1. 레벨 씬(화재 초급, 지진 고급 등)이 로드될 때 OnSceneLoaded이벤트로 플레이어 생성,
//OnJoinedRoom이나 로비에서 하는것보다 씬이 로드되고 플레이어 생성하는게 나아보임
//2. 포톤뷰 컴포넌트의 문제로 이전 씬으로 돌아가면 문제가 생긴다고 했는데, 한 씬에서 모든 일을 처리하면 노상관 않인가?
//씬 이동 -> 기존 오브젝트 파괴 -> 새 씬으로 넘어가면서 새로운 플레이어 생성
//커스텀 프로퍼티도 플레이어가 아닌 룸 만 이전 상태 저장용 쓸거같은데 엄
//흐름이 어찌 되는지 먼저 물어봐야 할덧함
//1. 방 생성 -> 화재 초급 -> 로비로 가서 방 파괴 -> 방 생성 -> 화재 중급
//2. 방 생성 -> 화재 초급 -> 로비로 안가고 방 유지 -> 화재 중급

