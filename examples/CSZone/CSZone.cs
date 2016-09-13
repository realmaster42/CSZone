/*
   CSZone v1.1.2
 * realmaster42: Automatically adjust to latest color and size + ScreenMouseDown & ScreenMouseUp events
   
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
using System.Drawing.Imaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
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
        bool hasSetup = false;
        int x = 0,
            y = 0,
            width = 0,
            height = 0;
        Image draw = null;

        /// <summary>
        /// Initializes a new GameObject with the following parameters.
        /// </summary>
        /// <param name="game">The CSZone class object.</param>
        /// <param name="x">The X coordinate to spawn.</param>
        /// <param name="y">The Y coordinate to spawn.</param>
        /// <param name="Image">The image file name and format (example.jpg).</param>
        public GameObject(CSZone game, int x = 0, int y = 0, string Image = "")
        {
            this.image = Image;
            this.key = ("cszonegameobject" + game.GetObjectAmount().ToString());
            this.x = x;
            this.y = y;

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
            if (MouseClick != null)
                MouseClick(this, e);
        }
        void OnMouseDown(object sender, MouseEventArgs e)
        {
            if (MouseDown != null)
                MouseDown(this, e);
        }
        void OnMouseHover(object sender, EventArgs e)
        {
            if (MouseHover != null)
                MouseHover(this);
        }
        void OnMouseUp(object sender, EventArgs e)
        {
            if (MouseUp != null)
                MouseUp(this);
        }
        void OnMouseEnter(object sender, EventArgs e)
        {
            if (MouseEnter != null)
                MouseEnter(this);
        }
        void OnMouseLeave(object sender, EventArgs e)
        {
            if (MouseLeave != null)
                MouseLeave(this);
        }
        public delegate void ObjectMouseEventHandler(GameObject sender, MouseEventArgs e);
        public delegate void ObjectEventHandler(GameObject sender);

        public event ObjectMouseEventHandler MouseClick;
        public event ObjectMouseEventHandler MouseDown;
        public event ObjectEventHandler MouseHover;
        public event ObjectEventHandler MouseUp;
        public event ObjectEventHandler MouseEnter;
        public event ObjectEventHandler MouseLeave;

        /// <summary>
        /// Returns if the current object is up-to-date.
        /// </summary>
        /// <returns>If the current object is up-to-date.</returns>
        public bool Updated()
        {
            return this.hasSetup;
        }
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
                        hasSetup = false;
                    }
                }
        }
        /// <summary>
        /// Returns the current object's Image state.
        /// </summary>
        /// <returns>The Image file belonging to this object.</returns>
        public Image GetImage()
        {
            return this.draw;
        }
        /// <summary>
        /// Draws the current state of the object.
        /// </summary>
        /// <param name="game">The CSZone class for reference.</param>
        public void Draw(CSZone game)
        {
            if (this.image != "")
            {
                if (draw == null)
                {
                    draw = Image.FromFile(@Environment.CurrentDirectory + @"\Images\" + this.image);
                }
                else
                {
                    if (game.HasFocus())
                    {
                        Point focus = game.GetFocusPoint();
                        Point permanentfocus = game.GetFocusViewPoint();

                        if (this.GetKey() == game.GetFocus().GetKey())
                            game.Overlap(draw, permanentfocus.X, permanentfocus.Y, width, height);
                        else
                            game.Overlap(draw, (permanentfocus.X - (this.x - focus.X)), (permanentfocus.Y - (this.y - focus.Y)), this.width, this.height);
                    }
                    else
                        game.Overlap(draw, this.x, this.y, this.width, this.height);

                    if (!hasSetup)
                    {
                        hasSetup = true;

                        draw = Image.FromFile(@Environment.CurrentDirectory + @"\Images\" + this.image);
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
            game.OnDestroy(this, null);
        }
    }
    /// <summary>
    /// Direction Information stores where the specified object should go.
    /// </summary>
    public class DirectionInformation
    {
        int velocityX = 0;
        int velocityY = 0;
        Point target = new Point(0, 0);
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
        PictureBox drawingArea;
        int timer = 10,
            objsAll = 0;
        Size lastSize;
        List<GameObject> objs,
            objsToAdd;
        GameObject focusObj;
        Point focusSpot,
            focusPermanentSpot;
        bool focus = false;

        void onScreenClick(object sender, MouseEventArgs e)
        {
            if (ScreenClick != null)
                ScreenClick(sender, e);
        }
        void onScreenMouseDown(object sender, MouseEventArgs e)
        {
            if (ScreenMouseDown != null)
                ScreenMouseDown(sender, e);
        }
        void onScreenMouseUp(object sender, MouseEventArgs e)
        {
            if (ScreenMouseUp != null)
                ScreenMouseUp(sender, e);
        }

        public event MouseEventHandler ScreenClick;
        public event MouseEventHandler ScreenMouseDown;
        public event MouseEventHandler ScreenMouseUp;

        /// <summary>
        /// Generates the Game-Engine class to use anywhere.
        /// </summary>
        /// <param name="Handle">The Windows Form being used, wich can be used later.</param>
        public CSZone(Form Handle = null)
        {
            this.template = Handle;

            objs = new List<GameObject>() { };
            objsToAdd = new List<GameObject>() { };
            focusObj = null;
            focusPermanentSpot = new Point(-999999, -999999);
            focusSpot = new Point(-999999, -999999);
            if (Handle != null)
            {
                this.drawingArea = new PictureBox();
                drawingArea.Name = "cszonedesigningbot";
                drawingArea.Size = new Size(Handle.Size.Width, Handle.Size.Height);
                drawingArea.Location = new Point(0, 0);
                drawingArea.MouseDown += new MouseEventHandler(onScreenMouseDown);
                drawingArea.MouseUp += new MouseEventHandler(onScreenMouseUp);
                drawingArea.MouseClick += new MouseEventHandler(onScreenClick);
                lastSize = new Size(Handle.Size.Width, Handle.Size.Height);
                Handle.Controls.Add(drawingArea);
                Handle.Refresh();
            }
        }
        /// <summary>
        /// Adds the specified object to the game screen.
        /// </summary>
        /// <param name="obj">The object to add.</param>
        public void AddObject(GameObject obj)
        {
            this.objsAll++;
            this.objsToAdd.Add(obj);
        }
        /// <summary>
        /// Overlaps two images. (PS: Thanks to Sirjosh3917 for this function)
        /// </summary>
        /// <param name="overlap">The image to overlap on the current image.</param>
        /// <returns></returns>
        public void Overlap(Image overlap, int x, int y, int width, int height)
        {
            if (drawingArea != null)
            {
                if (drawingArea.Image != null)
                {
                    Image currentImg = drawingArea.Image;
                    using (Graphics g = Graphics.FromImage(currentImg))
                    {
                        g.DrawImageUnscaled(overlap, x, y, width, height);
                        drawingArea.Invoke((MethodInvoker)(() => drawingArea.Image = currentImg));
                    }
                }
                else
                {
                    Bitmap empty = new Bitmap(this.template.Width, this.template.Height);
                    using (Graphics grp = Graphics.FromImage(empty))
                    {
                        System.Drawing.SolidBrush brush = new System.Drawing.SolidBrush(this.template.BackColor);
                        grp.FillRectangle(brush, 0, 0, this.template.Width, this.template.Height);

                        using (Graphics g = Graphics.FromImage(empty))
                        {
                            g.DrawImageUnscaled(overlap, x, y, width, height);

                            drawingArea.Invoke((MethodInvoker)(() => drawingArea.Image = empty));
                        }
                    }
                }
            }
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
                    if (lastSize.Width != this.template.Size.Width || lastSize.Height != this.template.Size.Height)
                    {
                        lastSize = new Size(this.template.Size.Width, this.template.Size.Height);
                        drawingArea.Invoke((MethodInvoker)(() => drawingArea.Size = new Size(this.template.Size.Width, this.template.Size.Height)));
                        drawingArea.Invoke((MethodInvoker)(() => drawingArea.Location = new Point(0, 0)));
                        drawingArea.Invoke((MethodInvoker)(() => drawingArea.Refresh()));
                    }

                    drawingArea.Invoke((MethodInvoker)(() => drawingArea.Image = null));

                    for (int i = 0; i < objs.Count; i++)
                    {
                        GameObject obj = objs[i];

                        obj.Draw(this);
                    }
                    int toRem = 0;
                    for (int x = 0; x < objsToAdd.Count; x++)
                    {
                        objs.Add(objsToAdd[x]);
                        toRem++;
                    }
                    if (toRem > 0)
                    {
                        for (int r = 0; r < toRem; r++)
                            objsToAdd.RemoveAt(0);
                    }

                    drawingArea.Invoke((MethodInvoker)(() => drawingArea.Refresh()));
                    this.template.Invoke((MethodInvoker)(() => this.template.Focus()));
                }
                catch (Exception x)
                {
                    Console.WriteLine("{Unexpected error occurred : " + x.Message + "}");
                }
            }
        }
        /// <summary>
        /// Sets the focus on the specified GameObject.
        /// </summary>
        /// <param name="obj">The GameObject to focus on.</param>
        /// <param name="viewPoint">The location where the object should be visible.</param>
        public void Focus(GameObject obj, Point viewPoint)
        {
            focus = true;
            focusPermanentSpot = viewPoint;
            focusObj = obj;
        }
        /// <summary>
        /// Sets the focus on the specified GameObject.
        /// </summary>
        /// <param name="obj">The GameObject to focus on.</param>
        /// <param name="x">The X coordinate where the focus is visible.</param>
        /// <param name="y">The Y coordinate where the focus is visible.</param>
        public void Focus(GameObject obj, int x, int y)
        {
            focus = true;
            focusPermanentSpot = new Point(x, y);
            focusObj = obj;
        }
        /// <summary>
        /// Sets the focus on the specified location.
        /// </summary>
        /// <param name="location">The location to focus on.</param>
        /// <param name="viewPoint">The location where the object should be visible.</param>
        public void Focus(Point location, Point viewPoint)
        {
            focus = true;
            focusPermanentSpot = viewPoint;
            focusObj = null;
            focusSpot = location;
        }
        /// <summary>
        /// Sets the focus on the specified location.
        /// </summary>
        /// <param name="location">The location to focus on.</param>
        /// <param name="x">The X coordinate where the focus is visible.</param>
        /// <param name="y">The Y coordinate where the focus is visible.</param>
        public void Focus(Point location, int x, int y)
        {
            focus = true;
            focusPermanentSpot = new Point(x, y);
            focusObj = null;
            focusSpot = location;
        }
        /// <summary>
        /// Sets the focus on the specified coordinates.
        /// </summary>
        /// <param name="x">The X coordinate to focus on.</param>
        /// <param name="y">The Y coordinate to focus on.</param>
        /// <param name="viewPoint">The location where the object should be visible.</param>
        public void Focus(int x, int y, Point viewPoint)
        {
            focus = true;
            focusPermanentSpot = viewPoint;
            focusObj = null;
            focusSpot = new Point(x, y);
        }
        /// <summary>
        /// Sets the focus on the specified coordinates.
        /// </summary>
        /// <param name="x">The X coordinate to focus on.</param>
        /// <param name="y">The Y coordinate to focus on.</param>
        /// <param name="x_">The X coordinate where the focus is visible.</param>
        /// <param name="y_">The Y coordinate where the focus is visible.</param>
        public void Focus(int x, int y, int _x, int _y)
        {
            focus = true;
            focusPermanentSpot = new Point(_x, _y);
            focusObj = null;
            focusSpot = new Point(x, y);
        }
        /// <summary>
        /// Loses focus on the current focused object/spot.
        /// </summary>
        public void LoseFocus()
        {
            focus = false;
            focusObj = null;
            focusPermanentSpot = new Point(-999999, -999999);
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
        /// Returns the current focusing view spot's location.
        /// </summary>
        /// <returns>The focus view spot's location.</returns>
        public Point GetFocusViewPoint()
        {
            return focusPermanentSpot;
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
            return this.focus;
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
        /// <param name="extras">Lists of GameObjects to search for the object and destroy.</param>
        public void OnDestroy(GameObject obj, params List<GameObject>[] extras)
        {
            if (extras != null)
            {
                if (extras.Length > 0)
                {
                    for (int i = 0; i < extras.Length; i++)
                    {
                        int remInde = -1;

                        if (extras[i].Count > 0)
                        {
                            for (int x = 0; x < extras[i].Count; x++)
                                if (extras[i][x].GetKey() == obj.GetKey())
                                    remInde = x;
                        }

                        if (remInde > -1)
                            extras[i].RemoveAt(remInde);
                    }
                }
            }

            int remInd = -1;

            if (objs.Count > 0)
            {
                for (int objIndex = 0; objIndex < objs.Count; objIndex++)
                    if (objs[objIndex].GetKey() == obj.GetKey())
                        remInd = objIndex;
            }

            if (remInd > -1)
            {
                if (focusObj != null)
                    if (objs[remInd].GetKey() == focusObj.GetKey())
                        focusObj = null;

                objs.RemoveAt(remInd);
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
            /*int sizeX1 = objectsize1.Width;
            int sizeY1 = objectsize1.Height;
            int sizeX2 = objectsize2.Width;
            int sizeY2 = objectsize2.Height;
            int posX1 = objectpoint1.X;
            int posY1 = objectpoint1.Y;
            int posX2 = objectpoint2.X;
            int posY2 = objectpoint2.Y;*/
            int min_a = objectpoint1.X + objectpoint1.Y;
            int max_a = min_a + (objectsize1.Width + objectsize1.Height);
            int min_b = objectpoint2.X + objectpoint2.Y;
            int max_b = min_b + (objectsize2.Width + objectsize2.Height);

            /*if ((sizeX1 == sizeX2) && (sizeY1 == sizeY2))
                return !((posY1 > posY2 && posX1 > posX2) || (posY1 < posY2 && posX1 > posX2) || (posY1 < posY2 && posX1 > posX2) || (posY1 < posY2 && posX1 < posX2));

            return false;*/

            return (min_a <= max_b && max_a >= min_b);
        }
    }
}