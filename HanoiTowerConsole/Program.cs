using System;
using System.Collections.Generic;
using System.Linq;

namespace HanoiTowerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            int towersCount = 3;
            int itemsCount = 4;
            Stack<int>[] towers = GetTowers(towersCount, itemsCount);

            DrawTowers(towers, itemsCount);

            PerformIterativeTowerMoving(ref towers, itemsCount);

            DrawTowers(towers, itemsCount);
        }

        static Stack<int>[] GetTowers(int towersCount, int itemsCount)
        {
            Stack<int>[] towers = new Stack<int>[towersCount];
            for (int i = 0; i < towers.Length; i++)
                towers[i] = new Stack<int>();
            for (int i = itemsCount; i > 0; i--)
                towers[0].Push(i);

            return towers;
        }

        static void DrawTowers(Stack<int>[] towers, int itemsCount)
        {
            for (int i = 0; i < 3; i++)
            {
                int[] tower = towers[i].Reverse().ToArray();
                for (int ii = 0; ii < itemsCount; ii++)
                {
                    if (ii < tower.Length)
                        Console.Write(tower[ii]);
                    else
                        Console.Write('-');
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static void PerformIterativeTowerMoving(ref Stack<int>[] towers, int itemsCount)
        {
            bool checkNeeded;
            bool hasSwap;
            do
            {
                checkNeeded = false;
                List<int> checkedTowers = new List<int>();
                do
                {
                    int currentTowerId = GetTowerIdWithHighestEndItem(towers, checkedTowers);

                    hasSwap = TrySwap(towers, currentTowerId, itemsCount);

                    if (!hasSwap)
                        checkedTowers.Add(currentTowerId);

                    if (towers[currentTowerId].Count == 0)
                        checkNeeded = true;
                } while (!hasSwap);

                DrawTowers(towers, itemsCount);
            } while(!(checkNeeded && IsEnd(towers)));
        }

        static int GetTowerIdWithHighestEndItem(Stack<int>[] towers, List<int> checkedTowers)
        {
            int id = -1;
            int max = 0;
            for (int i = 0; i < towers.Length; i++)
            {
                if (towers[i].Count > 0)
                {
                    if (!checkedTowers.Contains(i) && towers[i].Peek() > max)
                    {
                        max = towers[i].Peek();
                        id = i;
                    }
                }
            }

            if (id != -1)
                return id;
            else
                throw new Exception();
        }

        static bool TrySwap(Stack<int>[] towers, int towerId, int itemsCount)
        {
            int dir = ((towers[towerId].Peek() % 2) * 2 - 1) * (-(itemsCount % 2) * 2 + 1);
            int nextId = (towerId + dir + towers.Length) % towers.Length;
            if (towers[nextId].Count == 0 || towers[towerId].Peek() < towers[nextId].Peek())
            {
                towers[nextId].Push(towers[towerId].Pop());
                return true;
            }else
            {
                return false;
            }
        }

        static bool IsEnd(Stack<int>[] towers)
        {
            foreach (var tower in towers)
            {
                if (tower.Count != 0 && tower.Peek() != 1)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
