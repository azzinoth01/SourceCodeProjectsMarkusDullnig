using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// enum for Layer
/// </summary>
public enum Layer_enum
{
    @default = 0,
    TransparentFX = 1,
    Ignore_Raycast = 2,
    empty = 3,
    water = 4,
    ui = 5,
    player_bullets = 6,
    player = 7,
    enemy_bullets = 8,
    enemy = 9,
    UI_Overlay = 10,
    player_border = 11,
    enemy_border = 12,
    player_immunity = 13,
    bullet_border = 14,
    camera = 15,
}