using UnityEngine;

namespace InGame.MyUI
{
    // 작성자: 조혜찬
    // UI를 회전 시키는 클래스
    public class RotateUI : MonoBehaviour
    {
        [SerializeField] private float _speed;

        private void OnEnable()
        {
            transform.rotation = Quaternion.identity; // 회전 초기화
        }

        void Update()
        {
            transform.Rotate(new Vector3(0, 0, _speed * Time.deltaTime)); // _speed 속도로 무환 회전
        }
    }
}
// 마지막 작성 일자: 2025.08.06