using DG.Tweening;
using InGame.MyManager;
using InGame.MyManager.Global;
using InGame.MyManager.Local;
using InGame.MyObject;
using InGame.MyObject.Interface;
using InGame.MyObject.Piece;
using MyUtil.GameMode;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MyUtil.MyObjectPool
{
    // 작성자: 조혜찬
    // 오브젝트 풀링 싱글톤 클래스
    public class ObjectPoolManager : MonoSingleton<ObjectPoolManager>
    {
        [SerializeField] private List<ObjectPoolData> _poolDataList; // 인스펙터에서 풀링할 데이터를 담는 리스트 변수

        [SerializeField] private float _animationDuration; // 애니메이션 지속 시간

        private Dictionary<ObjectPoolType, ObjectPoolData> _poolDataMap = new(); // 풀링 맵 - 타입에 맞는 풀링 데이트를 할당
        private Dictionary<ObjectPoolType, Queue<GameObject>> _pool = new(); // 실제 풀 - 여기에 풀링 객체를 풀링 타입에 맞게 추가

        protected override void Awake()
        {
            base.Awake();

            Init(); // 풀 생성
        }

        private void OnDisable()
        {
            NetworkManager.Instance.Socket.Off("makeObject");
        }

        // 풀 생성 함수
        private void Init()
        {
            NetworkManager.Instance.Socket.On("makeObject", (data) =>
            {
                string json = data.GetValue().ToString();

                MakeObjectPoolData makeObjectPoolData = JsonUtility.FromJson<MakeObjectPoolData>(json);
                MainThreadDispatcher.Enqueue(() =>
                {
                    GameObject obj = null;
                    if (makeObjectPoolData.parentName != "") // 부모 객체가 있을 경우
                    {
                        Transform parent = GameObject.Find(makeObjectPoolData.parentName).transform;

                        obj = GetObject((ObjectPoolType)makeObjectPoolData.poolType, parent); // 객체 생성
                    }
                    else // 부모 객체가 없을 경우
                    {
                        obj = GetObject((ObjectPoolType)makeObjectPoolData.poolType); // 객체 생성
                    }

                    if (makeObjectPoolData.angle != 0) // 회전 값이 0이 아닐 경우
                    {
                        obj.transform.Rotate(0, makeObjectPoolData.angle, 0); // 각도 회전
                    }

                    obj.transform.localPosition = makeObjectPoolData.pos; // 객체 위치 할당

                    if(makeObjectPoolData.needAnimation) // 애니메이션이 필요하다면
                    {
                        Animation(obj, true, true);
                    }

                    INetworkIdObject networkIdObject = obj.GetComponent<INetworkIdObject>();
                    networkIdObject.NetworkId = makeObjectPoolData.Id; // 객체 ID 할당
                    ObjectIdManager.Instance.AddObject(networkIdObject.NetworkId, obj); // 객체 Id 정보 저장

                    if (makeObjectPoolData.roadPlacePlaneId != -1) // 도로 배치칸이 존재할 경우
                    {
                        GameObject roadPlacePlaneObj = ObjectIdManager.Instance.FindObject(makeObjectPoolData.roadPlacePlaneId);
                        if (roadPlacePlaneObj) // 도로 배치칸을 찾았을 때
                        {
                            RoadPlacePlaneObject roadPlacePlane = roadPlacePlaneObj.GetComponent<RoadPlacePlaneObject>();
                            PieceBase pieceBase = obj.GetComponent<PieceBase>();
                            InGameContext.Current.Data.PlacePlaneManager.ChangePlacePlaneState(roadPlacePlane, pieceBase, false); // 배치칸 상태 변경
                            InGameContext.Current.Data.PlacePlaneManager.FindCanPlacePlane();
                        }
                    }
                });
            });

            foreach(var data in _poolDataList) // 풀링할 데이터가 담긴 리스트 순회
            {
                _poolDataMap.Add(data.poolType, data); // 풀링 맵에 리스트에 담겨있던 데이터의 값에서 가져온 풀링 타입과 데이터를 추가
            }

            foreach(var data in _poolDataMap) // 풀링 맵 순회
            {
                var poolType = data.Key; // 풀링 타입 지역 변수
                var poolData = data.Value; // 풀링 데이터 지역 변수

                _pool.Add(poolType, new Queue<GameObject>()); // 풀에 풀링 타입과 새로운 큐 추가

                for(int i = 0; i < poolData.poolCount; i++) // 풀링 데이터에서 가져온 풀에 담을 객체의 수만큼 반복
                {
                    GameObject poolObject = CreateObject(poolType); // 새로운 풀링 객체를 생성
                    _pool[poolType].Enqueue(poolObject); // 생성한 풀링 객체를 풀링 타입의 큐에 추가
                }
            }
        }

        // 새로운 풀링 객체 생성 함수(매개변수로 풀링 타입을 받는다)
        private GameObject CreateObject(ObjectPoolType type)
        {
            GameObject newObject = Instantiate(_poolDataMap[type].poolObject, transform); // 새로운 객체를 풀링 맵에서 풀링 타입의 객체를 가져온 후 부모를 풀 매니저로 지정
            newObject.transform.position = Vector3.zero; // 새로 생성한 객체 위치 초기화
            newObject.transform.rotation = Quaternion.identity; // 새로 생성한 객체 회전 초기화
            newObject.SetActive(false); // 새로 생성한 객체 비활성화
            return newObject; // 새로 생성한 객체 반환
        }

        // 네트워크 ID가 필요한 객체를 만드는 함수
        public void MakeObject(ObjectPoolType type, Vector3 pos, Transform parent, bool needAnimation = false, int roadPlacePlaneId = -1, float angle = 0)
        {
            if (_poolDataMap[type].needNetworkID) // 네트워크 ID가 필요한 객체라면
            {
                MakeObjectPoolInfo makeObjectPoolInfo = new MakeObjectPoolInfo
                {
                    roomID = SceneMgr.Instance.CurrentRoomID, // 현재 방 ID
                    parentName = parent.name, // 객체 부모명 
                    poolType = (int)type, // 풀 타입
                    roadPlacePlaneId = roadPlacePlaneId,
                    needAnimation = needAnimation,
                    angle = angle,
                    pos = pos // 객체 위치
                };

                string json = JsonUtility.ToJson(makeObjectPoolInfo);

                if (GameModeManager.Instance.CurrentGameMode.UseServer())
                    NetworkManager.Instance.Socket.Emit("makePoolObject", json);
            }
        }

        // 외부에서 풀에서 객체를 가져올 때 부르는 함수(매개 변수로 풀링 타입, 부모 = 기본 값 null을 받는다)
        public GameObject GetObject(ObjectPoolType type, Transform parent = null)
        {
            if (_pool[type].Count > 0) // 풀링 타입의 풀에 객체가 존재한다면
            {
                GameObject obj = _pool[type].Dequeue(); // 풀링 타입의 풀에 있는 객체를 가져온다.
                obj.transform.SetParent(parent, false); // 풀에서 꺼낸 객체의 부모를 할당
                obj.SetActive(true); // 풀에서 꺼낸 객체 활성화

                return obj; // Task 완료 시 반환되는 GameObject 반환
            }
            else // 만약 풀링 타입의 풀에 객체가 존재하지 않는다면
            {
                GameObject newObj = null;
                newObj = CreateObject(type); // 새롭게 풀링 타입에 맞는 객체 생성
                newObj.transform.SetParent(parent, false); // 새롭게 생성한 객체의 부모를 할당
                newObj.SetActive(true); // 새롭게 생성한 객체 활성화

                return newObj; // Task 완료 시 반환되는 GameObject 반환
            }
        }

        // 외부에서 사용했던 객체를 다시 풀에 넣을 때 부르는 함수(매개 변수로 풀링 타입, 반환할 객체를 받는다, 객체인지 UI인지 여부, 애니메이션 필요 여부)
        public void ReturnObject(ObjectPoolType type, GameObject returnObj, bool needAnimation = false, bool isObject = true)
        {
            if (_poolDataMap[type].needNetworkID) // 네트워크 ID가 필요한 객체라면
            {
                INetworkIdObject networkIdObject = returnObj.GetComponent<INetworkIdObject>();
                ObjectIdManager.Instance.RemoveObject(networkIdObject.NetworkId); // id를 가진 객체를 목록에서 제거
            }

            if (needAnimation) // 애니메이션이 필요하다면
            {
                Animation(returnObj, isObject, false)
                    .OnComplete(() =>
                    {
                        ResetObject(type, returnObj);
                    });
            }
            else // 애니메이션이 필요 없다면
            {
                ResetObject(type, returnObj);
            }
        }

        // 풀링 대상 초기화 함수
        private void ResetObject(ObjectPoolType type, GameObject returnObj)
        {
            returnObj.transform.SetParent(transform); // 반환하는 객체의 부모를 풀 매니저로 지정
            returnObj.transform.localPosition = Vector3.zero; // 반환하는 객체의 위치 초기화
            returnObj.transform.localRotation = Quaternion.identity; // 반환하는 객체의 회전 초기화
            returnObj.transform.localScale = Vector3.one; // 크기 초기화

            returnObj.SetActive(false); // 반환하는 객체 비활성화

            _pool[type].Enqueue(returnObj); // 풀링 타입의 풀에 객체 추가
        }

        public Tween Animation(GameObject obj, bool isObject, bool isCreate)
        {
            float startValue = isCreate ? 1f : 0f; // 생성일 경우 1 할당 아닐 경우 0 할당
            float endValue = isObject ? isCreate ? 0f : 1f : isCreate ? 1f : 0f; // 객체이면서 생성일 경우 0 할당 객체이면서 생성이 아닐 경우 1 할당
                                                                                // UI이면서 생성일 경우 1 할당 UI이면서 생성이 아닐 경우 0 할당

            if (isObject) // 객체일 때
            {
                Renderer renderer = obj.GetComponentInChildren<Renderer>(); // 객체 랜더 탐색
                Material mat = new Material(renderer.sharedMaterial); // 공유 머티리얼로 새 머티리얼 생성
                renderer.material = mat; // 객체 랜더에 새 머티리얼 할당
                mat.SetFloat("_Cutoff", startValue); // 객체 보임 상태
                return mat.DOFloat(endValue, "_Cutoff", _animationDuration); // _animationDuration초 동안 페이드 아웃;
            }
            else // UI 일 때
            {
                CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>(); // 캔버스 그룹 가져오기
                return canvasGroup.DOFade(endValue, _animationDuration); // 이미지 페이드 아웃
            }
        }
    }
}
// 마지막 작성 일자: 2026.03.19