using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CollatzConjecture
{
    public class CollatzConjecture
    {
        private Dictionary<StepThread, Thread> threads;
        private int number;
        private Thread mainThread;
        private FileStream test;
        
        public CollatzConjecture(int threadCount)
        {
            mainThread = Thread.CurrentThread;
            
            test = File.OpenWrite("output.txt");

            threads = new Dictionary<StepThread, Thread>();
            number = 0;

            for (int i = 0; i < threadCount; i++)
            {
                number++;
                StepThread stepThread = new StepThread(number, new StepCallback(ResultCallback));

                Thread thread = new Thread(new ThreadStart(stepThread.Start));
                thread.Start();
                threads[stepThread] = thread;
                thread.Name = $"Step Thread #{i}";
            }

        }

        public void ResultCallback(StepThread stepThread, long steps, long startingNumber)
        {
            Console.WriteLine($"{startingNumber}:{steps}");

            number++;

            stepThread.startingNumber = number;
            stepThread.number = number;
            stepThread.currentStep = 0;

            stepThread.Start();
        }
    }

    public delegate void StepCallback(StepThread StepThread, long steps, long startingNumber);

    public class StepThread
    {
        private const long maxSteps = Int64.MaxValue;
        public long currentStep;
        public long number;
        public long startingNumber;

        public StepCallback callback;

        public StepThread(long startingNumber, StepCallback callbackDelegate)
        {
            this.startingNumber = startingNumber;
            this.number = startingNumber;
            this.callback = callbackDelegate;
            currentStep = 0;
        }

        public void Start()
        {
            Task task = new Task(() => calculateSteps());
            task.Start();
            task.Wait();

            callback(this, currentStep, startingNumber);

            task.Dispose();
        }

        private void calculateSteps()
        {
            if (number <= 0 | currentStep == maxSteps)
                return;
            
            while (number != 1)
            {
                currentStep += 1;
                if (number % 2 == 0)
                    number = number / 2;
                else 
                    number = number * 3 + 1;
            }
        }
    }
}
