namespace Zenthrill.Benchmarks;

using System;
using System.Collections.Generic;
using System.Linq;

public static class StoryDataGenerator
{
    private static Random random = new Random();

    public static List<Fragment> GenerateFragments(int count)
    {
        return Enumerable.Range(1, count).Select(id =>
            new Fragment
            {
                Id = id,
                Content = $"Это фрагмент истории номер {id}."
            }).ToList();
    }

    public static List<Branch> GenerateBranches(List<Fragment> fragments)
    {
        List<Branch> branches = new List<Branch>();

        foreach (var fragment in fragments)
        {
            // Создаём от 0 до 3 ветвлений из каждого фрагмента.
            int branchesCount = random.Next(4); // 0-3 ветвления

            // Так как мы не можем переходить к фрагментам, которых нет, ограничим макс. ID.
            int maxFragmentId = fragments.Max(f => f.Id);

            for (int i = 0; i < branchesCount; i++)
            {
                branches.Add(new Branch
                {
                    Id = branches.Count + 1, // Обеспечиваем уникальность Id
                    FromFragmentId = fragment.Id,
                    ToFragmentId = random.Next(1, maxFragmentId + 1), // Случайное ветвление в пределах диапазона
                    Description = $"Выбор {i + 1} из фрагмента {fragment.Id}"
                });
            }
        }

        return branches;
    }
}