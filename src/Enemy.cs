class Enemy
{
	private int damage;
	private string atkmsg;
	private string deathmsg;
	private int hp;
	private Random rng = new Random();
	public Enemy(int health,int dmg, string attackmsg, string dmsg){
		hp = health;
		damage = dmg;
		atkmsg = attackmsg;
		deathmsg = dmsg;
	}
	public bool IsAlive(){
		if(hp>0){
			return true;
		}
		else{
			return false;
		}
	}
	public void Damage(int amount){
		hp -= amount;
	}
	public int GetHp(){
		return hp;
	}
	public int GetDamage(){
		return rng.Next(damage/2, damage);
	}
	public string GetAttackMessage(){
		return atkmsg;
	}
	public string GetDeathMessage(){
		return deathmsg;
	}
}
