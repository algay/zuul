using System.Collections.Generic;

class Room
{
	// Private fields
	private string description;
	private Dictionary<string, Room> exits; // stores exits of this room.
	public Inventory inventory;
	private bool isLocked;
	private bool lockType;
	private string code;
	private string lockDescription;
	private Dictionary<string, Enemy> enemies;

	// Create a room described "description". Initially, it has no exits.
	// "description" is something like "in a kitchen" or "in a court yard".

	public Room(string desc)
	{
		description = desc;
		exits = new Dictionary<string, Room>();
		inventory = new Inventory(500);
		enemies = new Dictionary<string, Enemy>();
	}

	public Enemy GetEnemy(string name){
		return enemies[name];
	}
	public Dictionary<string, Enemy> GetEnemies(){
		return enemies;
	}
	public bool ContainsEnemy(string name){
		return enemies.ContainsKey(name);
	}
	public void AddEnemy(string name, Enemy enemy){
		enemies.Add(name, enemy);
	}
	public void RemoveEnemy(string name){
		enemies.Remove(name);
	}
	public void Lock(bool typeOfLock, string lockCode, string lockDesc){
		isLocked = true;
		lockType = typeOfLock;
		code = lockCode;
		lockDescription = lockDesc;
	}
	public bool IsLocked(){
		return isLocked;
	}
	public void Unlock(){
		isLocked = false;
	}
	public bool GetLockType(){
		return lockType;
	}
	public string GetCode(){
		return code;
	}
	public string GetLockDescription(){
		return lockDescription;
	}

	// Define an exit for this room.
	public void AddExit(string direction, Room neighbor)
	{
		exits.Add(direction, neighbor);
	}

	// Return the description of the room.
	public string GetShortDescription()
	{
		return description;
	}

	// Return a long description of this room, in the form:
	//     You are in the kitchen.
	//     Exits: north, west
	public string GetLongDescription()
	{
		string str = "You are ";
		str += description;
		str += ".\n";
		str += GetExitString();
		return str;
	}

	// Return the room that is reached if we go from this room in direction
	// "direction". If there is no room in that direction, return null.
	public Room GetExit(string direction)
	{
		if (exits.ContainsKey(direction))
		{
			return exits[direction];
		}
		return null;
	}

	// Return a string describing the room's exits, for example
	// "Exits: north, west".
	private string GetExitString()
	{
		string str = "Exits: ";
		str += String.Join(", ", exits.Keys);

		return str;
	}
}
