using System;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;

	// Constructor
	public Game()
	{
		NewGame();
	}
	public void NewGame()
	{
		parser = new Parser();
		player = new Player();
		CreateRooms();
	}

	// Initialise the Rooms (and the Items)
	private void CreateRooms()
	{
		// Create the rooms
		Room outside = new Room("outside the main entrance of the university");
		Room theatre = new Room("in a lecture theatre");
		Room pub = new Room("in the campus pub");
		Room lab = new Room("in a computing lab");
		Room office = new Room("in the computing admin office");

		// Initialise room exits
		outside.AddExit("east", theatre);
		outside.AddExit("south", lab);
		outside.AddExit("west", pub);
		outside.AddEnemy("goblin", new Enemy(20,5,"swings their branch at you", "dies from bleeding"));
		outside.inventory.Add("axe", new Weapon(5,"an axe, used for killing",9, "swing your axe at them"));

		theatre.AddExit("west", outside);
		pub.AddExit("east", outside);

		lab.AddExit("north", outside);
		lab.AddExit("east", office);

		office.AddExit("west", lab);

		// Create your Items here
		// ...
		// And add them to the Rooms
		// ...

		// Start game outside
		player.SetCurrentRoom(outside);
	}

	//  Main play routine. Loops until end of play.
	public void Play()
	{
		PrintWelcome();

		// Enter the main command loop. Here we repeatedly read commands and
		// execute them until the player wants to quit.
		bool finished = false;
		while (!finished)
		{
			if (player.IsAlive()){
				Command command = parser.GetCommand();
				finished = ProcessCommand(command);
			}
			else{
				Console.WriteLine("You Died!");
				Console.WriteLine("Press [Enter] to try again.");
				Console.ReadLine();
				NewGame();
				Look();
			}
		}
		Console.WriteLine("Thank you for playing.");
		Console.WriteLine("Press [Enter] to quit.");
		Console.ReadLine();
	}

	// Print out the opening message for the player.
	private void PrintWelcome()
	{
		Console.WriteLine();
		Console.WriteLine("Welcome to Zuul!");
		Console.WriteLine("Zuul is a new, incredibly boring adventure game.");
		Console.WriteLine("Type 'help' if you need help.");
		Console.WriteLine();
		Look();
	}

	// Given a command, process (that is: execute) the command.
	// If this command ends the game, it returns true.
	// Otherwise false is returned.
	private bool ProcessCommand(Command command)
	{
		bool wantToQuit = false;

		if(command.IsUnknown())
		{
			Console.WriteLine("I don't know what you mean...");
			return wantToQuit; // false
		}
		if(player.GetCurrentRoom().GetEnemies().Count() > 0 && command.CommandWord == "go"){
			Console.WriteLine("You cannot exit the room while enemies are in there.");
			return wantToQuit;
		}

		switch (command.CommandWord)
		{
			case "help":
				PrintHelp();
				break;
			case "go":
				GoRoom(command);
				player.Damage(5);
				break;
			case "quit":
				wantToQuit = true;
				break;
			case "look":
				Look();
				break;
			case "status":
				Status();
				break;
			case "take":
				Take(command);
				break;
			case "drop":
				Drop(command);
				break;
			case "use":
				Use(command);
				break;
			case "input":
				InputCode(command);
				break;
			case "attack":
				Attack(command);
				break;
		}
		if(command.CommandWord != "status" && command.CommandWord != "look" && command.CommandWord != "help" && command.CommandWord != "quit"){
			int damage;
			foreach(KeyValuePair<string, Enemy> enemy in player.GetCurrentRoom().GetEnemies()){
				damage = enemy.Value.GetDamage();
				player.Damage(damage);
				Console.WriteLine(enemy.Key+" "+enemy.Value.GetAttackMessage()+" for "+damage+" dmg.");
			}
		}

		return wantToQuit;
	}

	// ######################################
	// implementations of user commands:
	// ######################################

	// Print out some help information.
	// Here we print the mission and a list of the command words.
	private void PrintHelp()
	{
		Console.WriteLine("You are lost. You are alone.");
		Console.WriteLine("You wander around at the university.");
		Console.WriteLine();
		// let the parser print the commands
		parser.PrintValidCommands();
	}
	private void Look(){
		Console.WriteLine(player.GetCurrentRoom().GetLongDescription());
		if(player.GetCurrentRoom().inventory.GetWeight() != 0){
			Console.WriteLine("Items in room: ");
		}
		foreach(KeyValuePair<string, Item> item in player.GetCurrentRoom().inventory.GetInventory()){
			Console.WriteLine(item.Key+" - "+item.Value.Description+" - "+item.Value.Weight*10+"dag");
		}
		if(player.GetCurrentRoom().GetEnemies().Count() > 0){
			Console.WriteLine();
			Console.WriteLine("Enemies in room:");
		}
		foreach(KeyValuePair<string, Enemy> enemy in player.GetCurrentRoom().GetEnemies()){
			Console.WriteLine(enemy.Key+" - "+enemy.Value.GetHp()+"hp");
		}
	}
	private void Status(){
		Console.WriteLine($"Health: {player.GetHealth()}");
		if(player.inventory.GetWeight() != 0){
			Console.WriteLine("Items in inventory: ("+player.inventory.GetWeight()*10+"/"+player.inventory.GetMaxWeight()*10+")");
		}
		foreach(KeyValuePair<string, Item> item in player.inventory.GetInventory()){
			Console.Write(item.Key+" - "+item.Value.Description+" - "+item.Value.Weight*10+"dag");
			if(item.Value is Weapon){
				Weapon weapon = item.Value as Weapon;
				Console.WriteLine(" - "+weapon.GetMaxDamage()+" dmg");
			}
			else{
				Console.WriteLine();
			}
		}
	}
	// Try to go to one direction. If there is an exit, enter the new
	// room, otherwise print an error message.
	private void Take(Command command){
		if(!command.HasSecondWord()){
			Console.WriteLine("Which item?");
			return;
		}
		string itemName = command.SecondWord;
		if(player.GetCurrentRoom().inventory.Contains(itemName)){

			Item item = player.GetCurrentRoom().inventory.GetItem(itemName); 
			if(player.inventory.GetMaxWeight()>=player.inventory.GetWeight()+item.Weight){
				player.inventory.Add(itemName, item);
				player.GetCurrentRoom().inventory.Remove(itemName);
				Console.WriteLine(itemName+" was added to your inventory.");
			}
			else {
				Console.WriteLine("There isn't enough space in your inventory.");
			}
		}
		else{
			Console.WriteLine("Room doesn't contain "+itemName+".");
		}
	}
	private void Drop(Command command){
		if(!command.HasSecondWord()){
			Console.WriteLine("Which item?");
			return;
		}
		string itemName = command.SecondWord;
		if(player.inventory.Contains(itemName)){
			Item item = player.inventory.GetItem(itemName);
			if(player.GetCurrentRoom().inventory.GetMaxWeight()>=player.GetCurrentRoom().inventory.GetWeight()+item.Weight){
				player.GetCurrentRoom().inventory.Add(itemName, item);
				player.inventory.Remove(itemName);
				Console.WriteLine(itemName+" was dropped from your inventory.");
			}
			else {
				Console.WriteLine("There isn't enough space in the room");
			}
		}
		else{
			Console.WriteLine("Your inventory doesn't contain "+itemName+".");
		}
	}

	private void GoRoom(Command command)
	{
		if(!command.HasSecondWord())
		{
			// if there is no second word, we don't know where to go...
			Console.WriteLine("Go where?");
			player.Heal(5);
			return;
		}

		string direction = command.SecondWord;

		// Try to go to the next room.
		Room nextRoom = player.GetCurrentRoom().GetExit(direction);
		if (nextRoom == null)
		{
			Console.WriteLine("There is no door to "+direction+".");
			player.Heal(5);
			return;
		}
		if (nextRoom.IsLocked()){
			Console.WriteLine("The door is "+nextRoom.GetLockDescription()+".");
			player.Heal(5);
		}
		else{
			player.SetCurrentRoom(nextRoom);
			Console.WriteLine(player.GetCurrentRoom().GetLongDescription());
		}
	}
	private void Attack(Command command){
		if(!command.HasSecondWord()){
			Console.WriteLine("Attack what?");
			return;
		}
		if(!command.HasThirdWord()){
			Console.WriteLine("Attack with what?");
			return;
		}
		string enemy = command.SecondWord;
		string itemName = command.ThirdWord;
		if(player.GetCurrentRoom().ContainsEnemy(enemy)){
			if(player.inventory.Contains(itemName)){
				if(player.inventory.GetItem(itemName) is Weapon){
					Weapon weapon = player.inventory.GetItem(itemName) as Weapon;
					int damage = weapon.GetDamage();
					player.GetCurrentRoom().GetEnemy(enemy).Damage(damage);
					Console.WriteLine("You "+weapon.GetAttackMessage()+" for "+damage+" dmg.");
					if(!player.GetCurrentRoom().GetEnemy(enemy).IsAlive()){
						Console.WriteLine(enemy+" "+player.GetCurrentRoom().GetEnemy(enemy).GetDeathMessage()+".");
						player.GetCurrentRoom().RemoveEnemy(enemy);
					}
				}
				else{
					Console.WriteLine(itemName+" is not a weapon.");
				}
			}
			else{
				Console.WriteLine("You don't have a "+itemName+" in your inventory."); 
			}
		}
		else{
			Console.WriteLine("There is no "+enemy+" in the room.");
		}
	}
	private void Use(Command command){
		if(!command.HasSecondWord()){
			Console.WriteLine("Use what?");
			return;
		}
		if(!command.HasThirdWord()){
			Console.WriteLine("Use where?");
			return;
		}
		string item = command.SecondWord;
		string direction = command.ThirdWord;
		Room room = player.GetCurrentRoom().GetExit(direction);
		if (room == null){
			Console.WriteLine("There is no locked door to "+direction+".");
			return;
		}
		if(!room.IsLocked()){
			Console.WriteLine("The door is already unlocked.");
			return;
		}
		if(!room.GetLockType()){
			if(player.inventory.Contains(item)){
				if(room.GetCode() == item){
					Console.WriteLine("The door was unlocked.");
					room.Unlock();
				}
				else{
					Console.WriteLine("There is no "+item+" in your inventory.");
				}
			}
			else{
				Console.WriteLine("You don't have a "+item+" in your inventory.");
			}
		}
		else{
			Console.WriteLine("The door requires a code, and not a "+item+".");
		}
	}
	private void InputCode(Command command){
		if(!command.HasSecondWord()){
			Console.WriteLine("Input what?");
			return;
		}
		if(!command.HasThirdWord()){
			Console.WriteLine("Input where?");
			return;
		}
		string code = command.SecondWord;
		string direction = command.ThirdWord;
		Room room = player.GetCurrentRoom().GetExit(direction);
		if (room == null){
			Console.WriteLine("There is no locked door to "+direction+".");
			return;
		}
		if(!room.IsLocked()){
			Console.WriteLine("The door is already unlocked.");
			return;
		}
		if(room.GetLockType()){
			if(room.GetCode() == code){
				Console.WriteLine("The door was unlocked.");
				room.Unlock();
			}
			else{
				Console.WriteLine("The door stayed locked.");
			}
		}
		else{
			Console.WriteLine("The door requires an item, and not a code.");
		}
	}
}
