namespace Day11
{
    internal class VietnamHwatu
    {
        class GameManager
        {
            private int[] card = new int[52];
            private int used_card_count = 0;
            private int money = 0;
            enum Mark { Spade, Clover, Diamond, Heart }; // 마크표
            enum Value { A = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, J, Q, K }; // 숫자 매칭표

            public void GameReady() // 게임 전 카드 셋팅 및 셔플 처리
            {
                CardSetting();
                CardShuffle();
            }
            public void CardSetting() // 카드 값 설정
            {
                for (int i = 0; i < 52; i++)
                {

                    card[i] = i;
                }
            }
            public void CardShuffle() // 카드 셔플 (Fisher-Yates Shuffle 알고리즘)
            {
                Random rand = new Random();
                for (int i = 51; i >= 0; i--)
                {
                    int j = rand.Next(0, i + 1); // 아직 미확정 구간인 0~i 중 무작위 하나

                    int temp = card[i];
                    card[i] = card[j];
                    card[j] = temp;
                }
            }

            int ParsePokerMark(int card_value) // 카드의 마크 값 처리
            {
                return card_value / 13;
            }
            int ParsePokerValue(int card_value) // 카드의 숫자 값 처리
            {
                return card_value % 13 + 1;
            }
            public void CardPrint(int card_value) // 카드 프린트
            {
                Mark mark = (Mark)ParsePokerMark(card_value);
                Value value = (Value)ParsePokerValue(card_value);

                switch (mark) // mark에서 얻어온 숫자에 따라 처리
                {
                    case Mark.Spade:
                        Console.Write("♠");
                        break;
                    case Mark.Clover:
                        Console.Write("♣");
                        break;
                    case Mark.Diamond:
                        Console.Write("◆");
                        break;
                    case Mark.Heart:
                        Console.Write("♥");
                        break;
                    default:
                        break;
                }

                switch (value) // A,J,Q,K에 대해서만 알파벳 나머지는 숫자로 출력
                {
                    case Value.A:
                        Console.Write("A\t");
                        break;
                    case Value.J:
                        Console.Write("J\t");
                        break;
                    case Value.Q:
                        Console.Write("Q\t");
                        break;
                    case Value.K:
                        Console.Write("K\t");
                        break;
                    default:
                        Console.Write($"{(int)value}\t");
                        break;
                }
            }

            public int MyTryParse(string input) // 정수값이 올바른지 파싱 처리
            {
                int myMoney;
                if (int.TryParse(input, out myMoney) == false || myMoney > money || myMoney < 1000)
                {
                    myMoney = -1;
                }
                return myMoney;
            }
            public void GamePlay()
            {
                while (true)
                {
                    string input;
                    int betMoney;

                    if (used_card_count + 3 > 52) // 카드 모두 소진
                    {
                        Console.WriteLine("다음 라운드에 사용할 카드가 부족하므로, 게임을 종료합니다.");
                        break;
                    }
                    if (money < 1000) // 소지금이 1000원 미만인 경우
                    {
                        Console.WriteLine("소지금이 1000 미만이므로, 게임을 종료합니다.");
                        break;
                    }

                    for (int i = used_card_count; i < used_card_count + 3; i++) // 사용한 현재 카드의 수를 인덱스로 하여 3장을 출력한다.
                    {
                        CardPrint(card[i]);
                    }
                    Console.WriteLine();

                    Console.WriteLine($"내가 가진 시드머니  : {money}");

                    //bool test = GameLogicCheck(used_card_count); // 카드 다쓸 때 확인하기 위해 넣어놓은 코드
                    //Console.WriteLine($"Cheat : {test}");
                    while (true)
                    {
                        Console.Write("베팅액(최소 베팅 금액 1000원)을 입력하시오! ");
                        input = Console.ReadLine();

                        betMoney = MyTryParse(input);
                        if (betMoney == -1)
                        {
                            Console.WriteLine("올바른 베팅액을 입력해 주시길 바랍니다.");
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (GameLogicCheck(used_card_count) == true) // 승리한 경우
                    {
                        money += betMoney;
                        Console.WriteLine($"{betMoney} 원을 획득했다");

                    }
                    else
                    {
                        money -= betMoney;
                        Console.WriteLine($"{betMoney} 원을 잃었다~");
                    }

                    used_card_count += 3;
                    Console.WriteLine($"현재 사용한 카드 수 : {used_card_count}");
                    Console.WriteLine();
                }
            }

            public bool GameLogicCheck(int used_card_count) // 카드 로직 체크
            {

                int A = ParsePokerValue(card[used_card_count]); // 첫번째 카드 값 변환
                int B = ParsePokerValue(card[used_card_count + 1]); // 두번째 카드 값 변환
                int C = ParsePokerValue(card[used_card_count + 2]); // 세번째 카드 값 변환

                bool Win = (A - C) * (B - C) < 0; // C가 A~B 사이 값인지 확인

                return Win;
            }
            public void SetMoney(int myMoney)
            {
                money = myMoney;
            }
        }
        static void Main(string[] args)
        {

            GameManager gm = new GameManager();
            gm.GameReady();
            gm.SetMoney(10000);
            gm.GamePlay();
        }
    }
}
