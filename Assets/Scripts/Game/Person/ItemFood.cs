public class ItemFood : Item {
	public int ServingSize { get; private set; }
		
	public ItemFood(int id, string languageId, Money defaultPrice, int servingSize) {
		this.Id = id;
		this.languageId = languageId;
		this.DefaultPrice = defaultPrice;
		this.ServingSize = servingSize;
	}
	
	public static readonly ItemFood Macaroni = new ItemFood(1, "macaroni", new Money(2, 50), 80);
	public static readonly ItemFood SaltLick = new ItemFood(2, "saltLick", new Money(2, 00), 45);
}