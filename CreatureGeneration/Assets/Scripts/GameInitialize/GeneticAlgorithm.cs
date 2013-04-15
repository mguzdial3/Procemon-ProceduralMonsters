using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    /* Applies the genetic algorithm for optimizing the passed in problem */
    class GeneticAlgorithm
    {
        /* The problem that is consumed and we are finding a solution for */
        private GAProblem problem;

        /* Represents the current population */
        private List<Chromosome> population;

        /* Random generator */
        private Random rand;

        /* Instantiates the algorithm with the problem we are trying to solve Probabilities should be in the range of 0 - 1*/
        public GeneticAlgorithm(GAProblem problem)
        {
            this.problem = problem;
            this.rand = new Random();
            this.population = new List<Chromosome>();
        }

        /* Finds the solution to the problem using the genetic algorithm
         * @maxIter The maximum number of iterations to run the algorithm
         * @desitedFitness A terminatin condition that expresses if a calculated solution is close enough to the desired solution
         * @elitism If true then the fittest member of the last population is kept. Otherwise last population is discarded
         * @crossoverProbability How often a crossover is performed. For example, if 0 then the children are a copy of one of the parents. 80% - 95% is generally good
         * @mutationProbability How often a feature will be mutatated. 0.5% - 1% is generally good
         * @populationSize The size of the population. Generally 20-30 is good
         */
        public Chromosome evaluateProblem(int maxIter, double desiredFitness, Boolean elitism, double crossoverProbability, double mutationProbability, int populationSize) 
        {
            //Generate the initial population
            for (int i = 0; i < populationSize; i++)
            {
                population.Add(problem.generatePopulationMember());
            }

            int j = 0;
            //Evolve the population
            while (desiredFitness > getFittestValue() && j < maxIter) //Currently, a higher fitness score is better
            {
                List<Chromosome> newPopulation = new List<Chromosome>();
                int i = 0;
                if (elitism) //If we want to keep the best member from the last population then add it to the new population
                {
                    newPopulation.Add(getFittestMember());
                    i++;
                }
                for (; i < populationSize; i++) //Create a new population
                {
                    Chromosome firstParent = select(null);
                    Chromosome secondParent = select(firstParent);
                    Chromosome toAdd = crossover(firstParent, secondParent, crossoverProbability);
                    toAdd = mutate(toAdd, mutationProbability);
                    newPopulation.Add(toAdd);
                }
                this.population = newPopulation; //Use the new population we just generated
                j++;
            }

            return getFittestMember();
        }


        /* Performs a crossover on 2 different chromosomes and returns the result */
        private Chromosome crossover(Chromosome left, Chromosome right, double crossoverProbability)
        {
            List<Feature> newFeatureList = new List<Feature>();
            foreach(Feature f in left.chromosome)
            {
                newFeatureList.Add(new Feature(f.label, f.max, f.min, f.value, f.changeValue));
            }
            Chromosome newChromosome = new Chromosome(newFeatureList);
            for (int i = 0; i < left.chromosome.Count; i++)
            {
                double performCrossover = rand.NextDouble();
                if (performCrossover < crossoverProbability) //Perform a crossover
                {
                    Feature rightFeature = right.chromosome.ElementAt(i);
                    newChromosome.chromosome.ElementAt(i).value = rightFeature.value;
                }
            }
            return newChromosome;
        }

        /* Add or subtract the specified value within the Feature from the features value with mutationProbability */
        private Chromosome mutate(Chromosome chrom, double mutationProbability)
        {
            foreach (Feature f in chrom.chromosome)
            {
                double randNum = rand.NextDouble();
                if (randNum <= mutationProbability)
                {
                    if (f.max == f.value)
                        f.value -= f.changeValue;
                    else if (f.min == f.value)
                        f.value += f.changeValue;
                    else
                    {
                        double addOrSubtract = rand.NextDouble();
                        if (addOrSubtract >= 0.5)
                            f.value -= f.changeValue;
                        else
                            f.value += f.changeValue;
                    }

                }
                    
            }
            return chrom;
        }

        /* Select one chromosome to use as a parent*/
        private Chromosome select(Chromosome toIgnore)
        {
            double totalFitness = population.Sum(s =>this.problem.fitness(s));
            if (toIgnore != null)
                totalFitness -= this.problem.fitness(toIgnore);
            double index = rand.NextDouble() * (totalFitness);
            double curSum = 0;
            foreach (Chromosome s in population)
            {
                if (toIgnore != null && s.Equals(toIgnore))
                {
                    //Console.WriteLine("Ignoring");
                    continue;
                }
                else
                {
                    curSum += this.problem.fitness(s);
                    if (curSum >= index)
                        return s;
                }
            }
            return null;
        }

        /* Get the member of the population that is the most fit */
        private Chromosome getFittestMember()
        {
            return this.population.Aggregate((cur, next) => this.problem.fitness(cur) > this.problem.fitness(next) ? cur : next);
        }

        /* Gets the fitness of the fittest population member */
        private double getFittestValue()
        {
            double res = this.population.Max(val => this.problem.fitness(val));
            Console.WriteLine("Fittest: " + res);
            return res;
        }

        /* Calculates the euclidean distance between 2 different chromosomes */
        public static double euclideanDistance(Chromosome first, Chromosome second)
        {
            double curSum = 0;
            for (int i = 0; i < first.chromosome.Count; i++)
            {
                curSum += Math.Pow((first.chromosome.ElementAt(i).value - second.chromosome.ElementAt(i).value), 2);
            }
            return Math.Sqrt(curSum);
        }

        /* Just the absolute value of the distance between all the features */
        public static double simpleDistance(Chromosome first, Chromosome second)
        {
            double curSum = 0;
            for (int i = 0; i < first.chromosome.Count; i++)
            {
                curSum += Math.Abs(first.chromosome.ElementAt(i).value - second.chromosome.ElementAt(i).value);
            }
            //Console.WriteLine("Distance: " + curSum);
            return curSum;
        }

    }
