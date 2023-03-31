﻿using qm.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace qm.algorithm
{
    public class StrassenMultiplication<T> : IMatrixMultiplication<T> where T : IBitwiseOperators<T, T, T>
    {
        private NaiveMultiplication<T> naiveMultiplication = new NaiveMultiplication<T>();

        public int SwitchToNaiveStep = 4;

        public T[][] ConductSquareMultiplication(T[][] input)
        {
            // upsize to 2^n
            var operationalMatrix = Helpers.InitializeMatrix<T>(Helpers.CeilPower2(input.Length));
            Helpers.CopyMatrix<T>(operationalMatrix, input);

            // do actual multiplication
            operationalMatrix = StrassenRecursive(operationalMatrix, operationalMatrix);
            
            // downsize
            var resultHandlingMatrix = Helpers.InitializeMatrix<T>(input.Length);
            Helpers.CopyMatrix<T>(resultHandlingMatrix, operationalMatrix);

            return resultHandlingMatrix;
        }

        // result always in matrixA
        private T[][] StrassenRecursive(T[][] matrixA, T[][] matrixB)
        {
            if (matrixA.Length <= this.SwitchToNaiveStep)
            {
                return this.naiveMultiplication.ConductMultiplication(matrixA, matrixB);
            }


            var A11 = GetSubMatrixByIndex(matrixA, true, true);
            var A12 = GetSubMatrixByIndex(matrixA, false, true);
            var A21 = GetSubMatrixByIndex(matrixA, true, false);
            var A22 = GetSubMatrixByIndex(matrixA, false, false);

            var B11 = GetSubMatrixByIndex(matrixB, true, true);
            var B12 = GetSubMatrixByIndex(matrixB, false, true);
            var B21 = GetSubMatrixByIndex(matrixB, true, false);
            var B22 = GetSubMatrixByIndex(matrixB, false, false);

            var P1 = StrassenRecursive(AddMatrices(A11, A22), AddMatrices(B11, B22));
            var P2 = StrassenRecursive(AddMatrices(A21, A22), B11);
            var P3 = StrassenRecursive(A11, SubMatrices(B12, B22));
            var P4 = StrassenRecursive(A22, SubMatrices(B21, B11));
            var P5 = StrassenRecursive(AddMatrices(A11, A22), B22);
            var P6 = StrassenRecursive(SubMatrices(A21, A11), AddMatrices(B11, B12));
            var P7 = StrassenRecursive(SubMatrices(A12, A22), AddMatrices(B21, B22));

            // we can save it to A since it is not relevant anymore (memory saving)
            // in place operations used (memory saving)
            MergeIntoTarget(matrixA,
                // P7 not used anymore
                 SubMatricesInPlace(AddMatricesInPlace(AddMatricesInPlace(P7, P1), P4), P5),
                 // P5 not used anymore
                 AddMatricesInPlace(P5, P3),
                 // P4 not used anymore
                 AddMatricesInPlace(P4, P2),
                 SubMatricesInPlace(AddMatricesInPlace(P1, AddMatricesInPlace(P3, P6)), P2)
                 );

            return matrixA;
        }

        private T[][] GetSubMatrixByIndex(T[][] matrixA, bool firstX, bool firstY)
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

        private T[][] AddMatrices(T[][] matrixA, T[][] matrixB)
        {
            var result = Helpers.InitializeMatrix<T>(matrixA.Length);
            Helpers.CopyMatrix(result, matrixA);

            AddMatricesInPlace(result, matrixB);

            return result;
        }

        private T[][] SubMatrices(T[][] matrixA, T[][] matrixB)
        {
            var result = Helpers.InitializeMatrix<T>(matrixA.Length);
            Helpers.CopyMatrix(result, matrixA);

            SubMatricesInPlace(result, matrixB);

            return result;
        }

        private T[][] AddMatricesInPlace(T[][] matrixA, T[][] matrixB)
        {
            for (int i = 0; i < matrixA.Length; i++)
            {
                for (int j = 0; j < matrixA.Length; j++)
                {
                    matrixA[i][j] |= matrixB[i][j];
                }
            }
            return matrixA;
        }

        private T[][] SubMatricesInPlace(T[][] matrixA, T[][] matrixB)
        {
            for (int i = 0; i < matrixA.Length; i++)
            {
                for (int j = 0; j < matrixA.Length; j++)
                {
                    matrixA[i][j] ^= matrixB[i][j];
                }
            }

            return matrixA;
        }

        private void MergeIntoTarget(T[][] target, T[][] A11, T[][] A12, T[][] A21, T[][] A22)
        {
            var offset = target.Length / 2;
            for (int i = 0; i < offset; i++)
            {
                for (int j = 0; j < offset; j++)
                {
                    target[i][j] = A11[i][j];
                    target[i + offset][j] = A12[i][j];
                    target[i][j + offset] = A21[i][j];
                    target[i + offset][j + offset] = A22[i][j];
                }
            }
        }
    }
}
