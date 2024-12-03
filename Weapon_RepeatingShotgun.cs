//audio
datablock AudioProfile(RepeatingShotgunFireSound)
{
   filename    = "./Sounds/RepeatingShotgun_Fire.wav";
   description = AudioDefault3d;
   preload = true;
};

AddDamageType("RepeatingShotgun",   '<bitmap:add-ons/Weapon_Package_TTOverhaul_WWII/Icons/CI_RepeatingShotgun> %1 Jammed his gun and accidentally offed themselves',    '%2 quickly disposed <bitmap:add-ons/Weapon_Package_TTOverhaul_WWII/Icons/CI_RepeatingShotgun> %1\'s body with alot of pellets',0.75,1);
datablock ProjectileData(RepeatingShotgunProjectile : TTOBulletProjectile)
{
   directDamage        = 9; //14;
   directDamageType    = $DamageType::RepeatingShotgun;
   radiusDamageType    = $DamageType::RepeatingShotgun;


   impactImpulse         = 100;
   verticalImpulse     = 50;
   explosion           = gunExplosion;

   muzzleVelocity      = 120;

   gravityMod = 0.4;
};

//////////
// item //
//////////
datablock ItemData(RepeatingShotgunItem)
{
    category = "Weapon";  // Mission editor category
    className = "Weapon"; // For inventory system

     // Basic Item Properties
    shapeFile = "./Models/RepeatingShotgun.dts";
    rotate = false;
    mass = 1;
    density = 0.2;
    elasticity = 0.2;
    friction = 0.6;
    emap = true;

    //gui stuff
    uiName = "Repeating Shotgun";
    iconName = "./Icons/RepeatingShotgun";
    doColorShift = true;
    colorShiftColor = "0.6 0.6 0.6 1.000";

     // Dynamic properties defined by the scripts
    image = RepeatingShotgunImage;
    canDrop = true;

    TTO_ammoType = "shotgun";
    TTO_reloads = true;
    TTO_maxAmmo = 5;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(RepeatingShotgunImage)
{
   // Basic Item properties
   shapeFile = "./Models/RepeatingShotgun.dts";
   emap = true;

   // Specify mount point & offset for 3rd person, and eye offset
   // for first person rendering.
   mountPoint = 0;
   offset = "0 0 0";
   eyeOffset = 0; //"0.7 1.2 -0.5";
   rotation = eulerToMatrix( "0 0 0" );

   // When firing from a point offset from the eye, muzzle correction
   // will adjust the muzzle vector to point to the eye LOS point.
   // Since this weapon doesn't actually fire from the muzzle point,
   // we need to turn this off.  
   correctMuzzleVector = true;

   // Add the WeaponImage namespace as a parent, WeaponImage namespace
   // provides some hooks into the inventory system.
   className = "WeaponImage";

   // Projectile && Ammo.
   item = RepeatingShotgunItem;
   ammo = " ";
   projectile = RepeatingShotgunProjectile;
   projectileType = Projectile;

   casing = TwelveGaugeShellDebris;
   shellExitDir        = "1.0 0.1 1.0";
   shellExitOffset     = "0 0 0";
   shellExitVariance   = 10.0;  
   shellVelocity       = 5.0;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;
   minShotTime = 1000;

   doColorShift = true;
   colorShiftColor = RepeatingShotgunItem.colorShiftColor;

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]                    = "Activate";
	stateTimeoutValue[0]            = 0.128; //0.15
	stateTransitionOnTimeout[0]     = "DeathCheckA";
	stateSound[0]			  = weaponswitchSound;

	stateName[1]                    	= "Ready";
	stateTransitionOnTriggerDown[1] 	= "FireDelay";
	stateTransitionOnNotLoaded[1]		= "ReloadCheckA";
	stateScript[1]                  = "onReady";

	stateName[2]                    = "Fire";
	stateTransitionOnTimeout[2]     = "Smoke";
	stateTimeoutValue[2]            = 0.052;
	stateFire[2]                    = true;
	stateAllowImageChange[2]        = false;
	stateScript[2]                  = "onFire";

	stateName[3] 			  = "Smoke";
	stateTimeoutValue[3]            = 0.025; //0.2
	stateEmitter[3]                  = gunSmokeEmitter;
	stateEmitterTime[3]              = 0.2;
	stateEmitterNode[3]                  = "muzzleNode";
	stateTransitionOnTimeout[3]     = "DeathCheckA";

	stateName[4]			  = "Eject";
	stateTimeoutValue[4]		  = 0.021; //0.5
	stateTransitionOnTimeout[4]	  = "LoadCheckA";
	stateEjectShell[4]       	  = true;
	stateScript[4]                  = "onEject";

	stateName[5]            = "LoadCheckA";
	stateTimeoutValue[5]    = 0.01;
	stateScript[5]          = "TTO_onLoadCheck";
	stateTransitionOnTimeout[5] = "WaitForTriggerUp";

	stateName[6]				= "LoadCheckB";
	stateTransitionOnLoaded[6]		= "Ready";
	stateTransitionOnAmmo[6]		= "Reload";
	stateTransitionOnNoAmmo[6]    = "Empty";
	
	stateName[7]				= "ReloadCheckA";
	stateScript[7]				= "TTO_onReloadCheck";
	stateTimeoutValue[7]			= 0.01;
	stateTransitionOnTimeout[7]		= "ReloadCheckB";
						
	stateName[8]				= "ReloadCheckB";
	stateTransitionOnLoaded[8]		= "CompleteReload";
	stateTransitionOnNotLoaded[8]		= "LoadCheckB";

	stateName[9]                  = "Empty";
	stateTransitionOnLoaded[9]      = "Ready";
	stateTransitionOnAmmo[9]       = "Reload";
	stateTransitionOnTriggerDown[9] = "Fire";

	stateName[10]				= "Reload";
	stateTransitionOnTimeout[10]     	= "Reloaded";
	stateTransitionOnTriggerDown[10] 	= "Fire";
	stateTimeoutValue[10]			= 0.032;
	stateScript[10]				= "onReloadStart";

	stateName[11]				= "Reloaded";
	stateTransitionOnTimeout[11]     	= "ReloadCheckA";
	stateTimeoutValue[11]			= 0.2;
	stateScript[11]				= "onReloaded";

	stateName[12]			  = "CompleteReload";
	stateTimeoutValue[12]		  = 0.5;
	stateTransitionOnTimeout[12]     	= "Ready";
	stateSequence[12]			  = "fire";
	stateSound[12]			  = ReloadClick7Sound;
	stateScript[12]                  = "onEjectB";

	stateName[13]				= "FireDelay";
	stateTimeoutValue[13]		  = 0.01;
	stateTransitionOnTimeout[13]     	= "Fire";

	stateName[14]                  = "WaitForTriggerUp";
	stateTransitionOnTriggerUp[14]   = "LoadCheckB";

	stateName[15]                  = "DeathCheckA";
	stateTimeoutValue[15]          = 0.01;
	stateScript[15]                = "TTO_onDeathCheck";
	stateTransitionOnTimeout[15]   = "DeathCheckB";

	stateName[16]                  = "DeathCheckB";
	stateTransitionOnAmmo[16]      = "Eject";
	// stateTransitionOnNoAmmo[16]    = "Empty";
};

function RepeatingShotgunImage::onFire(%this,%obj,%slot)
{

    if(%this.TTO_canFire(%obj))
    {
		%obj.stopAudio(1);
		%obj.playAudio(1, RepeatingShotgunFireSound);
		
        %obj.playThread(2, shiftleft);
        %obj.playThread(3, shiftright);

        %this.TTO_decrementAmmo(%obj);

        if($Pref::Server::TTO::Recoil)
            %obj.spawnExplosion(TTOBigRecoilProjectile,"1 1 1");

        %projectile = %this.projectile;
        %spread = 0.0033;
        %shellCount = 9;

        %p = TTO_createProjectile(%this, %obj, %slot, %projectile, %shellCount, %spread);

        //just the muzzleflash
        //
        //nothing really special about this
        ///////////////////////////////////////////////////////////
    
        // doesn't actually show up, the projectile lifetime of 10 ms isn't enough to hit anything
        // also it doesn't explode on death so it just fades away into the abyss
        //TT_createProjectile(%this, %obj, %slot, shotgunFlashProjectile, 1);

        //shotgun blast projectile: only effective at point blank, sends targets flying off into the distance
        //
        //more or less represents the concussion blast. i can only assume such a thing exists because
        // i've never stood infront of a fucking shotgun before
        ///////////////////////////////////////////////////////////
    
        TTO_createProjectile(%this, %obj, %slot, ShotgunBlastProjectile, 1);
    }
    else if(!$Pref::Server::TTO::DeathStopFiring || %obj.getDamagePercent() < 1.0)
    {
        serverPlay3D(ShotgunPumpSound,%obj.getPosition());
    }
    %this.TTO_displayAmmo(%obj);
    return %p;
}

function SRepeatingShotgunImage::onEject(%this,%obj,%slot)
{
    %this.TTO_displayAmmo(%obj);
}

function RepeatingShotgunImage::onReloadStart(%this,%obj,%slot)
{
    if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
        return;
    %obj.playThread(0, shiftright);
    %obj.playThread(2, plant);
    %obj.playThread(3, leftRecoil);
    serverPlay3D(ReloadClick4Sound,%obj.getPosition());
    %this.TTO_displayAmmo(%obj);
}

function RepeatingShotgunImage::onReloaded(%this,%obj,%slot)
{
    // No check for DeathStopAnims since no anims/sounds here and it goes to ReloadCheckA after
    %this.TTO_incrementReload(%obj, %slot);
    %this.TTO_displayAmmo(%obj);
}
