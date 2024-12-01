//audio
datablock AudioProfile(TrenchShotgunFireSound)
{
   filename    = "./Sounds/TrenchShot_Fire.wav";
   description = AudioDefault3d;
   preload = true;
};

AddDamageType("TrenchShotgun",   '<bitmap:add-ons/Weapon_Expansion_TTOverhaul_Adventure/Icons/CI_TrenchShotgun> %1 Died to THE Trenchgun',    '%2 probably found <bitmap:add-ons/Weapon_Expansion_TTOverhaul_Adventure/Icons/CI_TrenchShotgun> %1 in the trenches',0.75,1);
datablock ProjectileData(TrenchShotgunProjectile : TTOBulletProjectile)
{
   directDamage        = 7; //14;
   directDamageType    = $DamageType::TrenchShotgun;
   radiusDamageType    = $DamageType::TrenchShotgun;

   brickExplosionRadius = 0.2;
   brickExplosionImpact = true;          //destroy a brick if we hit it directly?
   brickExplosionForce  = 15;
   brickExplosionMaxVolume = 20;          //max volume of bricks that we can destroy
   brickExplosionMaxVolumeFloating = 30;  //max volume of bricks that we can destroy if they aren't connected to the ground

   impactImpulse         = 100;
   verticalImpulse     = 50;
   explosion           = gunExplosion;

   muzzleVelocity      = 120;
   velInheritFactor    = 1;

   gravityMod = 0.4;
};

// datablock ProjectileData(ShotgunFlashProjectile : PumpShotgunProjectile)
// {
//    projectileShapeName = "";
//    directDamage        = 0; //14;
//    directDamageType    = $DamageType::PumpShotgun;
//    radiusDamageType    = $DamageType::PumpShotgun;

//    brickExplosionRadius = 0.4;
//    brickExplosionImpact = true;          //destroy a brick if we hit it directly?
//    brickExplosionForce  = 30;
//    brickExplosionMaxVolume = 25;          //max volume of bricks that we can destroy
//    brickExplosionMaxVolumeFloating = 35;  //max volume of bricks that we can destroy if they aren't connected to the ground

//    impactImpulse      = 300;
//    verticalImpulse     = 100;
//    explosion           = shotgunFlashExplosion;

//    muzzleVelocity      = 10;
//    velInheritFactor    = 1;

//    armingDelay         = 0;
//    lifetime            = 10;
//    fadeDelay           = 0;
//    isBallistic         = true;
//    gravityMod = 0.0;
// };

//////////
// item //
//////////
datablock ItemData(TrenchShotgunItem)
{
    category = "Weapon";  // Mission editor category
    className = "Weapon"; // For inventory system

     // Basic Item Properties
    shapeFile = "./Models/TrenchShotgun.dts";
    rotate = false;
    mass = 1;
    density = 0.2;
    elasticity = 0.2;
    friction = 0.6;
    emap = true;

    //gui stuff
    uiName = "Trench Shotgun";
    iconName = "./Icons/TrenchShotgun";
    doColorShift = true;
    colorShiftColor = "0.3 0.3 0.3 1.000";

     // Dynamic properties defined by the scripts
    image = TrenchShotgunImage;
    canDrop = true;

    TTO_ammoType = "shotgun";
    TTO_reloads = true;
    TTO_maxAmmo = 6;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(TrenchShotgunImage)
{
   // Basic Item properties
   shapeFile = "./Models/TrenchShotgun.dts";
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
   item = TrenchShotgunItem;
   ammo = " ";
   projectile = TrenchShotgunProjectile;
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
   colorShiftColor = TrenchShotgunItem.colorShiftColor;

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
    stateSound[0]             = weaponswitchSound;

    stateName[1]                        = "Ready";
    stateTransitionOnTriggerDown[1]     = "FireDelay";
    stateTransitionOnNotLoaded[1]       = "ReloadCheckA";
    stateScript[1]                  = "onReady";

    stateName[2]                    = "Fire";
    stateTransitionOnTimeout[2]     = "Smoke";
	stateSequence[2]                = "fire";
    stateTimeoutValue[2]            = 0.1;
    stateFire[2]                    = true;
    stateAllowImageChange[2]        = false;
    stateScript[2]                  = "onFire";

    stateName[3]              = "Smoke";
    stateTimeoutValue[3]            = 0.065; //0.2
	stateEmitter[3]                  = gunSmokeEmitter;
	stateEmitterTime[3]              = 0.2;
	stateEmitterNode[3]                  = "muzzleNode";
    stateTransitionOnTimeout[3]     = "DeathCheckA";


    stateName[4]              = "Eject";
    stateTimeoutValue[4]          = 0.35; //0.5
    stateTransitionOnTimeout[4]   = "LoadCheckA";
    stateEjectShell[4]            = true;
    stateSequence[4]              = "pump";
    stateSound[4]             = ShotgunPump1Sound;
    stateScript[4]                  = "onEject";


    stateName[5]            = "LoadCheckA";
    stateTimeoutValue[5]    = 0.01;
    stateScript[5]          = "TTO_onLoadCheck";
    stateTransitionOnTimeout[5] = "WaitForTriggerUp";


    stateName[6]                = "LoadCheckB";
    stateTransitionOnLoaded[6]      = "Ready";
    stateTransitionOnAmmo[6]        = "Reload";
    stateTransitionOnNoAmmo[6]    = "Empty";
    
    stateName[7]                = "ReloadCheckA";
    stateScript[7]              = "TTO_onReloadCheck";
    stateTimeoutValue[7]            = 0.01;
    stateTransitionOnTimeout[7]     = "ReloadCheckB";
                        
    stateName[8]                = "ReloadCheckB";
    stateTransitionOnLoaded[8]      = "CompleteReload";
    stateTransitionOnNotLoaded[8]       = "LoadCheckB";

    stateName[9]                  = "Empty";
    stateTransitionOnLoaded[9]      = "Ready";
    stateTransitionOnAmmo[9]       = "Reload";
    stateTransitionOnTriggerDown[9] = "Fire";

    stateName[10]               = "Reload";
    stateTransitionOnTimeout[10]        = "Reloaded";
    stateTransitionOnTriggerDown[10]    = "Fire";
    stateTimeoutValue[10]           = 0.032;
    stateScript[10]             = "onReloadStart";

    stateName[11]               = "Reloaded";
    stateTransitionOnTimeout[11]        = "ReloadCheckA";
    stateTimeoutValue[11]           = 0.2;
    stateScript[11]             = "onReloaded";

    stateName[12]             = "CompleteReload";
    stateTimeoutValue[12]         = 0.5;
    stateTransitionOnTimeout[12]        = "Ready";
    stateSequence[12]             = "pump";
    stateSound[12]            = ShotgunPump1Sound;
    stateScript[12]                  = "onEject";


    stateName[13]               = "FireDelay";
    stateTimeoutValue[13]         = 0.01;
    stateTransitionOnTimeout[13]        = "Fire";

    stateName[14]                  = "WaitForTriggerUp";
    stateTransitionOnTriggerUp[14]   = "LoadCheckB";


    stateName[15]                  = "DeathCheckA";
    stateTimeoutValue[15]          = 0.032; // necessary to sync state with animation and sound
    stateScript[15]                = "TTO_onDeathCheck";
    stateTransitionOnTimeout[15]   = "DeathCheckB";

    stateName[16]                  = "DeathCheckB";
    stateTransitionOnAmmo[16]      = "Eject";
};

function TrenchShotgunImage::onFire(%this,%obj,%slot)
{

    if(%this.TTO_canFire(%obj))
    {
		%obj.stopAudio(1);
		%obj.playAudio(1, TrenchShotgunFireSound);
		
        %obj.playThread(2, shiftleft);
        %obj.playThread(3, shiftright);

        %this.TTO_decrementAmmo(%obj);

        if($Pref::Server::TTO::Recoil)
            %obj.spawnExplosion(TTOBigRecoilProjectile,"1 1 1");

        %projectile = %this.projectile;
        %spread = 0.0038;
        %shellCount = 6;

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
        serverPlay3D(ShotgunPump1Sound,%obj.getPosition());
    }
    %this.TTO_displayAmmo(%obj);
    return %p;
}

function TrenchShotgunImage::onEject(%this,%obj,%slot)
{
    %obj.playThread(2, plant);
    %this.TTO_displayAmmo(%obj);
}

function TrenchShotgunImage::onReloadStart(%this,%obj,%slot)
{
    if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
        return;
    %obj.playThread(0, shiftright);
    %obj.playThread(2, plant);
    %obj.playThread(3, leftRecoil);
    serverPlay3D(ReloadClick4Sound,%obj.getPosition());
    %this.TTO_displayAmmo(%obj);
}

function TrenchShotgunImage::onReloaded(%this,%obj,%slot)
{
    // No check for DeathStopAnims since no anims/sounds here and it goes to ReloadCheckA after
    %this.TTO_incrementReload(%obj, %slot);
    %this.TTO_displayAmmo(%obj);
}
