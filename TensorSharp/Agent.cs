using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TensorSharp
{
    class Agent
    {
        static readonly float discountFactor = 0.99f;

        private GridEnvironment environment;
        private float[,] valueTable;
        private float[,,] policyTable;

        public Agent(GridEnvironment environment)
        {
            this.environment = environment;

            var envSize = GridEnvironment.gridSize;

            valueTable = new float[envSize, envSize];
            Array.Clear(valueTable, 0, valueTable.Length);

            policyTable = new float[envSize, envSize, 4];
            
            for(int i = 0; i < envSize * envSize; i++)
                for(int k=0;k<4;k++)
                    policyTable[i/5, i%5, k] = 0.25f;
        }

        public void Reset()
        {
            var envSize = GridEnvironment.gridSize;

            Array.Clear(valueTable, 0, valueTable.Length);

            for (int i = 0; i < envSize * envSize; i++)
                for (int k = 0; k < 4; k++)
                    policyTable[i / 5, i % 5, k] = 0.25f;
        }

        /*
         Bellman expectation equation

        */
        public void PolicyEvaluation()
        {
            var envSize = GridEnvironment.gridSize;

            float[,] nextValue = new float[envSize, envSize];
            Array.Clear(nextValue, 0, nextValue.Length);

            var allState = environment.allState;

            foreach (var state in allState)
            {
                var value = 0.0f;
                var x = state.Key;
                var y = state.Value;

                if (x == 2 && y == 2)
                {
                    nextValue[x, y] = 0.0f;
                    continue;
                }

                var actions = environment.PossibleAtions;

                foreach (var action in actions)
                {
                    var nextState = environment.StateAfterAction(state, action);
                    var reward = environment.GetReward(state, action);
                    var tempValue = GetValue(nextState);
                    value += GetPolicy(state, action) * (reward + discountFactor * tempValue);
                }

                nextValue[state.Key, state.Value] = (float) Math.Round(value, 2);
            }

            valueTable = nextValue;
        }

        public void PolicyImprovement()
        {
            float[,,] nextPolicy = policyTable;

            foreach(var state in environment.allState)
            {
                var x = state.Key;
                var y = state.Value;

                if (x == 2 && y == 2)
                    continue;

                var value = -99999.0f;
                var maxIndex = new List<int>();
                var result = new float[4]{ 0.0f, 0.0f, 0.0f, 0.0f };

                foreach(var action in environment.PossibleAtions)
                {
                    var nextState = environment.StateAfterAction(state, action);
                    var reward = environment.GetReward(state, action);
                    var nextValue = GetValue(state);
                    var temp = reward + discountFactor * nextValue;

                    if (temp == value)
                        maxIndex.Add(action);
                    else if(temp > value)
                    {
                        value = temp;
                        maxIndex.Clear();
                        maxIndex.Add(action);
                    }
                }

                var probability = 1 / maxIndex.Count;

                foreach (var idx in maxIndex)
                    nextPolicy[x, y, idx] = probability;
                
            }
        }

        public int GetAction(KeyValuePair<int, int> state)
        {
            var x = state.Key;
            var y = state.Value;

            var rand = new Random();
            var randomPick = rand.Next(100) / 100;
            var policySum = 0.0f;

            for (int i = 0; i < 4; i++)
            {
                policySum += policyTable[x, y, i];

                if (randomPick < policySum)
                    return i;
            }

            return 0;
        }

        public float GetPolicy(KeyValuePair<int, int> state, int action)
        {
            var x = state.Key;
            var y = state.Value;

            if (x == 2 && y == 2)
                return 0.0f;

            return policyTable[x, y, action] ;
        }

        public float GetValue(KeyValuePair<int, int> state)
        {
            return (float) Math.Round(valueTable[state.Key, state.Value], 2);
        }
    }
}
