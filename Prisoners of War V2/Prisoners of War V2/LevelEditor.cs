using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Newtonsoft.Json;

namespace Prisoners_of_War_V2
{
    class LevelEditor
    {
        Rectangle[,] buttons = new Rectangle[9,16];
        int[,] buttonValues = new int[9, 16];
        //Rectangle levelName = new Rectangle(1200, 833, 250, 40);
        Rectangle saveButton = new Rectangle(740, 830, 120, 50);

        public LevelEditor()
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    buttons[i,j] = new Rectangle(100 + (j * 75) + (j * 10), 50 + (i * 75) + (i * 10), 75,75);
                    buttonValues[i, j] = 1;
                }
            }
        }

        public GameState CheckButtonsForClick(MouseState ms)
        {
            for(int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    if(ms.LeftButton == ButtonState.Pressed)
                    {
                        if (buttons[i,j].Contains(ms.Position))
                        {
                            buttonValues[i, j]++;
                            if (buttonValues[i, j] > 3) buttonValues[i, j] = 1;
                        }
                        if (saveButton.Contains(ms.Position))
                        {
                            StreamWriter saveWriter = new StreamWriter("level.json");
                            string data = JsonConvert.SerializeObject(buttonValues);
                            saveWriter.WriteLine(data);
                            saveWriter.Close();
                            return GameState.MENU;
                        }
                    }
                }
            }
            return GameState.LEVELEDITOR;
        }

        public void Draw(SpriteBatch sb, Texture2D texture, SpriteFont font)
        {
            sb.DrawString(font, "                          Level Editor\nWhite = Empty, Blue = Barricade, Red = Turret", new Vector2(550,-3),Color.Black);
            Color color = Color.White;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 16; j++)
                {
                    switch (buttonValues[i, j])
                    {
                        case 1:
                            color = Color.White;
                            break;
                        case 2:
                            color = Color.Blue;
                            break;
                        case 3:
                            color = Color.Red;
                            break;
                    }
                    sb.Draw(texture, buttons[i, j], color);
                }
            }
            //sb.DrawString(font, "Level Name:", new Vector2(1050, 840), Color.Black);
            //sb.Draw(texture, levelName, Color.White);
            sb.Draw(texture, saveButton, Color.Green);
            sb.DrawString(font, "Save Level", new Vector2(740, 840), Color.Black);
        }
    }
}
