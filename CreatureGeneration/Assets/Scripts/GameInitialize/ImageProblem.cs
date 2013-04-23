using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    class ImageProblem : GAProblem
    {
        private List<SerializableCreature> creatureList;

        private Random random;

        private List<SerializableCreature> likedCreatures;

        private Chromosome desired;

        public ImageProblem(List<SerializableCreature> creatureList)
        {
            this.creatureList = creatureList;
            this.random = new Random();
            this.likedCreatures = determineCreaturesLike(this.creatureList); //Find the creatures the player liked from the last iteration
            this.desired = desiredCreature(this.likedCreatures);

        }

        public double fitness(Chromosome member)
        {
            double totalFitness = 1000;
			double curDistance = 10;
		 	if (this.desired != null) {
            	curDistance = creatureDistance(this.desired, member);
            	totalFitness -= curDistance;
			}

            if (curDistance <= 5) //Solution is too close
                totalFitness -= 50;
            return totalFitness;
        }

        /* Average the relevant values to find the desired creature */
        private Chromosome desiredCreature(List<SerializableCreature> liked)
        {
            if (liked != null && liked.Count > 0)
            {
                double bodySize = 0, headSize = 0, bodyType = 0, headType = 0, eyebrows = 0;

                foreach (SerializableCreature c in liked)
                {
                    bodySize += c.bodySize;
					headSize += c.headSize;
					bodyType += c.bodyType;
					headType += c.headType;
					eyebrows += c.hasEyebrows;
                }

                //Find the average for each relevant attribute
                bodySize /= liked.Count;
				headSize /= liked.Count;
				bodyType /= liked.Count;
				headType /= liked.Count;
				eyebrows /= liked.Count;

                //Create the desired chromosome
                Chromosome desired = new Chromosome(new List<Feature>());
                Feature bSize = new Feature("bodySize", 10, 0, bodySize, 1);
                desired.addFeature(bSize);
                Feature hSize = new Feature("headSize", 10, 0, headSize, 1);
                desired.addFeature(hSize);
                Feature bType = new Feature("bodyType", 10, 0, bodyType, 1);
                desired.addFeature(bType);
                Feature hType = new Feature("headType", 10, 0, headType, 1);
                desired.addFeature(hType);
                Feature eye = new Feature("eyebrows", 10, 0, eyebrows, 1);
                desired.addFeature(eye);
                return desired;
            }
            return null;
        }

        /* Get a list of procemon that the player likes. Likes a procemon if the score is positive */
        public List<SerializableCreature> determineCreaturesLike(List<SerializableCreature> creatureList)
        {
            List<SerializableCreature> result = new List<SerializableCreature>();
            if (creatureList != null)
            {
                foreach (SerializableCreature c in creatureList)
                {
                    if (c.score > 0.0)
                    {
                        result.Add(c);
                    }
                }
            }
            return result;
        }

        /* Calculates a simple distance from two chromosomes where no features are weighted differently */
        private double creatureDistance(Chromosome one, Chromosome two)
        {
            double distance = 0;
            for (int i = 0; i < one.chromosome.Count; i++)
            {
                Feature leftFeature = one.chromosome.ElementAt(i);
                Feature rightFeature = two.chromosome.ElementAt(i);
                distance += Math.Abs(leftFeature.value - rightFeature.value);

            }
            return distance;
        }

        public Chromosome generatePopulationMember()
        {
            Chromosome newPopulationMember = new Chromosome(new List<Feature>());
			Feature bodySize = new Feature("bodySize", 2, 0, (double) this.random.Next(0, 3), 1);
            newPopulationMember.addFeature(bodySize);
            Feature headSize = new Feature("headSize", 5, 3, (double)this.random.Next(3, 6), 1);
            newPopulationMember.addFeature(headSize);
            Feature bodyType = new Feature("bodyType", 16, 13, (double)this.random.Next(13, 17), 1);
            newPopulationMember.addFeature(bodyType);
            Feature headType = new Feature("headType", 19, 17, (double)this.random.Next(17, 20), 1);
            newPopulationMember.addFeature(headType);
            Feature hasEyebrows = new Feature("eyebrows", 0, 1, (double)this.random.Next (0, 2), 1);
			newPopulationMember.addFeature(hasEyebrows);
            return newPopulationMember;
        }
    }

