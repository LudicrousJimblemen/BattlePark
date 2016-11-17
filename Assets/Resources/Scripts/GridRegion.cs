using System;

public struct GridRegion {
	public int X;
	public int Y;
	public int Width;
	public int Height;
	
	public int Owner;
	
	public GridRegion(int x, int y, int width, int height, int owner) {
		this.X = x;
		this.Y = y;
		this.Width = width;
		this.Height = height;
		this.Owner = owner;
	}
	
	public void SetOwner(int playerId) {
		Owner = playerId;
	}
}