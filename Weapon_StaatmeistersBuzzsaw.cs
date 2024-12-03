//audio
datablock AudioProfile(StaatmeistersBuzzsawFireSound)
{
   filename    = "./Sounds/StaatmeistersBuzzsaw_Fire.wav";
   description = AudioDefault3d;
   preload = true;
};

AddDamageType("StaatmeistersBuzzsaw",   '<bitmap:add-ons/Weapon_Package_TTOverhaul_WWII/Icons/CI_StaatmeistersBuzzsaw> %1 Got deaf instantly',    '%2 KRAKRAKRAKROW\'d <bitmap:add-ons/Weapon_Package_TTOverhaul_WWII/Icons/CI_StaatmeistersBuzzsaw> %1',0.75,1);
datablock ProjectileData(StaatmeistersBuzzsawProjectile : TTOBulletProjectile)
{
   directDamage        = 25;
   directDamageType    = $DamageType::StaatmeistersBuzzsaw;
   radiusDamageType    = $DamageType::StaatmeistersBuzzsaw;

   impactImpulse	     = 300;
   verticalImpulse     = 20;


   muzzleVelocity      = 200;

   gravityMod = 0.7;
   explodeOnDeath = true;
   explodeOnPlayerImpact = false;
};

//////////
// item //
//////////
datablock ItemData(StaatmeistersBuzzsawItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./Models/StaatmeistersBuzzsaw.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Staatmeister's Buzzsaw";
	iconName = "./Icons/StaatmeistersBuzzsaw";
	doColorShift = true;
	colorShiftColor = "0.15 0.5 0.68 1.000";

	 // Dynamic properties defined by the scripts
	image = StaatmeistersBuzzsawImage;
	canDrop = true;

	TTO_ammoType = "762";
	TTO_reloads = true;
	TTO_maxAmmo = 50;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(StaatmeistersBuzzsawImage)
{
   // Basic Item properties
   shapeFile = "./Models/StaatmeistersBuzzsaw.dts";
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
   item = StaatmeistersBuzzsawItem;
   ammo = " ";
   projectile = StaatmeistersBuzzsawProjectile;
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
   colorShiftColor = StaatmeistersBuzzsawItem.colorShiftColor;

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
	stateTransitionOnTriggerDown[1]  = "Click";

	stateName[2]                    = "Fire";
	stateTransitionOnTimeout[2]     = "Delay";
	stateTimeoutValue[2]            = 0;
	//stateSound[2]				= LightMachinegunfire1Sound;
	stateFire[2]                    = true;
	stateAllowImageChange[2]        = false;
	stateScript[2]                  = "onFire";
	stateEjectShell[2]       	  = true;
	stateEmitter[2]					= TTOMuzzleFlashEmitter;
	stateEmitterTime[2]				= 0.01;
	stateEmitterNode[2]				= "muzzleNode";

	stateName[3]			= "Delay";
	stateTransitionOnTimeout[3]     = "FireLoadCheckA";
	stateTimeoutValue[3]            = 0.025;
	
	stateName[4]				= "LoadCheckA";
	stateScript[4]				= "TTO_onLoadCheck";
	stateTimeoutValue[4]			= 0.01;
	stateTransitionOnTimeout[4]		= "LoadCheckB";
	
	stateName[5]				= "LoadCheckB";
	stateTransitionOnLoaded[5]		= "Ready";
	stateTransitionOnNotLoaded[5]		= "Empty";

	stateName[6]				= "Reload";
	stateTimeoutValue[6]			= 1.4;
	stateScript[6]				= "onReloadStart";
	stateTransitionOnTimeout[6]		= "Wait";
	
	stateName[7]				= "Wait";
	stateTimeoutValue[7]			= 0.8;
	stateScript[7]				= "onReloadWait";
	stateTransitionOnTimeout[7]		= "Wait2";
	
	stateName[8]				= "Wait2";
	stateTimeoutValue[8]			= 0.8;
	stateScript[8]				= "onReloadWait2";
	stateTransitionOnTimeout[8]		= "Wait3";
	
	stateName[9]				= "Wait3";
	stateTimeoutValue[9]			= 0.8;
	stateScript[9]				= "onReloadWait3";
	stateTransitionOnTimeout[9]		= "Wait4";
	
	stateName[10]				= "Wait4";
	stateTimeoutValue[10]			= 0.8;
	stateScript[10]				= "onReloadWait4";
	stateTransitionOnTimeout[10]		= "Reloaded";
	
	stateName[11]				= "FireLoadCheckA";
	stateScript[11]				= "TTO_onLoadCheck";
	stateTimeoutValue[11]			= 0.01;
	stateTransitionOnTimeout[11]		= "FireLoadCheckB";
	
	stateName[12]				= "FireLoadCheckB";
	stateTransitionOnLoaded[12]		= "Smoke";
	stateTransitionOnNotLoaded[12]		= "ReloadSmoke";
	
	stateName[13] 				= "Smoke";
	stateEmitter[13]			= gunSmokeEmitter;
	stateEmitterTime[13]			= 0.3;
	stateEmitterNode[13]			= "muzzleNode";
	stateTimeoutValue[13]			= 0.03;
	stateTransitionOnTimeout[13]		= "Halt";
	stateTransitionOnTriggerDown[13]	= "FireCheckA";
	
	stateName[14] 				= "ReloadSmoke";
	stateEmitter[14]			= gunSmokeEmitter;
	stateEmitterTime[14]			= 0.3;
	stateEmitterNode[14]			= "muzzleNode";
	stateTimeoutValue[14]			= 0.03;
	stateTransitionOnTimeout[14]		= "EmptyTransition";
	
	stateName[15]				= "Reloaded";
	stateTimeoutValue[15]			= 0.04;
	stateScript[15]				= "onReloaded";
	stateTransitionOnTimeout[15]		= "Ready";

	stateName[16]			= "Halt";
	stateTransitionOnTimeout[16]     = "Ready";
	stateTimeoutValue[16]            = 0.3;
	stateEmitter[16]					= gunSmokeEmitter;
	stateEmitterTime[16]				= 0.48;
	stateEmitterNode[16]				= "muzzleNode";
	stateScript[16]                  = "onHalt";

	stateName[17]                     = "Click";
	stateTransitionOnTimeout[17]      = "FireCheckA";
	stateTimeoutValue[17]             = 0.06;
	stateSound[17]				= LightMachinegunClickSound;
	stateTransitionOnNotLoaded[17]        = "EmptyFire";

	stateName[18]                   = "Empty";
	stateTransitionOnLoaded[18]      = "Ready";
	stateTransitionOnAmmo[18]  = "Reload";
	stateTransitionOnTriggerDown[18]     = "FireCheckA";

	stateName[19]                    = "EmptyFire";
	stateScript[19]                  = "TTO_onEmptyFire";
	stateTransitionOnLoaded[19]      = "Ready";
	stateTransitionOnAmmo[19]        = "Reload";
	stateTransitionOnTriggerUp[19]   = "Empty";

	stateName[20]                    = "FireCheckA";
	stateScript[20]                  = "TTO_onFireCheck";
	stateTransitionOnTimeout[20]     = "FireCheckB";

	stateName[21]                    = "FireCheckB";
	stateTransitionOnLoaded[21]   = "Fire";
	stateTransitionOnNotLoaded[21]      = "EmptyFire";
};

function StaatmeistersBuzzsawImage::onFire(%this,%obj,%slot)
{

	%obj.stopAudio(1);
	%obj.playAudio(1, StaatmeistersBuzzsawFireSound);

	TTO_knockback(%obj, 0, 0 ,-1);
	
	%obj.lastShotTime = getSimTime();

	%shellCount = 1;
	%spread = 0.0035;
	// never true because of second clause
	// if(vectorLen(%obj.getVelocity()) < 0.1) && (getSimTime() - %obj.lastShotTime) > 1000)
	// {
	// 	%spread = 0.0004;
	// }
	// else
	// {
	// 	%spread = 0.0007;
	// }
	%projectile = %this.projectile;

	%obj.playThread(2, shiftRight);
	%obj.playThread(3, shiftLeft);

	%this.TTO_decrementAmmo(%obj);
	%this.TTO_displayAmmo(%obj);

	if($Pref::Server::TTO::Recoil)
		%obj.spawnExplosion(TTOBigRecoilProjectile,"1 1 1");

	return TTO_createProjectile(%this, %obj, %slot, %projectile, %shellCount, %spread);
}

function StaatmeistersBuzzsawImage::onReloadStart(%this,%obj,%slot)
{
	if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
		return;
	%this.TTO_displayAmmo(%obj);
	%obj.playThread(2, wrench);
	serverPlay3D(LMGCoverUpSound,%obj.getPosition());
}

function StaatmeistersBuzzsawImage::onReloadWait(%this,%obj,%slot)
{
	if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
		return;
	%this.TTO_displayAmmo(%obj);
	%obj.playThread(2, shiftRight);
	serverPlay3D(ReloadOut1Sound,%obj.getPosition());
}

function StaatmeistersBuzzsawImage::onReloadWait2(%this,%obj,%slot)
{
	if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
		return;
	%this.TTO_displayAmmo(%obj);
	%obj.playThread(2, plant);
	serverPlay3D(ReloadTap2Sound,%obj.getPosition());
}

function StaatmeistersBuzzsawImage::onReloadWait3(%this,%obj,%slot)
{
	if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
		return;
	%this.TTO_displayAmmo(%obj);
	%obj.playThread(2, shiftLeft);
	serverPlay3D(LMGChainSound,%obj.getPosition());
}

function StaatmeistersBuzzsawImage::onReloadWait4(%this,%obj,%slot)
{
	if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
		return;
	%this.TTO_displayAmmo(%obj);
	%obj.playThread(2, plant);
	serverPlay3D(LMGCoverDownSound,%obj.getPosition());
}

function StaatmeistersBuzzsawImage::onReloaded(%this,%obj,%slot)
{
	if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
		return %obj.setImageLoaded(%slot, 1);
	%this.TTO_reload(%obj, %slot, ReloadClick8Sound, plant);
	%this.TTO_displayAmmo(%obj);
}

function StaatmeistersBuzzsawImage::onHalt(%this,%obj,%slot)
{
	%this.TTO_displayAmmo(%obj);
}

function StaatmeistersBuzzsawProjectile::damage(%this,%obj,%col,%fade,%pos,%normal)
{
   if(%col.getType() & $TypeMasks::PlayerObjectType)
   {
      TTO_dampenVelocity(%col, 1.2);
   }
	Parent::damage(%this,%obj,%col,%fade,%pos,%normal);
}
