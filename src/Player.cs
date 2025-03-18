class Player
{
	private int health;
	private Room currentRoom;
	private Inventory inventory;
	
	public Player(){
		health = 100;
		inventory = new Inventory(10);
	}
	public Inventory GetInventory(){
		return inventory;
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
		if(this.health > 100){
			this.health = 100;
		}
	}
	public bool IsAlive(){
		return (this.health > 0) ? true: false;
	}
}
