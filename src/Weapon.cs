class Weapon : Item
{
	private int damage;
	private string atkmsg;
	private Random rng = new Random();
	public Weapon(int weight, string desc, int dmg, string attackmessage) : base(weight, desc)
	{
		damage = dmg;
		atkmsg = attackmessage;
	}
	public int GetDamage(){
		return rng.Next(damage/2, damage);
	}
	public string GetAttackMessage(){
		return atkmsg;
	}
	public int GetMaxDamage(){
		return damage;
	}
}
