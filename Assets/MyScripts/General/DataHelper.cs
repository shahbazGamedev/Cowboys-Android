using UnityEngine;
using System.Collections;

public class DataHelper{
	//pistol, gun, bottle, bomb, bow, dynamit?

    public static Vector3 HEALTH_BAR_DEAD = new Vector3(0, -500, 0);

    public static int WEAPONS_NUMBER = 10;
    public static int KID_WEAPON_INDEX = 10;

    public static int DEFAULT_SELECTED_WEAPON_INDEX = 2;

    public static float MOLOTOVE_DMG_RATE = .3f;
    public static float MOLOTOVE_DMG_TIME = 5;

    public static float FREEZE_TIME = 3;

    public static int ELECTRIC_BOMB_INDEX = 8;
    public static int SHIELD_INDEX = 9;

    // --------------------------------------------------     0   ,   1   ,  2    ,   3    ,   4  ,    5      ,     6     ,     7    ,     8         ,   9
    public static string[] WEAPONS_NAMES = new string[] { "pistol", "gun", "bottle", "bomb", "bow", "dynamite", "molotove", "iceBomb", "electricBomb", "shield" };

   // public static int[] ULOCKING_LEVEL =    new int[] { 1, 3, 1, 1, 6, 1, 1, 1, 1 };

    //temp
    public static int[] ULOCKING_LEVEL = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1,1 };

    public static int[] DEFAULT_OWNED  =    new int[] { 1, 0, 1, 1, 0, 1, 1, 1, 1, 1 };
    //temppp
    //public static int[] DEFAULT_OWNED = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
    //public static int[] DEFAULT_OWNED = new int[] { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 };

    public static int[] UNLOCK_PRICES = new int[] { 0, 7000, 0, 0, 12000, 0, 0, 0, 0, 0 };

    public static int[] WEAPONS_DEFAULT_BULLETS = new int[] {18, 0, 500, 0,  0, 0, 0, 0, 0, 0};
    //tempppp
    //public static int[] WEAPONS_DEFAULT_BULLETS = new int[] { 55, 55, 55, 55, 55, 55, 55, 55, 55, 66 };

	public static int [ , ] WeaponsPowerPerUpgrade = new int[ , ]{{25, 50, 75, 100, 125},
		{50, 	75, 	100, 	150, 	200},
		{25, 	50, 	75, 	75, 	75},
		{1000, 	1000, 	1000, 	1000, 	1000},
		{500, 	500, 	500, 	500, 	500},
		{500, 	500, 	500, 	500, 	500},
		{500, 	500, 	500, 	500, 	500},
		{0, 	0, 	0, 	0, 	0},
		{500, 	500, 	500, 	500, 	500},
        {500, 	500, 	500, 	500, 	500}};

	public static int [ , ] WeaponsFiresPerUpgrade = new int[ , ]{{1, 1, 1, 1, 1},
		{1, 1, 1, 1, 1},
		{1, 1, 1, 2, 3},
		{1, 1, 1, 1, 1},
		{1, 2, 3, 4, 5},
		{1, 1, 1, 1, 1},
		{1, 1, 1, 1, 1},
		{1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1},
		{1, 1, 1, 1, 1}};

	public static int [ , ] WeaponsPricesPerUpgrade = new int[ , ]{{1000, 3000, 5000, 7000},
		{1000, 3000, 5000, 7000},
		{1000, 3000, 5000, 7000},
		{0, 0, 0, 0},
		{1000, 3000, 5000, 7000},
		{0, 0, 0, 0},
		{1000, 3000, 5000, 7000},
		{1000, 3000, 5000, 7000},
        {1000, 3000, 5000, 7000},
		{1000, 3000, 5000, 7000}};

    public static int[] REVIVE_PRICES = new int[] {100, 500, 1000, 30000  };


    public static int REVIVE_COUNTER = 10;
    public static float WIN_DELAY = 4f;
    public static float DISABLE_COIN_DELAY =  6;
    public static int REVIVE_INGAME_PRICE = 200;
    public static float DISABLE_STAR_DELAY = 5;
    public static float DISABLE_HPGIFT_DELAY = 5;
    public static float HP_RESTORE_PERCENTAGE = 25;
    public static float HP_DROP_PROPABILITY = 25;
    public static float SHIELD_HEALTH = 100;
    public static float SHIELD_TIME = 10;
    public static int COINS_LOSE_POINTS = 5;

	
}
