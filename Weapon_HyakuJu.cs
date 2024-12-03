//audio
datablock AudioProfile(HyakuJuFireSound)
{
   filename    = "./Sounds/HyakuJu_Fire.wav";
   description = AudioDefault3d;
   preload = true;
};


AddDamageType("HyakuJu",   'Eventually, <bitmap:add-ons/Weapon_Package_TTOverhaul_WWII/Icons/CI_HyakuJu> %1 has stopped living',    '%2 Killed <bitmap:add-ons/Weapon_Package_TTOverhaul_WWII/Icons/CI_HyakuJu> %1 with another Sideways SMG',0.75,1);
datablock ProjectileData(HyakuJuProjectile : TTOBulletProjectile)
{
   directDamage        = 12;
   directDamageType    = $DamageType::HyakuJu;
   radiusDamageType    = $DamageType::HyakuJu;

   impactImpulse	     = 100;
   verticalImpulse     = 20;

   muzzleVelocity      = 170;

   gravityMod = 0.2;
};

//////////
// item //
//////////
datablock ItemData(HyakuJuItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./Models/HyakuJu.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Hyaku Ju";
	iconName = "./Icons/HyakuJu";
	doColorShift = true;
	colorShiftColor = "0.3 0.3 0.38 1.000";

	 // Dynamic properties defined by the scripts
	image = HyakuJuImage;
	canDrop = true;

	TTO_ammoType = "9MM";
	TTO_reloads = true;
	TTO_maxAmmo = 30;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(HyakuJuImage)
{
   // Basic Item properties
   shapeFile = "./Models/HyakuJu.dts";
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
   item = HyakuJuItem;
   ammo = " ";
   projectile = HyakuJuProjectile;
   projectileType = Projectile;

   casing = GunShellDebris;
   shellExitDir        = "1.0 0.1 1.0";
   shellExitOffset     = "0 0 0";
   shellExitVariance   = 10.0;	
   shellVelocity       = 5.0;

   //melee particles shoot from eye node for consistancy
   melee = false;
   //raise your arm up or not
   armReady = true;

   doColorShift = true;
   colorShiftColor = HyakuJuItem.colorShiftColor;

   // Images have a state system which controls how the animations
   // are run, which sounds are played, script callbacks, etc. This
   // state system is downloaded to the client so that clients can
   // predict state changes and animate accordingly.  The following
   // system supports basic ready->fire->reload transitions as
   // well as a no-ammo->dryfire idle state.

   // Initial start up state
	stateName[0]                     = "Activate";
	stateTimeoutValue[0]             = 0.05;
	stateTransitionOnTimeout[0]       = "LoadCheckA";
	stateSound[0]					= weaponswitchSound;

	stateName[1]                     = "Ready";
	stateTransitionOnNotLoaded[1]       = "Reload";
	stateTransitionOnTriggerDown[1]  = "FireCheckA";
	stateAllowImageChange[1]         = true;

	stateName[2]                    = "Fire";
	stateTransitionOnTimeout[2]     = "Delay";
	stateTimeoutValue[2]            = 0;
	stateFire[2]                    = true;
	stateAllowImageChange[2]        = false;
	stateSequence[2]                = "Fire";
	stateScript[2]                  = "onFire";
	stateEjectShell[2]       	  = true;
	stateEmitter[2]					= TTOMuzzleFlashEmitter;
	stateEmitterTime[2]				= 0.05;
	stateEmitterNode[2]				= "muzzleNode";
	//stateSound[2]					= submachinegunFire1Sound;

	stateName[3]			= "Delay";
	stateTransitionOnTimeout[3]     = "FireLoadCheckA";
	stateTimeoutValue[3]            = 0.075;
	stateEmitter[3]					= gunSmokeEmitter;
	stateEmitterTime[3]				= 0.001;
	stateEmitterNode[3]				= "muzzleNode";
	
	stateName[4]				= "LoadCheckA";
	stateScript[4]				= "TTO_onLoadCheck";
	stateTimeoutValue[4]			= 0.01;
	stateTransitionOnTimeout[4]		= "LoadCheckB";
	
	stateName[5]				= "LoadCheckB";
	stateTransitionOnLoaded[5]		= "Ready";
	stateTransitionOnNotLoaded[5]  = "Empty";

	stateName[6]				= "Reload";
	stateTimeoutValue[6]			= 0.5;
	stateScript[6]				= "onReloadStart";
	stateTransitionOnTimeout[6]		= "Wait";
	stateWaitForTimeout[6]			= true;
	
	stateName[7]				= "Wait";
	stateTimeoutValue[7]			= 0.42;
	stateScript[7]				= "onReloadWait";
	stateTransitionOnTimeout[7]		= "Reloaded";
	
	stateName[8]				= "FireLoadCheckA";
	stateScript[8]				= "TTO_onLoadCheck";
	stateTimeoutValue[8]			= 0.01;
	stateTransitionOnTimeout[8]		= "FireLoadCheckB";
	
	stateName[9]				= "FireLoadCheckB";
	stateTransitionOnLoaded[9]		= "Smoke";
	stateTransitionOnNotLoaded[9]		= "ReloadSmoke";
	
	stateName[10] 				= "Smoke";
	stateEmitter[10]			= gunSmokeEmitter;
	stateEmitterTime[10]			= 0.3;
	stateEmitterNode[10]			= "muzzleNode";
	stateTimeoutValue[10]			= 0.03;
	stateTransitionOnTimeout[10]		= "Ready";
	stateTransitionOnTriggerDown[10]	= "FireCheckA";

	stateName[11] 				= "ReloadSmoke";
	stateEmitter[11]			= gunSmokeEmitter;
	stateEmitterTime[11]			= 0.3;
	stateEmitterNode[11]			= "muzzleNode";
	stateTimeoutValue[11]			= 0.2;
	stateTransitionOnTimeout[11]		= "Empty";
	
	stateName[12]				= "Reloaded";
	stateTimeoutValue[12]			= 0.4;
	stateScript[12]				= "onReloaded";
	stateTransitionOnTimeout[12]		= "Ready";

	stateName[13]                    = "FireCheckA";
	stateScript[13]                  = "TTO_onFireCheck";
	stateTransitionOnTimeout[13]     = "FireCheckB";

	stateName[14]                    = "FireCheckB";
	stateTransitionOnLoaded[14]   = "Fire";
	stateTransitionOnNotLoaded[14]      = "EmptyFire";

	stateName[15]                    = "Empty";
	stateTransitionOnLoaded[15]      = "Ready";
	stateTransitionOnAmmo[15]        = "Reload";
	stateTransitionOnTriggerDown[15] = "FireCheckA";

	stateName[16]                    = "EmptyFire";
	stateScript[16]                  = "TTO_onEmptyFire";
	stateTransitionOnLoaded[16]      = "Ready";
	stateTransitionOnAmmo[16]        = "Reload";
	stateTransitionOnTriggerUp[16]   = "Empty";
};

function HyakuJuImage::onFire(%this,%obj,%slot)
{

	%obj.stopAudio(1);
	%obj.playAudio(1, TypewriterSMGFireSound);

	%projectile = %this.projectile;
	%spread = 0.0021;
	%shellCount = 1;

	%obj.playThread(2, shiftRight);
	%obj.playThread(3, shiftLeft);
	
	%this.TTO_decrementAmmo(%obj);
	%this.TTO_displayAmmo(%obj);

	if($Pref::Server::TTO::Recoil)
		%obj.spawnExplosion(TTOLittleRecoilProjectile,"1 1 1");

	return TTO_createProjectile(%this, %obj, %slot, %projectile, %shellCount, %spread);
}

function HyakuJuImage::onReloadStart(%this,%obj,%slot)
{
	if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
		return;
	%this.TTO_displayAmmo(%obj);
	%obj.playThread(2, shiftRight);
	serverPlay3D(ReloadOut1Sound,%obj.getPosition());
}

function HyakuJuImage::onReloadWait(%this,%obj,%slot)
{
	if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
		return;
	%this.TTO_displayAmmo(%obj);
	serverPlay3D(ReloadTap7Sound,%obj.getPosition());
	%obj.playThread(2, plant);
}

function HyakuJuImage::onReloaded(%this,%obj,%slot)
{
	if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
		return %obj.setImageLoaded(%slot, 1);
	%this.TTO_reload(%obj, %slot, reloadClick6Sound, plant);
	%this.TTO_displayAmmo(%obj);
}

function HyakuJuProjectile::damage(%this,%obj,%col,%fade,%pos,%normal)
{
	if(%col.getType() & $TypeMasks::PlayerObjectType)
	{
		TTO_dampenVelocity(%col, 1.1);
	}
	Parent::damage(%this,%obj,%col,%fade,%pos,%normal);
}