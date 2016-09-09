using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSZone
{
    public partial class Form1 : Form
    {
        public CSZone cszone;
        string keys = "";
        GameObject character;

        public Form1()
        {
            InitializeComponent();

            // CODE
            cszone = new CSZone(this); // Create game engine to current form
            character = new GameObject(null, false, 0, 0, "test.png"); // Create object
            cszone.AddObject(character); // Add object to game engine
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            string key = e.KeyData.ToString().ToLower(); // Start move
            if (key == "a" || key == "d" || key == "left" || key == "right")
                if (!keys.Contains(key))
                    keys += key;
        }
        
        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            string key = e.KeyData.ToString().ToLower(); // Start move
            if (key == "a" || key == "d" || key == "left" || key == "right")
                if (keys.Contains(key))
                    keys = keys.Remove(keys.IndexOf(key), key.Length);
        }

        private void Move_Tick(object sender, EventArgs e)
        {
            if (character.GetY() < 260)
                character.MoveTo(character.GetX(), character.GetY() + 2);

            if (keys.Contains("left") || keys.Contains("a"))
                character.MoveTo(character.GetX()-1, character.GetY());

            if (keys.Contains("right") || keys.Contains("d"))
                character.MoveTo(character.GetX() + 1, character.GetY());

            cszone.Draw();
        }
    }
}
