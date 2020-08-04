using Godot;
using System;
using System.Collections.Generic;

public class EnemySpawner
{

    Node parent;
    PackedScene enemyScene;
    PackedScene bigEnemyScene;
    PackedScene acidEnemyScene;
    List<Node> enemies = new List<Node>();

    public EnemySpawner(PackedScene _enemyScene, PackedScene _bigEnemyScene, PackedScene _acidEnemyScene, Node _parent)
    {
        enemyScene = _enemyScene;
        bigEnemyScene = _bigEnemyScene;
        acidEnemyScene = _acidEnemyScene;
        parent = _parent;
    }
    
    public void Spawn(int enemyCount, int bigEnemyCount, int acidEnemyCount)
    {
        // Randomly spawn enemeis
        enemies.Clear();
        Utlities.random.Randomize();

        for (int i = 0; i < Utlities.random.RandiRange(1, enemyCount); i++)
        {
            Node enemy = enemyScene.Instance();
            if (i >= enemyCount / bigEnemyCount)
                enemy = bigEnemyScene.Instance();

            enemies.Add(enemy);
            parent.AddChild(enemies[i], true);
        }
    }
}
