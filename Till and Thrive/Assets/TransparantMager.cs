using UnityEngine;

public class RemoveBlackBackground : MonoBehaviour
{
    public Texture2D originalTexture;

    void Start()
    {
        Texture2D newTexture = new Texture2D(originalTexture.width, originalTexture.height);
        Color[] pixels = originalTexture.GetPixels();

        for (int i = 0; i < pixels.Length; i++)
        {
            if (pixels[i] == Color.black)
            {
                pixels[i] = new Color(0, 0, 0, 0); // Set to transparent
            }
        }

        newTexture.SetPixels(pixels);
        newTexture.Apply();

        GetComponent<SpriteRenderer>().sprite = Sprite.Create(newTexture, new Rect(0, 0, newTexture.width, newTexture.height), Vector2.one * 0.5f);
    }
}
