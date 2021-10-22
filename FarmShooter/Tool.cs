using System;
using SFML.Graphics;
using SFML.System;

namespace FarmShooter
{
    enum ToolType { Hoe, Axe, Pickaxe };

    class Tool : Drawable
    {
        ToolType Type;

        Sprite MainSprite;
    }
}
