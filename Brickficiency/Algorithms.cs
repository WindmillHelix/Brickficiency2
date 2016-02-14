using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Brickficiency.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace Brickficiency {
    public partial class MainWindow : Form
    {
        private void CustomAlgorithm(int k, List<Store> storeList, List<Item> itemList) {
            AddStatus(Environment.NewLine + "Custom algorithm has not been implemented.  Nothing will be computed." + Environment.NewLine);
            // You should include the following line in the appopriate place(s) in your algorithm so that if
            // there is a request to stop searching you will do so.  Examples include at the beginning of a loop
            // that will perform a lot of calculations.
            //if (calcWorker.CancellationPending || stopAlgorithmEarly) { return; }

            // Include the following line in your code in an outer loop.
            // Progress();

            // Include code similar to this whenever you find a set of stores that are valid.
            // (A set of stores is valid if you can obtain enough of all of the items from those stores.)
            //List<string> storeNames = new List<string>();
            //// Put the store names on the list.
            //addFinalMatch(storeNames);
        }
        private void CustomApproximationAlgorithm(int k, List<Store> storeList, List<Item> itemList) {
            AddStatus(Environment.NewLine + "Custom approximation algorithm has not been implemented.  Nothing will be computed." + Environment.NewLine);
            // You should include the following line in the appopriate place(s) in your algorithm so that if
            // there is a request to stop searching you will do so.  Examples include at the beginning of a loop
            // that will perform a lot of calculations.
            //if (calcWorker.CancellationPending || stopAlgorithmEarly) { return; }

            // Include the following line in your code in an outer loop.
            // Progress();

            // Include code similar to this whenever you find a set of stores that are valid.
            // (A set of stores is valid if you can obtain enough of all of the items from those stores.)
            //List<string> storeNames = new List<string>();
            //// Put the store names on the list.
            //addFinalMatch(storeNames);
        }

        private void CustomPreProcess(ref List<Store> storeList, ref List<Item> itemList) {
            // Add any pre-processing of the data here.  Specifically, modify the two lists in 
            // whatever way you desire to improve the efficiency of your algorithm.
        }
        private void CustomApproximationPreProcess(ref List<Store> storeList, ref List<Item> itemList) {
            // Add any pre-processing of the data here.  Specifically, modify the two lists in 
            // whatever way you desire to improve the efficiency of your algorithm.
        }


        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        // DO NOT MAKE ANY CHANGES BELOW THIS POINT!
        // The code below is used by the "standard" algorithms.  You may use it for your reference, but you should
        // not modify any of it.
        //------------------------------------------------------------------------------------------------------------
        //------------------------------------------------------------------------------------------------------------
        public delegate void Algorithm(int k, List<Store> storeList, List<Item> itemList);
        public delegate void PreProcess(ref List<Store> storeList, ref List<Item> itemList);

        private void runTheAlgorithm(int minStores, int maxStores, bool continueWhenMatchesFound,
            List<Store> storeList, List<Item> itemList, Algorithm alg, PreProcess preProc = null) {
            // Pre-process the data
            if (preProc != null) {
                preProc(ref storeList, ref itemList);
            }

            // Now run the algorithms for the specified number of stores in the solution.
            for (int k = minStores; k <= maxStores; k++) {
                if ((!matchesfound || continueWhenMatchesFound) && !calcWorker.CancellationPending) {
                    beginCalculation(k, storeList.Count);
                    alg(k, storeList, itemList);
                    endCalculation(k);
                }
            }

        }

        private void runApproxAlgorithm(int minStores, int maxStores, int millisToRunEach, List<Store> storeList, List<Item> itemList, Algorithm alg, PreProcess preProc = null) {
            // Pre-process the data
            if (preProc != null) {
                preProc(ref storeList, ref itemList);
            }
            if (timeoutTimer != null) {
                timeoutTimer.Stop();
            } else {
                timeoutTimer = new System.Timers.Timer();
                timeoutTimer.Elapsed += timeoutTimer_Elapsed;
            }
            timeoutTimer.Interval = millisToRunEach;

            for (int k = minStores; k <= maxStores; k++) {
                if (!calcWorker.CancellationPending) {
                    beginCalculation(k, storeList.Count);
                    timeoutTimer.Start();
                    alg(k, storeList, itemList);
                    timeoutTimer.Stop();
                    endCalculation(k);
                }
            }

        }
        private void timeoutTimer_Elapsed(object sender, EventArgs e) {
            stopAlgorithmEarly = true;
            timeoutTimer.Stop();
        }
        #region Preprocessing stuff.
        public void StandardPreProcess(ref List<Store> storeList, ref List<Item> itemList) {
            // Sort the wanted list by availstores, ascending.  
            itemList = SortItemsByStoreAvailability(itemList);

            // Sort the stores in descending order by qty of the first 1-4 items on the sorted wanted list
            // depending on whether or not the number of items on the list is at least that large.
            storeList = SortStoresByNumberOfFirstSeveralItems(storeList, itemList);
        }

        private List<Item> SortItemsByStoreAvailability(List<Item> itemList) {
            return itemList.OrderBy(i => i.availstores).ToList();
        }
        // This sorts the stores so they are in decending order by the number of the first item on the wanted list.
        // If they have the same number, then they are ordered by the number of the second item, etc.
        private List<Store> SortStoresByNumberOfFirstSeveralItems(List<Store> storeList, List<Item> itemList) {
            StoreComparer sc = new StoreComparer(itemList);
            storeList.Sort(sc);
            return storeList;
        }
        #endregion

        #region New Algorithm
        private void KStoreCalc(int k, List<Store> storeList, List<Item> itemList) {
            if (k == 1) {
                OneStoreCalc(storeList, itemList);
            } else {
                // The stores are sorted by how many of the first item they have, and then by 2nd, 3rd, and 4th, etc.
                // Thus a valid solution must contain one of the first store in the list.  Once a store in the list has 0 of the first item
                // the rest will have 0 so it makes no sense to consider them as the first store since the rest of the store can't have that item
                // either, so no solution will ever be found.  This idea can be applied to the second store, etc., although it is more complicated.
                // So here we compute the last index of the first (up to) 5 items on the list. The last index of the 5th should be the final index.  
                // The last index of the rest might be smaller. I haven't figured out exactly how to use this information for items 2-5 yet since
                // it gets a little more complicated.
                // CAC, 2015-06-25
                int numToTrack = itemList.Count > 5 ? 5 : itemList.Count;
                int[] lastnonzeroindex = new int[numToTrack];
                for (int item = 0; item < numToTrack; item++) {
                    int j = storeList.Count - 1;
                    while (j >= 0 && storeList[j].getQty(itemList[item].extid) == 0) {
                        j--;
                    }
                    lastnonzeroindex[item] = j;
                }
                //Debug.WriteLine("LastNonZero: " + intArrayToString(lastnonzeroindex));

                // Need to add one to the second argument since it is exclusive.
                Parallel.For(0, lastnonzeroindex[0] + 1, store1 => {
                    if (calcWorker.CancellationPending || stopAlgorithmEarly) { return; }
                    // Do the next k stores have enough of the first element?  
                    // If not, none of the rest will so quit. CAC, 2015-06-25
                    int totalQtyFirst = 0;
                    int lastToCheck = Math.Min(storeList.Count - 1, store1 + k);
                    for (int i = store1; i < lastToCheck; i++) {
                        totalQtyFirst += storeList[i].getQty(itemList[0].extid);
                    }
                    if (totalQtyFirst < itemList[0].qty) { return; }

                    int[] start = new int[k];
                    int[] end = new int[k];
                    for (int i = 0; i < k; i++) {
                        start[i] = store1 + i;
                        end[i] = storeList.Count - k + i;
                        // The version below doesn't work.  We have to use a different method of omitting more
                        // possibilities based on lastnonzeroindex[i] for i>0.  Still need to think about this.
                        // I'm leaving it here commented out to remind me that I tried it and realized
                        // that it isn't correct.
                        // CAC, 2015-07-02.
                        //end[i] = (i < numToTrack) ? lastnonzeroindex[i] : storeList.Count - k + i;
                    }
                    end[0] = store1;
                    KSubsetGenerator subs = new KSubsetGenerator(storeList.Count, start, end);
                    while (subs.hasNext()) {
                        if (calcWorker.CancellationPending || stopAlgorithmEarly) { break; }

                        int[] current = subs.next();
                        Interlocked.Increment(ref longcount);
                        bool fail = false;
                        foreach (Item item in itemList) {
                            int totalQty = 0;
                            for (int i = 0; i < k; i++) {
                                totalQty += storeList[current[i]].getQty(item.extid);
                            }
                            if (totalQty < item.qty) {
                                fail = true;
                                break;
                            }
                        }
                        if (!fail) {
                            List<string> storeNames = new List<string>();
                            for (int j = 0; j < k; j++) {
                                storeNames.Add(storeList[current[j]].getName());
                            }
                            addFinalMatch(storeNames);
                        }
                    }
                    Progress();
                });
            }
        }
        #endregion

        #region Old Algorithm
        private void RunOldAlgorithmOn(int numStores, List<Store> storeList, List<Item> itemList) {
            switch (numStores) {
                case 1:
                    OneStoreCalc(storeList, itemList);
                    break;
                case 2:
                    TwoStoreCalc(storeList, itemList);
                    break;
                case 3:
                    ThreeStoreCalc(storeList, itemList);
                    break;
                case 4:
                    FourStoreCalc(storeList, itemList);
                    break;
                case 5:
                    FiveStoreCalc(storeList, itemList);
                    break;
            }
        }
        private void OneStoreCalc(List<Store> storeList, List<Item> itemList) {
            for (int store1 = 0; store1 < storeList.Count; store1++) {
                foreach (Item item in itemList) {
                    if (storeList[store1].getQty(item.extid) < item.qty) {
                        goto nextStore;
                    }
                }
                List<string> storeNames = new List<string>();
                storeNames.Add(storeList[store1].getName());
                addFinalMatch(storeNames);
            nextStore:
                Progress();
            }
        }
        private void TwoStoreCalc(List<Store> storeList, List<Item> itemList) {
            // This is like a for loop that makes the variable store1 go from 0 (inclusive) to storeList.Count (exclusive).
            // The only difference is that this one will run the loops in parallel.  All of the hard work to accomplish 
            // running it in parallel is done behind the scenes.  This only works because what each iteration of the loop
            // is doing is independent of the others.  If each loop depended on the previous loop this wouldn't work properly.
            Parallel.For(0, storeList.Count, store1 => {
                for (int store2 = store1 + 1; store2 < storeList.Count; store2++) {
                    if (calcWorker.CancellationPending) { return; }
                    Interlocked.Increment(ref longcount);
                    bool fail = false;
                    foreach (Item item in itemList) {
                        if (storeList[store1].getQty(item.extid) +
                            storeList[store2].getQty(item.extid) < item.qty) {
                            fail = true;
                            break;
                        }
                    }
                    if (!fail) {
                        List<string> storeNames = new List<string>();
                        storeNames.Add(storeList[store1].getName());
                        storeNames.Add(storeList[store2].getName());
                        addFinalMatch(storeNames);
                    }
                }
                Progress();
            });
        }
        private void ThreeStoreCalc(List<Store> storeList, List<Item> itemList) {
            Parallel.For(0, storeList.Count, store1 => {
                for (int store2 = store1 + 1; store2 < storeList.Count; store2++) {
                    if (calcWorker.CancellationPending) { return; }
                    for (int store3 = store2 + 1; store3 < storeList.Count; store3++) {
                        Interlocked.Increment(ref longcount);
                        if (calcWorker.CancellationPending) { return; }
                        bool fail = false;
                        foreach (Item item in itemList) {
                            if (storeList[store1].getQty(item.extid) +
                                storeList[store2].getQty(item.extid) +
                                storeList[store3].getQty(item.extid) < item.qty) {
                                fail = true;
                                break;
                            }
                        }
                        if (!fail) {
                            List<string> storeNames = new List<string>();
                            storeNames.Add(storeList[store1].getName());
                            storeNames.Add(storeList[store2].getName());
                            storeNames.Add(storeList[store3].getName());
                            addFinalMatch(storeNames);
                        }
                    }
                }
                Progress();
            });
        }
        private void FourStoreCalc(List<Store> storeList, List<Item> itemList) {
            Parallel.For(0, storeList.Count, store1 => {
                for (int store2 = store1 + 1; store2 < storeList.Count; store2++) {
                    if (calcWorker.CancellationPending) { return; }
                    for (int store3 = store2 + 1; store3 < storeList.Count; store3++) {
                        if (calcWorker.CancellationPending) { return; }
                        for (int store4 = store3 + 1; store4 < storeList.Count; store4++) {
                            if (calcWorker.CancellationPending) { return; }
                            Interlocked.Increment(ref longcount);
                            bool fail = false;
                            foreach (Item item in itemList) {
                                if (storeList[store1].getQty(item.extid) +
                                    storeList[store2].getQty(item.extid) +
                                    storeList[store3].getQty(item.extid) +
                                    storeList[store4].getQty(item.extid) < item.qty) {
                                    fail = true;
                                    break;
                                }
                            }
                            if (!fail) {
                                List<string> storeNames = new List<string>();
                                storeNames.Add(storeList[store1].getName());
                                storeNames.Add(storeList[store2].getName());
                                storeNames.Add(storeList[store3].getName());
                                storeNames.Add(storeList[store4].getName());
                                addFinalMatch(storeNames);
                            }
                        }
                    }
                }
                Progress();
            });
        }
        private void FiveStoreCalc(List<Store> storeList, List<Item> itemList) {
            Parallel.For(0, storeList.Count, store1 => {
                for (int store2 = store1 + 1; store2 < storeList.Count; store2++) {
                    if (calcWorker.CancellationPending) { return; }
                    for (int store3 = store2 + 1; store3 < storeList.Count; store3++) {
                        if (calcWorker.CancellationPending) { return; }
                        for (int store4 = store3 + 1; store4 < storeList.Count; store4++) {
                            if (calcWorker.CancellationPending) { return; }
                            for (int store5 = store4 + 1; store5 < storeList.Count; store5++) {
                                if (calcWorker.CancellationPending) { return; }
                                Interlocked.Increment(ref longcount);
                                bool fail = false;
                                foreach (Item item in itemList) {
                                    if (storeList[store1].getQty(item.extid) +
                                        storeList[store2].getQty(item.extid) +
                                        storeList[store3].getQty(item.extid) +
                                        storeList[store4].getQty(item.extid) +
                                        storeList[store5].getQty(item.extid) < item.qty) {
                                        fail = true;
                                        break;
                                    }
                                }
                                if (!fail) {
                                    List<string> storeNames = new List<string>();
                                    storeNames.Add(storeList[store1].getName());
                                    storeNames.Add(storeList[store2].getName());
                                    storeNames.Add(storeList[store3].getName());
                                    storeNames.Add(storeList[store4].getName());
                                    storeNames.Add(storeList[store5].getName());
                                    addFinalMatch(storeNames);
                                }
                            }
                        }
                    }
                }
                Progress();
            });
        }
        #endregion

        // Used for debugging stuff.  Leave it.
        private string intArrayToString(int[] a) {
            StringBuilder sb = new StringBuilder();
            foreach (int i in a) {
                sb.Append(i);
                sb.Append(", ");
            }
            return sb.ToString();
        }
    }
}
