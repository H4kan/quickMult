using Microsoft.Extensions.DependencyInjection;
using qm.algorithm.MatrixMultiplication;
using qm.algorithm.QmAlgorithm;
using qm.utils.Models;
using System.Numerics;

namespace qm.algorithm.QmAlgorithmFactory
{
    public class QmAlgorithmFactory<T> : IQmAlgorithmFactory<T> where T : IBitwiseOperators<T, T, T>, INumber<T>, IConvertible
    {
        private readonly IServiceProvider _serviceProvider;

        public QmAlgorithmFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IQmAlgorithm<T> Create(MatrixAlgorithm matrixAlgorithm)
        {
            IMatrixMultiplication<T> multiplicationAlgorithm = matrixAlgorithm switch
            {
                MatrixAlgorithm.Naive => _serviceProvider.GetRequiredService<NaiveMultiplication<T>>(),
                MatrixAlgorithm.Strassen => _serviceProvider.GetRequiredService<StrassenMultiplication<T>>(),
                MatrixAlgorithm.Hybrid => _serviceProvider.GetRequiredService<HybridMultiplication<T>>(),
                _ => throw new ArgumentOutOfRangeException(nameof(matrixAlgorithm), matrixAlgorithm, "Invalid matrix algorithm."),
            };
            return new QmAlgorithm<T>(multiplicationAlgorithm);
        }
    }
}
