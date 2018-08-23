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
        public static readonly int GOLD = 1;
        public static readonly int TRAP = -1;
        public static readonly int PLAIN = 0;
        public static readonly int gridSize = 5;

        private int[,] grid;

        public GridEnvironment()
        {
            //grid array initializing
            grid = new int[gridSize, gridSize];
            Array.Clear(grid, PLAIN, grid.Length);

            //Set trap, gold position;
            grid[1, 2] = grid[2, 1] = -1;
            grid[2, 2] = 1;
            
        }
        
        public List<float> GetAllState()
        {
            List<float> stateList = new List<float>();

            return stateList;
        }

        private void Set2DArray<T>(T[][] arr, int size)
        {
            arr = new T[size][];

            for (int i = 0; i < size; i++)
                arr[i] = new T[size];
        }
    }
}
