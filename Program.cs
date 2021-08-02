using System;
using System.Threading;
using System.Threading.Tasks;

namespace CollatzConjecture
{
    class Program
    {
        static async Task Main(string[] args)
        {
            new CollatzConjecture(10);

            await Task.Delay(0);
        }
    }
}
