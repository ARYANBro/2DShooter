using Godot;
using System;

public class PointsSpawner
{
    private PackedScene pointsScene;

    public PointsSpawner(PackedScene _pointsScene)
    {
        pointsScene = _pointsScene;
    }

    public void Spawn(Vector2 position, int _points, Vector2 size, SceneTree sceneTree)
    {
        // On top of enemies
        var points = pointsScene.Instance() as Node2D;
        var pointsLabel = points.GetNode<Label>("PointsAnimPlayer/Points");
        var pointsNode2D = points.GetNode<Node2D>("PointsAnimPlayer");

        pointsLabel.RectPosition = position;
        pointsLabel.RectScale = size;
        pointsLabel.Text = "+" + Convert.ToString(_points);

        sceneTree.CurrentScene.AddChild(points, true);
        sceneTree.CreateTimer(1f).Connect("timeout", points, "queue_free");
    }
}