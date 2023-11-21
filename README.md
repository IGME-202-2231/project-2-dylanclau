# Jujutsu Kaisen Curse Hunter

[Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Here-Cheatsheet)

_REPLACE OR REMOVE EVERYTING BETWEEN "\_"_

### Student Info

-   Name: Dylan Clauson
-   Section: 02

## Simulation Design

Follows the characters of Jujutsu Kaisen as they hunt curses throughout the scene.
Weaker curses will spawn in at random intervals frequently, while stronger curses will spawn more sparsely.
The sorcerers will flock to classmates and will hunt weaker curses (weaker curses will flock together and flee from sorcerers).
Stronger curses will hunt sorcerers, who will flee unless they are grouped together. 
Characters will generate points depending on how many curses they defeat (weaker curses are worth less points while stronger curses are worth more).
Player can use points to increase character stats such as speed and strength.

### Controls

-   _List all of the actions the player can have in your simulation_
    -   _Include how to preform each action ( keyboard, mouse, UI Input )_
    -   _Include what impact an action has in the simulation ( if is could be unclear )_

## Sorcerers
- Flock to classmates
- Seek weak curses
- Flee from strong curses (unless they're in groups of 2+, then they'll seek stronger curses).
- Generate points as they defeat curses (less for weak curses, more for strong curses).

### Lone

**Objective:** When sorcerers are alone they will only seek weak curses and flee from strong curses.

#### Steering Behaviors

- Seek - weak curses
- Flee - strong curses
- Flock - classmates
- Obstacles - strong curses
- Seperation - other classmates
   
#### State Transistions

- when sorcerers are alone.
   
### Grouped

**Objective:** When sorcerers are together they will seek all curses regardless of strength.

#### Steering Behaviors

- Seek - all curses
- Flock - classmates
- Obstacles - none
- Seperation - other classmates
   
#### State Transistions

- when sorcerers are in groups of 2+.

## Strong Curses

- Avoid each other
- Seek lone sorcerers
- Flee from grouped sorcerers

### Predator

**Objective:** Hunt lone sorcerers.

#### Steering Behaviors

- avoid - other strong curses
- seek - lone sorcerers
  
- Obstacles - none
- Seperation - other curses
   
#### State Transistions

- out of range of grouped sorcerers.
   
### Prey

**Objective:** Flee from groups of sorcerers.

#### Steering Behaviors

- flee - grouped sorcerers
- Obstacles - grouped sorcerers
- Seperation - other curses
   
#### State Transistions

- in range of grouped sourcerers.

## Weak Curses

- right now they just flee from all sorcerers and flock to each other, basically an easy way to farm points.

## Sources

-   _List all project sources here –models, textures, sound clips, assets, etc._
-   _If an asset is from the Unity store, include a link to the page and the author’s name_

## Make it Your Own

- _List out what you added to your game to make it different for you_
- _If you will add more agents or states make sure to list here and add it to the documention above_
- _If you will add your own assets make sure to list it here and add it to the Sources section

- Create my own assets
- Use flock
- Have a simple third agent (weak curses).

## Known Issues

_List any errors, lack of error checking, or specific information that I need to know to run your program_

### Requirements not completed

_If you did not complete a project requirement, notate that here_

