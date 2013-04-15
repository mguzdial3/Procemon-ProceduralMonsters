using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    /* Represents a genetic algorithm problem */
    interface GAProblem
    {
        /* Evaluates the fitness of a member of the population */
        double fitness(Chromosome member);

        /* Generates 1 member of the population for this Genetic Algorithm problem */
        Chromosome generatePopulationMember();
    }
