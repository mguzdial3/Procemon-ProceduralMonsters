using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    class StatsProblem : GAProblem
    {
        /* Random generator for stats */
        private Random random;

        /* The maximum value a normal stat can have */
        private const double MAX_STATS_VALUE = 10;

        /* The minimum value a normal stat can have */
        private const double MIN_STATS_VALUE = 1;

        /* What the total stats should sum to */
        private const int STAT_SUM = 10;

        /* The factor by which we penalize for the sum of the stats not being equal STAT_SUM*/
        private const int STAT_DISTANCE_PENALTY = 2;

        public const int DISTANCE = 100;

        private Chromosome testChromosome;

        /* The chromosome we want the creature to be liked */
        private Chromosome desired;

        /* The creatures passed in */
        private List<SerializableCreature> creatureList;

        /* Creatures we believe the player likes */
        private List<SerializableCreature> likedCreatures;

        public StatsProblem(List<SerializableCreature> creatureList)
        {
            this.creatureList = creatureList;
            this.random = new Random();
            this.testChromosome = createTestChromosome();
            this.likedCreatures = determineCreaturesLike(this.creatureList); //Find the creatures the player liked from the last iteration
            this.desired = desiredCreature(this.likedCreatures);
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

        //TODO Possibly weight based on how much they liked. Add more randomness. Vary types more
        public double fitness(Chromosome member) //TODO Determine fitness based on input received
        {
            double totalFitness = 1000; //Start fitness
            
            //Get the total stat points of the chromosome
            double statsPointSum = 0;
            int typeIndex = 0;
            int i = 0;
            foreach (Feature f in member.chromosome)
            {
                if (f.label.Equals("Type"))
                    typeIndex = i;
                if (!f.label.Equals("HP") && !f.label.Equals("Accuracy") && !f.label.Equals("Type"))
                {
                    statsPointSum += f.value;
                }
                i++;
            }

            //Penalize the creature for being close to 10 points
            totalFitness -= (Math.Abs(statsPointSum - STAT_SUM) * STAT_DISTANCE_PENALTY);
            
            statsPointSum += member.chromosome.ElementAt(typeIndex).value; //Factor in the type to the points scheme again

            //Decrease fitness for being far away from past procemon that the player liked
            double curDistance = creatureDistance(this.desired, member);
            totalFitness -= curDistance * STAT_DISTANCE_PENALTY;
            
            if (curDistance <= 3) //We are too close
                totalFitness -= 5;

            return totalFitness;
            //return DISTANCE - GeneticAlgorithm.simpleDistance(this.testChromosome, member); ;
        }

        /* Distance from two chromosomes */
        private double creatureDistance(Chromosome one, Chromosome two)
        {
            double distance = 0;
            for (int i = 0; i < one.chromosome.Count; i++)
            {
                Feature leftFeature = one.chromosome.ElementAt(i);
                Feature rightFeature = two.chromosome.ElementAt(i);
                //weight the relevant features
                if (leftFeature.label.Equals("Attack"))
                {
                    distance += Math.Abs(leftFeature.value - rightFeature.value);
                }
                else if (leftFeature.label.Equals("Speed"))
                {
                    distance += Math.Abs(leftFeature.value - rightFeature.value);
                }
                else if (leftFeature.label.Equals("Defense"))
                {
                    distance += Math.Abs(leftFeature.value - rightFeature.value);
                }
                else if (leftFeature.label.Equals("Special"))
                {
                    distance += Math.Abs(leftFeature.value - rightFeature.value);
                }
                else if (leftFeature.label.Equals("Type"))
                {
                    distance += Math.Abs(leftFeature.value - rightFeature.value);
                }

            }
            return distance;
        }

        /* Average the relevant values to find the desired creature */
        private Chromosome desiredCreature(List<SerializableCreature> liked)
        {
            if (liked != null && liked.Count > 0)
            {
                double attackAmt = 0, speedAmt = 0, defenseAmt = 0, specialAmt = 0, typeAmt = 0;

                foreach (SerializableCreature c in liked)
                {
                    attackAmt += c.attack;
                    speedAmt += c.speed;
                    defenseAmt += c.defense;
                    specialAmt += c.special;
                    typeAmt += c.type;
                }

                //Find the average for each relevant attribute
                attackAmt = attackAmt / liked.Count;
                speedAmt = speedAmt / liked.Count;
                defenseAmt = defenseAmt / liked.Count;
                specialAmt = specialAmt / liked.Count;
                typeAmt = typeAmt / liked.Count;

                //Create the desired chromosome
                Chromosome desired = new Chromosome(new List<Feature>());
                Feature attack = new Feature("Attack", MAX_STATS_VALUE, MIN_STATS_VALUE, attackAmt, 1);
                desired.addFeature(attack);
                Feature speed = new Feature("Speed", MAX_STATS_VALUE, MIN_STATS_VALUE, speedAmt, 1);
                desired.addFeature(speed);
                Feature defense = new Feature("Defense", MAX_STATS_VALUE, MIN_STATS_VALUE, defenseAmt, 1);
                desired.addFeature(defense);
                Feature special = new Feature("Special", MAX_STATS_VALUE, MIN_STATS_VALUE, specialAmt, 1);
                desired.addFeature(special);
                Feature accuracy = new Feature("Accuracy", MAX_STATS_VALUE, MIN_STATS_VALUE, 8, 0.1);
                desired.addFeature(accuracy);
                Feature hp = new Feature("HP", MAX_STATS_VALUE, MIN_STATS_VALUE, 55, 5);
                desired.addFeature(hp);
                Feature type = new Feature("Type", 4, 0, typeAmt, 1);
                desired.addFeature(type);
                return desired;
            }
            return null; 
        }

        /* Print the passed in chromosome to the console */
        public static void printChromosome(Chromosome c)
        {
            foreach (Feature f in c.chromosome)
            {
                Console.WriteLine(f.label + ": " + f.value);
            }
        }

        public Chromosome createTestChromosome()
        {
            Chromosome newPopulationMember = new Chromosome(new List<Feature>());
            Feature attack = new Feature("Attack", MAX_STATS_VALUE, MIN_STATS_VALUE, 7, 1);
            newPopulationMember.addFeature(attack);
            Feature speed = new Feature("Speed", MAX_STATS_VALUE, MIN_STATS_VALUE, 7, 1);
            newPopulationMember.addFeature(speed);
            Feature defense = new Feature("Defense", MAX_STATS_VALUE, MIN_STATS_VALUE, 7, 1);
            newPopulationMember.addFeature(defense);
            Feature special = new Feature("Special", MAX_STATS_VALUE, MIN_STATS_VALUE, 7, 1);
            newPopulationMember.addFeature(special);
            Feature accuracy = new Feature("Accuracy", MAX_STATS_VALUE, MIN_STATS_VALUE, 7, 0.1);
            newPopulationMember.addFeature(accuracy);
            Feature hp = new Feature("HP", MAX_STATS_VALUE, MIN_STATS_VALUE, 7, 5);
            newPopulationMember.addFeature(hp);
            Feature type = new Feature("Type", 5, 0, 4, 1);
            newPopulationMember.addFeature(type);
            return newPopulationMember;
        }

        public Chromosome generatePopulationMember()
        {
            Chromosome newPopulationMember = new Chromosome(new List<Feature>());
            Feature attack = new Feature("Attack", MAX_STATS_VALUE, MIN_STATS_VALUE, (double) this.random.Next((int) MIN_STATS_VALUE, (int)MAX_STATS_VALUE + 1), 1);
            newPopulationMember.addFeature(attack);
            Feature speed = new Feature("Speed", MAX_STATS_VALUE, MIN_STATS_VALUE, (double)this.random.Next((int)MIN_STATS_VALUE, (int)MAX_STATS_VALUE + 1), 1);
            newPopulationMember.addFeature(speed);
            Feature defense = new Feature("Defense", MAX_STATS_VALUE, MIN_STATS_VALUE, (double)this.random.Next((int)MIN_STATS_VALUE, (int)MAX_STATS_VALUE + 1), 1);
            newPopulationMember.addFeature(defense);
            Feature special = new Feature("Special", MAX_STATS_VALUE, MIN_STATS_VALUE, (double)this.random.Next((int)MIN_STATS_VALUE, (int)MAX_STATS_VALUE + 1), 1);
            newPopulationMember.addFeature(special);
            Feature accuracy = new Feature("Accuracy", 10, 8, (double)this.random.Next((int)8, (int)10 + 1), 1);
            newPopulationMember.addFeature(accuracy);
            double[] possHp = { 50, 55, 60, 65 };
            Feature hp = new Feature("HP", 65, 50, possHp[this.random.Next(0, possHp.Length)], 5);
            newPopulationMember.addFeature(hp);
            Feature type = new Feature("Type", 4, 0, (double)this.random.Next(0, 5), 1);
            newPopulationMember.addFeature(type);
            return newPopulationMember;
        }
    }
