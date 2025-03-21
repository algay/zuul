using System;

class Game
{
	// Private fields
	private Parser parser;
	private Player player;
	private Random rnd = new Random();

	// Constructor
	public Game()
	{
		NewGame();
	}
	public void NewGame()
	{
		Console.Clear();
		Console.WriteLine("\x1b[3J");
		parser = new Parser();
		player = new Player();
		CreateRooms();
	}

	// Initialise the Rooms (and the Items)
	private void CreateRooms()
	{
		string passwordI = Convert.ToString(rnd.Next(0,9999));
		for(int i = passwordI.Length; i<4; i++){
			passwordI = "0"+passwordI;
		}
		string passwordII = Convert.ToString(rnd.Next(0,9999));
		for(int i = passwordII.Length; i<4; i++){
			passwordI = "0"+passwordI;
		}
		// Create the rooms
		Room corridor = new Room("in an empty corridor.\nThe lights buzz with motive");
		Room storage = new Room("in the storage facility.\nDust has gathered everywhere");
		Room kitchen = new Room("in the kitchen.\nThere sits an extinguisher cabinet on the wall");
		Room engine = new Room("in the engine room.\nThe engine coughs every couple seconds from age");
		Room escaperoom = new Room("in the escape pod room.\nThere is one escape pod left");
		Room bedroom = new Room("in the crew bedroom.\nA mirror in the corner.\nOne of the bunkbeds sticks out");
		Room bridge = new Room("on the ship's bridge.\nThere are a couple sticky notes on the wall");
		Room medical = new Room("in the medical room.\nThe air feels empty");
		Room dockingport = new Room("in the docking area.\nThere is emergency foam on the walls");
		Room electrical = new Room("in the electrical room.\nSome wires are split");
		Room escapepod = new Room("in the escape pod.\nYou are finally safe");

		// Initialise room exits
		corridor.AddExit("escape-pod-room", escaperoom);
		corridor.AddExit("kitchen", kitchen);
		corridor.AddExit("medical", medical);
		corridor.AddExit("bridge", bridge);

		kitchen.AddExit("corridor", corridor);
		kitchen.AddExit("electrical", electrical);
		kitchen.AddExit("engine", engine);
		kitchen.AddExit("bedroom", bedroom);
		kitchen.GetInventory().Add("fire-extinguisher", new Weapon(5, "a tool used for extinguishing fires", 8, "swing the fire extinguisher"));
		
		bedroom.AddExit("kitchen",kitchen);
		bedroom.GetInventory().Add("captains-diary", new Item(1,"on it it is inscribed: "+passwordI));
		
		engine.AddExit("kitchen", kitchen);
		engine.GetInventory().Add("sledgehammer", new Weapon(7, "a sledgehammer, used for smashing",12,"swing the sledgehammer"));
		engine.Lock(false,"engine-key","locked");
		engine.AddEnemy("squishy-alien", new Enemy(8,10,"squishes you","gets squished and dies"));

		bridge.AddExit("corridor", corridor);
		bridge.Lock(false, "helmsmans-key", "locked");
		bridge.GetInventory().Add("sticky-note", new Item(1, "new code for storage: "+passwordII));
		bridge.AddEnemy("silly-alien", new Enemy(20,5,"does something silly","dies from being too silly"));

		medical.AddExit("corridor", corridor);
		medical.AddExit("storage", storage);
		medical.Lock(false,"fire-extinguisher","buried in flames");
		medical.GetInventory().Add("helmsmans-key", new Item(1, "a key held by the helmsman"));
		medical.AddEnemy("just-a-normal-alien", new Enemy(15,6,"shrugs at you","dies from muscle pain"));
		
		storage.AddExit("medical", medical);
		storage.AddExit("docking-port", dockingport);
		storage.Lock(true,passwordII,"locked by a keycode");
		storage.AddEnemy("head-sized-alien", new Enemy(9,6,"bites you", "falls over and dies"));

		dockingport.AddExit("storage", storage);
		dockingport.Lock(false,"sledgehammer","fixed by rock-hard emergency foam");
		dockingport.GetInventory().Add("electrical-key", new Item(1, "a small key with \"electrics\" written on it"));
		dockingport.AddEnemy("squiggly-alien", new Enemy(12,4,"slaps you with their squiggly hand", "squiggles and dies"));
		dockingport.AddEnemy("giant-alien", new Enemy(17,8,"swings hard and hits you","falls over and dies"));

		electrical.AddExit("kitchen", kitchen);
		electrical.Lock(false,"electrical-key","locked");
		electrical.GetInventory().Add("breaker", new Item(2, "a circuit breaker"));
		electrical.AddEnemy("electrocuted-alien", new Enemy(20,10,"electrifies you with the power of ohms", "dies from self-induced electrocution"));

		escaperoom.AddExit("corridor", corridor);
		escaperoom.AddExit("escape-pod", escapepod);
		escaperoom.Lock(true,passwordI,"locked by a keycode");
		escaperoom.GetInventory().Add("engine-key", new Item(1, "a key"));

		escapepod.Lock(false,"breaker","missing an emergency breaker");

		// Create your Items here
		// ...
		// And add them to the Rooms
		// ...

		// Start game outside
		player.SetCurrentRoom(corridor);
	}

	//  Main play routine. Loops until end of play.
	public void Play()
	{
		PrintWelcome();

		// Enter the main command loop. Here we repeatedly read commands and
		// execute them until the player wants to quit.
		bool finished = false;
		Console.WriteLine();
		while (!finished)
		{
			if (player.IsAlive()){
				Command command = parser.GetCommand();
				Console.WriteLine();
				finished = ProcessCommand(command);
				if(player.GetCurrentRoom().GetShortDescription() == "in the escape pod.\nYou are finally safe"){
					finished = true;
					Console.WriteLine();
					Console.WriteLine("                ▒                                   ");
					Console.WriteLine("               ▒████▓▒                              ");
					Console.WriteLine("           ▒▓▒ ▒██████████▓▒                        ");
					Console.WriteLine("           ▓█▒ ▒▓██████████████▒    ▒▒              ");
					Console.WriteLine("           ▓█      ▒▒▓████████▓▒    ▒               ");
					Console.WriteLine("          ▓██▒▒          ▒▓███▒    ▒▒               ");
					Console.WriteLine("         ▒▓██████▓▒▒                      ▓▓        ");
					Console.WriteLine("          ▒▓██████████▓▒               ▒▒   ▓▓▒     ");
					Console.WriteLine("              ▒▓██████████▓▓          ▒▒▓    ▓▒     ");
					Console.WriteLine("                  ▒▓████████▒       ▒▓▓▓█▓  ▓█▒     ");
					Console.WriteLine("                      ▒▓████▒  ▒▓▓▓▒   ▒▓██▓▓▒      ");
					Console.WriteLine("                         ▒▒▒ ▒▓▓██▓      ▒▒▒▒       ");
					Console.WriteLine("                                 ▒▒  ▓ ▓            ");
					Console.WriteLine("                                     █▓             ");
					Console.WriteLine("                                    ██              ");
					Console.WriteLine();
				}
				if(!finished){
					Console.WriteLine();
				}
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
		Console.WriteLine("                                                    ");
		Console.WriteLine(" ▓▓▓▓▓▓▓▓▓▓▒  █▓▓▓▓█▒▓▒▒▒▒  ▒▒▒▒                    ");
		Console.WriteLine(" ▓       ▒▓▒  ▓    ▓▒▓  ▒▓ ▒▒  ▓▒█▒▒▒▒▓ ▒▓▒▒▒▒▓     ");
		Console.WriteLine(" ▓  ▒▓▓▒ ▒▓▒  ▓    ▓▒▓  ▒▓ ▒▓  ▓▒▓    ▓ ▒▓    ▓     ");
		Console.WriteLine(" ▓  ▒▓▒▓ ▒▓▒  ▓  ▒▓█▒▒▓▒▒▓ ▒▓  ▓▒▓    ▓▒▒▓    ▓▒    ");
		Console.WriteLine(" ▓████▒▓▒▒▓▒  ▓   ▓ ▓▒  ▒▓ ▒▓  ▓▒█▓▒▒ ▒▓▒▓▒   ▓▒    ");
		Console.WriteLine(" ▒█▓▓▓▓▓ ▒▓▒  ▓   ▓ ▓▒  ▒▓ ▒▓  ▓▓▒▒▒▒ ▒▓▒▓▒  ▒▓▒▒▒▒ ");
		Console.WriteLine(" ▒▓ ▒█▓▓▓▓▒  ▒▓   ▓▒▓▒  ▒▓ ▒▓  ▒▒▒▓▒▒ ▒▓ ▒▒  ▓▒▒▒▒▓ ");
		Console.WriteLine(" ▒▓  ▓▓▒▒▒▒▓▒▒▓   ▓▒▓▒   ▓ ▒▓    ▒▓▒▒  █ ▒▒  ▓▓▒ ▒▓ ");
		Console.WriteLine(" ▒▓  ▓▒   ▒▓ ▒▓          ▓▒▒▓▒    ▒▒▒  █ ▒▒  ▓▒  ▒▓ ");
		Console.WriteLine(" ▒▒       ▒▓▒ ▓          ▓▒ ▒▒▒▒▒      █ ▒▒      ▒▓▒");
		Console.WriteLine(" ▒▓▓▓▒▒▒▒▒▓▒  ▒▒▒▒▒▒▓▓▒▒▒▒   ▒▒▒▒▒▒▒▓▓▓▒ ▒▓▓▓▒▓▓▒▒▒ ");
		Console.WriteLine("  ▒▓▓▒ ▒▓▓    ▓▒  ▒▒   ▒▒▓▒                 ▓▒█▓▒   ");
		Console.WriteLine("   ▒▒▒         ▒▒ ▒▓ ▒                              ");
		Console.WriteLine();
		Console.WriteLine("                ▒                                   ");
		Console.WriteLine("               ▒████▓▒                              ");
		Console.WriteLine("           ▒▓▒ ▒██████████▓▒                        ");
		Console.WriteLine("           ▓█▒ ▒▓██████████████▒    ▒▒              ");
		Console.WriteLine("           ▓█      ▒▒▓████████▓▒    ▒               ");
		Console.WriteLine("          ▓██▒▒          ▒▓███▒    ▒▒               ");
		Console.WriteLine("         ▒▓██████▓▒▒                                ");
		Console.WriteLine("          ▒▓██████████▓▒               ▒▒▒  ▒▒▒     ");
		Console.WriteLine("              ▒▓██████████▓▓          ▒▒▓▓   ▓▒     ");
		Console.WriteLine("                  ▒▓████████▒       ▒▓▓▓██▓ ▓█▒     ");
		Console.WriteLine("                      ▒▓████▒  ▒▓▓▓▒   ▒▓████▓▒     ");
		Console.WriteLine("                         ▒▒▒ ▒▓▓██▓      ▒▒▒▒       ");
		Console.WriteLine("                                 ▒▒                 ");
		Console.WriteLine();
		Console.WriteLine("Type 'help' for commands.");
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
		if(command.CommandWord != "status" && command.CommandWord != "help" && command.CommandWord != "go" && command.CommandWord != "quit"){
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
		if(player.GetCurrentRoom().GetInventory().GetWeight() != 0){
			Console.WriteLine();
			Console.WriteLine("Items in room: ");
		}
		foreach(KeyValuePair<string, Item> item in player.GetCurrentRoom().GetInventory().GetItems()){
			Console.WriteLine(item.Key+" - "+item.Value.Description+" - "+item.Value.Weight*10);
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
		if(player.GetInventory().GetWeight() != 0){
			Console.WriteLine();
			Console.WriteLine("Items in inventory: ("+player.GetInventory().GetWeight()*10+"/"+player.GetInventory().GetMaxWeight()*10+")");
		}
		foreach(KeyValuePair<string, Item> item in player.GetInventory().GetItems()){
			Console.Write(item.Key+" - "+item.Value.Description+" - "+item.Value.Weight*10);
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
		if(player.GetCurrentRoom().GetInventory().Contains(itemName)){

			Item item = player.GetCurrentRoom().GetInventory().GetItem(itemName); 
			switch (player.GetInventory().Add(itemName, item))
			{
				case 0:
					player.GetCurrentRoom().GetInventory().Remove(itemName);
					Console.WriteLine(itemName+" was added to your inventory.");
					break;
				case 1:
					Console.WriteLine("There isn't enough space in your inventory.");
					break;
				case 2:
					Console.WriteLine("There already exists an item named "+itemName+" in your inventory.");
					break;
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
		if(player.GetInventory().Contains(itemName)){
			Item item = player.GetInventory().GetItem(itemName);
			switch (player.GetCurrentRoom().GetInventory().Add(itemName, item)){
				case 0:
					player.GetCurrentRoom().GetInventory().Add(itemName, item);
					player.GetInventory().Remove(itemName);
					Console.WriteLine(itemName+" was dropped from your inventory.");
					break;
				case 1:
					Console.WriteLine("There isn't enough space in the room");
					break;
				case 2:
					Console.WriteLine("There already exists an item named "+itemName+" in the room.");
					break;
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
			return;
		}

		string direction = command.SecondWord;

		// Try to go to the next room.
		Room nextRoom = player.GetCurrentRoom().GetExit(direction);
		if (nextRoom == null)
		{
			Console.WriteLine("There is no door to "+direction+".");
			return;
		}
		if (nextRoom.IsLocked()){
			Console.WriteLine("The door is "+nextRoom.GetLockDescription()+".");
		}
		else{
			player.SetCurrentRoom(nextRoom);
			Look();
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
			if(player.GetInventory().Contains(itemName)){
				if(player.GetInventory().GetItem(itemName) is Weapon){
					Weapon weapon = player.GetInventory().GetItem(itemName) as Weapon;
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
			if(player.GetInventory().Contains(item)){
				if(room.GetCode() == item){
					Console.WriteLine("The door is no longer "+room.GetLockDescription()+".");
					room.Unlock();
				}
				else{
					Console.WriteLine("The door stays "+room.GetLockDescription()+".");
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
				Console.WriteLine("The door is no longer locked.");
				room.Unlock();
			}
			else{
				Console.WriteLine("The door stays locked.");
			}
		}
		else{
			Console.WriteLine("The door requires an item, and not a code.");
		}
	}
}
