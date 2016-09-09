/*
   CSZone v1.0.0
   
   CSZone created by realmaster42
   Open-source 2D light-weight C# .NET game-engine: https://github.com/realmaster42/CSZone
   It's free. If you pay for it you are getting scammed!
   
   Licensed under MIT License.
   For help on how to use visit the GitHub page.
*/


// Imports
using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace CSZone // Put here your namespace's name
{
    /// <summary>
    /// Handle Unknown exception occurrs when the Handler is null.
    /// </summary>
    public class CSZoneHandleUnknown : Exception // Handler is null when required
    {
        public CSZoneHandleUnknown() : base("Handler Unknown", new Exception("The code specified requires a Handle but the Handle is null.")) { }
    }
    /// <summary>
    /// Image Unknown exception occurrs when the image location sent does not exist.
    /// </summary>
    public class CSZoneImageUnknown : Exception // Image location was not found
    {
        public CSZoneImageUnknown(string img) : base("Image Unknown", new Exception("The image '" + img + "' was not found in the current directory.")) { }
    }
    /// <summary>
    /// Image Unknown exception occurrs when the image folder does not exist.
    /// </summary>
    public class CSZoneImageFolderUnknown : Exception // Image folder was not found
    {
        public CSZoneImageFolderUnknown() : base("Image Folder missing", new Exception("The image folder was not found.")) { }
    }
    /// <summary>
    /// Draws and creates an object to be used.
    /// </summary>
    public class GameObject
    {
        string image,
            key = "";
        bool invisible,
            hasSetup = false;
        Panel drawz;
        int x = 0,
            y = 0,
            width = 0,
            height = 0;

        /// <summary>
        /// Initializes a new GameObject with the following parameters.
        /// </summary>
        /// <param name="DrawZone">The Panel to draw the image in.</param>
        /// <param name="invisible">Wether or not the object should be visible.</param>
        /// <param name="x">The X coordinate to spawn.</param>
        /// <param name="y">The Y coordinate to spawn.</param>
        /// <param name="Image">The image file name and format (example.jpg).</param>
        public GameObject(Panel DrawZone = null, bool invisible = false, int x = 0, int y = 0, string Image = "")
        {
            this.image = Image;
            this.invisible = invisible;
            this.x = x;
            this.y = y;
            this.drawz = DrawZone;

            if (Image != "")
                if (!Directory.Exists(@Environment.CurrentDirectory + @"\Images"))
                    throw new CSZoneImageFolderUnknown();
                else
                {
                    if (!File.Exists(@Environment.CurrentDirectory + @"\Images\" + Image))
                        throw new CSZoneImageUnknown(Image);
                    else
                    {
                        System.Drawing.Image testImage = System.Drawing.Image.FromFile(@Environment.CurrentDirectory + @"\Images\" + Image);
                        this.width = testImage.Width;
                        this.height = testImage.Height;
                    }
                }
        }

        void OnMouseClick(object sender, MouseEventArgs e)
        {
            MouseClick(sender, e);
        }
        void OnMouseDown(object sender, MouseEventArgs e)
        {
            MouseDown(sender, e);
        }
        void OnMouseHover(object sender, EventArgs e)
        {
            MouseHover(sender, e);
        }
        void OnMouseUp(object sender, EventArgs e)
        {
            MouseUp(sender, e);
        }
        void OnMouseEnter(object sender, EventArgs e)
        {
            MouseEnter(sender, e);
        }
        void OnMouseLeave(object sender, EventArgs e)
        {
            MouseLeave(sender, e);
        }

        public event MouseEventHandler MouseClick;
        public event MouseEventHandler MouseDown;
        public event EventHandler MouseHover;
        public event EventHandler MouseUp;
        public event EventHandler MouseEnter;
        public event EventHandler MouseLeave;

        /// <summary>
        /// Returns the current X coordinate.
        /// </summary>
        /// <returns>The object's X coordinate.</returns>
        public int GetX()
        {
            return this.x;
        }
        /// <summary>
        /// Returns the current Y coordinate.
        /// </summary>
        /// <returns>The object's Y coordinate.</returns>
        public int GetY()
        {
            return this.y;
        }
        /// <summary>
        /// Goes to the specified location.
        /// </summary>
        /// <param name="location">The location to go.</param>
        public void MoveTo(Point location)
        {
            this.x = location.X;
            this.y = location.Y;
        }
        /// <summary>
        /// Goes to the specfied location.
        /// </summary>
        /// <param name="x">The X coordinate to go.</param>
        /// <param name="y">The Y coordinate to go.</param>
        public void MoveTo(int x, int y)
        {
            this.x = x;
            this.y = y;
        }
        /// <summary>
        /// Changes the current object's image.
        /// </summary>
        /// <param name="Image">The image file name and format (example.jpg).</param>
        public void Reshape(string Image = "")
        {
            this.image = Image;

            if (Image != "")
                if (!Directory.Exists(@Environment.CurrentDirectory + @"\Images"))
                    throw new CSZoneImageFolderUnknown();
                else
                {
                    if (!File.Exists(@Environment.CurrentDirectory + @"\Images\" + Image))
                        throw new CSZoneImageUnknown(Image);
                    else
                    {
                        System.Drawing.Image testImage = System.Drawing.Image.FromFile(@Environment.CurrentDirectory + @"\Images\" + Image);
                        this.width = testImage.Width;
                        this.height = testImage.Height;
                    }
                }
        }
        /// <summary>
        /// Draws the current state of the object.
        /// </summary>
        /// <param name="handle">The Windows Form to draw the object in.</param>
        /// <param name="game">The CSZone class for reference.</param>
        public void Draw(Form handle, CSZone game)
        {
            if (!invisible)
            {
                if (handle == null)
                    throw new CSZoneHandleUnknown();
                else
                {
                    if (drawz == null)
                    {
                        drawz = new Panel();
                        drawz.Name = ("gameobject" + game.GetObjectAmount().ToString());
                        this.key = ("gameobject" + game.GetObjectAmount().ToString());
                        handle.Controls.Add(drawz);
                        drawz.MouseClick += new MouseEventHandler(OnMouseClick);
                        drawz.MouseDown += new MouseEventHandler(OnMouseDown);
                        drawz.MouseHover += new EventHandler(OnMouseHover);
                        drawz.MouseUp += new MouseEventHandler(OnMouseUp);
                        drawz.MouseEnter += new EventHandler(OnMouseEnter);
                        drawz.MouseLeave += new EventHandler(OnMouseLeave);
                    }
                    else
                    {
                        if (game.HasFocus())
                            drawz.Location = new Point((this.x - game.GetFocusPoint().X), (this.y - game.GetFocusPoint().Y));
                        else
                            drawz.Location = new Point(this.x, this.y);

                        if (!hasSetup)
                        {
                            hasSetup = true;

                            drawz.Size = new Size(this.width, this.height);
                            drawz.BackgroundImage = Image.FromFile(@Environment.CurrentDirectory + @"\Images\" + this.image);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Returns the object's name.
        /// </summary>
        /// <returns>The object's control name.</returns>
        public string GetKey()
        {
            return this.key;
        }
        /// <summary>
        /// Removes the current object forever.
        /// <param name="game">The CSZone class for object reference.</param>
        /// </summary>
        public void Destroy(CSZone game)
        {
            game.OnDestroy(this);
        }
    }
    /// <summary>
    /// Direction Information stores where the specified object should go.
    /// </summary>
    public class DirectionInformation
    {
        int velocityX = 0;
        int velocityY = 0;
        Point target = new Point(0,0);
        GameObject item;

        /// <summary>
        /// Creates a default DirectionInformation class using specified variables.
        /// </summary>
        /// <param name="targetPosition">The location to point at.</param>
        /// <param name="obj">The graphical GameObject.</param>
        /// <param name="game">The CSZone class object for reference.</param>
        public DirectionInformation(Point targetPosition, GameObject obj, CSZone game)
        {
            int delay = game.GetTimerTickDelay();
        }
    }
    /// <summary>
    /// CSZone developed by realmaster42 and open source. | C# Game-Engine.
    /// </summary>
    public class CSZone
    {
        Form template;
        int timer = 10,
            objsAll = 0;
        List<GameObject> objs;
        GameObject focusObj;
        Point focusSpot;

        /// <summary>
        /// Generates the Game-Engine class to use anywhere.
        /// </summary>
        /// <param name="Handle">The Windows Form being used, wich can be used later.</param>
        public CSZone(Form Handle = null)
        {
            this.template = Handle;
            objs = new List<GameObject>() { };
            focusObj = null;
            focusSpot = new Point(-999999,-999999);
        }
        /// <summary>
        /// Adds the specified object to the game screen.
        /// </summary>
        /// <param name="obj">The object to add.</param>
        public void AddObject(GameObject obj)
        {
            this.objsAll++;
            this.objs.Add(obj);
        }
        /// <summary>
        /// Refreshes all game objects.
        /// </summary>
        public void Draw()
        {
            if (this.template != null)
            {
                try
                {
                    foreach (GameObject obj in objs)
                        obj.Draw(this.template, this);
                }
                catch (Exception x)
                {
                    Console.WriteLine("{Object removed/added while drawing. Will load every object next tick : " + x.Message + "}");
                }

                this.template.Refresh();
                this.template.Focus();
            }
        }
        /// <summary>
        /// Sets the focus on the specified GameObject.
        /// </summary>
        /// <param name="obj">The GameObject to focus on.</param>
        public void Focus(GameObject obj)
        {
            focusObj = obj;
        }
        /// <summary>
        /// Sets the focus on the specified location.
        /// </summary>
        /// <param name="location">The location to focus on.</param>
        public void Focus(Point location)
        {
            focusObj = null;
            focusSpot = location;
        }
        /// <summary>
        /// Sets the focus on the specified coordinates.
        /// </summary>
        /// <param name="x">The X coordinate to focus on.</param>
        /// <param name="y">The Y coordinate to focus on.</param>
        public void Focus(int x, int y)
        {
            focusObj = null;
            focusSpot = new Point(x, y);
        }
        /// <summary>
        /// Loses focus on the current focused object/spot.
        /// </summary>
        public void LoseFocus()
        {
            focusObj = null;
            focusSpot = new Point(-999999, -999999);
        }
        /// <summary>
        /// Returns the current focusing spot's location.
        /// </summary>
        /// <returns>The focus spot's location.</returns>
        public Point GetFocusPoint()
        {
            if (focusObj != null)
                focusSpot = new Point(focusObj.GetX(), focusObj.GetY());

            return focusSpot;
        }
        /// <summary>
        /// Returns the current focusing object.
        /// </summary>
        /// <returns>The current focus object.</returns>
        public GameObject GetFocus()
        {
            return focusObj;
        }
        /// <summary>
        /// Returns if focus is currently enabled or not.
        /// </summary>
        /// <returns>If focus is enabled or not.</returns>
        public bool HasFocus()
        {
            return (!(this.focusSpot.X == -999999 && this.focusSpot.Y == -999999));
        }
        /// <summary>
        /// Returns the amount of objects already created.
        /// </summary>
        /// <returns>The amount of objects already created.</returns>
        public int GetObjectAmount()
        {
            return this.objsAll;
        }
        /// <summary>
        /// Changes the amount of milliseconds between next frame.
        /// </summary>
        /// <param name="tickDelay">The amount of milliseconds between next frame.</param>
        public void SetTimerTickDelay(int tickDelay = 10)
        {
            this.timer = tickDelay;
        }
        /// <summary>
        /// How many milliseconds between each frame.
        /// </summary>
        /// <returns>The amount of milliseconds between each frame.</returns>
        public int GetTimerTickDelay()
        {
            return this.timer;
        }
        /// <summary>
        /// Destroys the specified GameObject.
        /// </summary>
        /// <param name="obj">The GameObject to destroy.</param>
        public void OnDestroy(GameObject obj)
        {
            int remInd = -1;

            if (objs.Count > 0)
            {
                for (int objIndex = 0; objIndex < objs.Count; objIndex++)
                    if (objs[objIndex] == obj)
                        remInd = objIndex;
            }

            if (remInd > -1)
            {
                if (focusObj != null)
                    if (objs[remInd] == focusObj)
                        focusObj = null;

                objs.RemoveAt(remInd);
            }

            if (this.template != null)
                if (this.template.Controls.Count > 0)
                {
                    if (obj.GetKey() != "")
                        if (this.template.Controls.Find(obj.GetKey(), true).Length > 0)
                            this.template.Controls.RemoveByKey(obj.GetKey());
                }
        }
        /*/// <summary>
        /// Returns if a rectangle is touching another rectangle.
        /// </summary>
        /// <param name="sizex1">The first object's width.</param>
        /// <param name="sizey1">The first object's height.</param>
        /// <param name="sizex2">The second object's width.</param>
        /// <param name="sizey2">The second object's height.</param>
        /// <param name="x1">The X coordinate of the first object.</param>
        /// <param name="y1">The Y coordinate of the first object.</param>
        /// <param name="x2">The X coordinate of the second object.</param>
        /// <param name="y2">The Y coordinate of the second object.</param>
        /// <returns></returns>
        public bool Colliding(int sizex1, int sizey1, int sizex2, int sizey2, int x1, int y1, int x2, int y2)
        {
            return ((y1 == y2)
        }*/
        /// <summary>
        /// Returns if a rectangle is touching another rectangle.
        /// </summary>
        /// <param name="objectsize1">The first object's size.</param>
        /// <param name="objectsize2">The second object's size.</param>
        /// <param name="objectpoint1">The first object's location.</param>
        /// <param name="objectpoint2">The second object's location.</param>
        /// <returns></returns>
        public bool Colliding(Size objectsize1, Size objectsize2, Point objectpoint1, Point objectpoint2)
        {
            int sizeX1 = objectsize1.Width;
            int sizeY1 = objectsize1.Height;
            int sizeX2 = objectsize2.Width;
            int sizeY2 = objectsize2.Height;
            int posX1 = objectpoint1.X;
            int posY1 = objectpoint1.Y;
            int posX2 = objectpoint2.X;
            int posY2 = objectpoint2.Y;

            if ((sizeX1 == sizeX2) && (sizeY1 == sizeY2))
                return !((posY1 > posY2 && posX1 > posX2) || (posY1 < posY2 && posX1 > posX2) || (posY1 < posY2 && posX1 > posX2) || (posY1 < posY2 && posX1 < posX2));

            return false;
        }
    }
}
