using qm.utils;
using System.Numerics;

namespace qm.algorithm
{
    public class HybridMultiplication<T> : IMatrixMultiplication<T> where T : IBitwiseOperators<T, T, T>, INumber<T>
    {
        private NaiveMultiplication<T> naiveMultiplication;
        private StrassenMultiplication<T> StrassenMultiplication;

        public int SwitchToNaiveStep;

        // 512 is optimal value
        public HybridMultiplication(int switchToNaiveStep = 512)
        {
            this.SwitchToNaiveStep = switchToNaiveStep;
            this.naiveMultiplication = new NaiveMultiplication<T>();
            this.StrassenMultiplication = new StrassenMultiplication<T>(switchToNaiveStep);
        }

        public T[][] ConductSquareMultiplication(T[][] input)
        {
            if (IsStrassenFaster(input.Length))
            {
                return this.StrassenMultiplication.ConductSquareMultiplication(input);
            }
            else
            {
                return this.naiveMultiplication.ConductSquareMultiplication(input);
            }
        }

        private bool IsStrassenFaster(int n)
        {
            // get k
            int k = Helpers.CeilLog2(n);

            // calculate r(k)
            double r = Math.Cbrt((Math.Pow(7, k + 1) - 5 * Math.Pow(4, k)) / 2);

            return n > r;
        }
    }
}
