namespace qm.generator
{
    public interface IMatrixGenerator
    {
        byte[][] GenerateRandomResultMatrix(int n);
        byte[][] GenerateLoserResultMatrix(int n, float loserPerc = 80);
        byte[][] GeneratePowerMatrix(int n, int playerRange = 1000);
        byte[][] GenerateAutoPowerMatrix(int n);
        byte[][] GenerateDominationMatrix(int n);
    }
}
