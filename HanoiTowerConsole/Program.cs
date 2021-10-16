using System;
using System.Collections.Generic;
using System.Linq;

namespace HanoiTowerConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            AskParameters(out int algotrithmNum, out int towersNum, out int itemsNum);
            Field field = new Field(towersNum, itemsNum);

            field.DrawTowers();

            if (algotrithmNum == 1)
                IterativeAlgorithmPack.PerformIterativeTowerMoving(ref field);
            else
                RecursiveAlgorithmPack.PerformRecursiveTowerMoving(ref field);

            field.DrawTowers();
        }

        static void AskParameters(out int algotrithmNum, out int towersNum, out int itemsNum)
        {
            Console.Write("Введите номер алгоритма (1-итеративный; 2-рекурсивный): ");
            ConsoleKeyInfo firsQ = Console.ReadKey(false);
            if (firsQ.Key == ConsoleKey.D1)
                algotrithmNum = 1;
            else if (firsQ.Key == ConsoleKey.D2)
                algotrithmNum = 2;
            else
                throw new Exception();
            Console.WriteLine();

            if (algotrithmNum == 2)
            {
                Console.Write("Введите номер башен: ");
                String secondQ = Console.ReadLine();
                if (!(Int32.TryParse(secondQ, out towersNum) && towersNum > 1))
                    throw new Exception("Ты шо наделал!");
            }
            else
            {
                towersNum = 3;
            }

            Console.Write("Введите количество элементов: ");
            String thirdQ = Console.ReadLine();
            if (!(Int32.TryParse(thirdQ, out itemsNum) && towersNum > 1))
                throw new Exception("Ну и зачем?");
            Console.WriteLine();
        }
    }

    class Field
    {
        public readonly int TowersCount, ItemsCount;
        readonly Stack<int>[] towers;
        public Field(int towersCount, int itemsCount)
        {
            TowersCount = towersCount;
            ItemsCount = itemsCount;
            towers = GetTowersWithStartState(towersCount, itemsCount);
        }
        static Stack<int>[] GetTowersWithStartState(int towersCount, int itemsCount)
        {
            Stack<int>[] towers = new Stack<int>[towersCount];
            for (int i = 0; i < towers.Length; i++)
                towers[i] = new Stack<int>();
            for (int i = itemsCount; i > 0; i--)
                towers[0].Push(i);

            return towers;
        }
        public void DrawTowers()
        {
            for (int i = 0; i < towers.Length; i++)
            {
                int[] tower = towers[i].Reverse().ToArray();
                for (int ii = 0; ii < ItemsCount; ii++)
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
        public void Drag(int fromTowerNum, int toTowerNum)
        {
            if (towers[fromTowerNum].Count != 0)
                if (towers[toTowerNum].Count == 0 || towers[fromTowerNum].Peek() < towers[toTowerNum].Peek())
                    towers[toTowerNum].Push(towers[fromTowerNum].Pop());
                else
                    throw new Exception("Попытка перетащить больший элемент на меньший");
            else
                throw new Exception("Попытка перетащить элемент из пустой башни");

            DrawTowers();
        }
        public int GetNumOfItems(int towerId)
        {
            return towers[towerId].Count;
        }
        public int GetLastItem(int towerId)
        {
            return towers[towerId].Peek();
        }
        public bool IsNormalState()
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

    static class IterativeAlgorithmPack
    {
        public static void PerformIterativeTowerMoving(ref Field field)
        {
            bool checkNeeded;
            bool hasSwap;
            do
            {
                checkNeeded = false;
                List<int> checkedTowers = new List<int>();
                do
                {
                    int currentTowerId = GetTowerIdWithHighestEndItem(field, checkedTowers);

                    hasSwap = TrySwap(ref field, currentTowerId);

                    if (!hasSwap)
                        checkedTowers.Add(currentTowerId);

                    if (field.GetNumOfItems(currentTowerId) == 0)
                        checkNeeded = true;
                } while (!hasSwap);

            } while (!(checkNeeded && field.IsNormalState()));
        }

        static int GetTowerIdWithHighestEndItem(Field field, List<int> checkedTowers)
        {
            int id = -1;
            int max = 0;
            for (int i = 0; i < field.TowersCount; i++)
            {
                if (field.GetNumOfItems(i) > 0)
                {
                    if (!checkedTowers.Contains(i) && field.GetLastItem(i) > max)
                    {
                        max = field.GetLastItem(i);
                        id = i;
                    }
                }
            }

            if (id != -1)
                return id;
            else
                throw new Exception();
        }

        static bool TrySwap(ref Field field, int towerId)
        {
            int dir = ((field.GetLastItem(towerId) % 2) * 2 - 1) * (-(field.ItemsCount % 2) * 2 + 1);
            int nextId = (towerId + dir + field.TowersCount) % field.TowersCount;
            if (field.GetNumOfItems(nextId) == 0 || field.GetLastItem(towerId) < field.GetLastItem(nextId))
            {
                field.Drag(towerId, nextId);
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    static class RecursiveAlgorithmPack
    {
        public static void PerformRecursiveTowerMoving(ref Field field)
        {
            DoRecursiveTowerMoving(ref field, 0, field.TowersCount - 1, 0);
        }
        static void DoRecursiveTowerMoving(ref Field field, int currentTowerId, int currentAimTowerId, int deep)
        {
            if (deep != field.GetNumOfItems(currentTowerId) - 1)
            {
                int transitTowerId = DefineNewAimTowerId(currentTowerId, currentAimTowerId);
                int itemCount = field.GetNumOfItems(transitTowerId);
                DoRecursiveTowerMoving(ref field, currentTowerId, transitTowerId, deep + 1);
                field.Drag(currentTowerId, currentAimTowerId);
                DoRecursiveTowerMoving(ref field, transitTowerId, currentAimTowerId, itemCount);
                return;
            }

            field.Drag(currentTowerId, currentAimTowerId);
        }

        static int DefineNewAimTowerId(int currentTowerId, int pastAimTowerId)
        {
            int i = 0;
            do
            {
                if (i != currentTowerId && i != pastAimTowerId)
                    return i;
                i++;
            } while (true);
        }
    }
}
