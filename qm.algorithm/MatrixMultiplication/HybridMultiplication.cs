using qm.utils;
using System.Numerics;

namespace qm.algorithm.MatrixMultiplication
{
    public class HybridMultiplication<T> : IMatrixMultiplication<T> where T : IBitwiseOperators<T, T, T>, INumber<T>
    {
        private readonly NaiveMultiplication<T> _naiveMultiplication;
        private readonly StrassenMultiplication<T> _strassenMultiplication;

        public int SwitchToNaiveStep;


        // 512 is optimal value
        public HybridMultiplication(NaiveMultiplication<T> naiveMultiplication,
            StrassenMultiplication<T> strassenMultiplication, int switchToNaiveStep = 512)
        {
            SwitchToNaiveStep = switchToNaiveStep;
            _naiveMultiplication = naiveMultiplication;
            _strassenMultiplication = strassenMultiplication;
        }

        public T[][] ConductSquareMultiplication(T[][] input)
        {
            if (HybridMultiplication<T>.IsStrassenFaster(input.Length))
            {
                return _strassenMultiplication.ConductSquareMultiplication(input);
            }
            else
            {
                return _naiveMultiplication.ConductSquareMultiplication(input);
            }
        }

        private static bool IsStrassenFaster(int n)
        {
            // get k
            int k = Helpers.CeilLog2(n);

            // calculate r(k)
            double r = Math.Cbrt((Math.Pow(7, k + 1) - 5 * Math.Pow(4, k)) / 2);

            return n > r;
        }
    }
}
