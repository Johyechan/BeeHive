using InGame.MyManager.Global;
using MyUtil.GameMode;
using Tutorial.MyEnum;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace Tutorial
{
    // 작성자: 조혜찬
    // 스탠실 홀에 따라 레이캐스트 판정도 홀에 맞춰 뚫어주는 클래스
    public class StencilHoleRaycastFilter : MonoBehaviour, ICanvasRaycastFilter
    {
        [SerializeField] private RectTransform _rect; // 기준 UI

        [SerializeField] private Material _stencilMat; // 스탠실 머티리얼

        private Vector2 _holeCenter = new(0.5f, 0.5f);
        private Vector2 _holeScale = Vector2.one;

        private float _holeRadius = 0.45f;
        
        // 레이캐스트가 유효한 위치인지 확인하는 함수(화면 클릭 좌표, 카메라)
        public bool IsRaycastLocationValid(Vector2 sp, Camera eventCamera)
        {
            if(GameModeManager.Instance.CurrentGameMode.IsTutorial()) // 튜토리얼 일 때
            {
                if (TutorialManager.Instance.CurrentTutorialState == TutorialState.Turn6_AI) // 여섯 번째 턴(AI 턴)일 경우
                    return true; // 바로 true(현재 UI가 레이캐스트를 받도록)반환
            }
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_rect, sp, eventCamera, out var local); // 클릭 위치를 UI 내부 위치로 변경(기준 UI 영역, 화면 클릭 좌표, 카메라, 변환된 로컬 좌표)

            var realMat = _rect.GetComponent<Graphic>().materialForRendering;

            _holeCenter = realMat.GetVector("_HoleCenter");
            _holeScale = realMat.GetVector("_HoleScale");
            _holeRadius = realMat.GetFloat("_HoleRadius");

            var r = _rect.rect;

            // Local → UV
            Vector2 uv = new(
                (local.x - r.x) / r.width,
                (local.y - r.y) / r.height
            );  // 셰이더와 같은 좌표계를 사용하기 위해서 생성 (pivot 기준 좌표계(UI 좌표계) -> 좌하단 기준 좌표계(셰이더 UV 좌표계))
                // pivot 기준 좌표는 x: -width / 2 ~ +width / 2, y: -height / 2 ~ +height / 2 (음양 혼합 좌표계)
                // 셰이더 기준 좌표는 x: 0~1, y: 0~1 (항상 양수)
                // 그래서 pivot 기준 좌표를 그대로 쓰면 (0,0) -> 좌하단으로 인식(실제는 중앙), (-0.5, 0) -> 화면 밖으로 인식, (0.5, 0) -> 중앙 쯤으로 인식
                // 그래서 셰이더에서 정한 구멍의 위치를 찾기 위해서 좌하단 기준 좌표로 변경을 하면서(원점으로 이동) 정규화를 진행(크기로 나누기)

            // 셰이더와 동일 계산 (클릭 위치와 구멍의 중앙 값과 얼마나 떨어졌는지 확인)
            Vector2 offset = uv - _holeCenter;

            // 화면 비율 맞추기
            float aspect = (float)Screen.width / Screen.height;
            offset.x *= aspect;

            offset /= _holeScale;

            float dist = offset.magnitude;

            return dist >= _holeRadius;
        }
    }
}
// 마지막 작성 일자: 2026.03.25