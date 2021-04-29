## Table of Contents
	
	1. Team Members
	2. Program Controls
	3. Tool Documentation
	4. Description of Bugs
	5. Future Implementation Plans
	

## Team members - Covid Crew
	
	- Brian Sharp
	- Benjamin Nagel
	- Matthew Anakin Crabtree
	- Jared Lawson
	- Christopher Toth
	- Michael Frank
	
## Program Controls

    Program Controls Guide
    
    Available Controllers
        Keyboard
        Mouse
        Gamepad
    
    Command Name
    
        Controll Keyboard
        
        Controll Gamepad
        
        Controll Mouse
	
    **Singleplayer Commands**
    
	    Next Room Command

		F2

		Right Trigger

	    Previous Room Command

		F1

		Left Trigger

	    Quit Command

		Q, Escape
		
		Back

	    Reset Command

		R 
		Y

	    Toggle Hitboxes

		F3
		
		Right Stick (Press)

	    Volume Down

		[
	    Volume Up

		]

	    Mute 

		P

	    Pause

		N
		
		Start

	    Move Player Up Command

		W, Arrow Key Up

		Directionalpad Up

	    Move Player Down Command 

		S, Arrow Key Down

		Directionalpad Down

	    Move Plater Left Command 

		A, Arrow Key Left

		Directionalpad Left

	    Move Player Right Command

		D, Arrow Key Right

		Directionalpad Right

	    Use Player Slot 1 Command 

		J
		
		Left Click

		A

	    Use Player Slot 2 Command

		K
		
		Right Click

		B

	    Open Inventory COmmand

		Enter

	    Move Inventory Selector Up Command

		W, Up Arrow
		
		DPadUp

	    Move Inventory Selector Down Command

		S, Down Arrow
		
		DPadDown

	    Move Inventory Selector Left Command

		A, Left Arrow
		
		DPadLeft

	    Move Inventory Selector Right Command

		D, Right Arrow
		
		DPadRight
		
    **Multiplayer Commands**
    
	    Next Room Command

		F2

		Right Trigger

	    Previous Room Command

		F1

		Left Trigger

	    Quit Command

		Q, Escape
		
		Back

	    Reset Command

		R 
		Y

	    Toggle Hitboxes

		F3
		
		Right Stick (Press)

	    Volume Down

		[
	    Volume Up

		]

	    Mute 

		P

	    Pause

		N
		
		Start

	    Move Player 1 Up Command

		W
		
	    Move Player 2 Up Command

		Arrow Key Up

		Directionalpad Up


	    Move Player 1 Down Command 

		S
		
	    Move Player 2 Down Command 

		Arrow Key Down

		Directionalpad Down

	    Move Player 1 Left Command 

		A
		
	    Move Player 2 Left Command 

		Arrow Key Left

		Directionalpad Left

	    Move Player 1 Right Command

		D
		
	    Move Player 2 Right Command

		Arrow Key Right

		Directionalpad Right

	    Use Player 1 Slot 1 Command 

		J
		
	    Use Player 2 Slot 1 Command 
		
		Left Click

		A

	    Use Player 1 Slot 2 Command

		K
		
	    Use Player 2 Slot 2 Command
		
		Right Click

		B

	    Open Player 1 Inventory Command

		Enter
		
	    Open Player 2 Inventory Command

		Shift

	    Move Player 1 Inventory Selector Up Command

		W
		
	    Move Player 2 Inventory Selector Up Command

		Up Arrow
		
		DPadUp

	    Move Player 1 Inventory Selector Down Command

		S 
		
	    Move Player 2 Inventory Selector Down Command

		Down Arrow
		
		DPadDown

	    Move Player 1 Inventory Selector Left Command

		A
		
	    Move Player 2 Inventory Selector Left Command

		Left Arrow
		
		DPadLeft

	    Move Player 1 Inventory Selector Right Command

		D
		
	    Move Player 2 Inventory Selector Right Command

		Right Arrow
		
		DPadRight
	
	
## Tool Documentation

	Visual Studio 2019
	
		IDE chosen for this project class using C#. As well as monogame, and Xna framework.
	
	Code Metrics
	
		Used the visual studios code metrics in order to give us an idea of the maintainability
		of our code base. Using the lines of executable code, inheritance depth, and class coupling
		we were able to see where changes might need to be made.
	
	FxCop - Microsoft Code Analysis 2019 extension
		
		This is the tool we used to analyze our code for warnings, and errors. Using this tool let 
		us be aware of code that needs adjustment, or that we need to document areas not yet 
		implemented. 
		
	Trello
	
		This is the tool we used for task tracking and task review. Each card on our board allowed 
		us to assign members to a task in addition to a task reviewer for each one. The tool can
		build a burndown chart based on our estimated and actual completion of cards. Below is a 
		screen shot.
	
	SpriteCow
		This website was used for getting the sprite dimensions on sprite sheets.
	
## Description of Bugs

	-Rarely Link can be knocked outside of the map
	
	-If player1 opens a menu, and player2 attempts to open the menu. The game will bug out since our controller
		does not right now stop the other player from opening/closing the menu.
	
	-Window doesn't resize to match for single player, screen size is set for multiplayer.
	
	-This means there is a portion of the bottom screen that is visible, not a big issue, but with more time 
		easily fixed. 
	
	-Sometimes the menu selector is hidden when attempting to select the bow, but it is a draw/layering issue 
		and all items in menu selection are selectable.

## Future Implementaton Plans

	Add more levels.
	Add more bosses to existing levels.
	Continue removing remaining magic numbers.
	Continue reducing file size.
	
	
