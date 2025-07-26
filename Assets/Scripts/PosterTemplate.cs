using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PosterTemplate : MonoBehaviour
{
    [SerializeField] RawImage posterImage;
    [SerializeField] Toggle posterToggle;

    public Toggle PosterToggle => posterToggle;

    public int Id { get; set; }

    Texture2D posterTexture;
    public Texture2D PosterTexture
    {
        get => posterTexture;
        set
        {
            posterTexture = value;
            posterImage.texture = posterTexture;
        }
    }
}
