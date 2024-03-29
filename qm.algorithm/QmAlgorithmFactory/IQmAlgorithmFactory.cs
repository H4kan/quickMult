﻿using qm.algorithm.QmAlgorithm;
using qm.utils.Models;
using System.Numerics;

namespace qm.algorithm.QmAlgorithmFactory
{
    public interface IQmAlgorithmFactory<T> where T : IBitwiseOperators<T, T, T>, INumber<T>, IConvertible
    {
        IQmAlgorithm<T> Create(MatrixAlgorithm matrixAlgorithm);
    }
}
