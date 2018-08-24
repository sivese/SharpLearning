using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TensorFlow;

namespace TensorSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            var env = new GridEnvironment();
            var agent = new Agent(env);
            var game = new FrozenLake(agent);
            game.GameLoop();
        }
    }
}
