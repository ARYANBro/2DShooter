using Godot;
using System;

public class PointsSpawner
{
    private PackedScene pointsScene;

    public PointsSpawner(PackedScene _pointsScene) => (pointsScene) = (_pointsScene);
    public void Spawn(Vector2 position, int _points, Vector2 size, SceneTree sceneTree)
    {
        // On top of enemies
        var points = pointsScene.Instance() as Node2D;
        var pointsLabel = points.GetNode<Label>("PointsAnimPlayer/Points");
        var pointsNode2D = points.GetNode<Node2D>("PointsAnimPlayer");

        // Set Position
        pointsLabel.RectPosition = position;
        pointsLabel.RectScale = size;

        pointsLabel.Text = '+' + _points.ToString();

        sceneTree.CurrentScene.AddChild(points);
        
        // Cleanup
        sceneTree.CreateTimer(1f).Connect("timeout", points, "queue_free");
    }
}
