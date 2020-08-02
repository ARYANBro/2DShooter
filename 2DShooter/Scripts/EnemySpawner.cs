using Godot;
using System;
using System.Collections.Generic;

public class EnemySpawner
{

    Node parent;
    PackedScene enemyScene;
    PackedScene bigEnemyScene;
    List<Node> enemies = new List<Node>();

    public EnemySpawner(PackedScene _enemyScene, PackedScene _bigEnemyScene, Node _parent) =>
    (enemyScene, bigEnemyScene, parent) = (_enemyScene, _bigEnemyScene, _parent);
    
    public void Spawn(int enemyCount, int bigEnemyCount)
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
    }
}
