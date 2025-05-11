# AudioScene

## Overview

This repository contains an immersive forest environment with comprehensive audio implementation. The scene features a First Person Controller allowing users to explore and interact with the audio landscape.

## Features

- **3D and 2D Audio Sources**: Various audio elements throughout the environment using both spatial and non-spatial audio techniques
- **Audio Mixer Integration**: Organized sound management using multiple mixer groups
- **Audio Snapshots**: Dynamic audio filtering based on game state
- **Keyframe Animation-Driven Audio**: Special effects synchronized with animations
- **Dynamic Footsteps**: Surface-specific footstep sounds for enhanced realism
- **Rich Ambient Soundscape**: Multiple environmental audio sources creating an immersive forest experience

## Audio Implementation

### Audio Mixer Structure

The audio system is categorized into different mixer groups:
- Master
- Ambience
- Music
- Player

### Surface-Based Footsteps

The player controller includes 4 distinct footstep sounds for different terrain types:
- Dirt/Earth
- Grass
- Water
- Stone

### Ambient Sound Sources

The environment features multiple ambient sound elements:
- Wind through trees
- Flowing river
- Bird calls
- Frog croaking
- Other forest ambient sounds

### Snapshot System

The scene includes audio snapshots for different game states:
- **Paused**: Applies a lowpass filter to create a muffled effect when the game is paused
- **Unpaused**: Returns to the default audio mix during active gameplay

### Keyframe Based Effects

- **Animated Radio**: Features a bass beat sound that plays in sync with the radio's jumping animation using keyframe events
