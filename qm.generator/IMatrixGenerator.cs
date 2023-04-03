namespace qm.generator
{
    public interface IMatrixGenerator
    {
        byte[][] GenerateRandomResultMatrix(int n);
        byte[][] GenerateLoserResultMatrix(int n, float loserPerc = 80);

        public byte[][] GeneratePowerMatrix(int n, int playerRange = 1000);
    }
}
