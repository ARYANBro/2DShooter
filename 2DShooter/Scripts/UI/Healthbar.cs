using System;
using Godot;

[Tool]
public class Healthbar : TextureProgress
{
    [Export] public bool isAcidic;

    public Sprite heart;
    public Player player;

    [Export] private Texture acidicTextureProgress = null;
    [Export] private Texture orignalTextureProgress = null;
    [Export] private Texture heartOrignalTexture = null;
    [Export] private Texture heartAcidicTexture = null;
    

    public override void _Ready()
    {
        if (!Engine.EditorHint)
        {
            player = GetTree().CurrentScene.GetNode<Player>("Player");
            heart = FindNode("Heart", true, false) as Sprite;
        }
    }


    public override void _Process(float delta)
    {
        if (isAcidic)
        {
            TextureProgress_ = acidicTextureProgress;

            if (!Engine.EditorHint)
               heart.Texture = heartAcidicTexture;
        }
        else
        {
            TextureProgress_ = orignalTextureProgress;

            if (!Engine.EditorHint)
                heart.Texture = heartOrignalTexture;
        }

        if (!Engine.EditorHint)
        {
            if (player != null)
                Value = Mathf.Lerp((float)Value, player.Hp, 0.5f);
        }
    }
}