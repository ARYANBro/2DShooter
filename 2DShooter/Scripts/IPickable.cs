using Godot;
using System;

 public interface IPickable
{
	bool isEquiped { get; set; }
	bool wantToEquipGun { get; set; }
    bool ParentCheck { get; }
	bool AlreadySpawned{ get; set; }

	Inventory inventory { get; set; }
	PackedScene weaponScene { get; set; }

    void Equip();
	void UnEquip();
}