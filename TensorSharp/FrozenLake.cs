using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TensorSharp
{
    class FrozenLake
    {
        private const int PLAIN = 0, TRAP = 1, GOLD = 2, CHARACTER = 3;
        private const int SIZE = 5, WAIT = 500;
        private Agent agent;
        private GridEnvironment environment;
        private bool quit;

        private int[,] map;

        public FrozenLake(Agent agent)
        {
            Console.Title = "FrozenLake";
            environment = new GridEnvironment();
            this.agent = agent;
            quit = false;

            //map initializing
            map = new int[SIZE, SIZE];
            Array.Clear(map, PLAIN, map.Length);
            map[2, 2] = GOLD;
            map[2, 1] = map[1, 2] = TRAP;
            map[0, 0] = CHARACTER;
        }

        public void GameLoop()
        {
            while (!quit)
            {
                PrintLake();
                PrintMenu();
                System.Threading.Thread.Sleep(WAIT);
            }
        }
        
        private void Reset()
        {
            map[2, 2] = GOLD;
            map[2, 1] = map[1, 2] = TRAP;
            map[0, 0] = CHARACTER;
            agent.Reset();
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
            Console.ReadLine();
        }
    }
}
