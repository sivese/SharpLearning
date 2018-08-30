using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TensorSharp
{
    class FrozenLake
    {
        enum MENU
        {
            EVALUATE = 1,
            IMPROVE = 2,
            MOVE = 3,
            RESET = 4,
            QUIT = 5
        };

        private MENU selectMenu;
        private const int PLAIN = 0, TRAP = 1, GOLD = 2, CHARACTER = 3;
        private const int SIZE = 5, WAIT = 200;

        private Agent agent;
        private GridEnvironment environment;
        private Thread inputThread;

        private bool moving;
        private bool quit;
        private int improvementCount;
        private int evaluateCount;

        private int[,] map;

        public FrozenLake(Agent agent)
        {
            Console.Title = "FrozenLake";
            environment = new GridEnvironment();
            this.agent = agent;
            quit = false;
            moving = false;

            //map initializing
            map = new int[SIZE, SIZE];
            Array.Clear(map, PLAIN, map.Length);
            map[2, 2] = GOLD;
            map[2, 1] = map[1, 2] = TRAP;
            map[0, 0] = CHARACTER;

            improvementCount = 0;
            evaluateCount = 0;
        }

        public void GameLoop()
        {
            while (!quit)
            {
                Console.Clear();

                PrintLake();

                if (!moving)
                {
                    if (inputThread != null && inputThread.IsAlive)
                        inputThread.Abort();

                    PrintMenu();
                    var select = (MENU)Console.Read() - '0';
                    SelectMenu(select);
                }
                else
                {
                    Console.SetCursorPosition(0, SIZE + 2);
                    Console.WriteLine("Press M to select Menu");

                    MoveByPolicy();

                    System.Threading.Thread.Sleep(WAIT);
                }
            }
        }
        
        private void SelectMenu(MENU select)
        {
            switch (select)
            {
                case MENU.EVALUATE:
                    EvaluatePolicy();
                    break;
                case MENU.IMPROVE:
                    ImprovePolicy();
                    break;
                case MENU.MOVE:
                    moving = true;
                    CreateInputThread();
                    inputThread.Start();
                    break;
                case MENU.QUIT:
                    quit = true;
                    break;
                case MENU.RESET:
                    break;
            }
        }

        private void Reset()
        {
            map[2, 2] = GOLD;
            map[2, 1] = map[1, 2] = TRAP;
            map[0, 0] = CHARACTER;
            agent.Reset();
        }

        private void CreateInputThread()
        {
            inputThread = new Thread(new ThreadStart(() =>
            {
                waiting:

                var readVal = Console.ReadKey(true).KeyChar;

                if (readVal == 'M' || readVal == 'm')
                {
                    moving = false;

                    for (int i = 0; i < SIZE; i++)
                        for (int k = 0; k < SIZE; k++)
                        {
                            if (map[i, k] == CHARACTER)
                            {
                                map[i, k] = PLAIN;
                                map[0, 0] = CHARACTER;
                                map[2, 2] = GOLD;
                            }
                        }
                }
                else
                    goto waiting;
            }));
        }

        private void Move(int action)
        {
            var x = 0;
            var y = 0;

            var posX = 0;
            var posY = 0;

            for(int i=0;i<SIZE;i++)
                for(int k = 0; k < SIZE; k++)
                {
                    if(map[i, k] == CHARACTER)
                    {
                        posX = i;
                        posY = k;
                    }
                }

            if (action == 0 && posX > 0)
                x = -1;
            else if (action == 1 && posX < 4)
                x = 1;
            else if (action == 2 && posY > 0)
                y = -1;
            else if (action == 3 && posY < 4)
                y = 1;

            map[posX, posY] = PLAIN;
            map[posX + x, posY + y] = CHARACTER;
        }

        private void ImprovePolicy()
        {
            improvementCount++;
            agent.PolicyImprovement();
        }

        private void EvaluatePolicy()
        {
            evaluateCount++;
            agent.PolicyEvaluation();
        }

        private void MoveByPolicy()
        {
            if (improvementCount != 0)
            {
                var posX = 0;
                var posY = 0;

                for (int i = 0; i < SIZE; i++)
                    for (int k = 0; k < SIZE; k++)
                    {
                        if (map[i, k] == CHARACTER)
                        {
                            posX = i;
                            posY = k;
                        }
                    }

                if(map[2, 2] != CHARACTER)
                {
                    var action = agent.GetAction(new KeyValuePair<int, int>(posX, posY));
                    Move(action);
                }
                
            }

        }

        private void PrintLake()
        {
            for(int i=0;i<SIZE;i++)
                for(int k = 0; k < SIZE; k++)
                {
                    Console.SetCursorPosition(i*2, k);
                    
                    switch(map[i, k])
                    {
                        case PLAIN:
                            Console.Write("□");
                            break;
                        case TRAP:
                            Console.Write("■");
                            break;
                        case GOLD:
                            Console.Write("★");
                            break;
                        case CHARACTER:
                            Console.Write("＆");
                            break;
                    }
                }
        }

        private void PrintMenu()
        {
            Console.SetCursorPosition(0, SIZE+2);
            Console.WriteLine("******Select menu******");
            Console.WriteLine("1.Evaluate");
            Console.WriteLine("2.Improve");
            Console.WriteLine("3.Move");
            Console.WriteLine("4.Reset");
            Console.WriteLine("5.QUIT");
        }
    }
}
