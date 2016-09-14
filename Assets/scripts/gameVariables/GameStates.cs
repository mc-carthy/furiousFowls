﻿using UnityEngine;
using System.Collections;

public enum SlingshotState {
	Idle,
	UserPulling,
	BirdFlying
}

public enum GameState {
	Start,
	BirdMovingToSlingshot,
	Playing,
	Won,
	Lost
}

public enum BirdState {
	BeforeThrown,
	Thrown
}