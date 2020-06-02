using Godot;

public class Player : KinematicBody2D {
	[Export] public float bulletSpeed = 10000.0f;
	[Export] public float acceleration = 7.0f;
	[Export] public float speed = 8.0f;
	public PackedScene bulletScene;
	public Position2D firePoint;

	private Vector2 direction;
	private Vector2 velocity;
	private Vector2 mousePos;

	public override void _Ready() {
		bulletScene = ResourceLoader.Load<PackedScene>("res://Assets/Bullet.tscn");
		firePoint = (Position2D)GetTree().Root.GetNode("FirePoint");
	}

	public override void _Process(float delta) {

		mousePos = GetViewport().GetMousePosition();
		
		/* Temporary put it in a game rules script */
		if (Input.IsActionPressed("ui_cancel")) {
			GetTree().Quit();
		}

		firePoint.Position += Position;
	}

	public override void _PhysicsProcess(float delta) {
		Move(delta);

		Vector2 lookDir = mousePos - Position;
		float angle = Mathf.Atan2(lookDir.y, lookDir.x);
		RotationDegrees = Mathf.Rad2Deg(angle) + 90;

		if (Input.IsActionJustPressed("Fire")) {
			Shoot();
		}
	}

	private void Move(float delta) {

		direction = new Vector2();

		if (Input.IsActionPressed("MoveForward"))
			direction -= new Vector2(0.0f, 1.0f) * speed;
		else if (Input.IsActionPressed("MoveBackward"))
			direction += new Vector2(0.0f, 1.0f) * speed;

		if (Input.IsActionPressed("MoveLeft"))
			direction -= new Vector2(1.0f, 0.0f) * speed;
		else if (Input.IsActionPressed("MoveRight"))
			direction += new Vector2(1.0f, 0.0f) * speed;

		direction = direction.Normalized();

		velocity = velocity.LinearInterpolate(direction * speed, acceleration * delta);
		MoveAndSlide(velocity);
	}

	private void Shoot() {
		RigidBody2D bullet = (RigidBody2D)bulletScene.Instance();
		bullet.RotationDegrees = RotationDegrees;
		bullet.Transform = Transform;
		bullet.AddForce(new Vector2(0.0f, 0.0f), (mousePos - Position) * bulletSpeed); ;

		GetTree().Root.AddChild(bullet);
	}
}
