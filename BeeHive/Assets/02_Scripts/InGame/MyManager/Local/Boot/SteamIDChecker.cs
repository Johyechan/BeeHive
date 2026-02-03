using InGame.MyManager.Boot.Struct;
using InGame.MyManager.Global;
using Steamworks;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyManager.Local.Boot
{
    // 작성자: 조혜찬
    // 스팀 ID 체크로 클라이언트 확인 클래스
    public class SteamIDChecker : CheckerBase
    {
        private BootingManager _bootingManager; // 부팅 매니저 클래스

        private Callback<GetTicketForWebApiResponse_t> _getTicketForWebApiCallback;

        private TaskCompletionSource<string> _getTicketTcs;

        private bool _callbackInitialized; // 콜백 초기화 여부

        public SteamIDChecker(BootingManager bootingManager)
        {
            _bootingManager = bootingManager;
        }

        private void CallbackInit() // 콜백 초기화
        {
            if (_callbackInitialized) // 이미 콜백이 초기화 되어있다면
            {
                NetworkManager.Instance.Socket.Emit("debug", "이미 콜백이 초기화 되어있음 - SteamIDChecker");
                return; // 반환
            }

            _getTicketForWebApiCallback =
                Callback<GetTicketForWebApiResponse_t>.Create(cb => // 리스너 추가 생성
                {
                    if (cb.m_eResult != EResult.k_EResultOK) // 이 Steam API 호출의 결과가 정상 성공이 아닐 경우
                    {
                        NetworkManager.Instance.Socket.Emit("debug", "Steam API 호출 결과가 정상이 아님");
                        _getTicketTcs?.TrySetException(new Exception("Steam auth failed")); // 예외 발생 Steam 검증 실패
                    }
                    else
                    {
                        _getTicketTcs?.TrySetResult(Convert.ToBase64String(
                            cb.m_rgubTicket, 0, (int)cb.m_cubTicket)); // m_rgubTicket은 Steam이 발급한 인증 티켓의 원본 바이너리 데이터(byte 배열)
                                                                       // m_cubTicket은 이 티켓 데이터의 실제 길이
                                                                       // Convert.ToBase64String 함수는 이 바이너리 티켓을 문자열로 변환해라(Base64로 인코딩)
                                                                       // 0은 함수에서 배열의 변환 인덱스 지점(변환 시작 인덱스)
                    }


                    _getTicketTcs = null; // 작업이 끝난 tcs는 초기화(중복 방지)
                });

            _callbackInitialized = true;
        }

        // 스팀에게 인증 티켓 발급 요청 함수
        private Task<string> RequestWebApiTicketAsync()
        {
            CallbackInit(); // 콜백 초기화
            if (_getTicketTcs != null) // 이미 tcs가 존재한다면
            {
                NetworkManager.Instance.Socket.Emit("debug", "이미 tcs가 존재함 - SteamIDChecker");
                return _getTicketTcs.Task; // 이미 존재하는 tcs 반환
            }

            _getTicketTcs = new TaskCompletionSource<string>(); // 새로운 티켓 값을 가지는 tcs 생성

            SteamUser.GetAuthTicketForWebApi("my_gameserver"); // Steam 클라이언트에게 Web API 인증용 티켓 발급 요청(문자열은 식별자)

            return _getTicketTcs.Task;
        }

        protected override async Task<bool> Check()
        {
            _bootingManager.CreateSteamAuthEndTcs(); // 스팀 인증 대기 tcs 생성

            NetworkManager.Instance.Socket.Emit("debug", "스팀 체크");

            string authTicketBase64 = "";
            try
            {
                authTicketBase64 = await RequestWebApiTicketAsync();
            }
            catch (Exception ex)
            {
                NetworkManager.Instance.Socket.Emit("debug", $"{ex}");
                return false;
            }

            SteamAuthInfo authInfo = new SteamAuthInfo()
            {
                ticket = authTicketBase64,
                appID = 480//4317470 <- 이게 실제 appID
            };

            string json = JsonUtility.ToJson(authInfo);
            NetworkManager.Instance.Socket.Emit("steamAuth", json);

            bool result = await _bootingManager.WaitSteamAuth();

            return result; 
        }
    }
}
// 마지막 작성 일자: 2026.02.03