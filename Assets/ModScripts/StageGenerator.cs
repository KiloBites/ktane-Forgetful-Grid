﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class StageGenerator
{
    public List<ColorGrid> StagesGenerated;

    public List<List<int>> IndividualIxes;

    private List<int> stageIxes;

    public List<GridColorOption[]> CombinedSets;

    private PriorityNumbers prioritySet;

    public StageGenerator(Material[] colorMats, int count, PriorityNumbers prioritySet)
    {
        StagesGenerated = Enumerable.Range(0, count).Select(_ => new ColorGrid(colorMats)).ToList().Shuffle();
        this.prioritySet = prioritySet;
        CombinedSets = CombinedSet(colorMats.Last());
        stageIxes = Enumerable.Range(0, count).ToList();
        IndividualIxes = GetIndividualStageIxes();
    }

    private List<List<int>> GetIndividualStageIxes()
    {
        var finalList = new List<List<int>>();

        var currentStageList = stageIxes.ToList();

        var setLength = prioritySet.Sets.Length;

        do
        {
            finalList.Add(currentStageList.Count <= setLength ? currentStageList.ToList() : currentStageList.Take(setLength).ToList());

            currentStageList.RemoveRange(0, currentStageList.Count <= setLength ? currentStageList.Count : setLength);
        }
        while (currentStageList.Count > 0);

        return finalList;
    }

    private List<GridColorOption[]> CombinedSet(Material black)
    {
        var set = prioritySet.Sets;
        var setLength = set.Length;

        var currentList = StagesGenerated.ToList();

        var finalList = new List<GridColorOption[]>();

        do
        {
            var currentSet = currentList.Take(setLength).Select(a => a.Grid).ToList();
            var currentSetOrdered = Enumerable.Range(0, Mathf.Min(currentSet.Count, setLength)).OrderBy(a => set[a]).Select(a => currentSet[a]).ToList();


            var newGrid = Enumerable.Repeat(new GridColorOption("Empty", black), 25).ToArray();

            foreach (var grid in currentSetOrdered)
                for (int i = 0; i < grid.Length; i++)
                    if (grid[i].ColorName != "Empty")
                        newGrid[i] = grid[i];

            finalList.Add(newGrid);

            currentList.RemoveRange(0, currentList.Count < setLength ? currentList.Count : setLength);
        }
        while (currentList.Count > 0);

        return finalList;
    }
}
