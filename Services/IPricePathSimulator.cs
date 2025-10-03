namespace FinanceApp.Services;

public interface IPricePathSimulator
{
    double[] Generate(double sigma, double mean, double initialPrice, int numDays, int? seed = null);

    List<double[]> GenerateMany(int simulations, double sigma, double mean,
                                double initialPrice, int numDays, int? baseSeed = null);
}
