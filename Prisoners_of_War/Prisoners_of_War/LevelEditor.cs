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

namespace Prisoners_of_War
{
    class LevelEditor
    {
        // Attributes
        const int ROWS = 4;
        const int COLUMNS = 6;
        Rectangle[,] buttons = new Rectangle[ROWS,COLUMNS];
        int[,] buttonValues = new int[ROWS, COLUMNS];
        Rectangle saveButton = new Rectangle(870, 475, 140, 50);
        Rectangle cancelButton = new Rectangle(590, 475, 140, 50);
        Color titleColor = new Color(254, 202, 1);
        Color darkTitleColor = new Color(42, 40, 40);
        //Rectangle levelName = new Rectangle(1200, 833, 250, 40);

        // Constructor
        public LevelEditor()
        {
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    buttons[i,j] = new Rectangle(553 + (j * 75) + (j * 10), 100 + (i * 75) + (i * 10), 75,75);
                    buttonValues[i, j] = 1;
                }
            }
        }

        /// <summary>
        /// Checks if left mouse button is being pressed and returns a gamestate
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public GameState CheckButtonsForClick(MouseState ms)
        {
            if(ms.LeftButton == ButtonState.Pressed)
            {
                if (saveButton.Contains(ms.Position))
                {
                    StreamWriter saveWriter = new StreamWriter("level.json");
                    string data = JsonConvert.SerializeObject(buttonValues);
                    saveWriter.WriteLine(data);
                    saveWriter.Close();
                    return GameState.MENU;
                }
                else if (cancelButton.Contains(ms.Position))
                {
                    return GameState.MENU;
                }
                else
                {
                    for (int i = 0; i < ROWS; i++)
                    {
                        for (int j = 0; j < COLUMNS; j++)
                        {
                            if (buttons[i, j].Contains(ms.Position))
                            {
                                buttonValues[i, j]++;
                                if (buttonValues[i, j] > 3) buttonValues[i, j] = 1;
                            }
                        }
                    }
                }
            }
            return GameState.LEVELEDITOR;
        }

        /// <summary>
        /// Checks if 'C' or 'S' keys are being pressed and returns a gamestate
        /// </summary>
        /// <param name="ks"></param>
        /// <returns></returns>
        public GameState CheckButtonsForClick(KeyboardState ks)
        {
            if (ks.IsKeyDown(Keys.S))
            {
                StreamWriter saveWriter = new StreamWriter("level.json");
                string data = JsonConvert.SerializeObject(buttonValues);
                saveWriter.WriteLine(data);
                saveWriter.Close();
                return GameState.MENU;
            }
            if (ks.IsKeyDown(Keys.C))
            {
                return GameState.MENU;
            }
            return GameState.LEVELEDITOR;
        }

        // Draws icon textures over the boxes
        public void Draw(SpriteBatch sb, Texture2D backgroundTexture, Texture2D barricade, Texture2D turret, SpriteFont font)
        {
            sb.DrawString(font, "Level Editor", new Vector2(738, 11), darkTitleColor);
            sb.DrawString(font, "Level Editor", new Vector2(738, 10), titleColor);
            sb.DrawString(font, "White", new Vector2(550, 49), Color.White);
            sb.DrawString(font, " = Empty | ", new Vector2(550 + font.MeasureString("White").X, 49), Color.Black);
            sb.DrawString(font, "Blue", new Vector2(550 + font.MeasureString("White = Empty | ").X, 49), Color.Blue);
            sb.DrawString(font, " = Barricade | ", new Vector2(550 + font.MeasureString("White = Empty | Blue").X, 49), Color.Black);
            sb.DrawString(font, "Red", new Vector2(550 + font.MeasureString("White = Empty | Blue = Barricade | ").X, 49), Color.Red);
            sb.DrawString(font, " = Turret", new Vector2(550 + font.MeasureString("White = Empty | Blue = Barricade | Red").X, 49), Color.Black);


            Color color = Color.White;
            Texture2D icon = null;
            for (int i = 0; i < ROWS; i++)
            {
                for (int j = 0; j < COLUMNS; j++)
                {
                    switch (buttonValues[i, j])
                    {
                        case 1:
                            color = Color.White;
                            icon = null;
                            break;
                        case 2:
                            color = Color.Blue;
                            icon = barricade;
                            break;
                        case 3:
                            color = Color.Red;
                            icon = turret;
                            break;
                    }
                    sb.Draw(backgroundTexture, buttons[i, j], color);
                    if(icon != null)
                        sb.Draw(icon, buttons[i, j], Color.White);
                }
            }
            //sb.DrawString(font, "Level Name:", new Vector2(1050, 840), Color.Black);
            //sb.Draw(texture, levelName, Color.White);
            sb.Draw(backgroundTexture, saveButton, Color.Green);
            sb.Draw(backgroundTexture, cancelButton, Color.Red);
            sb.DrawString(font, "Save Level", new Vector2(870 + (font.MeasureString("Save Level").X/2 - 42), 485), Color.Black);
            sb.DrawString(font, "Cancel", new Vector2(590 + (font.MeasureString("Cancel").X / 2), 485), Color.Black);
        }
    }
}
