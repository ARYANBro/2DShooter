using Godot;
using System;

public class Shop : Node2D
{
    public AnimationPlayer gunAnimPlayer;

    private enum GunOnMenue
    {
        Pistol, Shotgun, RocketLauncher
    };

    GunOnMenue gunOnMenue;

    public override void _Ready()
    {
        gunAnimPlayer = GetNode<AnimationPlayer>("Guns/GunAnimPlayer");
        gunOnMenue = GunOnMenue.Pistol;
    }

    private void OnRightSlideArrowPressed()
    {
        if (gunOnMenue == GunOnMenue.Pistol)
        {
            gunOnMenue = GunOnMenue.Shotgun;
            gunAnimPlayer.Play("ShowShotgunFromLeft");
        }
        else if (gunOnMenue == GunOnMenue.Shotgun)
        {
            gunOnMenue = GunOnMenue.RocketLauncher;
            gunAnimPlayer.Play("ShowRocketLauncher");
        }
    }

    private void OnLeftSlideArrowPressed()
    {
        if (gunOnMenue == GunOnMenue.RocketLauncher)
        {
            gunOnMenue = GunOnMenue.Shotgun;
            gunAnimPlayer.Play("ShowShotgunFromRight");
        }
        else if (gunOnMenue == GunOnMenue.Shotgun)
        {
            gunOnMenue = GunOnMenue.Pistol;
            gunAnimPlayer.Play("ShowPistol");
        }
    }
}