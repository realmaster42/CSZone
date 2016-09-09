# CSZone
CSZone is a simple light-weight 2D C# .NET Game Engine for Windows that facilitates the game-working.
This Game Engine is for Windows Form Applications.

To add it to your code add a new class on the project called CSZone.cs, and then add the CSZone.cs code inside it.


For an instant search for help do CTRL+F and search for your issue.
For an instant search for a tutorial part do CTRL+F and search # Part [number].

The tutorial below is how to create games using CSZone.


# Part 1 - Creating
To get started, you will have to create the class inside your Windows Form.
For this add a variable below public Form1 : Form;
--------------------------
public Form1 : Form
{
	public CSZone cszone;
	[rest of code]
}
--------------------------

Then, you will want it to be created, so it is not null anymore.
You will have to look at how to create it...
--------------------------
new CSZone(Form handle)
--------------------------

This means you can use it on multiple forms.
Handle can be null, because it'll still have some usefull functions for you, but it won't draw or create anything that way.

Okay, so now that you know this, let's just build it:
--------------------------
public Form1()
{
	InitializeComponents();
	cszone = new CSZone(this);
}
--------------------------

Nice! You've got your first CSZone created.


# Part 2 - Beginning for objects
Now that you've created CSZone, you might want to start creating the objects.
But the thing is that CSZone doesn't bring all of it for you, you also have to code.
In this case, you only miss a folder.

For this, on the folder you are debugging it (if on visual studio use either Debug or Release folders, the one you are using),
add a new folder called "Images".
This folder will be where you store your images.


# Part 3 - Creating objects
The next step is creating objects.
Objects are in a class called GameObject.

To create one, you will need to take a look at how they are made:
--------------------------
GameObject test = new GameObject([Panel DrawZone = null], [bool invisible = false], [int x = 0], [int y = 0], [string Image = ""]);
--------------------------

Wow, so many optional parammeters!
Let's slow down and solve it one by one.

Being all optional, you can simply do test = new GameObject();
But there is a problem... Image = "", therefore it will not draw.

Now, DrawZone. DrawZone is a panel!?
Well yes, its the area that the figure is drawn.
By being null, it generates a new one, otherwhise you are overdrawing on a certain panel.

invisible, invisible is the parammeter that makes the object invisible.
If it is false, it will draw according to Image, otherwhise it won't render.

x is the X coordinate for it to appear once ready.
y is the Y coordinate for it to appear once ready.

Image is the file location, for example, test.png
However, you can use another folder inside Images!

For example, new GameObject(null, false, 0, 0, @"particles\water.png");


Now you've got your first GameObject done! Good job!


# Part 4 - Rendering objects
Oh... The hardest bit... Or is it?

Thanks to CSZone, much of the stuff are here for you!
However, it needs a small code from you to make it run properly.

You will have to add a timer to your form, enable it and change it's Interval to 10.
10 is the default rendering delay for CSZone, but if you want to change it, you can do...
--------------------------
cszone.SetTimerTickDelay(delay);
--------------------------

Then, foreach tick it does, you have to run the code:
--------------------------
cszone.Draw();
--------------------------

cszone.Draw() runs Draw() on each object added, rendering every object.
However, this won't draw anything yet... We haven't added the object to cszone yet!

For this, just do:
--------------------------
cszone.AddObject(test);
--------------------------


# Part 5 - Focusing
Focusing is a thing CSZone helps you a lot on.
Focusing makes 2D games easier to make a lot, especially if multiplayer.

What focus does is make a certain object or spot the camera area, being for example...
If X is focused, the area around it is visible, even if its higher/lower than the screen size.

This is awesome, because then you can do a max area of 1800x1800 while screen size 800x600 (example).

You can focus the object by doing:
--------------------------
cszone.Focus(test);
--------------------------

CSZone also has some functions to help you out on focusing matters, such as:
GetFocus(), GetFocusPoint(), HasFocus() and LoseFocus()


# Part 6 - Destroying objects
Now that you've got the object rendering done, you can destroy objects freely.
To destroy objects it is very simple, you only need to run Destroy() on objects.


Later on CSZone plans to have rotation and velocity for more game-working help. (For example, bullets)


Thanks for reading,
realmaster42
