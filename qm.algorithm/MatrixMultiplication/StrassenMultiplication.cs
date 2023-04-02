using qm.utils;
using System.Numerics;

namespace qm.algorithm.MatrixMultiplication
{
    public class StrassenMultiplication<T> : IMatrixMultiplication<T> where T : IBitwiseOperators<T, T, T>, INumber<T>
    {
        private readonly NaiveMultiplication<T> _naiveMultiplication;

        public int SwitchToNaiveStep;

        // 512 is optimal value
        public StrassenMultiplication(NaiveMultiplication<T> naiveMultiplication, int switchToNaiveStep = 512)
        {
            _naiveMultiplication = naiveMultiplication;
            SwitchToNaiveStep = switchToNaiveStep;
        }

        public T[][] ConductSquareMultiplication(T[][] input)
        {
            // upsize to 2^n
            var operationalMatrix = Helpers.InitializeMatrix<T>(Helpers.CeilPower2(input.Length));
            Helpers.CopyMatrix(operationalMatrix, input);

            // do actual multiplication
            operationalMatrix = StrassenRecursive(operationalMatrix, operationalMatrix);

            // downsize
            var resultHandlingMatrix = Helpers.InitializeMatrix<T>(input.Length);
            Helpers.CopyMatrix(resultHandlingMatrix, operationalMatrix);

            return resultHandlingMatrix;
        }

        private T[][] StrassenRecursive(T[][] matrixA, T[][] matrixB)
        {
            if (matrixA.Length <= SwitchToNaiveStep)
            {
                return _naiveMultiplication.ConductMultiplication(matrixA, matrixB);
            }

            var A11 = StrassenMultiplication<T>.GetSubMatrixByIndex(matrixA, true, true);
            var A21 = StrassenMultiplication<T>.GetSubMatrixByIndex(matrixA, false, true);
            var A12 = StrassenMultiplication<T>.GetSubMatrixByIndex(matrixA, true, false);
            var A22 = StrassenMultiplication<T>.GetSubMatrixByIndex(matrixA, false, false);

            var B11 = StrassenMultiplication<T>.GetSubMatrixByIndex(matrixB, true, true);
            var B21 = StrassenMultiplication<T>.GetSubMatrixByIndex(matrixB, false, true);
            var B12 = StrassenMultiplication<T>.GetSubMatrixByIndex(matrixB, true, false);
            var B22 = StrassenMultiplication<T>.GetSubMatrixByIndex(matrixB, false, false);

            // some memory savings
            var P1 = StrassenRecursive(StrassenMultiplication<T>.AddMatrices(A11, A22), StrassenMultiplication<T>.AddMatrices(B11, B22));
            var P2 = StrassenRecursive(StrassenMultiplication<T>.AddMatrices(A21, A22), B11);
            var P5 = StrassenRecursive(StrassenMultiplication<T>.AddMatrices(A11, A12), B22);
            var P6 = StrassenRecursive(StrassenMultiplication<T>.SubMatricesInPlace(A21, A11), StrassenMultiplication<T>.AddMatrices(B11, B12));
            var P7 = StrassenRecursive(StrassenMultiplication<T>.SubMatricesInPlace(A12, A22), StrassenMultiplication<T>.AddMatrices(B21, B22));
            var P3 = StrassenRecursive(A11, StrassenMultiplication<T>.SubMatricesInPlace(B12, B22));
            var P4 = StrassenRecursive(A22, StrassenMultiplication<T>.SubMatricesInPlace(B21, B11));

            // we can save it to A since it is not relevant anymore (memory saving)
            // in place operations used (memory saving)
            StrassenMultiplication<T>.MergeIntoTarget(matrixA,
                 // P7 not used anymore
                 StrassenMultiplication<T>.SubMatricesInPlace(StrassenMultiplication<T>.AddMatricesInPlace(StrassenMultiplication<T>.AddMatricesInPlace(P7, P1), P4), P5),
                 // P5 not used anymore
                 StrassenMultiplication<T>.AddMatricesInPlace(P5, P3),
                 // P4 not used anymore
                 StrassenMultiplication<T>.AddMatricesInPlace(P4, P2),
                 StrassenMultiplication<T>.SubMatricesInPlace(StrassenMultiplication<T>.AddMatricesInPlace(StrassenMultiplication<T>.AddMatricesInPlace(P3, P6), P1), P2)
                 );

            return matrixA;
        }

        private static T[][] GetSubMatrixByIndex(T[][] matrixA, bool firstX, bool firstY)
        {
            var halfSize = matrixA.Length / 2;
            var result = Helpers.InitializeMatrix<T>(halfSize);

            int startX = firstX ? 0 : halfSize;
            int startY = firstY ? 0 : halfSize;


            for (int i = 0; i < halfSize; i++)
            {
                for (int j = 0; j < halfSize; j++)
                {
                    result[i][j] = matrixA[startX + i][startY + j];
                }
            }
            return result;
        }

        private static T[][] AddMatrices(T[][] matrixA, T[][] matrixB)
        {
            var result = Helpers.InitializeMatrix<T>(matrixA.Length);
            Helpers.CopyMatrix(result, matrixA);

            StrassenMultiplication<T>.AddMatricesInPlace(result, matrixB);

            return result;
        }

        private static T[][] AddMatricesInPlace(T[][] matrixA, T[][] matrixB)
        {
            for (int i = 0; i < matrixA.Length; i++)
            {
                for (int j = 0; j < matrixA.Length; j++)
                {
                    matrixA[i][j] += matrixB[i][j];
                }
            }
            return matrixA;
        }

        private static T[][] SubMatricesInPlace(T[][] matrixA, T[][] matrixB)
        {
            for (int i = 0; i < matrixA.Length; i++)
            {
                for (int j = 0; j < matrixA.Length; j++)
                {
                    matrixA[i][j] -= matrixB[i][j];
                }
            }

            return matrixA;
        }

        private static void MergeIntoTarget(T[][] target, T[][] A11, T[][] A12, T[][] A21, T[][] A22)
        {
            var offset = target.Length / 2;
            for (int i = 0; i < offset; i++)
            {
                for (int j = 0; j < offset; j++)
                {
                    target[i][j] = A11[i][j];
                    target[i + offset][j] = A21[i][j];
                    target[i][j + offset] = A12[i][j];
                    target[i + offset][j + offset] = A22[i][j];
                }
            }
        }
    }
}
