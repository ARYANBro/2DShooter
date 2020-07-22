using Godot;
using System;
using System.Collections.Generic;

public class EnemySpawner
{
    private List<Node> enemies = new List<Node>();
    private PackedScene enemyScene;
    private PackedScene bigEnemyScene;
    private Node parent;

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
