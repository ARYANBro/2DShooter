using Godot;
using System;

 public interface IPickable
{
	bool isEquiped { get; set; }
	bool wantToEquipGun { get; set; }
	Inventory inventory { get; set; }
	PackedScene weaponScene { get; set; }

    bool ParentCheck { get; }
	bool isUnlocked { get; set; }
	bool AlreadySpawned{ get; set; }

    void Equip();
	void UnEquip();
}