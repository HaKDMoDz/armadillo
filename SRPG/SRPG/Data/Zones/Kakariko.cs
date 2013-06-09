using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SRPG.Scene.Overworld;
using Torch;
using System.Collections.Generic;
using Game = Torch.Game;

namespace SRPG.Data.Zones
{
    class Kakariko : Zone
    {
        public Kakariko()
        {

            Name = "Kakariko Village";
            Sandbag = Grid.FromBitmap("kakariko_sandbag");
            ImageLayers = new List<ImageObject>
                {
                    new ImageObject(Game.GetInstance().Content.Load<Texture2D>("kakariko")) {Z = -1}, 
                    new ImageObject(Game.GetInstance().Content.Load<Texture2D>("kakariko_arch")) {X = 2568, Y = 2784, Z = 2925}, 
                    new ImageObject(Game.GetInstance().Content.Load<Texture2D>("kakariko_house_1")) {X = 1728, Y = 336, Z = 587}
                };

            Objects = new List<InteractiveObject>();

            // mailbox
            var obj = new InteractiveObject { Location = new Rectangle(486, 1200, 36, 24) };
            obj.Interact += SimpleDialog("kakariko/town", "mailbox");
            Objects.Add(obj);

            // statue
            obj = new InteractiveObject { Location = new Rectangle(1512, 1224, 96, 96) };
            obj.Interact += SimpleDialog("kakariko/town", "statue");
            Objects.Add(obj);

            // cliff
            obj = new InteractiveObject { Location = new Rectangle(268, 445, 312 - 268, 5) };
            obj.Interact += SimpleDialog("kakariko/town", "cliff");
            Objects.Add(obj);
        }
    }
}
