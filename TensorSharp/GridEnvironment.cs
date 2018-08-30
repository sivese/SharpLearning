using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TensorSharp
{
    class GridEnvironment
    {
        //Grid information
        public static readonly int GOLD = 1, TRAP = -1, PLAIN = 0;
        public static readonly int gridSize = 5;

        public List<KeyValuePair<int, int>> allState{ get; }
        public List<int> PossibleAtions { get; }

        private static readonly int X = 0, Y = 1;
        private static int[,] move;
        private int[,] grid;

        public GridEnvironment()
        {
            //grid array initializing
            grid = new int[gridSize, gridSize];
            Array.Clear(grid, PLAIN, grid.Length);

            //Set trap, gold position;
            grid[1, 2] = grid[2, 1] = TRAP;
            grid[2, 2] = GOLD;

            //Move direction define *UP, DOWN, LEFT, RIGHT
            move = new int[4, 2]{ { -1, 0}, {1, 0}, { 0, -1}, { 0, 1} };

            allState = new List<KeyValuePair<int, int>>();

            for (int i = 0; i < 5; i++)
                for (int k = 0; k < 5; k++) {
                    var temp = new KeyValuePair<int, int>(i, k);
                    allState.Add(temp);
                }

            PossibleAtions = new List<int>();

            for(int i=0;i<4;i++)
                PossibleAtions.Add(i);
        }

        private void Set2DArray<T>(T[][] arr, int size)
        {
            arr = new T[size][];

            for (int i = 0; i < size; i++)
                arr[i] = new T[size];
        }

        public KeyValuePair<int, int> StateAfterAction(KeyValuePair<int, int> state, int actionIdx)
        {
            var moveX = move[actionIdx, X];
            var moveY = move[actionIdx, Y];

            var x = state.Key;
            var y = state.Value;

            if (x + moveX > -1 && x + moveX < 5)
                x += moveX;

            if (y + moveY > -1 && y + moveY< 5)
                y += moveY;

            return new KeyValuePair<int, int>(x, y);
        }

        public int GetReward(KeyValuePair<int, int> state, int action)
        {
            var nextState = StateAfterAction(state, action);
            return grid[nextState.Key, nextState.Value];
        }
    }
}
