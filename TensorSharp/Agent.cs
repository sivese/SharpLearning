using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TensorSharp
{
    class Agent
    {
        static readonly float dicountFactor = 0.99f;

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
                    policyTable[i/4, i%4, k] = 0.25f;
        }

        public void PolicyEvaluation()
        {
            var envSize = GridEnvironment.gridSize;

            float[,] nextValue = new float[envSize, envSize];
            Array.Clear(nextValue, 0, nextValue.Length);

            var allState = environment.GetAllState();

            foreach (var state in allState)
            {
                var value = 0.0;

                /*
                if(state==[2, 2])
                {
                    nextValue[state[0], state[1]] = 0.0f;
                    continue;
                }
                */
                
            }

        }

        public void PolicyImprovement()
        {

        }

        public int GetAction()
        {
            int idx = 0;

            return idx;
        }

        public float GetPolicy()
        {
            float state = 0.1f ;

            return state;
        }

        public float GetValue()
        {
            return valueTable[0, 0];
        }
    }
}
