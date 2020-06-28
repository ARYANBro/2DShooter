using Godot;
using System;

 public interface IPickable
{
	bool isEquiped { get; set; }
	Inventory inventory { get; set; }
	String path { get; set; }

	void ParentCheck();
	void Equip();
	void UnEquip();
	void OnBodyEntered(object body);
}
