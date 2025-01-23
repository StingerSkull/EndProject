using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Randomizations;
using System;

namespace Platformars
{
    public class Rhythm : ChromosomeBase
    {
		#region fields
		private static int _rhythmLength;
		#endregion

		#region constructor
		public Rhythm(int pRhythmLength) : base(pRhythmLength)
        {
            _rhythmLength = pRhythmLength;
            CreateGenes();
        }
		#endregion

		#region ChromosomeBase
		public override Gene GenerateGene(int geneIndex)
        {
            return new Gene(RandomizationProvider.Current.GetInt(0, 3));
        }

        public override IChromosome CreateNew()
        {
            return new Rhythm(_rhythmLength);
        }
		#endregion
	}
}
