using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    /* Represents the fields for one member of the population */
    class Chromosome
    {
        /* The list of features */
        public List<Feature> chromosome { get; set; }

        public Chromosome(List<Feature> featureSet)
        {
            this.chromosome = featureSet;
        }

        /* Add a feature to the feature set */
        public void addFeature(Feature feature)
        {
            this.chromosome.Add(feature);
        }
    }
