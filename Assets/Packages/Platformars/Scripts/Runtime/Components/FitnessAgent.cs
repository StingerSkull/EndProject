using GeneticSharp.Domain.Chromosomes;
using System;
using GeneticSharp.Domain.Fitnesses;
using System.Collections;
using System.Collections.Generic;

namespace Platformars
{
    public class FitnessAgent : IFitness
    {
        #region properties
        double desiredDifficulty;
        #endregion

        #region constructor
        public FitnessAgent(double pDesiredDifficulty)
        {
            desiredDifficulty = pDesiredDifficulty;
        }
        #endregion

        #region methods
        public double CalculateDifficultyLevel(IChromosome chromosome)
        {
            Gene[] genes = chromosome.GetGenes();
            int length = genes.Length;

            double N = 0.0;
            double V = 0.0;
            double C = 0.0;
            double D = 0.5;

            double weightN = 15;
            double weightV = 10;
            double weightC = 20;
            double weightD = 5;
            double totalWeight = weightN + weightV + weightC + weightD;

            weightN = weightN / totalWeight;
            weightV = weightV / totalWeight;
            weightC = weightC / totalWeight;
            weightD = weightD / totalWeight;

            double sum = 0.0;
            double avg = 0.0;

            double[,] actionChangeDifficulty =  {
                                                    {0, 2, 4},
                                                    {0, 3, 6},
                                                    {0, 2, 4}
                                                };

            for (int i = 0; i < length; i++)
            {
                N = N + (int)genes[i].Value;
                sum = sum + (int)genes[i].Value;
            }

            avg = sum / length;
            for (int i = 0; i < length; i++)
            {
                V = V + Math.Pow(avg - Convert.ToDouble(genes[i].Value), 2);
            }

            for (int i = 1; i < length; i++)
            {
                C = C + actionChangeDifficulty[(int)genes[i - 1].Value, (int)genes[i].Value];
            }

            double result = weightN * N + weightV * V + weightC * C + weightD * D;
            return result;
        }
        #endregion

        #region IFitness
        public double Evaluate(IChromosome chromosome)
        {
            double difficultyLevel = CalculateDifficultyLevel(chromosome);
            return desiredDifficulty / (desiredDifficulty + Math.Abs(desiredDifficulty - difficultyLevel));
        }
        #endregion
    }

}
