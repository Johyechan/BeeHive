using InGame.MyManager.Boot.Struct;
using Steamworks;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace InGame.MyManager.Boot
{
    // 작성자: 조혜찬
    // 스팀 ID 체크로 클라이언트 확인 클래스
    public class SteamIDChecker : CheckerBase
    {
        byte[] _ticketBuffer = new byte[1024]; // 스팀이 주인 인증 티켓 저장 배열 - byte 배열인 이유는 authTicket이 암호화된 데이터이기 때문에 임의의 0~255 값이 섞인 순수 이진 데이터이기 때문
        uint _ticketSize; // 스팀이 실제로 몇 바이트를 사용했는지 알기위한 변수

        protected override async Task Check()
        {
            SteamNetworkingIdentity identity = new SteamNetworkingIdentity(); // 이 인증 티켓을 누구한테 보여줄 건지 묻는 구조체
            identity.Clear(); // 이 티켓은 특정 클라이언트나 피어에게 제시하는 것이 아니고 중앙 서버 또는 스팀 서버 검증용이다 라고 설정

            HAuthTicket authTicket = SteamUser.GetAuthSessionTicket(
                _ticketBuffer, // _ticketBuffer 변수를 사용해라
                _ticketBuffer.Length, // 티켓 저장 가능 공간은 _ticketBuffer 배열의 길이 만큼 있다
                out _ticketSize, // 공간을 얼마나 사용했는지 _ticketSize 변수에 저장해라
                ref identity
            );

            byte[] authTicketBytes = new byte[_ticketSize]; // 실제 인증 티켓 길이의 byte 배열 생성
            Array.Copy(_ticketBuffer, authTicketBytes, _ticketSize); // _ticketBuffer배열의 앞부터 _ticketSize 길이 만큼 authTicketBytes 배열에 복사 - 이걸 하는 이유는 뒷부분은 쓸모없는 값들이 있기 때문 그래서 그대로 사용 시 문제 발생 가능

            string authTicketBase64 = Convert.ToBase64String(authTicketBytes); // 이진 데이터를 네트워크로 안전하게 보낼 수 있는 문자열 형태로 변경 - base64는 이진 데이터를 문자로 바꿔주는 포장 규칙(100% 원본 복구 가능, 문자의 집합임, 텍스트 안전 - JSON, URL, HTTP 사용가능) - Convert는 형변환 전문 클래스 - 정리: 스팀에서 받은 티켓은 이진 데이터이기 때문에 그대로 네트워크 전송 불가, Convert(형 변환 전문 클래스)를 사용하여 Base64 문자열 형태로 변환하여 json, url, http 환경에서도 손실없이 서버로 전달할 수 있도록 함

            SteamAuthInfo authInfo = new SteamAuthInfo()
            {
                ticket = authTicketBase64,
                appID = 480
            };

            NetworkManager.Instance.Socket.Emit("steamAuth", authInfo);
            await Task.CompletedTask;
        }
    }
}
// 마지막 작성 일자: 2025.12.31