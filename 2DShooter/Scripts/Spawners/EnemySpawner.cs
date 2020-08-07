using Godot;
using System;
using System.Collections.Generic;

public class EnemySpawner
{

    Node parent;
    List<Node> enemies;

    public EnemySpawner(Node _parent)
    {
        parent = _parent;
        enemies = new List<Node>();
    }
    
    public void Spawn(PackedScene enemyScene, int enemyCount)
    {
        enemies.Clear();
        for (int i = 0; i < enemyCount; i++)
        {
            var enemy = enemyScene.Instance() as Node;
            enemies.Add(enemy);
            parent.AddChild(enemies[i]);
        }
    }
}
