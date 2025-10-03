using System;

namespace FinanceApp.Services;

public sealed class BrownianSimulator : IPricePathSimulator
{
    public double[] Generate(double sigma, double mean, double initialPrice, int numDays, int? seed = null)
    {
        var rand = seed.HasValue ? new Random(seed.Value) : new Random();
        var prices = new double[numDays];
        prices[0] = initialPrice;

        for (int i = 1; i < numDays; i++)
        {
            double u1 = 1.0 - rand.NextDouble();
            double u2 = 1.0 - rand.NextDouble();
            double z = Math.Sqrt(-2.0 * Math.Log(u1)) * Math.Cos(2.0 * Math.PI * u2);

            double retornoDiario = mean + sigma * z;
            prices[i] = prices[i - 1] * Math.Exp(retornoDiario);
        }

        return prices;
    }

    public List<double[]> GenerateMany(int simulations, double sigma, double mean, double initialPrice, int numDays, int? baseSeed = null)
    {
        var list = new List<double[]>(simulations);
        for (int i = 0; i < simulations; i++)
        {
            int? seed = baseSeed.HasValue ? baseSeed.Value + i * 17 : (int?)null;
            list.Add(Generate(sigma, mean, initialPrice, numDays, seed));
        }
        return list;
    }
}
