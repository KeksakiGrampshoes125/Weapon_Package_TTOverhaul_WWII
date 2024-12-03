//audio
datablock AudioProfile(SturmGewehrFireSound)
{
   filename    = "./Sounds/SturmGewehr_Fire.wav";
   description = AudioDefault3d;
   preload = true;
};

AddDamageType("SturmGewehr",   '<bitmap:add-ons/Weapon_Package_TTOverhaul_WWII/Icons/CI_SturmGewehr> %1 Mein lieben\'d',    '%2 Showed <bitmap:add-ons/Weapon_Package_TTOverhaul_WWII/Icons/CI_SturmGewehr> %1 was impressed by german technology',0.75,1);
datablock ProjectileData(SturmGewehrProjectile : TTOBigBulletProjectile)
{
   directDamage        = 20;
   directDamageType    = $DamageType::SturmGewehr;
   radiusDamageType    = $DamageType::SturmGewehr;

   impactImpulse	     = 140;
   verticalImpulse     = 10;

   muzzleVelocity      = 180;

   gravityMod = 0.2;

   uiName = "";
};

//////////
// item //
//////////
datablock ItemData(SturmGewehrItem)
{
	category = "Weapon";  // Mission editor category
	className = "Weapon"; // For inventory system

	 // Basic Item Properties
	shapeFile = "./Models/SturmGewehr.dts";
	rotate = false;
	mass = 1;
	density = 0.2;
	elasticity = 0.2;
	friction = 0.6;
	emap = true;

	//gui stuff
	uiName = "Sturm Gewehr";
	iconName = "./Icons/SturmGewehr";
	doColorShift = true;
	colorShiftColor = "0.4 0.4 0.5 1.000";

	 // Dynamic properties defined by the scripts
	image = SturmGewehrImage;
	canDrop = true;

	TTO_ammoType = "762";
	TTO_reloads = true;
	TTO_maxAmmo = 30;
};

////////////////
//weapon image//
////////////////
datablock ShapeBaseImageData(SturmGewehrImage)
{
   // Basic Item properties
   shapeFile = "./Models/SturmGewehr.dts";
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
   item = SturmGewehrItem;
   ammo = " ";
   projectile = SturmGewehrProjectile;
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
   colorShiftColor = SturmGewehrItem.colorShiftColor;

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

	stateName[2]                    = "Fire";
	stateTransitionOnTimeout[2]     = "Delay";
	stateTimeoutValue[2]            = 0.01;
	stateFire[2]                    = true;
	stateAllowImageChange[2]        = false;
	stateScript[2]                  = "onFire";
	stateEjectShell[2]       	  = true;
	stateEmitter[2]					= TTOMuzzleFlashEmitter;
	stateEmitterTime[2]				= 0.05;
	stateEmitterNode[2]				= "muzzleNode";
	//stateSound[2]					= BrushyRifleFireSound;

	stateName[3]			= "Delay";
	stateTransitionOnTimeout[3]     = "FireLoadCheckA";
	stateTimeoutValue[3]            = 0.036;
	stateEmitter[3]					= gunSmokeEmitter;
	stateEmitterTime[3]				= 0.001;
	stateEmitterNode[3]				= "muzzleNode";
	
	stateName[4]				= "LoadCheckA";
	stateScript[4]				= "TTO_onLoadCheck";
	stateTimeoutValue[4]			= 0.01;
	stateTransitionOnTimeout[4]		= "LoadCheckB";
	
	stateName[5]				= "LoadCheckB";
	stateTransitionOnLoaded[5]		= "Ready";
	stateTransitionOnNotLoaded[5]		= "Empty";

	stateName[6]				= "Reload";
	stateTimeoutValue[6]			= 0.5;
	stateScript[6]				= "onReloadStart";
	stateTransitionOnTimeout[6]		= "Wait";
	
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
	stateTimeoutValue[11]			= 0.03;
	stateTransitionOnTimeout[11]		= "Empty";
	

	stateName[13]				= "Reloaded";
	stateTimeoutValue[13]			= 0.4;
	stateScript[13]				= "onReloaded";
	stateTransitionOnTimeout[13]		= "Ready";

	stateName[14]                   = "Empty";
	stateTransitionOnLoaded[14]      = "Ready";
	stateTransitionOnAmmo[14]  = "Reload";
	stateTransitionOnTriggerDown[14]     = "FireCheckA";

	stateName[15]                   = "EmptyFire";
	stateScript[15]                 = "TTO_onEmptyFire";
	stateTransitionOnLoaded[15]      = "Ready";
	stateTransitionOnAmmo[15]  = "Reload";
	stateTransitionOnTriggerUp[15]     = "Empty";

	stateName[16]                    = "FireCheckA";
	stateScript[16]                  = "TTO_onFireCheck";
	stateTransitionOnTimeout[16]     = "FireCheckB";

	stateName[17]                    = "FireCheckB";
	stateTransitionOnLoaded[17]   = "Fire";
	stateTransitionOnNotLoaded[17]      = "EmptyFire";
};

function SturmGewehrImage::onFire(%this,%obj,%slot)
{

	%obj.stopAudio(1);
	%obj.playAudio(1, SturmGewehrFireSound);

	%projectile = %this.projectile;
	if((getSimTime() - %obj.lastShotTime) > 200)
	{
		%spread = 0.0007;
	}
	else
	{
		%spread = 0.0012;
	}
	%shellCount = 1;

	TTO_knockback(%obj, 0, 0, -1);

	%obj.playThread(2, shiftright);
	%obj.playThread(3, shiftleft);

	%this.TTO_decrementAmmo(%obj);
	%this.TTO_displayAmmo(%obj);

	if($Pref::Server::TTO::Recoil)
		%obj.spawnExplosion(TTOMedRecoilProjectile,"1 1 1");

	return TTO_createProjectile(%this, %obj, %slot, %projectile, %shellCount, %spread);
}

function SturmGewehrImage::onReloadStart(%this,%obj,%slot)
{
	if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
		return;
	%this.TTO_displayAmmo(%obj);
	%obj.playThread(2, shiftTo);
	serverPlay3D(ReloadOut1Sound,%obj.getPosition());
}

function SturmGewehrImage::onReloadWait(%this,%obj,%slot)
{
	if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
		return;
	%this.TTO_displayAmmo(%obj);
	%obj.playThread(2, shiftleft);
	serverPlay3D(ReloadTap3Sound,%obj.getPosition());
}

function SturmGewehrImage::onReloaded(%this,%obj,%slot)
{
	if($Pref::Server::TTO::DeathStopAnims && %obj.getDamagePercent() >= 1.0)
		return %obj.setImageLoaded(%slot, 1);
	%this.TTO_reload(%obj, %slot, reloadClick8Sound, plant);
	%this.TTO_displayAmmo(%obj);
}

function SturmGewehrProjectile::damage(%this,%obj,%col,%fade,%pos,%normal)
{
   if(%col.getType() & $TypeMasks::PlayerObjectType)
   {
      TTO_dampenVelocity(%col, 1.2);
   }
	Parent::damage(%this,%obj,%col,%fade,%pos,%normal);
}
