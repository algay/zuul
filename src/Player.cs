class Player
{
	private int health;
	private Room currentRoom;
	public Inventory inventory;
	
	public Player(){
		health = 100;
		inventory = new Inventory(25);
	}

	public int GetHealth(){
		return this.health;
	}
	public Room GetCurrentRoom(){
		return this.currentRoom;
	}
	public void SetCurrentRoom(Room room){
		this.currentRoom = room;
	}
	public void Damage(int damage){
		this.health = this.health - damage;
	}
	public void Heal(int amount){
		this.health = this.health + amount;
	}
	public bool IsAlive(){
		return (this.health > 0) ? true: false;
	}
}
