using Godot;
using System;
using System.Collections.Generic;

public class EnemySpawner
{

    private Node parent;
    private PackedScene enemyScene;
    private PackedScene bigEnemyScene;
    private List<Node> enemies = new List<Node>();

    private bool spawnEnemies = false;

    public bool SpawnEnemies
    {
        get
        {
            return spawnEnemies;
        }

        set
        {
            spawnEnemies = value;
        }
    }

    public EnemySpawner(PackedScene _enemyScene, PackedScene _bigEnemyScene, Node _parent) =>
    (enemyScene, bigEnemyScene, parent) = (_enemyScene, _bigEnemyScene, _parent);


    public void SetSpawnParams(int enemyCount, int bigEnemyCount)
    {
        if (spawnEnemies)
        {
            // Randomly spawn enemeis
            enemies.Clear();
            Utlities.randNumGenerator.Randomize();

            for (int i = 0; i < Utlities.randNumGenerator.RandiRange(1, enemyCount); i++)
            {
                Node enemy = enemyScene.Instance();
                if (i >= enemyCount / bigEnemyCount)
                    enemy = bigEnemyScene.Instance();
                enemies.Add(enemy);

                parent.AddChild(enemies[i], true);

            }

            spawnEnemies = false;
        }
    }
}
