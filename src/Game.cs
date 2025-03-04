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
				Console.WriteLine("Press [Enter] to try again");
				Console.ReadLine();
				NewGame();
			}
		}
		Console.WriteLine("Thank you for playing.");
		Console.WriteLine("Press [Enter] to continue.");
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
	}
	private void Status(){
		Console.WriteLine($"Health: {player.GetHealth()}");
		if(player.inventory.GetWeight() != 0){
			Console.WriteLine("Items in inventory: ("+player.inventory.GetWeight()*10+"dag/"+player.inventory.GetMaxWeight()*10+"dag)");
		}
		foreach(KeyValuePair<string, Item> item in player.inventory.GetInventory()){
			Console.WriteLine(item.Key+" - "+item.Value.Description+" - "+item.Value.Weight*10+"dag");
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
				Console.WriteLine(itemName+" was added to your inventory!");
			}
			else {
				Console.WriteLine("There isn't enough space in your inventory!");
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
				Console.WriteLine(itemName+" was dropped from your inventory!");
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
	private void Use(Command command){
		if(!command.HasThirdWord()){
			Console.WriteLine("Not enough arguments passed.");
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
			Console.WriteLine("The door is already unlocked");
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
}
