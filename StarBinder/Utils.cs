using System;
using CocosSharp;

namespace StarBinder
{
	public static class Utils
	{
		public static CCRect GetRect(float x, float y, float width, float height)
		{
			return GameManager.Instance.Calculator.RectRelToAbsByMinSize (x, y, width, height).Convert (); 
		}

		public static CCColor4B GetColor(string hex)
		{
			// rgba
			int a = Int32.Parse(hex.Substring(1,2), System.Globalization.NumberStyles.HexNumber);
			int r = Int32.Parse(hex.Substring(3,2), System.Globalization.NumberStyles.HexNumber);
			int g = Int32.Parse(hex.Substring(5,2), System.Globalization.NumberStyles.HexNumber);
			int b = Int32.Parse(hex.Substring(7,2), System.Globalization.NumberStyles.HexNumber);
			return new CCColor4B(r,g,b,a);
		}
	}
}

