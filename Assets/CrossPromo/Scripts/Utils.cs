using System.IO;
using UnityEngine;

namespace Devshifu.CrossPromo
{
    public class Utils
    {

        public static Texture2D LoadPNG(string filePath)
        {
            Texture2D tex = null;
            byte[] fileData;

            if (File.Exists(filePath))
            {
                fileData = File.ReadAllBytes(filePath);
                tex = new Texture2D(2, 2);
                tex.LoadImage(fileData);
            }
            return tex;
        }

        public static Sprite SpriteFromTex2d(Texture2D tex)
        {
            Texture2D old = tex;
            Texture2D left = new Texture2D((int)(old.width), old.height, old.format, false);
            Color[] colors = old.GetPixels(0, 0, (int)(old.width), old.height);
            left.SetPixels(colors);
            left.Apply();

            Sprite sprite = Sprite.Create(left, new Rect(0, 0, left.width, left.height), new Vector2(0.5f, 0.5f), 40);
            return sprite;
        }

        public static Sprite GetSprite(string path)
        {
            return SpriteFromTex2d(LoadPNG(path));
        }

        public static Sprite GetPromoSprite()
        {
            return GetSprite(GetImagePath());
        }

        public static string GetImagePath()
        {
            return Path.Combine(Application.persistentDataPath, "Cross_Promo_Image.png");
        }
    }
}