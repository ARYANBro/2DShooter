using Godot;
using System;
using System.Collections.Generic;

public class EnemySpawner
{
    public static ulong previousSeed;

    private Node parent;
    private PackedScene enemyScene;
    private PackedScene bigEnemyScene;
    private List<Node> enemies = new List<Node>();

    public EnemySpawner(PackedScene _enemyScene, PackedScene _bigEnemyScene, Node _parent)
    {
        enemyScene = _enemyScene;
        bigEnemyScene = _bigEnemyScene;
        parent = _parent;
    }

    public void Spawn(int enemyCount, int bigEnemyCount, ref bool spawnEnemies)
    {
        // Randomly spawn enemeis
        enemies.Clear();
        Utlities.randNumGenerator.Randomize();
        previousSeed = Utlities.randNumGenerator.Seed;
        
        for (int i = 0; i < Utlities.randNumGenerator.RandiRange(1, enemyCount); i++)
        {
            var enemy = enemyScene.Instance();
            if (i >= enemyCount / bigEnemyCount)
                enemy = bigEnemyScene.Instance();
            enemies.Add(enemy);
        }

        for (int i = 0; i < enemies.Count; i++)
            parent.AddChild(enemies[i], true);

        spawnEnemies = false;
    }
}
