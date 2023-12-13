# Jujutsu Kaisen Curse Hunter

[Markdown Cheatsheet](https://github.com/adam-p/markdown-here/wiki/Markdown-Here-Cheatsheet)

_REPLACE OR REMOVE EVERYTING BETWEEN "\_"_

### Student Info

-   Name: Dylan Clauson
-   Section: 02

## Simulation Design

Follows the characters of Jujutsu Kaisen as they hunt curses throughout the scene.
Curses will flock in a swarm (like flies) and will run from sorcerers when they're nearbly.
Sorcerers will chase curses that they are near.

### Controls

- When player hits space, more curses will spawn.

## Sorcerers
- Flock to classmates.
- Seek curses within range.
- Wander.

### Wander

**Objective:** When sorcerers are alone they will wander.

#### Steering Behaviors

- When sorceres are not flocked they will wander until they find their group again.
   
#### State Transistions

- when sorcerers are alone.
   
### Hunt

**Objective:** When sorcerers are together they will seek curses.

#### Steering Behaviors

- Seek curses.
- Flock to other classmates.
   
#### State Transistions

- When sorcerers are in a group.

## Curses

- Flock together.
- Avoid sorcerers when close.

### Passive

**Objective:** Wander and flock to other curses.

#### Steering Behaviors

- Wander.
- Flock to other curses.
   
#### State Transistions

- Out of range of sorcerers.
   
### Prey

**Objective:** Flee from sorcerers in range.

#### Steering Behaviors

- Flee from sorcerers in range.
   
#### State Transistions

- In range of sorcerers.

## Sources

- I made all of the assets.
- I used the slides and lectures to develop behavior.

## Make it Your Own

- Create my own assets.
- Use flock.

## Known Issues

- Sometimes the agents get overwhelmed and get a little stuck, most of the time though, theyll unstick and resume relevant behavior.

### Requirements not completed

- I belive I completed all requirements.

