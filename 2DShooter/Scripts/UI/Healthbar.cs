using System;
using Godot;

[Tool]
public class Healthbar : TextureProgress
{
    [Export] public bool isAcidic;

    [Export(PropertyHint.File, "*png")] string acidicTextureProgressFilePath;
    [Export(PropertyHint.File, "*png")] string orignalTextureProgressFilePath;

    [Export(PropertyHint.File, "*png")] string heartOrignalTextureFilePath;
    [Export(PropertyHint.File, "*png")] string heartAcidicTextureFilePath;
    
    public Sprite heart;
    public Player player;

    private Texture acidicTextureProgress;
    private Texture orignalTextureProgress;
    private Texture heartOrignalTexture;
    private Texture heartAcidicTexture;

    public override void _Ready()
    {
        if (!Engine.EditorHint)
        {
            player = GetTree().CurrentScene.GetNode<Player>("Player");
            heart = FindNode("Heart", true, false) as Sprite;

            acidicTextureProgress = ResourceLoader.Load<Texture>(acidicTextureProgressFilePath);
            orignalTextureProgress = ResourceLoader.Load<Texture>(orignalTextureProgressFilePath);

            heartOrignalTexture = ResourceLoader.Load<Texture>(heartOrignalTextureFilePath);
            heartAcidicTexture = ResourceLoader.Load<Texture>(heartAcidicTextureFilePath);
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