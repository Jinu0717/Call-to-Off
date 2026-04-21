using System.Collections.Generic;

public static class ARSDataFactory
{
    public static List<ARSNodeData> CreateAllNodes()
    {
        return new List<ARSNodeData>()
        {
            new ARSNodeData
            {
                nodeId = 0,
                nodeName = "시작",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "안녕하세요. 스마일 홈 통합 고객지원 ARS입니다. 전화 주셔서 감사합니다.\n휴대폰 번호 {PHONE_FULL} 고객님,\n요금 문의는 1번, 설치 및 이전 문의는 2번, 가전 원격 제어 및 스마트홈 서비스는 3번, 기타문의는 4번을 눌러주세요.",
                choices = new List<ARSChoice>
                {
                    new ARSChoice { inputKey = "1", choiceText = "요금 문의", nextNodeId = 1 },
                    new ARSChoice { inputKey = "2", choiceText = "설치 및 이전 문의", nextNodeId = 2 },
                    new ARSChoice { inputKey = "3", choiceText = "가전 원격 제어 및 스마트홈 서비스", nextNodeId = 3 },
                    new ARSChoice { inputKey = "4", choiceText = "기타 문의", nextNodeId = 4 },
                }
            },

            new ARSNodeData
            {
                nodeId = 1,
                nodeName = "요금 문의",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "고객님의 쾌적한 생활을 응원합니다.\n당월 전기요금 예상 조회는 1번, 누진세 구간 안내는 2번, 여름철 냉방기기 절전 팁은 3번을 눌러주세요.",
                choices = new List<ARSChoice>
                {
                    new ARSChoice { inputKey = "1", choiceText = "당월 전기요금 예상 조회", nextNodeId = 11 },
                    new ARSChoice { inputKey = "2", choiceText = "누진세 구간 안내", nextNodeId = 12 },
                    new ARSChoice { inputKey = "3", choiceText = "절전 팁", nextNodeId = 13 },
                }
            },

            new ARSNodeData
            {
                nodeId = 11,
                nodeName = "당월 전기요금 예상 조회",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "현재 고객님의 사용 패턴상 ‘가볍게 불안한 수준’, ‘친구에게 하소연할 수 있는 수준’, ‘가족 단톡방에 사과문을 올려야 하는 수준’ 중 세 번째 구간에 진입할 가능성이 있습니다."
            },

            new ARSNodeData
            {
                nodeId = 12,
                nodeName = "누진세 구간 안내",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "전기를 많이 사용할수록 더 많은 요금을 부담하게 되는 제도입니다. 에어컨을 켜 둔 채 해외로 출국하시는 경우 매우 슬픈 결과가 발생할 수 있습니다."
            },

            new ARSNodeData
            {
                nodeId = 13,
                nodeName = "절전 팁",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "냉방기기를 외출 전에 반드시 꺼주세요. 꺼져 있는지 불안하다면 스마트홈 원격 제어 기능을 사용해 주세요. 단, 해당 기능은 사전 등록된 사용자만 이용 가능합니다."
            },

            new ARSNodeData
            {
                nodeId = 2,
                nodeName = "설치 문의 및 이전 문의",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "설치 일정 조회는 1번, 이전 설치는 2번, 실외기 위치 변경 문의는 3번을 눌러주세요.",
                choices = new List<ARSChoice>
                {
                    new ARSChoice { inputKey = "1", choiceText = "설치 일정 조회", nextNodeId = 21 },
                    new ARSChoice { inputKey = "2", choiceText = "이전 설치", nextNodeId = 22 },
                    new ARSChoice { inputKey = "3", choiceText = "실외기 위치 변경 문의", nextNodeId = 23 },
                }
            },

            new ARSNodeData
            {
                nodeId = 21,
                nodeName = "설치 일정 조회",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "이미 설치가 완료된 상태입니다. 다른 도움이 필요하신 경우 상위 메뉴를 이용해 주세요."
            },

            new ARSNodeData
            {
                nodeId = 22,
                nodeName = "이전 설치",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "이전 설치는 최소 3일 이상 소요됩니다. 현재 상황 해결에는 적합하지 않을 수 있습니다."
            },

            new ARSNodeData
            {
                nodeId = 23,
                nodeName = "실외기 위치 변경 문의",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "실외기 작업 전에는 전원을 반드시 꺼야 합니다. 먼저 전원을 끄신 후 다시 문의해 주세요."
            },

            new ARSNodeData
            {
                nodeId = 3,
                nodeName = "가전 원격 제어 및 스마트홈 서비스",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "스마트홈 앱 연결 문의는 1번, 해외 체류 중 원격 제어는 2번, 등록된 가전 목록 조회는 3번, 기기 코드 안내는 4번을 눌러주세요.",
                choices = new List<ARSChoice>
                {
                    new ARSChoice { inputKey = "1", choiceText = "스마트홈 앱 연결 문의", nextNodeId = 31 },
                    new ARSChoice { inputKey = "2", choiceText = "해외 체류 중 원격 제어", nextNodeId = 32 },
                    new ARSChoice { inputKey = "3", choiceText = "등록된 가전 목록 조회", nextNodeId = 33 },
                    new ARSChoice { inputKey = "4", choiceText = "기기 코드 안내", nextNodeId = 34 },
                }
            },

            new ARSNodeData
            {
                nodeId = 31,
                nodeName = "스마트홈 앱 연결 문의",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "앱 연결에 문제가 있으시다면 앱을 최신 버전으로 업데이트하신 뒤 다시 시도해 주십시오.\n상담원 연결을 원하신다면 1번을 눌러주세요.",
                choices = new List<ARSChoice>
                {
                    new ARSChoice { inputKey = "1", choiceText = "상담원 연결", nextNodeId = 311 },
                }
            },

            new ARSNodeData
            {
                nodeId = 311,
                nodeName = "상담원 연결",
                nodeType = ARSNodeType.Loop,
                dialogue = "상담원 연결을 도와드리겠습니다. 현재 대기 인원은 23명이며, 예상 대기 시간은 41분입니다."
            },

            new ARSNodeData
            {
                nodeId = 32,
                nodeName = "해외 체류 중 원격 제어",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "해외 체류 중 등록된 가전을 제어하시려면 본인 확인 절차가 필요합니다.\n계약자 본인 인증을 진행하시려면 1번, 가족 구성원 대리 인증을 진행하시려면 2번을 눌러주세요.",
                choices = new List<ARSChoice>
                {
                    new ARSChoice { inputKey = "1", choiceText = "계약자 본인 인증", nextNodeId = 321 },
                    new ARSChoice { inputKey = "2", choiceText = "가족 구성원 대리 인증", nextNodeId = 322 },
                }
            },

            new ARSNodeData
            {
                nodeId = 321,
                nodeName = "계약자 본인 인증",
                nodeType = ARSNodeType.NumberInput,
                dialogue = "휴대폰 번호 뒤 네 자리를 입력한 후 우물정자를 눌러주세요."
            },

            new ARSNodeData
            {
                nodeId = 3211,
                nodeName = "본인 인증 실패",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "입력하신 번호가 일치하지 않습니다. 계약자 본인 확인에 실패했습니다."
            },

            new ARSNodeData
            {
                nodeId = 3210,
                nodeName = "기기 코드 입력",
                nodeType = ARSNodeType.NumberInput,
                dialogue = "전원을 제어할 가전의 기기 코드 네 자리를 입력한 후 우물정자를 눌러주세요."
            },

            new ARSNodeData
            {
                nodeId = 321012,
                nodeName = "존재하지 않는 기기 코드",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "입력하신 기기 코드는 등록된 목록에 없습니다. 기기 코드 안내 메뉴에서 코드를 다시 확인해 주세요."
            },

            new ARSNodeData
            {
                nodeId = 321013,
                nodeName = "다른 가전의 기기 코드 입력",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "입력하신 기기 코드는 등록된 기기 코드이지만 거실 스탠드형 에어컨의 코드가 아닙니다. 다른 가전의 기기 코드가 입력된 것으로 보입니다."
            },

            new ARSNodeData
            {
                nodeId = 32107,
                nodeName = "거실 스탠드형 에어컨",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "거실 스탠드형 에어컨이 선택되었습니다.\n현재 상태를 확인하시려면 1번, 전원을 끄시려면 2번, 희망 온도를 조정하시려면 3번을 눌러주세요.",
                choices = new List<ARSChoice>
                {
                    new ARSChoice { inputKey = "1", choiceText = "현재 상태 확인", nextNodeId = 321071 },
                    new ARSChoice { inputKey = "2", choiceText = "전원 끄기", nextNodeId = 321072 },
                    new ARSChoice { inputKey = "3", choiceText = "희망 온도 조정", nextNodeId = 321073 },
                }
            },

            new ARSNodeData
            {
                nodeId = 321071,
                nodeName = "현재 상태 확인",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "현재 거실 스탠드형 에어컨은 냉방 강풍 모드로 작동 중이며 설정 온도는 18도입니다."
            },

            new ARSNodeData
            {
                nodeId = 321072,
                nodeName = "전원 끄기",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "보호자 없는 냉방 지속으로 인한 전기요금 상승이 우려됩니다. 정말 전원을 끄시겠습니까?\n확실하다면 1번을 눌러주세요.",
                choices = new List<ARSChoice>
                {
                    new ARSChoice { inputKey = "1", choiceText = "전원 차단", nextNodeId = 999 },
                }
            },

            new ARSNodeData
            {
                nodeId = 321073,
                nodeName = "희망 온도 조정",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "희망 온도 조정 기능은 현재 지원되지 않습니다."
            },

            new ARSNodeData
            {
                nodeId = 322,
                nodeName = "가족 구성원 대리 인증",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "가족 구성원 대리 인증은 현재 사용할 수 없습니다."
            },

            new ARSNodeData
            {
                nodeId = 33,
                nodeName = "등록된 가전 목록 조회",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "등록된 가전 목록 확인을 위해 거주지 유형을 선택해 주세요.\n아파트는 1번, 오피스텔은 2번, 단독주택은 3번을 눌러주세요.",
                choices = new List<ARSChoice>
                {
                    new ARSChoice { inputKey = "1", choiceText = "아파트", nextNodeId = 331 },
                    new ARSChoice { inputKey = "2", choiceText = "오피스텔", nextNodeId = 332 },
                    new ARSChoice { inputKey = "3", choiceText = "단독주택", nextNodeId = 333 },
                }
            },

            new ARSNodeData
            {
                nodeId = 331,
                nodeName = "아파트",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "등록된 기기 목록을 안내해 드립니다. 거실 조명은 {LIGHT_DEVICE_NO}번, 거실 스탠드형 에어컨은 {AIRCON_DEVICE_NO}번, 로봇 청소기는 {ROBOT_DEVICE_NO}번, 공기청정기는 {PURIFIER_DEVICE_NO}번입니다.\n기기 상세 코드 안내를 원하시면 9번을 눌러주세요.",
                choices = new List<ARSChoice>
                {
                    new ARSChoice { inputKey = "9", choiceText = "상세 코드 안내", nextNodeId = 3319 },
                }
            },

            new ARSNodeData
            {
                nodeId = 3319,
                nodeName = "상세 코드 안내",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "기기 상세 코드 안내입니다. 거실 조명은 {LIGHT_DETAIL_CODE}번, 거실 스탠드형 에어컨은 {AIRCON_DETAIL_CODE}번, 로봇 청소기는 {ROBOT_DETAIL_CODE}번, 공기청정기는 {PURIFIER_DETAIL_CODE}번입니다."
            },

            new ARSNodeData
            {
                nodeId = 332,
                nodeName = "오피스텔",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "현재 확인된 오피스텔 주소에는 에어컨 설치 및 등록 정보가 없습니다. 등록 가능한 기기는 제한되어 있습니다."
            },

            new ARSNodeData
            {
                nodeId = 333,
                nodeName = "단독주택",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "현재 확인된 단독주택 주소에는 에어컨 설치 및 등록 정보가 없습니다. 등록된 가전 정보가 존재하지 않습니다."
            },

            new ARSNodeData
            {
                nodeId = 34,
                nodeName = "기기 코드 안내 - 본인 확인",
                nodeType = ARSNodeType.NumberInput,
                dialogue = "기기 코드 안내를 위해 계약자 본인 확인을 진행합니다. 휴대폰 번호 뒤 네 자리를 입력한 후 우물정자를 눌러주세요."
            },

            new ARSNodeData
            {
                nodeId = 34011,
                nodeName = "기기 코드 안내 - 본인 확인 실패",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "입력하신 번호가 일치하지 않습니다. 기기 코드 안내를 위한 본인 확인에 실패했습니다."
            },

            new ARSNodeData
            {
                nodeId = 3401,
                nodeName = "기기 코드 안내 - 기기 번호 입력",
                nodeType = ARSNodeType.NumberInput,
                dialogue = "기기 코드를 조회할 가전의 기기 번호를 입력한 후 우물정자를 눌러주세요."
            },

            new ARSNodeData
            {
                nodeId = 34012,
                nodeName = "기기 코드 안내 - 존재하지 않는 기기 번호",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "입력하신 번호는 등록된 기기 번호 목록에 없습니다. 등록된 가전 목록 조회에서 기기 번호를 다시 확인해 주세요."
            },

            new ARSNodeData
            {
                nodeId = 3402,
                nodeName = "기기 코드 안내 - 상세 코드 입력",
                nodeType = ARSNodeType.NumberInput,
                dialogue = "선택하신 기기의 상세 코드를 입력한 후 우물정자를 눌러주세요."
            },

            new ARSNodeData
            {
                nodeId = 34028,
                nodeName = "기기 코드 안내 - 존재하지 않는 상세 코드",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "입력하신 상세 코드는 등록된 코드 목록에 없습니다. 기기 상세 코드 안내를 다시 확인해 주세요."
            },

            new ARSNodeData
            {
                nodeId = 34029,
                nodeName = "기기 코드 안내 - 다른 가전의 상세 코드",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "입력하신 상세 코드는 등록된 코드이지만 방금 선택하신 기기의 상세 코드와 일치하지 않습니다. 다른 가전의 상세 코드가 입력된 것으로 보입니다."
            },

            new ARSNodeData
            {
                nodeId = 34021,
                nodeName = "거실 조명 기기 코드 안내",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "거실 조명의 기기 코드는 {LIGHT_DEVICE_CODE}입니다."
            },

            new ARSNodeData
            {
                nodeId = 34022,
                nodeName = "거실 스탠드형 에어컨 기기 코드 안내",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "거실 스탠드형 에어컨의 기기 코드는 {AIRCON_DEVICE_CODE}입니다."
            },

            new ARSNodeData
            {
                nodeId = 34023,
                nodeName = "로봇 청소기 기기 코드 안내",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "로봇 청소기의 기기 코드는 {ROBOT_DEVICE_CODE}입니다."
            },

            new ARSNodeData
            {
                nodeId = 34024,
                nodeName = "공기청정기 기기 코드 안내",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "공기청정기의 기기 코드는 {PURIFIER_DEVICE_CODE}입니다."
            },

            new ARSNodeData
            {
                nodeId = 4,
                nodeName = "기타 문의",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "분실물 문의는 1번, 이벤트 응모는 2번, ARS 음성 속도 조절은 3번을 눌러주세요.",
                choices = new List<ARSChoice>
                {
                    new ARSChoice { inputKey = "1", choiceText = "분실물 문의", nextNodeId = 41 },
                    new ARSChoice { inputKey = "2", choiceText = "이벤트 응모", nextNodeId = 42 },
                    new ARSChoice { inputKey = "3", choiceText = "ARS 음성 속도 조절", nextNodeId = 43 },
                }
            },

            new ARSNodeData
            {
                nodeId = 41,
                nodeName = "분실물 문의",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "분실된 물품에 대해서는 당사에서 책임지지 않습니다. 수고하세요."
            },

            new ARSNodeData
            {
                nodeId = 42,
                nodeName = "이벤트 응모",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "참여해주셔서 감사합니다. 기기가 고효율로 전환됩니다."
            },

            new ARSNodeData
            {
                nodeId = 43,
                nodeName = "ARS 음성 속도 조절",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "현재 음성 속도는 매우 느림으로 설정되어 있습니다.\n더 느리게 들으시려면 1번, 지금 속도를 유지하시려면 2번을 눌러주세요.",
                choices = new List<ARSChoice>
                {
                    new ARSChoice { inputKey = "1", choiceText = "더 느리게 듣기", nextNodeId = 431 },
                    new ARSChoice { inputKey = "2", choiceText = "속도 유지", nextNodeId = 4 },
                }
            },

            new ARSNodeData
            {
                nodeId = 431,
                nodeName = "더 느리게 듣기",
                nodeType = ARSNodeType.NormalMenu,
                dialogue = "감사합니다. 이제부터 더 또렷하고 천천히, 고객님의 소중한 시간을 넉넉하게 사용하겠습니다."
            },

            new ARSNodeData
            {
                nodeId = 999,
                nodeName = "성공 엔딩",
                nodeType = ARSNodeType.EndingSuccess,
                dialogue = "전원 차단 명령이 정상적으로 전송되었습니다. 거실 스탠드형 에어컨의 작동이 종료되었습니다. 쾌적한 여행되시길 바랍니다. 상담을 종료합니다."
            },

            new ARSNodeData
            {
                nodeId = 1000,
                nodeName = "실패 엔딩",
                nodeType = ARSNodeType.EndingFail,
                dialogue = "전기요금이 임계치를 초과했습니다. 게임 오버입니다."
            }
        };
    }
}