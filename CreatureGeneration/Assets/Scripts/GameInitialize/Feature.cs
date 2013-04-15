using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/* Represents a feature within a genetic algorithm problem. For example, could be attack, hp, etc. */
class Feature
{
	/* What this population member represents*/
    public string label { get; set; }

    /* The maximum value this feature can have */
    public double max { get; set; }

    /* The minimum value this feature can have */
    public double min { get; set; }

    /* The current value of this property */
    public double value { get; set; }

    public double changeValue { get; set; }

    /* Constructs a feature */
    public Feature(string label, double max, double min, double value, double changeValue)
    {
        this.label = label;
        this.max = max;
        this.min = min;
        this.value = value;
        this.changeValue = changeValue;
    }


}

