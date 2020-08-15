using Godot;

public class Slot
{
    public Gun Gun
    { 
        get
        {
            return gun;
        }

        set 
        {
            gun = value;
        }
    }

    Gun gun;
     
    public Slot() {}

    public Slot(Gun _gun) 
    {
        gun = _gun;
    } 
}