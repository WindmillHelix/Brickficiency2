using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brickficiency.Classes {
    /**
     * This class generates all unordered k-subsets of n. Note that this class returns numbers between 0 and n-1 rather than
     * 1 through n. This class is very bad in terms of OOP, but in terms of efficiency it is pretty good. 
     * 
     * OldKSubsetGenerator has increased functionality but is slower.
     * 
     * @author Aaron Green, May 22, 2015
     */
    public class KSubsetGenerator {
        protected int n;
        protected int k;
        protected int[] subset;
        protected int[] maxVals;
        protected int[] startingSubset;
        protected int[] endingSubset;

        public KSubsetGenerator(int n, int k) {
            this.n = n;
            this.k = k;
            subset = new int[k];
            startingSubset = new int[k];
            endingSubset = new int[k];
            for (int i = 0; i < k; i++) {
                startingSubset[i] = i;
                endingSubset[i] = n - k + i;
            }
            maxVals = new int[k];
            reset();
        }

        public KSubsetGenerator(int n, int[] startingSubset, int[] endingSubset) {
            this.n = n;
            this.k = startingSubset.Length;
            this.startingSubset = startingSubset;
            this.endingSubset = endingSubset;
            reset();
        }
        public void reset() {
            subset = new int[k];
            maxVals = new int[k];
            for (int i = 0; i < k; i++) {
                subset[i] = startingSubset[i];
                maxVals[i] = n - k + i;
            }
            subset[k - 1]--;
        }
        public Boolean hasNext() {
            for (int i = 0; i < k; i++) {
                if (subset[i] != endingSubset[i]) {
                    return true;
                }
            }
            return false;
        }

        /**
         * This generates the next subset. If hasNext() == false, this method is unpredictable.
         * This does not return a copy of the subset. It returns the actual subset.
         * If this gets edited from any outside source, it will screw up the generator.
         * 
         * @return The next subset.
         */
        public int[] next() {
            for (int i = k - 1; i >= 0; i--) {
                if (subset[i] < maxVals[i]) {
                    subset[i]++;
                    for (int j = i + 1; j < k; j++) {
                        subset[j] = subset[j - 1] + 1;
                    }
                    return subset;
                }
            }
            return subset;
        }
    }
}
